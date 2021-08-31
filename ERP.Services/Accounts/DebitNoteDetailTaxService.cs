using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class DebitNoteDetailTaxService : Repository<Debitnotedetailtax>, IDebitNoteDetailTax
    {
        IDebitNoteDetail debitNoteDetail;

        public DebitNoteDetailTaxService(ErpDbContext dbContext,  IDebitNoteDetail _debitNoteDetail) : base(dbContext)
        {
            debitNoteDetail = _debitNoteDetail;
        }
        
        /// <summary>
        /// generate sr no based on debitNoteDetId
        /// </summary>
        /// <param name="debitNoteDetId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int debitNoteDetId)
        {
            int srNo = 0;

            if (await Any(w => w.DebitNoteDetTaxId != 0 && w.DebitNoteDetId == debitNoteDetId))
            {
                srNo = await GetQueryByCondition(w => w.DebitNoteDetTaxId != 0 && w.DebitNoteDetId == debitNoteDetId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel)
        {
            int debitNoteDetailTaxId = 0;
            int multiplier = 1;

            DebitNoteDetailModel debitNoteDetailModel = null;
            debitNoteDetailModel = await debitNoteDetail.GetDebitNoteDetailById((Int32)debitNoteDetailTaxModel.DebitNoteDetId);

            // assign values.
            Debitnotedetailtax debitNoteDetailTax = new Debitnotedetailtax();

            debitNoteDetailTax.DebitNoteDetId = debitNoteDetailTaxModel.DebitNoteDetId;
            debitNoteDetailTax.SrNo = debitNoteDetailTaxModel.SrNo;
            debitNoteDetailTax.TaxLedgerId = debitNoteDetailTaxModel.TaxLedgerId;
            debitNoteDetailTax.TaxPercentageOrAmount = debitNoteDetailTaxModel.TaxPercentageOrAmount;
            debitNoteDetailTax.TaxPerOrAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc;

            if (DiscountType.Percentage.ToString() == debitNoteDetailTaxModel.TaxPercentageOrAmount)
            {
                debitNoteDetailTaxModel.TaxAmountFc = (debitNoteDetailModel.GrossAmountFc * debitNoteDetailTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                debitNoteDetailTaxModel.TaxAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc;
            }

            if (TaxAddOrDeduct.Deduct.ToString() == debitNoteDetailTaxModel.TaxAddOrDeduct)
            {
                multiplier = -1;
            }

            debitNoteDetailTax.TaxAddOrDeduct = debitNoteDetailTaxModel.TaxAddOrDeduct;
            debitNoteDetailTax.TaxAmountFc = multiplier * debitNoteDetailTaxModel.TaxAmountFc;
            debitNoteDetailTax.TaxAmount = multiplier * debitNoteDetailTaxModel.TaxAmount;
            debitNoteDetailTax.Remark = debitNoteDetailTaxModel.Remark;

            await Create(debitNoteDetailTax);

            debitNoteDetailTaxId = debitNoteDetailTax.DebitNoteDetTaxId;


            if (debitNoteDetailTaxId != 0)
            {
                await debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailTax.DebitNoteDetId);
            }

            return debitNoteDetailTaxId; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Debitnotedetailtax debitNoteDetailTax = await GetQueryByCondition(w => w.DebitNoteDetTaxId == debitNoteDetailTaxModel.DebitNoteDetTaxId)
                                                                .Include(w => w.DebitNoteDet).FirstOrDefaultAsync();

            if (null != debitNoteDetailTax)
            {
                // assign values.
                debitNoteDetailTax.DebitNoteDetId = debitNoteDetailTaxModel.DebitNoteDetId;
                debitNoteDetailTax.SrNo = debitNoteDetailTaxModel.SrNo;
                debitNoteDetailTax.TaxLedgerId = debitNoteDetailTaxModel.TaxLedgerId;
                debitNoteDetailTax.TaxPercentageOrAmount = debitNoteDetailTaxModel.TaxPercentageOrAmount;
                debitNoteDetailTax.TaxPerOrAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc;

                if (DiscountType.Percentage.ToString() == debitNoteDetailTaxModel.TaxPercentageOrAmount)
                {
                    debitNoteDetailTaxModel.TaxAmountFc = (debitNoteDetailTax.DebitNoteDet.GrossAmountFc * debitNoteDetailTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    debitNoteDetailTaxModel.TaxAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == debitNoteDetailTaxModel.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                debitNoteDetailTax.TaxAddOrDeduct = debitNoteDetailTaxModel.TaxAddOrDeduct;
                debitNoteDetailTax.TaxAmountFc = multiplier * debitNoteDetailTaxModel.TaxAmountFc;
                debitNoteDetailTax.TaxAmount = multiplier * debitNoteDetailTaxModel.TaxAmount;
                debitNoteDetailTax.Remark = debitNoteDetailTaxModel.Remark;

                isUpdated = await Update(debitNoteDetailTax);
            }

            if (isUpdated != false)
            {
                await debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailTax.DebitNoteDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteDebitNoteDetailTax(int debitNoteDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Debitnotedetailtax debitNoteDetailTax = await GetByIdAsync(w => w.DebitNoteDetTaxId == debitNoteDetailTaxId);

            if (null != debitNoteDetailTax)
            {
                isDeleted = await Delete(debitNoteDetailTax);
            }

            if (isDeleted != false)
            {
                await debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailTax.DebitNoteDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<DebitNoteDetailTaxModel> GetDebitNoteDetailTaxById(int debitNoteDetailTaxId)
        {
            DebitNoteDetailTaxModel debitNoteDetailTaxModel = null;

            IList<DebitNoteDetailTaxModel> debitNoteDetailTaxModelList = await GetDebitNoteDetailTaxList(debitNoteDetailTaxId, 0, 0);

            if (null != debitNoteDetailTaxModelList && debitNoteDetailTaxModelList.Any())
            {
                debitNoteDetailTaxModel = debitNoteDetailTaxModelList.FirstOrDefault();
            }

            return debitNoteDetailTaxModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxByDebitNoteDetailId(int debitNoteDetailId)
        {
            DataTableResultModel<DebitNoteDetailTaxModel> resultModel = new DataTableResultModel<DebitNoteDetailTaxModel>();

            IList<DebitNoteDetailTaxModel> debitNoteDetailTaxModelList = await GetDebitNoteDetailTaxList(0, debitNoteDetailId, 0);
            if (null != debitNoteDetailTaxModelList && debitNoteDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteDetailTaxModel>();
                resultModel.ResultList = debitNoteDetailTaxModelList;
                resultModel.TotalResultCount = debitNoteDetailTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<DebitNoteDetailTaxModel>();
                resultModel.ResultList = new List<DebitNoteDetailTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxList()
        {
            DataTableResultModel<DebitNoteDetailTaxModel> resultModel = new DataTableResultModel<DebitNoteDetailTaxModel>();

            IList<DebitNoteDetailTaxModel> debitNoteDetailTaxModelList = await GetDebitNoteDetailTaxList(0, 0, 0);

            if (null != debitNoteDetailTaxModelList && debitNoteDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteDetailTaxModel>();
                resultModel.ResultList = debitNoteDetailTaxModelList;
                resultModel.TotalResultCount = debitNoteDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxList(int debitNoteDetailTaxId, int debitNoteDetailId, int debitNoteId)
        {
            IList<DebitNoteDetailTaxModel> debitNoteDetailTaxModelList = null;

            // create query.
            IQueryable<Debitnotedetailtax> query = GetQueryByCondition(w => w.DebitNoteDetTaxId != 0)
                                                        .Include(w => w.TaxLedger);

            // apply filters.
            if (0 != debitNoteDetailTaxId)
                query = query.Where(w => w.DebitNoteDetTaxId == debitNoteDetailTaxId);

            // apply filters.
            if (0 != debitNoteDetailId)
                query = query.Where(w => w.DebitNoteDetId == debitNoteDetailId);

            // apply filters.
            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteDet.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnotedetailtax> debitNoteDetailTaxList = await query.ToListAsync();

            if (null != debitNoteDetailTaxList && debitNoteDetailTaxList.Count > 0)
            {
                debitNoteDetailTaxModelList = new List<DebitNoteDetailTaxModel>();

                foreach (Debitnotedetailtax debitNoteDetailTax in debitNoteDetailTaxList)
                {
                    debitNoteDetailTaxModelList.Add(await AssignValueToModel(debitNoteDetailTax));
                }
            }

            return debitNoteDetailTaxModelList; // returns.
        }

        private async Task<DebitNoteDetailTaxModel> AssignValueToModel(Debitnotedetailtax debitNoteDetailTax)
        {
            return await Task.Run(() =>
            {
                DebitNoteDetailTaxModel debitNoteDetailTaxModel = new DebitNoteDetailTaxModel();

                debitNoteDetailTaxModel.DebitNoteDetTaxId = debitNoteDetailTax.DebitNoteDetTaxId;
                debitNoteDetailTaxModel.DebitNoteDetId = debitNoteDetailTax.DebitNoteDetId;
                debitNoteDetailTaxModel.SrNo = debitNoteDetailTax.SrNo;
                debitNoteDetailTaxModel.TaxLedgerId = debitNoteDetailTax.TaxLedgerId;
                debitNoteDetailTaxModel.TaxPercentageOrAmount = debitNoteDetailTax.TaxPercentageOrAmount;
                debitNoteDetailTaxModel.TaxPerOrAmountFc = debitNoteDetailTax.TaxPerOrAmountFc;
                debitNoteDetailTaxModel.TaxAddOrDeduct = debitNoteDetailTax.TaxAddOrDeduct;
                debitNoteDetailTaxModel.TaxAmountFc = debitNoteDetailTax.TaxAmountFc;
                debitNoteDetailTaxModel.TaxAmount = debitNoteDetailTax.TaxAmount;
                debitNoteDetailTaxModel.Remark = debitNoteDetailTax.Remark;

                debitNoteDetailTaxModel.TaxLedgerName = null != debitNoteDetailTax.TaxLedger ? debitNoteDetailTax.TaxLedger.LedgerName : null;;

                return debitNoteDetailTaxModel;
            });
        }
    }
}

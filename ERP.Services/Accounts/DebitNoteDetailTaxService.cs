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
        private readonly IDebitNoteDetail _debitNoteDetail;
        private readonly ITaxRegisterDetail _taxRegisterDetail;


        public DebitNoteDetailTaxService(ErpDbContext dbContext, IDebitNoteDetail debitNoteDetail, ITaxRegisterDetail taxRegisterDetail) : base(dbContext)
        {
            _debitNoteDetail = debitNoteDetail;
            _taxRegisterDetail = taxRegisterDetail;
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

            // assign values.
            Debitnotedetailtax debitNoteDetailTax = new Debitnotedetailtax();

            debitNoteDetailTax.DebitNoteDetId = debitNoteDetailTaxModel.DebitNoteDetId;
            debitNoteDetailTax.SrNo = debitNoteDetailTaxModel.SrNo;
            debitNoteDetailTax.TaxLedgerId = debitNoteDetailTaxModel.TaxLedgerId;
            debitNoteDetailTax.TaxPercentageOrAmount = debitNoteDetailTaxModel.TaxPercentageOrAmount;
            debitNoteDetailTax.TaxPerOrAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc;
            debitNoteDetailTax.TaxAddOrDeduct = debitNoteDetailTaxModel.TaxAddOrDeduct;
            debitNoteDetailTax.TaxAmountFc = 0;
            debitNoteDetailTax.TaxAmount = 0;
            debitNoteDetailTax.Remark = debitNoteDetailTaxModel.Remark;

            await Create(debitNoteDetailTax);

            debitNoteDetailTaxId = debitNoteDetailTax.DebitNoteDetTaxId;

            if (debitNoteDetailTaxId != 0)
            {
                await UpdateDebitNoteDetailTaxAmount(debitNoteDetailTaxId);
            }

            return debitNoteDetailTaxId; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel)
        {
            bool isUpdated = false;

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
                debitNoteDetailTax.TaxPerOrAmountFc = debitNoteDetailTaxModel.TaxPerOrAmountFc; ;
                debitNoteDetailTax.TaxAddOrDeduct = debitNoteDetailTaxModel.TaxAddOrDeduct;
                debitNoteDetailTax.TaxAmountFc = 0;
                debitNoteDetailTax.TaxAmount = 0;
                debitNoteDetailTax.Remark = debitNoteDetailTaxModel.Remark;

                isUpdated = await Update(debitNoteDetailTax);
            }

            if (isUpdated != false)
            {
                await UpdateDebitNoteDetailTaxAmount(debitNoteDetailTaxModel.DebitNoteDetTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetailTaxAmount(int? debitNoteDetailTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Debitnotedetailtax debitNoteDetailTax = await GetQueryByCondition(w => w.DebitNoteDetTaxId == debitNoteDetailTaxId)
                                                                .Include(w => w.DebitNoteDet).ThenInclude(w => w.DebitNote).FirstOrDefaultAsync();

            if (null != _debitNoteDetail)
            {
                if (DiscountType.Percentage.ToString() == debitNoteDetailTax.TaxPercentageOrAmount)
                {
                    debitNoteDetailTax.TaxAmountFc = (debitNoteDetailTax.DebitNoteDet.GrossAmountFc * debitNoteDetailTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    debitNoteDetailTax.TaxAmountFc = debitNoteDetailTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == debitNoteDetailTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                debitNoteDetailTax.TaxAmountFc = multiplier * debitNoteDetailTax.TaxAmountFc;

                debitNoteDetailTax.TaxAmount = debitNoteDetailTax.TaxAmountFc / debitNoteDetailTax.DebitNoteDet.DebitNote.ExchangeRate;

                isUpdated = await Update(debitNoteDetailTax);
            }

            if (isUpdated != false)
            {
                await _debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailTax.DebitNoteDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetailTaxAmountOnDetailUpdate(int? debitNoteDetailId)
        {
            bool isUpdated = false;

            // get record.
            IList<Debitnotedetailtax> debitNoteDetailTaxList = await GetQueryByCondition(w => w.DebitNoteDetId == (int)debitNoteDetailId).ToListAsync();

            foreach (Debitnotedetailtax debitNoteDetailTax in debitNoteDetailTaxList)
            {
                isUpdated = await UpdateDebitNoteDetailTaxAmount(debitNoteDetailTax.DebitNoteDetTaxId);
            }

            if (isUpdated != false)
            {
                await _debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailId);
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
                await _debitNoteDetail.UpdateDebitNoteDetailAmount(debitNoteDetailTax.DebitNoteDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> AddDebitNoteDetailTaxByDebitNoteId(int debitNoteId, int taxRegisterId)
        {
            bool isUpdated = false;

            IList<DebitNoteDetailModel> debitNoteDetailModelList = await _debitNoteDetail.GetDebitNoteDetailListByDebitNoteId(debitNoteId);

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await _taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            DebitNoteDetailTaxModel debitNoteDetailTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    foreach (DebitNoteDetailModel debitNoteDetailModel in debitNoteDetailModelList)
                    {
                        debitNoteDetailTaxModel = new DebitNoteDetailTaxModel()
                        {
                            DebitNoteDetTaxId = 0,
                            DebitNoteDetId = debitNoteDetailModel.DebitNoteDetId,
                            SrNo = (int)taxRegisterDetailModel.SrNo,
                            TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                            TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                            TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                            TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                            TaxAmountFc = 0,
                            TaxAmount = 0,
                            Remark = ""
                        };

                        await CreateDebitNoteDetailTax(debitNoteDetailTaxModel);
                    }
                }
            }

            return isUpdated; // returns.
        }

        public async Task<bool> AddDebitNoteDetailTaxByDebitNoteDetId(int debitNoteDetId, int taxRegisterId)
        {
            bool isUpdated = false;

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await _taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            DebitNoteDetailTaxModel debitNoteDetailTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    debitNoteDetailTaxModel = new DebitNoteDetailTaxModel()
                    {
                        DebitNoteDetTaxId = 0,
                        DebitNoteDetId = debitNoteDetId,
                        SrNo = (int)taxRegisterDetailModel.SrNo,
                        TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                        TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                        TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                        TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                        TaxAmountFc = 0,
                        TaxAmount = 0,
                        Remark = ""
                    };

                    await CreateDebitNoteDetailTax(debitNoteDetailTaxModel);
                }
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteDebitNoteDetailTaxByDebitNoteId(int debitNoteId)
        {
            bool isDeleted = false;

            // get record.
            IList<Debitnotedetailtax> debitNoteDetailTaxList = await GetQueryByCondition(w => w.DebitNoteDetTaxId != 0).Include(W => W.DebitNoteDet)
                                                                                .Where(W => W.DebitNoteDet.DebitNoteId == debitNoteId)
                                                                                .ToListAsync();

            foreach (Debitnotedetailtax debitNoteDetailTax in debitNoteDetailTaxList)
            {
                isDeleted = await DeleteDebitNoteDetailTax(debitNoteDetailTax.DebitNoteDetTaxId);
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

                debitNoteDetailTaxModel.TaxLedgerName = null != debitNoteDetailTax.TaxLedger ? debitNoteDetailTax.TaxLedger.LedgerName : null; ;

                return debitNoteDetailTaxModel;
            });
        }
    }
}

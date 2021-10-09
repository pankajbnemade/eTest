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
    public class DebitNoteTaxService : Repository<Debitnotetax>, IDebitNoteTax
    {
        private readonly IDebitNote debitNote;
        private readonly ITaxRegisterDetail taxRegisterDetail;

        public DebitNoteTaxService(ErpDbContext dbContext, IDebitNote _debitNote, ITaxRegisterDetail _taxRegisterDetail) : base(dbContext)
        {
            debitNote = _debitNote;
            taxRegisterDetail = _taxRegisterDetail;
        }

        /// <summary>
        /// generate sr no based on debitNoteId
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int debitNoteId)
        {
            int srNo = 0;

            if (await Any(w => w.DebitNoteTaxId != 0 && w.DebitNoteId == debitNoteId))
            {
                srNo = await GetQueryByCondition(w => w.DebitNoteTaxId != 0 && w.DebitNoteId == debitNoteId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateDebitNoteTax(DebitNoteTaxModel debitNoteTaxModel)
        {
            int debitNoteTaxId = 0;

            // assign values.
            DebitNoteModel debitNoteModel = null;

            debitNoteModel = await debitNote.GetDebitNoteById((int)debitNoteTaxModel.DebitNoteId);

            Debitnotetax debitNoteTax = new Debitnotetax();

            debitNoteTax.DebitNoteId = debitNoteTaxModel.DebitNoteId;
            debitNoteTax.SrNo = debitNoteTaxModel.SrNo;
            debitNoteTax.TaxLedgerId = debitNoteTaxModel.TaxLedgerId;
            debitNoteTax.TaxPercentageOrAmount = debitNoteTaxModel.TaxPercentageOrAmount;
            debitNoteTax.TaxPerOrAmountFc =  debitNoteTaxModel.TaxPerOrAmountFc;
            debitNoteTax.TaxAddOrDeduct = debitNoteTaxModel.TaxAddOrDeduct;
            debitNoteTax.TaxAmountFc = 0;
            debitNoteTax.TaxAmount = 0;
            debitNoteTax.Remark = debitNoteTaxModel.Remark;

            await Create(debitNoteTax);
            debitNoteTaxId = debitNoteTax.DebitNoteTaxId;

            if (debitNoteTaxId != 0)
            {
                await UpdateDebitNoteTaxAmount(debitNoteTaxId); ;
            }

            return debitNoteTaxId; // returns.
        }

        public async Task<bool> UpdateDebitNoteTax(DebitNoteTaxModel debitNoteTaxModel)
        {
            bool isUpdated = false;

            // get record.
            Debitnotetax debitNoteTax = await GetQueryByCondition(w => w.DebitNoteTaxId == debitNoteTaxModel.DebitNoteTaxId)
                        .Include(w => w.DebitNote).FirstOrDefaultAsync();

            if (null != debitNoteTax)
            {
                // assign values.

                debitNoteTax.DebitNoteId = debitNoteTaxModel.DebitNoteId;
                debitNoteTax.SrNo = debitNoteTaxModel.SrNo;
                debitNoteTax.TaxLedgerId = debitNoteTaxModel.TaxLedgerId;
                debitNoteTax.TaxPercentageOrAmount = debitNoteTaxModel.TaxPercentageOrAmount;
                debitNoteTax.TaxPerOrAmountFc =  debitNoteTaxModel.TaxPerOrAmountFc;
                debitNoteTax.TaxAddOrDeduct = debitNoteTaxModel.TaxAddOrDeduct;
                debitNoteTax.TaxAmountFc = 0;
                debitNoteTax.TaxAmount = 0;
                debitNoteTax.Remark = debitNoteTaxModel.Remark;

                isUpdated = await Update(debitNoteTax);
            }

            if (isUpdated != false)
            {
                await UpdateDebitNoteTaxAmount(debitNoteTaxModel.DebitNoteTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateDebitNoteTaxAmount(int? debitNoteTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Debitnotetax debitNoteTax = await GetQueryByCondition(w => w.DebitNoteTaxId == debitNoteTaxId)
                                                                 .Include(w => w.DebitNote).FirstOrDefaultAsync();

            if (null != debitNote)
            {
                if (DiscountType.Percentage.ToString() == debitNoteTax.TaxPercentageOrAmount)
                {
                    debitNoteTax.TaxAmountFc = (debitNoteTax.DebitNote.GrossAmountFc * debitNoteTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    debitNoteTax.TaxAmountFc = debitNoteTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == debitNoteTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                debitNoteTax.TaxAmountFc = multiplier * debitNoteTax.TaxAmountFc;

                debitNoteTax.TaxAmount = debitNoteTax.TaxAmountFc / debitNoteTax.DebitNote.ExchangeRate;

                isUpdated = await Update(debitNoteTax);
            }

            if (isUpdated != false)
            {
                await debitNote.UpdateDebitNoteMasterAmount(debitNoteTax.DebitNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateDebitNoteTaxAmountAll(int? debitNoteId)
        {
            bool isUpdated = false;

            // get record.
            IList<Debitnotetax> debitNoteTaxList = await GetQueryByCondition(w => w.DebitNoteId == (int)debitNoteId).ToListAsync();

            foreach (Debitnotetax debitNoteTax in debitNoteTaxList)
            {
                isUpdated = await UpdateDebitNoteTaxAmount(debitNoteTax.DebitNoteTaxId);
            }

            if (isUpdated != false)
            {
                await debitNote.UpdateDebitNoteMasterAmount(debitNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteDebitNoteTax(int debitNoteTaxId)
        {
            bool isDeleted = false;

            // get record.
            Debitnotetax debitNoteTax = await GetByIdAsync(w => w.DebitNoteTaxId == debitNoteTaxId);

            if (null != debitNoteTax)
            {
                isDeleted = await Delete(debitNoteTax);
            }

            if (isDeleted != false)
            {
                await debitNote.UpdateDebitNoteMasterAmount(debitNoteTax.DebitNoteId);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> AddDebitNoteTaxByDebitNoteId(int debitNoteId, int taxRegisterId)
        {
            bool isUpdated = false;

            // get record.
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            DebitNoteTaxModel debitNoteTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    debitNoteTaxModel = new DebitNoteTaxModel()
                    {
                        DebitNoteTaxId = 0,
                        DebitNoteId = debitNoteId,
                        SrNo = (int)taxRegisterDetailModel.SrNo,
                        TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                        TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                        TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                        TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                        TaxAmountFc = 0,
                        TaxAmount = 0,
                        Remark = ""
                    };

                    await CreateDebitNoteTax(debitNoteTaxModel);
                }
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteDebitNoteTaxByDebitNoteId(int debitNoteId)
        {
            bool isDeleted = false;

            // get record.
            IList<Debitnotetax> debitNoteTaxList = await GetQueryByCondition(w => w.DebitNoteId == (int)debitNoteId).ToListAsync();

            foreach (Debitnotetax debitNoteTax in debitNoteTaxList)
            {
                isDeleted = await DeleteDebitNoteTax(debitNoteTax.DebitNoteTaxId);
            }

            return isDeleted; // returns.
        }

        public async Task<DebitNoteTaxModel> GetDebitNoteTaxById(int debitNoteTaxId)
        {
            DebitNoteTaxModel debitNoteTaxModel = null;

            IList<DebitNoteTaxModel> debitNoteTaxModelList = await GetDebitNoteTaxList(debitNoteTaxId, 0);

            if (null != debitNoteTaxModelList && debitNoteTaxModelList.Any())
            {
                debitNoteTaxModel = debitNoteTaxModelList.FirstOrDefault();
            }

            return debitNoteTaxModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteTaxModel>> GetDebitNoteTaxByDebitNoteId(int debitNoteId)
        {
            DataTableResultModel<DebitNoteTaxModel> resultModel = new DataTableResultModel<DebitNoteTaxModel>();

            IList<DebitNoteTaxModel> debitNoteTaxModelList = await GetDebitNoteTaxList(0, debitNoteId);

            if (null != debitNoteTaxModelList && debitNoteTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteTaxModel>();
                resultModel.ResultList = debitNoteTaxModelList;
                resultModel.TotalResultCount = debitNoteTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<DebitNoteTaxModel>();
                resultModel.ResultList = new List<DebitNoteTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteTaxModel>> GetDebitNoteTaxList()
        {
            DataTableResultModel<DebitNoteTaxModel> resultModel = new DataTableResultModel<DebitNoteTaxModel>();

            IList<DebitNoteTaxModel> debitNoteTaxModelList = await GetDebitNoteTaxList(0, 0);

            if (null != debitNoteTaxModelList && debitNoteTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteTaxModel>();
                resultModel.ResultList = debitNoteTaxModelList;
                resultModel.TotalResultCount = debitNoteTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<DebitNoteTaxModel>> GetDebitNoteTaxList(int debitNoteTaxId, int debitNoteId)
        {
            IList<DebitNoteTaxModel> debitNoteTaxModelList = null;

            // create query.
            IQueryable<Debitnotetax> query = GetQueryByCondition(w => w.DebitNoteTaxId != 0).Include(t => t.TaxLedger);

            // apply filters.
            if (0 != debitNoteTaxId)
                query = query.Where(w => w.DebitNoteTaxId == debitNoteTaxId);

            // apply filters.
            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnotetax> debitNoteTaxList = await query.ToListAsync();
            if (null != debitNoteTaxList && debitNoteTaxList.Count > 0)
            {
                debitNoteTaxModelList = new List<DebitNoteTaxModel>();

                foreach (Debitnotetax debitNoteTax in debitNoteTaxList)
                {
                    debitNoteTaxModelList.Add(await AssignValueToModel(debitNoteTax));
                }
            }

            return debitNoteTaxModelList; // returns.
        }

        private async Task<DebitNoteTaxModel> AssignValueToModel(Debitnotetax debitNoteTax)
        {
            return await Task.Run(() =>
            {
                DebitNoteTaxModel debitNoteTaxModel = new DebitNoteTaxModel();

                debitNoteTaxModel.DebitNoteTaxId = debitNoteTax.DebitNoteTaxId;
                debitNoteTaxModel.DebitNoteId = debitNoteTax.DebitNoteId;
                debitNoteTaxModel.SrNo = debitNoteTax.SrNo;
                debitNoteTaxModel.TaxLedgerId = debitNoteTax.TaxLedgerId;
                debitNoteTaxModel.TaxPercentageOrAmount = debitNoteTax.TaxPercentageOrAmount;
                debitNoteTaxModel.TaxPerOrAmountFc = debitNoteTax.TaxPerOrAmountFc;
                debitNoteTaxModel.TaxAddOrDeduct = debitNoteTax.TaxAddOrDeduct;
                debitNoteTaxModel.TaxAmountFc = debitNoteTax.TaxAmountFc;
                debitNoteTaxModel.TaxAmount = debitNoteTax.TaxAmount;
                debitNoteTaxModel.Remark = debitNoteTax.Remark;

                debitNoteTaxModel.TaxLedgerName = null != debitNoteTax.TaxLedger ? debitNoteTax.TaxLedger.LedgerName : null;

                return debitNoteTaxModel;
            });
        }
    }
}

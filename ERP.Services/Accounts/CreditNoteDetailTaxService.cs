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
    public class CreditNoteDetailTaxService : Repository<Creditnotedetailtax>, ICreditNoteDetailTax
    {
        ICreditNoteDetail creditNoteDetail;

        public CreditNoteDetailTaxService(ErpDbContext dbContext,  ICreditNoteDetail _creditNoteDetail) : base(dbContext)
        {
            creditNoteDetail = _creditNoteDetail;
        }
        
        /// <summary>
        /// generate sr no based on creditNoteDetId
        /// </summary>
        /// <param name="creditNoteDetId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int creditNoteDetId)
        {
            int srNo = 0;

            if (await Any(w => w.CreditNoteDetTaxId != 0 && w.CreditNoteDetId == creditNoteDetId))
            {
                srNo = await GetQueryByCondition(w => w.CreditNoteDetTaxId != 0 && w.CreditNoteDetId == creditNoteDetId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel)
        {
            int creditNoteDetailTaxId = 0;
            int multiplier = 1;

            CreditNoteDetailModel creditNoteDetailModel = null;
            creditNoteDetailModel = await creditNoteDetail.GetCreditNoteDetailById((Int32)creditNoteDetailTaxModel.CreditNoteDetId);

            // assign values.
            Creditnotedetailtax creditNoteDetailTax = new Creditnotedetailtax();

            creditNoteDetailTax.CreditNoteDetId = creditNoteDetailTaxModel.CreditNoteDetId;
            creditNoteDetailTax.SrNo = creditNoteDetailTaxModel.SrNo;
            creditNoteDetailTax.TaxLedgerId = creditNoteDetailTaxModel.TaxLedgerId;
            creditNoteDetailTax.TaxPercentageOrAmount = creditNoteDetailTaxModel.TaxPercentageOrAmount;
            creditNoteDetailTax.TaxPerOrAmountFc = creditNoteDetailTaxModel.TaxPerOrAmountFc;

            if (DiscountType.Percentage.ToString() == creditNoteDetailTaxModel.TaxPercentageOrAmount)
            {
                creditNoteDetailTaxModel.TaxAmountFc = (creditNoteDetailModel.GrossAmountFc * creditNoteDetailTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                creditNoteDetailTaxModel.TaxAmountFc = creditNoteDetailTaxModel.TaxPerOrAmountFc;
            }

            if (TaxAddOrDeduct.Deduct.ToString() == creditNoteDetailTaxModel.TaxAddOrDeduct)
            {
                multiplier = -1;
            }

            creditNoteDetailTax.TaxAddOrDeduct = creditNoteDetailTaxModel.TaxAddOrDeduct;
            creditNoteDetailTax.TaxAmountFc = multiplier * creditNoteDetailTaxModel.TaxAmountFc;
            creditNoteDetailTax.TaxAmount = multiplier * creditNoteDetailTaxModel.TaxAmount;
            creditNoteDetailTax.Remark = creditNoteDetailTaxModel.Remark;

            await Create(creditNoteDetailTax);

            creditNoteDetailTaxId = creditNoteDetailTax.CreditNoteDetTaxId;


            if (creditNoteDetailTaxId != 0)
            {
                await creditNoteDetail.UpdateCreditNoteDetailAmount(creditNoteDetailTax.CreditNoteDetId);
            }

            return creditNoteDetailTaxId; // returns.
        }

        public async Task<bool> UpdateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Creditnotedetailtax creditNoteDetailTax = await GetQueryByCondition(w => w.CreditNoteDetTaxId == creditNoteDetailTaxModel.CreditNoteDetTaxId)
                                                                .Include(w => w.CreditNoteDet).FirstOrDefaultAsync();

            if (null != creditNoteDetailTax)
            {
                // assign values.
                creditNoteDetailTax.CreditNoteDetId = creditNoteDetailTaxModel.CreditNoteDetId;
                creditNoteDetailTax.SrNo = creditNoteDetailTaxModel.SrNo;
                creditNoteDetailTax.TaxLedgerId = creditNoteDetailTaxModel.TaxLedgerId;
                creditNoteDetailTax.TaxPercentageOrAmount = creditNoteDetailTaxModel.TaxPercentageOrAmount;
                creditNoteDetailTax.TaxPerOrAmountFc = creditNoteDetailTaxModel.TaxPerOrAmountFc;

                if (DiscountType.Percentage.ToString() == creditNoteDetailTaxModel.TaxPercentageOrAmount)
                {
                    creditNoteDetailTaxModel.TaxAmountFc = (creditNoteDetailTax.CreditNoteDet.GrossAmountFc * creditNoteDetailTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    creditNoteDetailTaxModel.TaxAmountFc = creditNoteDetailTaxModel.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == creditNoteDetailTaxModel.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                creditNoteDetailTax.TaxAddOrDeduct = creditNoteDetailTaxModel.TaxAddOrDeduct;
                creditNoteDetailTax.TaxAmountFc = multiplier * creditNoteDetailTaxModel.TaxAmountFc;
                creditNoteDetailTax.TaxAmount = multiplier * creditNoteDetailTaxModel.TaxAmount;
                creditNoteDetailTax.Remark = creditNoteDetailTaxModel.Remark;

                isUpdated = await Update(creditNoteDetailTax);
            }

            if (isUpdated != false)
            {
                await creditNoteDetail.UpdateCreditNoteDetailAmount(creditNoteDetailTax.CreditNoteDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteCreditNoteDetailTax(int creditNoteDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Creditnotedetailtax creditNoteDetailTax = await GetByIdAsync(w => w.CreditNoteDetTaxId == creditNoteDetailTaxId);

            if (null != creditNoteDetailTax)
            {
                isDeleted = await Delete(creditNoteDetailTax);
            }

            if (isDeleted != false)
            {
                await creditNoteDetail.UpdateCreditNoteDetailAmount(creditNoteDetailTax.CreditNoteDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<CreditNoteDetailTaxModel> GetCreditNoteDetailTaxById(int creditNoteDetailTaxId)
        {
            CreditNoteDetailTaxModel creditNoteDetailTaxModel = null;

            IList<CreditNoteDetailTaxModel> creditNoteDetailTaxModelList = await GetCreditNoteDetailTaxList(creditNoteDetailTaxId, 0, 0);

            if (null != creditNoteDetailTaxModelList && creditNoteDetailTaxModelList.Any())
            {
                creditNoteDetailTaxModel = creditNoteDetailTaxModelList.FirstOrDefault();
            }

            return creditNoteDetailTaxModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxByCreditNoteDetailId(int creditNoteDetailId)
        {
            DataTableResultModel<CreditNoteDetailTaxModel> resultModel = new DataTableResultModel<CreditNoteDetailTaxModel>();

            IList<CreditNoteDetailTaxModel> creditNoteDetailTaxModelList = await GetCreditNoteDetailTaxList(0, creditNoteDetailId, 0);
            if (null != creditNoteDetailTaxModelList && creditNoteDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteDetailTaxModel>();
                resultModel.ResultList = creditNoteDetailTaxModelList;
                resultModel.TotalResultCount = creditNoteDetailTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<CreditNoteDetailTaxModel>();
                resultModel.ResultList = new List<CreditNoteDetailTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxList()
        {
            DataTableResultModel<CreditNoteDetailTaxModel> resultModel = new DataTableResultModel<CreditNoteDetailTaxModel>();

            IList<CreditNoteDetailTaxModel> creditNoteDetailTaxModelList = await GetCreditNoteDetailTaxList(0, 0, 0);

            if (null != creditNoteDetailTaxModelList && creditNoteDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteDetailTaxModel>();
                resultModel.ResultList = creditNoteDetailTaxModelList;
                resultModel.TotalResultCount = creditNoteDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxList(int creditNoteDetailTaxId, int creditNoteDetailId, int creditNoteId)
        {
            IList<CreditNoteDetailTaxModel> creditNoteDetailTaxModelList = null;

            // create query.
            IQueryable<Creditnotedetailtax> query = GetQueryByCondition(w => w.CreditNoteDetTaxId != 0)
                                                        .Include(w => w.TaxLedger);

            // apply filters.
            if (0 != creditNoteDetailTaxId)
                query = query.Where(w => w.CreditNoteDetTaxId == creditNoteDetailTaxId);

            // apply filters.
            if (0 != creditNoteDetailId)
                query = query.Where(w => w.CreditNoteDetId == creditNoteDetailId);

            // apply filters.
            if (0 != creditNoteId)
                query = query.Where(w => w.CreditNoteDet.CreditNoteId == creditNoteId);

            // get records by query.
            List<Creditnotedetailtax> creditNoteDetailTaxList = await query.ToListAsync();

            if (null != creditNoteDetailTaxList && creditNoteDetailTaxList.Count > 0)
            {
                creditNoteDetailTaxModelList = new List<CreditNoteDetailTaxModel>();

                foreach (Creditnotedetailtax creditNoteDetailTax in creditNoteDetailTaxList)
                {
                    creditNoteDetailTaxModelList.Add(await AssignValueToModel(creditNoteDetailTax));
                }
            }

            return creditNoteDetailTaxModelList; // returns.
        }

        private async Task<CreditNoteDetailTaxModel> AssignValueToModel(Creditnotedetailtax creditNoteDetailTax)
        {
            return await Task.Run(() =>
            {
                CreditNoteDetailTaxModel creditNoteDetailTaxModel = new CreditNoteDetailTaxModel();

                creditNoteDetailTaxModel.CreditNoteDetTaxId = creditNoteDetailTax.CreditNoteDetTaxId;
                creditNoteDetailTaxModel.CreditNoteDetId = creditNoteDetailTax.CreditNoteDetId;
                creditNoteDetailTaxModel.SrNo = creditNoteDetailTax.SrNo;
                creditNoteDetailTaxModel.TaxLedgerId = creditNoteDetailTax.TaxLedgerId;
                creditNoteDetailTaxModel.TaxPercentageOrAmount = creditNoteDetailTax.TaxPercentageOrAmount;
                creditNoteDetailTaxModel.TaxPerOrAmountFc = creditNoteDetailTax.TaxPerOrAmountFc;
                creditNoteDetailTaxModel.TaxAddOrDeduct = creditNoteDetailTax.TaxAddOrDeduct;
                creditNoteDetailTaxModel.TaxAmountFc = creditNoteDetailTax.TaxAmountFc;
                creditNoteDetailTaxModel.TaxAmount = creditNoteDetailTax.TaxAmount;
                creditNoteDetailTaxModel.Remark = creditNoteDetailTax.Remark;

                creditNoteDetailTaxModel.TaxLedgerName = null != creditNoteDetailTax.TaxLedger ? creditNoteDetailTax.TaxLedger.LedgerName : null;;

                return creditNoteDetailTaxModel;
            });
        }
    }
}

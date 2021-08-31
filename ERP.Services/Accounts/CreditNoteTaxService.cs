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
    public class CreditNoteTaxService : Repository<Creditnotetax>, ICreditNoteTax
    {
        private readonly ICreditNote creditNote;

        public CreditNoteTaxService(ErpDbContext dbContext, ICreditNote _creditNote) : base(dbContext)
        {
            creditNote = _creditNote;
        }

        /// <summary>
        /// generate sr no based on creditNoteId
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int creditNoteId)
        {
            int srNo = 0;

            if (await Any(w => w.CreditNoteTaxId != 0 && w.CreditNoteId == creditNoteId))
            {
                srNo = await GetQueryByCondition(w => w.CreditNoteTaxId != 0 && w.CreditNoteId == creditNoteId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateCreditNoteTax(CreditNoteTaxModel creditNoteTaxModel)
        {
            int creditNoteTaxId = 0;
            int multiplier = 1;

            // assign values.
            CreditNoteModel creditNoteModel = null;

            creditNoteModel = await creditNote.GetCreditNoteById((int)creditNoteTaxModel.CreditNoteId);

            Creditnotetax creditNoteTax = new Creditnotetax();
            creditNoteTax.CreditNoteId = creditNoteTaxModel.CreditNoteId;
            creditNoteTax.SrNo = creditNoteTaxModel.SrNo;
            creditNoteTax.TaxLedgerId = creditNoteTaxModel.TaxLedgerId;
            creditNoteTax.TaxPercentageOrAmount = creditNoteTaxModel.TaxPercentageOrAmount;
            creditNoteTax.TaxPerOrAmountFc = creditNoteTaxModel.TaxPerOrAmountFc;

            if (DiscountType.Percentage.ToString() == creditNoteTax.TaxPercentageOrAmount)
            {
                creditNoteTaxModel.TaxAmountFc = (creditNoteModel.GrossAmountFc * creditNoteTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                creditNoteTaxModel.TaxAmountFc = creditNoteTaxModel.TaxPerOrAmountFc;
            }

            if (TaxAddOrDeduct.Deduct.ToString() == creditNoteTaxModel.TaxAddOrDeduct)
            {
                multiplier = -1;
            }

            creditNoteTax.TaxAddOrDeduct = creditNoteTaxModel.TaxAddOrDeduct;
            creditNoteTax.TaxAmountFc = multiplier * creditNoteTaxModel.TaxAmountFc;
            creditNoteTax.TaxAmount = multiplier * creditNoteTaxModel.TaxAmount;
            creditNoteTax.Remark = creditNoteTaxModel.Remark;

            await Create(creditNoteTax);
            creditNoteTaxId = creditNoteTax.CreditNoteTaxId;

            if (creditNoteTaxId != 0)
            {
                await creditNote.UpdateCreditNoteMasterAmount(creditNoteTaxId);
            }

            return creditNoteTaxId; // returns.
        }

        public async Task<bool> UpdateCreditNoteTax(CreditNoteTaxModel creditNoteTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Creditnotetax creditNoteTax = await GetQueryByCondition(w => w.CreditNoteTaxId == creditNoteTaxModel.CreditNoteTaxId)
                        .Include(w => w.CreditNote).FirstOrDefaultAsync();

            if (null != creditNoteTax)
            {
                // assign values.

                creditNoteTax.CreditNoteId = creditNoteTaxModel.CreditNoteId;
                creditNoteTax.SrNo = creditNoteTaxModel.SrNo;
                creditNoteTax.TaxLedgerId = creditNoteTaxModel.TaxLedgerId;
                creditNoteTax.TaxPercentageOrAmount = creditNoteTaxModel.TaxPercentageOrAmount;
                creditNoteTax.TaxPerOrAmountFc = creditNoteTaxModel.TaxPerOrAmountFc;

                if (DiscountType.Percentage.ToString() == creditNoteTax.TaxPercentageOrAmount)
                {
                    creditNoteTaxModel.TaxAmountFc = (creditNoteTax.CreditNote.GrossAmountFc * creditNoteTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    creditNoteTaxModel.TaxAmountFc = creditNoteTaxModel.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == creditNoteTaxModel.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                creditNoteTax.TaxAddOrDeduct = creditNoteTaxModel.TaxAddOrDeduct;
                creditNoteTax.TaxAmountFc = multiplier * creditNoteTaxModel.TaxAmountFc;
                creditNoteTax.TaxAmount = multiplier * creditNoteTaxModel.TaxAmount;
                creditNoteTax.Remark = creditNoteTaxModel.Remark;

                isUpdated = await Update(creditNoteTax);
            }

            if (isUpdated != false)
            {
                await creditNote.UpdateCreditNoteMasterAmount(creditNoteTax.CreditNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteCreditNoteTax(int creditNoteTaxId)
        {
            bool isDeleted = false;

            // get record.
            Creditnotetax creditNoteTax = await GetByIdAsync(w => w.CreditNoteTaxId == creditNoteTaxId);

            if (null != creditNoteTax)
            {
                isDeleted = await Delete(creditNoteTax);
            }

            if (isDeleted != false)
            {
                await creditNote.UpdateCreditNoteMasterAmount(creditNoteTax.CreditNoteId);
            }

            return isDeleted; // returns.
        }

        public async Task<CreditNoteTaxModel> GetCreditNoteTaxById(int creditNoteTaxId)
        {
            CreditNoteTaxModel creditNoteTaxModel = null;

            IList<CreditNoteTaxModel> creditNoteTaxModelList = await GetCreditNoteTaxList(creditNoteTaxId, 0);

            if (null != creditNoteTaxModelList && creditNoteTaxModelList.Any())
            {
                creditNoteTaxModel = creditNoteTaxModelList.FirstOrDefault();
            }

            return creditNoteTaxModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteTaxModel>> GetCreditNoteTaxByCreditNoteId(int creditNoteId)
        {
            DataTableResultModel<CreditNoteTaxModel> resultModel = new DataTableResultModel<CreditNoteTaxModel>();

            IList<CreditNoteTaxModel> creditNoteTaxModelList = await GetCreditNoteTaxList(0, creditNoteId);

            if (null != creditNoteTaxModelList && creditNoteTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteTaxModel>();
                resultModel.ResultList = creditNoteTaxModelList;
                resultModel.TotalResultCount = creditNoteTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<CreditNoteTaxModel>();
                resultModel.ResultList = new List<CreditNoteTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteTaxModel>> GetCreditNoteTaxList()
        {
            DataTableResultModel<CreditNoteTaxModel> resultModel = new DataTableResultModel<CreditNoteTaxModel>();

            IList<CreditNoteTaxModel> creditNoteTaxModelList = await GetCreditNoteTaxList(0, 0);

            if (null != creditNoteTaxModelList && creditNoteTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteTaxModel>();
                resultModel.ResultList = creditNoteTaxModelList;
                resultModel.TotalResultCount = creditNoteTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CreditNoteTaxModel>> GetCreditNoteTaxList(int creditNoteTaxId, int creditNoteId)
        {
            IList<CreditNoteTaxModel> creditNoteTaxModelList = null;

            // create query.
            IQueryable<Creditnotetax> query = GetQueryByCondition(w => w.CreditNoteTaxId != 0).Include(t => t.TaxLedger);

            // apply filters.
            if (0 != creditNoteTaxId)
                query = query.Where(w => w.CreditNoteTaxId == creditNoteTaxId);

            // apply filters.
            if (0 != creditNoteId)
                query = query.Where(w => w.CreditNoteId == creditNoteId);

            // get records by query.
            List<Creditnotetax> creditNoteTaxList = await query.ToListAsync();
            if (null != creditNoteTaxList && creditNoteTaxList.Count > 0)
            {
                creditNoteTaxModelList = new List<CreditNoteTaxModel>();

                foreach (Creditnotetax creditNoteTax in creditNoteTaxList)
                {
                    creditNoteTaxModelList.Add(await AssignValueToModel(creditNoteTax));
                }
            }

            return creditNoteTaxModelList; // returns.
        }

        private async Task<CreditNoteTaxModel> AssignValueToModel(Creditnotetax creditNoteTax)
        {
            return await Task.Run(() =>
            {
                CreditNoteTaxModel creditNoteTaxModel = new CreditNoteTaxModel();

                creditNoteTaxModel.CreditNoteTaxId = creditNoteTax.CreditNoteTaxId;
                creditNoteTaxModel.CreditNoteId = creditNoteTax.CreditNoteId;
                creditNoteTaxModel.SrNo = creditNoteTax.SrNo;
                creditNoteTaxModel.TaxLedgerId = creditNoteTax.TaxLedgerId;
                creditNoteTaxModel.TaxPercentageOrAmount = creditNoteTax.TaxPercentageOrAmount;
                creditNoteTaxModel.TaxPerOrAmountFc = creditNoteTax.TaxPerOrAmountFc;
                creditNoteTaxModel.TaxAddOrDeduct = creditNoteTax.TaxAddOrDeduct;
                creditNoteTaxModel.TaxAmountFc = creditNoteTax.TaxAmountFc;
                creditNoteTaxModel.TaxAmount = creditNoteTax.TaxAmount;
                creditNoteTaxModel.Remark = creditNoteTax.Remark;

                creditNoteTaxModel.TaxLedgerName = null != creditNoteTax.TaxLedger ? creditNoteTax.TaxLedger.LedgerName : null;

                return creditNoteTaxModel;
            });
        }
    }
}

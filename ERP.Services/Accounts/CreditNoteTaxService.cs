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
        private readonly ITaxRegisterDetail taxRegisterDetail;

        public CreditNoteTaxService(ErpDbContext dbContext, ICreditNote _creditNote, ITaxRegisterDetail _taxRegisterDetail) : base(dbContext)
        {
            creditNote = _creditNote;
            taxRegisterDetail = _taxRegisterDetail;
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

            // assign values.
            CreditNoteModel creditNoteModel = null;

            creditNoteModel = await creditNote.GetCreditNoteById((int)creditNoteTaxModel.CreditNoteId);

            Creditnotetax creditNoteTax = new Creditnotetax();

            creditNoteTax.CreditNoteId = creditNoteTaxModel.CreditNoteId;
            creditNoteTax.SrNo = creditNoteTaxModel.SrNo;
            creditNoteTax.TaxLedgerId = creditNoteTaxModel.TaxLedgerId;
            creditNoteTax.TaxPercentageOrAmount = creditNoteTaxModel.TaxPercentageOrAmount;
            creditNoteTax.TaxPerOrAmountFc =  creditNoteTaxModel.TaxPerOrAmountFc;
            creditNoteTax.TaxAddOrDeduct = creditNoteTaxModel.TaxAddOrDeduct;
            creditNoteTax.TaxAmountFc = 0;
            creditNoteTax.TaxAmount = 0;
            creditNoteTax.Remark = creditNoteTaxModel.Remark;

            await Create(creditNoteTax);
            creditNoteTaxId = creditNoteTax.CreditNoteTaxId;

            if (creditNoteTaxId != 0)
            {
                await UpdateCreditNoteTaxAmount(creditNoteTaxId); ;
            }

            return creditNoteTaxId; // returns.
        }

        public async Task<bool> UpdateCreditNoteTax(CreditNoteTaxModel creditNoteTaxModel)
        {
            bool isUpdated = false;

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
                creditNoteTax.TaxPerOrAmountFc =  creditNoteTaxModel.TaxPerOrAmountFc;
                creditNoteTax.TaxAddOrDeduct = creditNoteTaxModel.TaxAddOrDeduct;
                creditNoteTax.TaxAmountFc = 0;
                creditNoteTax.TaxAmount = 0;
                creditNoteTax.Remark = creditNoteTaxModel.Remark;

                isUpdated = await Update(creditNoteTax);
            }

            if (isUpdated != false)
            {
                await UpdateCreditNoteTaxAmount(creditNoteTaxModel.CreditNoteTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateCreditNoteTaxAmount(int? creditNoteTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Creditnotetax creditNoteTax = await GetQueryByCondition(w => w.CreditNoteTaxId == creditNoteTaxId)
                                                                 .Include(w => w.CreditNote).FirstOrDefaultAsync();

            if (null != creditNote)
            {
                if (DiscountType.Percentage.ToString() == creditNoteTax.TaxPercentageOrAmount)
                {
                    creditNoteTax.TaxAmountFc = (creditNoteTax.CreditNote.GrossAmountFc * creditNoteTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    creditNoteTax.TaxAmountFc = creditNoteTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == creditNoteTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                creditNoteTax.TaxAmountFc = multiplier * creditNoteTax.TaxAmountFc;

                creditNoteTax.TaxAmount = creditNoteTax.TaxAmountFc / creditNoteTax.CreditNote.ExchangeRate;

                isUpdated = await Update(creditNoteTax);
            }

            if (isUpdated != false)
            {
                await creditNote.UpdateCreditNoteMasterAmount(creditNoteTax.CreditNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateCreditNoteTaxAmountAll(int? creditNoteId)
        {
            bool isUpdated = false;

            // get record.
            IList<Creditnotetax> creditNoteTaxList = await GetQueryByCondition(w => w.CreditNoteId == (int)creditNoteId).ToListAsync();

            foreach (Creditnotetax creditNoteTax in creditNoteTaxList)
            {
                isUpdated = await UpdateCreditNoteTaxAmount(creditNoteTax.CreditNoteTaxId);
            }

            if (isUpdated != false)
            {
                await creditNote.UpdateCreditNoteMasterAmount(creditNoteId);
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

        public async Task<bool> AddCreditNoteTaxByCreditNoteId(int creditNoteId, int taxRegisterId)
        {
            bool isUpdated = false;

            // get record.
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            CreditNoteTaxModel creditNoteTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    creditNoteTaxModel = new CreditNoteTaxModel()
                    {
                        CreditNoteTaxId = 0,
                        CreditNoteId = creditNoteId,
                        SrNo = (int)taxRegisterDetailModel.SrNo,
                        TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                        TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                        TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                        TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                        TaxAmountFc = 0,
                        TaxAmount = 0,
                        Remark = ""
                    };

                    await CreateCreditNoteTax(creditNoteTaxModel);
                }
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteCreditNoteTaxByCreditNoteId(int creditNoteId)
        {
            bool isDeleted = false;

            // get record.
            IList<Creditnotetax> creditNoteTaxList = await GetQueryByCondition(w => w.CreditNoteId == (int)creditNoteId).ToListAsync();

            foreach (Creditnotetax creditNoteTax in creditNoteTaxList)
            {
                isDeleted = await DeleteCreditNoteTax(creditNoteTax.CreditNoteTaxId);
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

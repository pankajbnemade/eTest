using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace ERP.Services.Accounts
{
    public class CreditNoteService : Repository<Creditnote>, ICreditNote
    {
        ICommon common;
        public CreditNoteService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        public async Task<GenerateNoModel> GenerateCreditNoteNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 4;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => m.MaxNo);

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo(Convert.ToInt32(maxNo), voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase invoice.
        /// </summary>
        /// <param name="creditNoteModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateCreditNote(CreditNoteModel creditNoteModel)
        {
            int creditNoteId = 0;

            GenerateNoModel generateNoModel = await GenerateCreditNoteNo(creditNoteModel.CompanyId, creditNoteModel.FinancialYearId);

            // assign values.
            Creditnote creditNote = new Creditnote();

            creditNote.CreditNoteNo = generateNoModel.VoucherNo;
            creditNote.MaxNo = generateNoModel.MaxNo;
            creditNote.VoucherStyleId = generateNoModel.VoucherStyleId;

            creditNote.CreditNoteDate = creditNoteModel.CreditNoteDate;
            creditNote.PartyLedgerId = creditNoteModel.PartyLedgerId;
            creditNote.BillToAddressId = creditNoteModel.BillToAddressId;
            creditNote.AccountLedgerId = creditNoteModel.AccountLedgerId;
            creditNote.PartyReferenceNo = creditNoteModel.PartyReferenceNo;
            creditNote.PartyReferenceDate = creditNoteModel.PartyReferenceDate;
            creditNote.OurReferenceNo = creditNoteModel.OurReferenceNo;
            creditNote.OurReferenceDate = creditNoteModel.OurReferenceDate;
            creditNote.CreditLimitDays = creditNoteModel.CreditLimitDays;
            creditNote.PaymentTerm = creditNoteModel.PaymentTerm;
            creditNote.Remark = creditNoteModel.Remark;
            creditNote.TaxModelType = creditNoteModel.TaxModelType;
            creditNote.TaxRegisterId = creditNoteModel.TaxRegisterId;
            creditNote.CurrencyId = creditNoteModel.CurrencyId;
            creditNote.ExchangeRate = creditNoteModel.ExchangeRate;
            creditNote.TotalLineItemAmountFc = 0;
            creditNote.TotalLineItemAmount = 0;
            creditNote.GrossAmountFc = 0;
            creditNote.GrossAmount = 0;
            creditNote.NetAmountFc = 0;
            creditNote.NetAmount = 0;
            creditNote.NetAmountFcinWord = "";
            creditNote.TaxAmountFc = 0;
            creditNote.TaxAmount = 0;

            creditNote.DiscountPercentageOrAmount = creditNoteModel.DiscountPercentageOrAmount;
            creditNote.DiscountPerOrAmountFc = creditNoteModel.DiscountPerOrAmountFc;
            creditNote.DiscountAmountFc = 0;
            creditNote.DiscountAmount = 0;

            creditNote.StatusId = 1;
            creditNote.CompanyId = creditNoteModel.CompanyId;
            creditNote.FinancialYearId = creditNoteModel.FinancialYearId;

            await Create(creditNote);
            creditNoteId = creditNote.CreditNoteId;

            if (creditNoteId != 0)
            {
                await UpdateCreditNoteMasterAmount(creditNoteId);
            }

            return creditNoteId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="creditNoteModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateCreditNote(CreditNoteModel creditNoteModel)
        {
            bool isUpdated = false;

            // get record.
            Creditnote creditNote = await GetByIdAsync(w => w.CreditNoteId == creditNoteModel.CreditNoteId);

            if (null != creditNote)
            {
                // assign values.

                creditNote.CreditNoteDate = creditNoteModel.CreditNoteDate;
                creditNote.PartyLedgerId = creditNoteModel.PartyLedgerId;
                creditNote.BillToAddressId = creditNoteModel.BillToAddressId;
                creditNote.AccountLedgerId = creditNoteModel.AccountLedgerId;
                creditNote.PartyReferenceNo = creditNoteModel.PartyReferenceNo;
                creditNote.PartyReferenceDate = creditNoteModel.PartyReferenceDate;
                creditNote.OurReferenceNo = creditNoteModel.OurReferenceNo;
                creditNote.OurReferenceDate = creditNoteModel.OurReferenceDate;
                creditNote.CreditLimitDays = creditNoteModel.CreditLimitDays;
                creditNote.PaymentTerm = creditNoteModel.PaymentTerm;
                creditNote.Remark = creditNoteModel.Remark;
                creditNote.TaxModelType = creditNoteModel.TaxModelType;
                creditNote.TaxRegisterId = creditNoteModel.TaxRegisterId;
                creditNote.CurrencyId = creditNoteModel.CurrencyId;
                creditNote.ExchangeRate = creditNoteModel.ExchangeRate;

                creditNote.TotalLineItemAmountFc = 0;
                creditNote.TotalLineItemAmount = 0;
                creditNote.GrossAmountFc = 0;
                creditNote.GrossAmount = 0;
                creditNote.NetAmountFc = 0;
                creditNote.NetAmount = 0;
                creditNote.NetAmountFcinWord = "";
                creditNote.TaxAmountFc = 0;
                creditNote.TaxAmount = 0;

                creditNote.DiscountPercentageOrAmount = creditNoteModel.DiscountPercentageOrAmount;
                creditNote.DiscountPerOrAmountFc = creditNoteModel.DiscountPerOrAmountFc;

                creditNote.DiscountAmountFc = 0;
                creditNote.DiscountAmount = 0;

                isUpdated = await Update(creditNote);
            }

            if (isUpdated != false)
            {
                await UpdateCreditNoteMasterAmount(creditNote.CreditNoteId);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="salesCreditNoteId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteCreditNote(int salesCreditNoteId)
        {
            bool isDeleted = false;

            // get record.
            Creditnote creditNote = await GetByIdAsync(w => w.CreditNoteId == salesCreditNoteId);
            if (null != creditNote)
            {
                isDeleted = await Delete(creditNote);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateCreditNoteMasterAmount(int? creditNoteId)
        {
            bool isUpdated = false;

            // get record.
            Creditnote creditNote = await GetQueryByCondition(w => w.CreditNoteId == creditNoteId)
                                                    .Include(w => w.Creditnotedetails).Include(w => w.Creditnotetaxes)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != creditNote)
            {
                creditNote.TotalLineItemAmountFc = creditNote.Creditnotedetails.Sum(w => w.GrossAmountFc);
                creditNote.TotalLineItemAmount = creditNote.TotalLineItemAmountFc * creditNote.ExchangeRate;

                if (DiscountType.Percentage.ToString() == creditNote.DiscountPercentageOrAmount)
                {
                    creditNote.DiscountAmountFc = (creditNote.TotalLineItemAmountFc * creditNote.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    creditNote.DiscountAmountFc = creditNote.DiscountPerOrAmountFc;
                }

                creditNote.DiscountAmount = creditNote.DiscountAmountFc * creditNote.ExchangeRate;

                creditNote.GrossAmountFc = creditNote.TotalLineItemAmountFc + creditNote.DiscountAmountFc;
                creditNote.GrossAmount = creditNote.GrossAmountFc * creditNote.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == creditNote.TaxModelType)
                {
                    creditNote.TaxAmountFc = creditNote.Creditnotedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    creditNote.TaxAmountFc = creditNote.Creditnotetaxes.Sum(w => w.TaxAmountFc);
                }

                creditNote.TaxAmount = creditNote.TaxAmountFc * creditNote.ExchangeRate;

                creditNote.NetAmountFc = creditNote.GrossAmountFc + creditNote.DiscountAmountFc;
                creditNote.NetAmount = creditNote.NetAmountFc * creditNote.ExchangeRate;

                creditNote.NetAmountFcinWord = await common.AmountInWord_Million(creditNote.NetAmountFc.ToString(), creditNote.Currency.CurrencyCode, creditNote.Currency.Denomination);

                isUpdated = await Update(creditNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on salesCreditNoteId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<CreditNoteModel> GetCreditNoteById(int creditNoteId)
        {
            CreditNoteModel creditNoteModel = null;

            IList<CreditNoteModel> creditNoteModelList = await GetCreditNoteList(creditNoteId);

            if (null != creditNoteModelList && creditNoteModelList.Any())
            {
                creditNoteModel = creditNoteModelList.FirstOrDefault();
            }

            return creditNoteModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<CreditNoteModel>> GetCreditNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterCreditNoteModel searchFilterModel)
        {
            string searchBy = dataTableAjaxPostModel.search?.value;
            int take = dataTableAjaxPostModel.length;
            int skip = dataTableAjaxPostModel.start;

            string sortBy = string.Empty;
            string sortDir = string.Empty;

            if (dataTableAjaxPostModel.order != null)
            {
                sortBy = dataTableAjaxPostModel.columns[dataTableAjaxPostModel.order[0].column].data;
                sortDir = dataTableAjaxPostModel.order[0].dir.ToLower();
            }

            // search the dbase taking into consideration table sorting and paging
            DataTableResultModel<CreditNoteModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        /// <summary>
        /// get credit note List based on partyLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetCreditNoteListByPartyLedgerId(int partyLedgerId)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Creditnote> query = GetQueryByCondition(w => w.CreditNoteId != 0);

            // apply filters.
            if (0 != partyLedgerId)
                query = query.Where(w => w.PartyLedgerId == partyLedgerId);

            // get records by query.
            List<Creditnote> creditNoteList = await query.ToListAsync();

             outstandingInvoiceModelList = new List<OutstandingInvoiceModel>();

            if (null != creditNoteList && creditNoteList.Count > 0)
            {

                foreach (Creditnote creditNote in creditNoteList)
                {
                    outstandingInvoiceModelList.Add(new OutstandingInvoiceModel()
                    {
                        InvoiceId = creditNote.CreditNoteId,
                        InvoiceType = "Credit Note",
                        InvoiceNo = creditNote.CreditNoteNo,
                        InvoiceDate = creditNote.CreditNoteDate,
                        InvoiceAmount = creditNote.NetAmount,
                        OutstandingAmount = creditNote.NetAmount,
                        CreditNoteId = creditNote.CreditNoteId,
                         SalesInvoiceId = 0,
                        PurchaseInvoiceId = 0,
                        DebitNoteId = 0
                    });
                }
            }

            return outstandingInvoiceModelList; // returns.
        }



        #region Private Methods

        /// <summary>
        /// get records from database.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        private async Task<DataTableResultModel<CreditNoteModel>> GetDataFromDbase(SearchFilterCreditNoteModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<CreditNoteModel> resultModel = new DataTableResultModel<CreditNoteModel>();

            IQueryable<Creditnote> query = GetQueryByCondition(w => w.CreditNoteId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.CreditNoteNo))
            {
                query = query.Where(w => w.CreditNoteNo.Contains(searchFilterModel.CreditNoteNo));
            }

            if (null != searchFilterModel.PartyLedgerId)
            {
                query = query.Where(w => w.PartyLedgerId == searchFilterModel.PartyLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.CreditNoteDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.CreditNoteDate <= searchFilterModel.ToDate);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            //sorting
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDir))
            {
                query = query.OrderBy($"{sortBy} {sortDir}");
            }

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.CreditNoteNo.ToLower().Contains(searchBy.ToLower()));
            }


            // get records based on pagesize.
            query = query.Skip(skip).Take(take);
            resultModel.ResultList = await query.Select(s => new CreditNoteModel
            {
                CreditNoteId = s.CreditNoteId,
                CreditNoteNo = s.CreditNoteNo,
                CreditNoteDate = s.CreditNoteDate,
                NetAmount = s.NetAmount,
            }).ToListAsync();
            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<CreditNoteModel>> GetCreditNoteList(int creditNoteId)
        {
            IList<CreditNoteModel> creditNoteModelList = null;

            // create query.
            IQueryable<Creditnote> query = GetQueryByCondition(w => w.CreditNoteId != 0)
                                            .Include(w => w.PartyLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);



            // apply filters.
            if (0 != creditNoteId)
                query = query.Where(w => w.CreditNoteId == creditNoteId);

            // get records by query.
            List<Creditnote> creditNoteList = await query.ToListAsync();

            if (null != creditNoteList && creditNoteList.Count > 0)
            {
                creditNoteModelList = new List<CreditNoteModel>();

                foreach (Creditnote creditNote in creditNoteList)
                {
                    creditNoteModelList.Add(await AssignValueToModel(creditNote));
                }
            }

            return creditNoteModelList; // returns.
        }

        private async Task<CreditNoteModel> AssignValueToModel(Creditnote creditNote)
        {
            return await Task.Run(() =>
            {
                CreditNoteModel creditNoteModel = new CreditNoteModel();

                creditNoteModel.CreditNoteId = creditNote.CreditNoteId;
                creditNoteModel.CreditNoteNo = creditNote.CreditNoteNo;
                creditNoteModel.CreditNoteDate = creditNote.CreditNoteDate;
                creditNoteModel.PartyLedgerId = creditNote.PartyLedgerId;
                creditNoteModel.BillToAddressId = creditNote.BillToAddressId;
                creditNoteModel.AccountLedgerId = creditNote.AccountLedgerId;
                creditNoteModel.PartyReferenceNo = creditNote.PartyReferenceNo;
                creditNoteModel.PartyReferenceDate = creditNote.PartyReferenceDate;
                creditNoteModel.OurReferenceNo = creditNote.OurReferenceNo;
                creditNoteModel.OurReferenceDate = creditNote.OurReferenceDate;
                creditNoteModel.CreditLimitDays = creditNote.CreditLimitDays;
                creditNoteModel.PaymentTerm = creditNote.PaymentTerm;
                creditNoteModel.Remark = creditNote.Remark;
                creditNoteModel.TaxModelType = creditNote.TaxModelType;
                creditNoteModel.TaxRegisterId = creditNote.TaxRegisterId;
                creditNoteModel.CurrencyId = creditNote.CurrencyId;
                creditNoteModel.ExchangeRate = creditNote.ExchangeRate;
                creditNoteModel.TotalLineItemAmountFc = creditNote.TotalLineItemAmountFc;
                creditNoteModel.TotalLineItemAmount = creditNote.TotalLineItemAmount;
                creditNoteModel.GrossAmountFc = creditNote.GrossAmountFc;
                creditNoteModel.GrossAmount = creditNote.GrossAmount;
                creditNoteModel.NetAmountFc = creditNote.NetAmountFc;
                creditNoteModel.NetAmount = creditNote.NetAmount;
                creditNoteModel.NetAmountFcinWord = creditNote.NetAmountFcinWord;
                creditNoteModel.TaxAmountFc = creditNote.TaxAmountFc;
                creditNoteModel.TaxAmount = creditNote.TaxAmount;
                creditNoteModel.DiscountPercentageOrAmount = creditNote.DiscountPercentageOrAmount;
                creditNoteModel.DiscountPerOrAmountFc = creditNote.DiscountPerOrAmountFc;
                creditNoteModel.DiscountAmountFc = creditNote.DiscountAmountFc;
                creditNoteModel.DiscountAmount = creditNote.DiscountAmount;
                creditNoteModel.StatusId = creditNote.StatusId;
                creditNoteModel.CompanyId = Convert.ToInt32(creditNote.CompanyId);
                creditNoteModel.FinancialYearId = Convert.ToInt32(creditNote.FinancialYearId);
                creditNoteModel.MaxNo = creditNote.MaxNo;
                creditNoteModel.VoucherStyleId = creditNote.VoucherStyleId;
                //creditNoteModel.PreparedByUserId = creditNote.PreparedByUserId;
                //creditNoteModel.UpdatedByUserId = creditNote.UpdatedByUserId;
                //creditNoteModel.PreparedDateTime = creditNote.PreparedDateTime;
                //creditNoteModel.UpdatedDateTime = creditNote.UpdatedDateTime;

                // ###
                creditNoteModel.PartyLedgerName = null != creditNote.PartyLedger ? creditNote.PartyLedger.LedgerName : null;
                creditNoteModel.BillToAddress = null != creditNote.BillToAddress ? creditNote.BillToAddress.AddressDescription : null;
                creditNoteModel.AccountLedgerName = null != creditNote.AccountLedger ? creditNote.AccountLedger.LedgerName : null;
                creditNoteModel.TaxRegisterName = null != creditNote.TaxRegister ? creditNote.TaxRegister.TaxRegisterName : null;
                creditNoteModel.CurrencyName = null != creditNote.Currency ? creditNote.Currency.CurrencyName : null;
                creditNoteModel.StatusName = null != creditNote.Status ? creditNote.Status.StatusName : null;
                creditNoteModel.PreparedByName = null != creditNote.PreparedByUser ? creditNote.PreparedByUser.UserName : null;

                return creditNoteModel;
            });

        }

        #endregion Private Methods
    }
}

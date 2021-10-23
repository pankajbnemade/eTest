using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class CreditNoteService : Repository<Creditnote>, ICreditNote
    {
        private readonly ICommon common;
        public CreditNoteService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        /// <summary>
        /// generate CreditNote no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return CreditNote no.
        /// </returns>
        public async Task<GenerateNoModel> GenerateCreditNoteNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 4;

            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase CreditNote.
        /// </summary>
        /// <param name="creditNoteModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateCreditNote(CreditNoteModel creditNoteModel)
        {
            int creditNoteId = 0;

            try
            {

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
                creditNote.OurReferenceNo = creditNoteModel.OurReferenceNo;
                creditNote.OurReferenceDate = creditNoteModel.OurReferenceDate;
                creditNote.PartyReferenceNo = creditNoteModel.PartyReferenceNo;
                creditNote.PartyReferenceDate = creditNoteModel.PartyReferenceDate;
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

                creditNote.StatusId = (int)DocumentStatus.Inprocess;
                creditNote.CompanyId = creditNoteModel.CompanyId;
                creditNote.FinancialYearId = creditNoteModel.FinancialYearId;

                await Create(creditNote);

                creditNoteId = creditNote.CreditNoteId;

                if (creditNoteId != 0)
                {
                    await UpdateCreditNoteMasterAmount(creditNoteId);
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return creditNoteId; // returns.
        }

        /// <summary>
        /// update purchase CreditNote.
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
                creditNote.OurReferenceNo = creditNoteModel.OurReferenceNo;
                creditNote.OurReferenceDate = creditNoteModel.OurReferenceDate;
                creditNote.PartyReferenceNo = creditNoteModel.PartyReferenceNo;
                creditNote.PartyReferenceDate = creditNoteModel.PartyReferenceDate;
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

        public async Task<bool> UpdateStatusCreditNote(int creditNoteId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Creditnote creditNote = await GetByIdAsync(w => w.CreditNoteId == creditNoteId);

            if (null != creditNote)
            {
                creditNote.StatusId = statusId;
                isUpdated = await Update(creditNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase CreditNote.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteCreditNote(int creditNoteId)
        {
            bool isDeleted = false;

            // get record.
            Creditnote creditNote = await GetByIdAsync(w => w.CreditNoteId == creditNoteId);

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
                creditNote.TotalLineItemAmount = creditNote.TotalLineItemAmountFc / creditNote.ExchangeRate;

                if (DiscountType.Percentage.ToString() == creditNote.DiscountPercentageOrAmount)
                {
                    creditNote.DiscountAmountFc = (creditNote.TotalLineItemAmountFc * creditNote.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    creditNote.DiscountAmountFc = creditNote.DiscountPerOrAmountFc;
                }

                creditNote.DiscountAmount = creditNote.DiscountAmountFc / creditNote.ExchangeRate;
                creditNote.GrossAmountFc = creditNote.TotalLineItemAmountFc - creditNote.DiscountAmountFc;
                creditNote.GrossAmount = creditNote.GrossAmountFc / creditNote.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == creditNote.TaxModelType)
                {
                    creditNote.TaxAmountFc = creditNote.Creditnotedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    creditNote.TaxAmountFc = creditNote.Creditnotetaxes.Sum(w => w.TaxAmountFc);
                }

                creditNote.TaxAmount = creditNote.TaxAmountFc / creditNote.ExchangeRate;
                creditNote.NetAmountFc = creditNote.GrossAmountFc + creditNote.TaxAmountFc;
                creditNote.NetAmount = creditNote.NetAmountFc / creditNote.ExchangeRate;

                creditNote.NetAmountFcinWord = await common.AmountInWord_Million(creditNote.NetAmountFc.ToString(), creditNote.Currency.CurrencyCode, creditNote.Currency.Denomination);

                if (creditNote.StatusId == (int)DocumentStatus.Approved || creditNote.StatusId == (int)DocumentStatus.ApprovalRequested || creditNote.StatusId == (int)DocumentStatus.Cancelled)
                {
                    creditNote.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(creditNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase CreditNote based on creditNoteId
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
        /// get search purchase CreditNote result list.
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
        /// get purchase CreditNote List based on partyLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetCreditNoteListByPartyLedgerId(int partyLedgerId, DateTime? voucherDate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Creditnote> query = GetQueryByCondition(w => w.CreditNoteId != 0
                                                                && w.StatusId == (int)DocumentStatus.Approved);

            // apply filters.
            if (0 != partyLedgerId)
                query = query.Where(w => w.PartyLedgerId == partyLedgerId);

            if (null != voucherDate)
            {
                query = query.Where(w => w.CreditNoteDate <= voucherDate);
            }

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
                        SalesInvoiceId = null,
                        PurchaseInvoiceId = null,
                        DebitNoteId = null
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

            IQueryable<Creditnote> query = GetQueryByCondition(w => w.CreditNoteId != 0)
                                                .Include(w => w.PartyLedger).Include(w => w.Currency)
                                                .Include(w => w.PreparedByUser).Include(w => w.Status);

            //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "CreditNoteNo";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

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

            if (!string.IsNullOrEmpty(searchFilterModel.PartyReferenceNo))
            {
                query = query.Where(w => w.PartyReferenceNo.Contains(searchFilterModel.PartyReferenceNo));
            }

            if (null != searchFilterModel.AccountLedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

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
                NetAmountFc = s.NetAmountFc,
                PartyReferenceNo = s.PartyReferenceNo,
                PartyReferenceDate = s.PartyReferenceDate,
                PartyLedgerName = s.PartyLedger.LedgerName,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

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
                creditNoteModel.NetAmountFcInWord = creditNote.NetAmountFcinWord;
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
                creditNoteModel.CurrencyCode = null != creditNote.Currency ? creditNote.Currency.CurrencyCode : null;
                creditNoteModel.StatusName = null != creditNote.Status ? creditNote.Status.StatusName : null;
                creditNoteModel.PreparedByName = null != creditNote.PreparedByUser ? creditNote.PreparedByUser.UserName : null;

                return creditNoteModel;
            });

        }

        #endregion Private Methods
    }
}

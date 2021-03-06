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
    public class DebitNoteService : Repository<Debitnote>, IDebitNote
    {
        private readonly ICommon _common;
        public DebitNoteService(ErpDbContext dbContext, ICommon common) : base(dbContext)
        {
            _common = common;
        }

        /// <summary>
        /// generate DebitNote no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return DebitNote no.
        /// </returns>
        public async Task<GenerateNoModel> GenerateDebitNoteNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 5;

            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await _common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase DebitNote.
        /// </summary>
        /// <param name="debitNoteModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateDebitNote(DebitNoteModel debitNoteModel)
        {
            int debitNoteId = 0;

            try
            {

                GenerateNoModel generateNoModel = await GenerateDebitNoteNo(debitNoteModel.CompanyId, debitNoteModel.FinancialYearId);

                // assign values.
                Debitnote debitNote = new Debitnote();

                debitNote.DebitNoteNo = generateNoModel.VoucherNo;
                debitNote.MaxNo = generateNoModel.MaxNo;
                debitNote.VoucherStyleId = generateNoModel.VoucherStyleId;

                debitNote.DebitNoteDate = debitNoteModel.DebitNoteDate;
                debitNote.PartyLedgerId = debitNoteModel.PartyLedgerId;
                debitNote.BillToAddressId = debitNoteModel.BillToAddressId;
                debitNote.AccountLedgerId = debitNoteModel.AccountLedgerId;
                debitNote.OurReferenceNo = debitNoteModel.OurReferenceNo;
                debitNote.OurReferenceDate = debitNoteModel.OurReferenceDate;
                debitNote.PartyReferenceNo = debitNoteModel.PartyReferenceNo;
                debitNote.PartyReferenceDate = debitNoteModel.PartyReferenceDate;
                debitNote.CreditLimitDays = debitNoteModel.CreditLimitDays;
                debitNote.PaymentTerm = debitNoteModel.PaymentTerm;
                debitNote.Remark = debitNoteModel.Remark;
                debitNote.TaxModelType = debitNoteModel.TaxModelType;
                debitNote.TaxRegisterId = debitNoteModel.TaxRegisterId;
                debitNote.CurrencyId = debitNoteModel.CurrencyId;
                debitNote.ExchangeRate = debitNoteModel.ExchangeRate;
                debitNote.TotalLineItemAmountFc = 0;
                debitNote.TotalLineItemAmount = 0;
                debitNote.GrossAmountFc = 0;
                debitNote.GrossAmount = 0;
                debitNote.NetAmountFc = 0;
                debitNote.NetAmount = 0;
                debitNote.NetAmountFcinWord = "";
                debitNote.TaxAmountFc = 0;
                debitNote.TaxAmount = 0;

                debitNote.DiscountPercentageOrAmount = debitNoteModel.DiscountPercentageOrAmount;
                debitNote.DiscountPerOrAmountFc = debitNoteModel.DiscountPerOrAmountFc;
                debitNote.DiscountAmountFc = 0;
                debitNote.DiscountAmount = 0;

                debitNote.StatusId = (int)DocumentStatus.Inprocess;
                debitNote.CompanyId = debitNoteModel.CompanyId;
                debitNote.FinancialYearId = debitNoteModel.FinancialYearId;

                await Create(debitNote);

                debitNoteId = debitNote.DebitNoteId;

                if (debitNoteId != 0)
                {
                    await UpdateDebitNoteMasterAmount(debitNoteId);
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return debitNoteId; // returns.
        }

        /// <summary>
        /// update purchase DebitNote.
        /// </summary>
        /// <param name="debitNoteModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateDebitNote(DebitNoteModel debitNoteModel)
        {
            bool isUpdated = false;

            // get record.
            Debitnote debitNote = await GetByIdAsync(w => w.DebitNoteId == debitNoteModel.DebitNoteId);

            if (null != debitNote)
            {
                // assign values.

                debitNote.DebitNoteDate = debitNoteModel.DebitNoteDate;
                debitNote.PartyLedgerId = debitNoteModel.PartyLedgerId;
                debitNote.BillToAddressId = debitNoteModel.BillToAddressId;
                debitNote.AccountLedgerId = debitNoteModel.AccountLedgerId;
                debitNote.OurReferenceNo = debitNoteModel.OurReferenceNo;
                debitNote.OurReferenceDate = debitNoteModel.OurReferenceDate;
                debitNote.PartyReferenceNo = debitNoteModel.PartyReferenceNo;
                debitNote.PartyReferenceDate = debitNoteModel.PartyReferenceDate;
                debitNote.CreditLimitDays = debitNoteModel.CreditLimitDays;
                debitNote.PaymentTerm = debitNoteModel.PaymentTerm;
                debitNote.Remark = debitNoteModel.Remark;
                debitNote.TaxModelType = debitNoteModel.TaxModelType;
                debitNote.TaxRegisterId = debitNoteModel.TaxRegisterId;
                debitNote.CurrencyId = debitNoteModel.CurrencyId;
                debitNote.ExchangeRate = debitNoteModel.ExchangeRate;

                debitNote.TotalLineItemAmountFc = 0;
                debitNote.TotalLineItemAmount = 0;
                debitNote.GrossAmountFc = 0;
                debitNote.GrossAmount = 0;
                debitNote.NetAmountFc = 0;
                debitNote.NetAmount = 0;
                debitNote.NetAmountFcinWord = "";
                debitNote.TaxAmountFc = 0;
                debitNote.TaxAmount = 0;

                debitNote.DiscountPercentageOrAmount = debitNoteModel.DiscountPercentageOrAmount;
                debitNote.DiscountPerOrAmountFc = debitNoteModel.DiscountPerOrAmountFc;

                debitNote.DiscountAmountFc = 0;
                debitNote.DiscountAmount = 0;

                isUpdated = await Update(debitNote);
            }

            if (isUpdated != false)
            {
                await UpdateDebitNoteMasterAmount(debitNote.DebitNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusDebitNote(int debitNoteId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Debitnote debitNote = await GetByIdAsync(w => w.DebitNoteId == debitNoteId);

            if (null != debitNote)
            {
                debitNote.StatusId = statusId;
                isUpdated = await Update(debitNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase DebitNote.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteDebitNote(int debitNoteId)
        {
            bool isDeleted = false;

            // get record.
            Debitnote debitNote = await GetByIdAsync(w => w.DebitNoteId == debitNoteId);

            if (null != debitNote)
            {
                isDeleted = await Delete(debitNote);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateDebitNoteMasterAmount(int? debitNoteId)
        {
            bool isUpdated = false;

            // get record.
            Debitnote debitNote = await GetQueryByCondition(w => w.DebitNoteId == debitNoteId)
                                                    .Include(w => w.Debitnotedetails).Include(w => w.Debitnotetaxes)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != debitNote)
            {
                debitNote.TotalLineItemAmountFc = debitNote.Debitnotedetails.Sum(w => w.GrossAmountFc);
                debitNote.TotalLineItemAmount = debitNote.TotalLineItemAmountFc / debitNote.ExchangeRate;

                if (DiscountType.Percentage.ToString() == debitNote.DiscountPercentageOrAmount)
                {
                    debitNote.DiscountAmountFc = (debitNote.TotalLineItemAmountFc * debitNote.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    debitNote.DiscountAmountFc = debitNote.DiscountPerOrAmountFc;
                }

                debitNote.DiscountAmount = debitNote.DiscountAmountFc / debitNote.ExchangeRate;
                debitNote.GrossAmountFc = debitNote.TotalLineItemAmountFc - debitNote.DiscountAmountFc;
                debitNote.GrossAmount = debitNote.GrossAmountFc / debitNote.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == debitNote.TaxModelType)
                {
                    debitNote.TaxAmountFc = debitNote.Debitnotedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    debitNote.TaxAmountFc = debitNote.Debitnotetaxes.Sum(w => w.TaxAmountFc);
                }

                debitNote.TaxAmount = debitNote.TaxAmountFc / debitNote.ExchangeRate;
                debitNote.NetAmountFc = debitNote.GrossAmountFc + debitNote.TaxAmountFc;
                debitNote.NetAmount = debitNote.NetAmountFc / debitNote.ExchangeRate;

                debitNote.NetAmountFcinWord = await _common.AmountInWord_Million(debitNote.NetAmountFc.ToString(), debitNote.Currency.CurrencyCode, debitNote.Currency.Denomination);

                if (debitNote.StatusId == (int)DocumentStatus.Approved || debitNote.StatusId == (int)DocumentStatus.ApprovalRequested || debitNote.StatusId == (int)DocumentStatus.Cancelled)
                {
                    debitNote.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(debitNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase DebitNote based on debitNoteId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<DebitNoteModel> GetDebitNoteById(int debitNoteId)
        {
            DebitNoteModel debitNoteModel = null;

            IList<DebitNoteModel> debitNoteModelList = await GetDebitNoteList(debitNoteId);

            if (null != debitNoteModelList && debitNoteModelList.Any())
            {
                debitNoteModel = debitNoteModelList.FirstOrDefault();
            }

            return debitNoteModel; // returns.
        }

        /// <summary>
        /// get search purchase DebitNote result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<DebitNoteModel>> GetDebitNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterDebitNoteModel searchFilterModel)
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
            DataTableResultModel<DebitNoteModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        /// <summary>
        /// get purchase DebitNote List based on partyLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetDebitNoteListByPartyLedgerId(int partyLedgerId, DateTime? voucherDate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0
                                                               && w.StatusId == (int)DocumentStatus.Approved);

            // apply filters.
            if (0 != partyLedgerId)
                query = query.Where(w => w.PartyLedgerId == partyLedgerId);

            if (null != voucherDate)
            {
                query = query.Where(w => w.DebitNoteDate <= voucherDate);
            }

            // get records by query.
            List<Debitnote> debitNoteList = await query.ToListAsync();

            outstandingInvoiceModelList = new List<OutstandingInvoiceModel>();

            if (null != debitNoteList && debitNoteList.Count > 0)
            {
                foreach (Debitnote debitNote in debitNoteList)
                {
                    outstandingInvoiceModelList.Add(new OutstandingInvoiceModel()
                    {
                        InvoiceId = debitNote.DebitNoteId,
                        InvoiceType = "Debit Note",
                        InvoiceNo = debitNote.DebitNoteNo,
                        InvoiceDate = debitNote.DebitNoteDate,
                        InvoiceAmount = debitNote.NetAmount,
                        OutstandingAmount = debitNote.NetAmount,
                        DebitNoteId = debitNote.DebitNoteId,
                        SalesInvoiceId = null,
                        PurchaseInvoiceId = null,
                        CreditNoteId = null
                    });
                }
            }

            return outstandingInvoiceModelList; // returns.
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Debitnote> query = GetQueryByCondition(w => w.StatusId == (int)DocumentStatus.Approved && w.FinancialYearId == yearId && w.CompanyId == companyId)
                                                .Include(i => i.Currency);

            query = query.Where(w => w.PartyLedgerId == ledgerId);

            query = query.Where(w => w.DebitNoteDate >= fromDate && w.DebitNoteDate <= toDate);

            // get records by query.
            List<Debitnote> debitNoteList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != debitNoteList && debitNoteList.Count > 0)
            {
                foreach (Debitnote debitNote in debitNoteList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = debitNote.DebitNoteId,
                        DocumentType = "Debit Note",
                        DocumentNo = debitNote.DebitNoteNo,
                        DocumentDate = debitNote.DebitNoteDate,
                        Amount_FC = debitNote.NetAmountFc,
                        Amount = debitNote.NetAmount,
                        DebitAmount_FC = debitNote.NetAmountFc,
                        DebitAmount = debitNote.NetAmount,
                        DebitNoteId = debitNote.DebitNoteId,
                        CurrencyId = debitNote.CurrencyId,
                        CurrencyCode = debitNote.Currency.CurrencyCode,
                        ExchangeRate = debitNote.ExchangeRate,
                        PartyReferenceNo = debitNote.PartyReferenceNo,
                        OurReferenceNo = debitNote.OurReferenceNo,
                    });
                }
            }

            return generalLedgerModelList; // returns.
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
        private async Task<DataTableResultModel<DebitNoteModel>> GetDataFromDbase(SearchFilterDebitNoteModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<DebitNoteModel> resultModel = new DataTableResultModel<DebitNoteModel>();

            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0)
                                                .Include(w => w.PartyLedger).Include(w => w.Currency)
                                                .Include(w => w.PreparedByUser).Include(w => w.Status);

            query = query.Where(w => w.CompanyId==searchFilterModel.CompanyId);
            query = query.Where(w => w.FinancialYearId==searchFilterModel.FinancialYearId);

            //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "DebitNoteNo";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

            if (!string.IsNullOrEmpty(searchFilterModel.DebitNoteNo))
            {
                query = query.Where(w => w.DebitNoteNo.Contains(searchFilterModel.DebitNoteNo));
            }

            if (null != searchFilterModel.PartyLedgerId)
            {
                query = query.Where(w => w.PartyLedgerId == searchFilterModel.PartyLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.DebitNoteDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.DebitNoteDate <= searchFilterModel.ToDate);
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
                query = query.Where(w => w.DebitNoteNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new DebitNoteModel
            {
                DebitNoteId = s.DebitNoteId,
                DebitNoteNo = s.DebitNoteNo,
                DebitNoteDate = s.DebitNoteDate,
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

        private async Task<IList<DebitNoteModel>> GetDebitNoteList(int debitNoteId)
        {
            IList<DebitNoteModel> debitNoteModelList = null;

            // create query.
            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0)
                                            .Include(w => w.PartyLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnote> debitNoteList = await query.ToListAsync();

            if (null != debitNoteList && debitNoteList.Count > 0)
            {
                debitNoteModelList = new List<DebitNoteModel>();

                foreach (Debitnote debitNote in debitNoteList)
                {
                    debitNoteModelList.Add(await AssignValueToModel(debitNote));
                }
            }

            return debitNoteModelList; // returns.
        }

        private async Task<DebitNoteModel> AssignValueToModel(Debitnote debitNote)
        {
            return await Task.Run(() =>
            {
                DebitNoteModel debitNoteModel = new DebitNoteModel();

                debitNoteModel.DebitNoteId = debitNote.DebitNoteId;
                debitNoteModel.DebitNoteNo = debitNote.DebitNoteNo;
                debitNoteModel.DebitNoteDate = debitNote.DebitNoteDate;
                debitNoteModel.PartyLedgerId = debitNote.PartyLedgerId;
                debitNoteModel.BillToAddressId = debitNote.BillToAddressId;
                debitNoteModel.AccountLedgerId = debitNote.AccountLedgerId;
                debitNoteModel.PartyReferenceNo = debitNote.PartyReferenceNo;
                debitNoteModel.PartyReferenceDate = debitNote.PartyReferenceDate;
                debitNoteModel.OurReferenceNo = debitNote.OurReferenceNo;
                debitNoteModel.OurReferenceDate = debitNote.OurReferenceDate;
                debitNoteModel.CreditLimitDays = debitNote.CreditLimitDays;
                debitNoteModel.PaymentTerm = debitNote.PaymentTerm;
                debitNoteModel.Remark = debitNote.Remark;
                debitNoteModel.TaxModelType = debitNote.TaxModelType;
                debitNoteModel.TaxRegisterId = debitNote.TaxRegisterId;
                debitNoteModel.CurrencyId = debitNote.CurrencyId;
                debitNoteModel.ExchangeRate = debitNote.ExchangeRate;
                debitNoteModel.TotalLineItemAmountFc = debitNote.TotalLineItemAmountFc;
                debitNoteModel.TotalLineItemAmount = debitNote.TotalLineItemAmount;
                debitNoteModel.GrossAmountFc = debitNote.GrossAmountFc;
                debitNoteModel.GrossAmount = debitNote.GrossAmount;
                debitNoteModel.NetAmountFc = debitNote.NetAmountFc;
                debitNoteModel.NetAmount = debitNote.NetAmount;
                debitNoteModel.NetAmountFcInWord = debitNote.NetAmountFcinWord;
                debitNoteModel.TaxAmountFc = debitNote.TaxAmountFc;
                debitNoteModel.TaxAmount = debitNote.TaxAmount;
                debitNoteModel.DiscountPercentageOrAmount = debitNote.DiscountPercentageOrAmount;
                debitNoteModel.DiscountPerOrAmountFc = debitNote.DiscountPerOrAmountFc;
                debitNoteModel.DiscountAmountFc = debitNote.DiscountAmountFc;
                debitNoteModel.DiscountAmount = debitNote.DiscountAmount;
                debitNoteModel.StatusId = debitNote.StatusId;
                debitNoteModel.CompanyId = Convert.ToInt32(debitNote.CompanyId);
                debitNoteModel.FinancialYearId = Convert.ToInt32(debitNote.FinancialYearId);
                debitNoteModel.MaxNo = debitNote.MaxNo;
                debitNoteModel.VoucherStyleId = debitNote.VoucherStyleId;
                //debitNoteModel.PreparedByUserId = debitNote.PreparedByUserId;
                //debitNoteModel.UpdatedByUserId = debitNote.UpdatedByUserId;
                //debitNoteModel.PreparedDateTime = debitNote.PreparedDateTime;
                //debitNoteModel.UpdatedDateTime = debitNote.UpdatedDateTime;

                // ###
                debitNoteModel.PartyLedgerName = null != debitNote.PartyLedger ? debitNote.PartyLedger.LedgerName : null;
                debitNoteModel.BillToAddress = null != debitNote.BillToAddress ? debitNote.BillToAddress.AddressDescription : null;
                debitNoteModel.AccountLedgerName = null != debitNote.AccountLedger ? debitNote.AccountLedger.LedgerName : null;
                debitNoteModel.TaxRegisterName = null != debitNote.TaxRegister ? debitNote.TaxRegister.TaxRegisterName : null;
                debitNoteModel.CurrencyCode = null != debitNote.Currency ? debitNote.Currency.CurrencyCode : null;
                debitNoteModel.StatusName = null != debitNote.Status ? debitNote.Status.StatusName : null;
                debitNoteModel.PreparedByName = null != debitNote.PreparedByUser ? debitNote.PreparedByUser.UserName : null;

                return debitNoteModel;
            });

        }

        #endregion Private Methods
    }
}

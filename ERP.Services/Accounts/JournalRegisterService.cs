using ERP.DataAccess.EntityData;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class JournalRegisterService : IJournalRegister
    {
        private readonly ErpDbContext _dbContext;

        public JournalRegisterService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<JournalRegisterModel>> GetReport(SearchFilterJournalRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<JournalRegisterModel> journalRegisterModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<JournalRegisterModel> resultModel = new DataTableResultModel<JournalRegisterModel>();

            if (null != journalRegisterModelList && journalRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<JournalRegisterModel>();
                resultModel.ResultList = journalRegisterModelList;
                resultModel.TotalResultCount = journalRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<JournalRegisterModel>();
                resultModel.ResultList = new List<JournalRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<JournalRegisterModel>> GetList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<JournalRegisterModel> journalRegisterModelList = new List<JournalRegisterModel>();

            IList<JournalRegisterModel> journalRegisterModelList_Trans = null;

            journalRegisterModelList_Trans = await GetTransactionList(fromDate, toDate, financialYearId, companyId);

            if (journalRegisterModelList_Trans==null)
            {
                journalRegisterModelList_Trans= new List<JournalRegisterModel>();
            }

            journalRegisterModelList = journalRegisterModelList_Trans;

            if (journalRegisterModelList.Any())
            {
                journalRegisterModelList.Add(new JournalRegisterModel()
                {
                    SequenceNo = 3,
                    SrNo = journalRegisterModelList.Max(w => w.SrNo)+1,
                    DocumentNo = "Total Amount",
                    DocumentDate = toDate,
                    CreditAmount = journalRegisterModelList.Sum(w => w.CreditAmount),
                    DebitAmount = journalRegisterModelList.Sum(w => w.DebitAmount),
                });

                return journalRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return journalRegisterModelList;
        }

        private async Task<IList<JournalRegisterModel>> GetTransactionList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<JournalRegisterModel> journalRegisterModelList = null;

                journalRegisterModelList = _dbContext
                            .Journalvoucherdetails
                            .Include(i => i.ParticularLedger)
                            .Include(i => i.PurchaseInvoice)
                            .Include(i => i.SalesInvoice)
                            .Include(i => i.DebitNote)
                            .Include(i => i.CreditNote)
                            //.Include(i => i.JournalVoucher).ThenInclude(i => i.AccountLedger)
                            .Include(i => i.JournalVoucher).ThenInclude(i => i.Currency)
                            .Where(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved
                                && w.JournalVoucher.FinancialYearId == financialYearId
                                && w.JournalVoucher.CompanyId == companyId
                                && w.JournalVoucher.VoucherDate >= fromDate
                                && w.JournalVoucher.VoucherDate <= toDate
                                )
                            .ToList()
                            .Select((row, index) => new JournalRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                DocumentDetId = row.JournalVoucherDetId,
                                DocumentId = row.JournalVoucherId,
                                DocumentNo = row.JournalVoucher.VoucherNo,
                                DocumentDate = row.JournalVoucher.VoucherDate,
                                //ChequeNo = row.JournalVoucher.ChequeNo,
                                //ChequeDate = row.JournalVoucher.ChequeDate,
                                //BankName = (null != row.JournalVoucher ? (null != row.JournalVoucher.AccountLedger ? row.JournalVoucher.AccountLedger.LedgerName : "") : ""),
                                CurrencyId = row.JournalVoucher.CurrencyId,
                                CurrencyCode = (null != row.JournalVoucher ? (null != row.JournalVoucher.Currency ? row.JournalVoucher.Currency.CurrencyCode : "") : ""),
                                ExchangeRate = row.JournalVoucher.ExchangeRate,
                                ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                                Narration = row.Narration,
                                PurchaseInvoiceId = row.PurchaseInvoiceId,
                                SalesInvoiceId = row.SalesInvoiceId,
                                DebitNoteId = row.DebitNoteId,
                                CreditNoteId = row.CreditNoteId,
                                InvoiceType = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? "Purchase Invoice"
                                                : row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? "Sales Invoice"
                                                : row.DebitNoteId != 0 && row.DebitNoteId != null ? "Debit Note"
                                                : row.CreditNoteId != 0 && row.CreditNoteId != null ? "Credit Note"
                                                : "",
                                InvoiceNo = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? (null != row.PurchaseInvoice ? row.PurchaseInvoice.InvoiceNo : "")
                                            : row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? (null != row.SalesInvoice ? row.SalesInvoice.InvoiceNo : "")
                                            : row.DebitNoteId != 0 && row.DebitNoteId != null ? (null != row.DebitNote ? row.DebitNote.DebitNoteNo : "")
                                            : row.CreditNoteId != 0 && row.CreditNoteId != null ? (null != row.CreditNote ? row.CreditNote.CreditNoteNo : "")
                                            : "",
                                CreditAmount_FC = row.CreditAmountFc,
                                CreditAmount = row.CreditAmount,
                                DebitAmount_FC = row.DebitAmountFc,
                                DebitAmount = row.DebitAmount
                            })
                            .ToList();

                if (journalRegisterModelList==null)
                {
                    journalRegisterModelList= new List<JournalRegisterModel>();
                }

                return journalRegisterModelList; // returns.
            });
        }

    }
}

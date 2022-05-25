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
    public class ContraRegisterService : IContraRegister
    {
        ErpDbContext dbContext;

        public ContraRegisterService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<ContraRegisterModel>> GetReport(SearchFilterContraRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<ContraRegisterModel> contraRegisterModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<ContraRegisterModel> resultModel = new DataTableResultModel<ContraRegisterModel>();

            if (null != contraRegisterModelList && contraRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<ContraRegisterModel>();
                resultModel.ResultList = contraRegisterModelList;
                resultModel.TotalResultCount = contraRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ContraRegisterModel>();
                resultModel.ResultList = new List<ContraRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<ContraRegisterModel>> GetList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<ContraRegisterModel> contraRegisterModelList = new List<ContraRegisterModel>();

            IList<ContraRegisterModel> contraRegisterModelList_Trans = null;

            contraRegisterModelList_Trans = await GetTransactionList(fromDate, toDate, financialYearId, companyId);

            if (contraRegisterModelList_Trans==null)
            {
                contraRegisterModelList_Trans= new List<ContraRegisterModel>();
            }

            contraRegisterModelList = contraRegisterModelList_Trans;

            if (contraRegisterModelList.Any())
            {
                contraRegisterModelList.Add(new ContraRegisterModel()
                {
                    SequenceNo = 3,
                    SrNo = contraRegisterModelList.Max(w => w.SrNo)+1,
                    DocumentNo = "Total Amount",
                    DocumentDate = toDate,
                    CreditAmount = contraRegisterModelList.Sum(w => w.CreditAmount),
                    DebitAmount = contraRegisterModelList.Sum(w => w.DebitAmount),
                });

                return contraRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return contraRegisterModelList;
        }

        private async Task<IList<ContraRegisterModel>> GetTransactionList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<ContraRegisterModel> contraRegisterModelList = null;

                contraRegisterModelList = dbContext
                            .Contravoucherdetails
                            .Include(i => i.ParticularLedger)
                            //.Include(i => i.PurchaseInvoice)
                            //.Include(i => i.SalesInvoice)
                            //.Include(i => i.DebitNote)
                            //.Include(i => i.CreditNote)
                            //.Include(i => i.ContraVoucher).ThenInclude(i => i.AccountLedger)
                            .Include(i => i.ContraVoucher).ThenInclude(i => i.Currency)
                            .Where(w => w.ContraVoucher.StatusId == (int)DocumentStatus.Approved
                                && w.ContraVoucher.FinancialYearId == financialYearId
                                && w.ContraVoucher.CompanyId == companyId
                                && w.ContraVoucher.VoucherDate >= fromDate
                                && w.ContraVoucher.VoucherDate <= toDate
                                )
                            .ToList()
                            .Select((row, index) => new ContraRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                DocumentDetId = row.ContraVoucherDetId,
                                DocumentId = row.ContraVoucherId,
                                DocumentNo = row.ContraVoucher.VoucherNo,
                                DocumentDate = row.ContraVoucher.VoucherDate,
                                ChequeNo = row.ContraVoucher.ChequeNo,
                                ChequeDate = row.ContraVoucher.ChequeDate,
                                //BankName = (null != row.ContraVoucher ? (null != row.ContraVoucher.AccountLedger ? row.ContraVoucher.AccountLedger.LedgerName : "") : ""),
                                CurrencyId = row.ContraVoucher.CurrencyId,
                                CurrencyCode = (null != row.ContraVoucher ? (null != row.ContraVoucher.Currency ? row.ContraVoucher.Currency.CurrencyCode : "") : ""),
                                ExchangeRate = row.ContraVoucher.ExchangeRate,
                                ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                                Narration = row.Narration,
                                //PurchaseInvoiceId = row.PurchaseInvoiceId,
                                //DebitNoteId = row.DebitNoteId,
                                //InvoiceType = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? "Purchase Invoice"
                                //                : row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? "Sales Invoice"
                                //                : row.DebitNoteId != 0 && row.DebitNoteId != null ? "Debit Note"
                                //                : row.CreditNoteId != 0 && row.CreditNoteId != null ? "Credit Note"
                                //                : "",
                                //InvoiceNo = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? (null != row.PurchaseInvoice ? row.PurchaseInvoice.InvoiceNo : "")
                                //            : row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? (null != row.SalesInvoice ? row.SalesInvoice.InvoiceNo : "") 
                                //            : row.DebitNoteId != 0 && row.DebitNoteId != null ? (null != row.DebitNote ? row.DebitNote.DebitNoteNo : "") 
                                //            : row.CreditNoteId != 0 && row.CreditNoteId != null ? (null != row.CreditNote ? row.CreditNote.CreditNoteNo : "") 
                                //            : "",
                                CreditAmount_FC = row.CreditAmountFc,
                                CreditAmount = row.CreditAmount,
                                DebitAmount_FC = row.DebitAmountFc,
                                DebitAmount = row.DebitAmount
                            })
                            .ToList();

                if (contraRegisterModelList==null)
                {
                    contraRegisterModelList= new List<ContraRegisterModel>();
                }

                return contraRegisterModelList; // returns.
            });
        }

    }
}

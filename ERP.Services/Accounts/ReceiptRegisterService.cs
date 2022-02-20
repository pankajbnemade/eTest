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
    public class ReceiptRegisterService : IReceiptRegister
    {
        ErpDbContext dbContext;

        public ReceiptRegisterService(ErpDbContext _dbContext
                                )
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<ReceiptRegisterModel>> GetReport(SearchFilterReceiptRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<ReceiptRegisterModel> receiptRegisterModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<ReceiptRegisterModel> resultModel = new DataTableResultModel<ReceiptRegisterModel>();

            if (null != receiptRegisterModelList && receiptRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceiptRegisterModel>();
                resultModel.ResultList = receiptRegisterModelList;
                resultModel.TotalResultCount = receiptRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ReceiptRegisterModel>();
                resultModel.ResultList = new List<ReceiptRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<ReceiptRegisterModel>> GetList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<ReceiptRegisterModel> receiptRegisterModelList = new List<ReceiptRegisterModel>();

            IList<ReceiptRegisterModel> receiptRegisterModelList_Trans = null;

            receiptRegisterModelList_Trans = await GetTransactionList(fromDate, toDate, financialYearId, companyId);

            if (receiptRegisterModelList_Trans==null)
            {
                receiptRegisterModelList_Trans= new List<ReceiptRegisterModel>();
            }

            receiptRegisterModelList = receiptRegisterModelList_Trans;

            receiptRegisterModelList.Add(new ReceiptRegisterModel()
            {
                SequenceNo = 3,
                SrNo = receiptRegisterModelList.Max(w => w.SrNo)+1,
                DocumentNo = "Total Amount",
                DocumentDate = toDate,
                Amount = receiptRegisterModelList.Sum(w => w.Amount),
            });

            return receiptRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
        }

        private async Task<IList<ReceiptRegisterModel>> GetTransactionList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<ReceiptRegisterModel> receiptRegisterModelList = null;

                receiptRegisterModelList = dbContext
                            .Receiptvoucherdetails
                            .Include(i => i.ParticularLedger)
                            .Include(i => i.SalesInvoice)
                            .Include(i => i.CreditNote)
                            .Include(i => i.ReceiptVoucher).ThenInclude(i => i.AccountLedger)
                            .Include(i => i.ReceiptVoucher).ThenInclude(i => i.Currency)
                            .Where(w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved
                                && w.ReceiptVoucher.FinancialYearId == financialYearId
                                && w.ReceiptVoucher.CompanyId == companyId
                                && w.ReceiptVoucher.VoucherDate >= fromDate
                                && w.ReceiptVoucher.VoucherDate <= toDate
                                )
                            .ToList()
                            .Select((row, index) => new ReceiptRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                DocumentDetId = row.ReceiptVoucherDetId,
                                DocumentId = row.ReceiptVoucherId,
                                DocumentNo = row.ReceiptVoucher.VoucherNo,
                                DocumentDate = row.ReceiptVoucher.VoucherDate,
                                ChequeNo = row.ReceiptVoucher.ChequeNo,
                                ChequeDate = row.ReceiptVoucher.ChequeDate,
                                BankName = (null != row.ReceiptVoucher ? (null != row.ReceiptVoucher.AccountLedger ? row.ReceiptVoucher.AccountLedger.LedgerName : "") : ""),
                                CurrencyId = row.ReceiptVoucher.CurrencyId,
                                CurrencyCode = (null != row.ReceiptVoucher ? (null != row.ReceiptVoucher.Currency ? row.ReceiptVoucher.Currency.CurrencyCode : "") : ""),
                                ExchangeRate = row.ReceiptVoucher.ExchangeRate,
                                ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                                Narration = row.Narration,
                                SalesInvoiceId = row.SalesInvoiceId,
                                CreditNoteId = row.CreditNoteId,
                                InvoiceType = row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? "Sales Invoice"
                                                : row.CreditNoteId != 0 && row.CreditNoteId != null ? "Credit Note" : "",
                                InvoiceNo = row.SalesInvoiceId != 0 && row.SalesInvoiceId != null ? (null != row.SalesInvoice ? row.SalesInvoice.InvoiceNo : "")
                                            : row.CreditNoteId != 0 && row.CreditNoteId != null ? (null != row.CreditNote ? row.CreditNote.CreditNoteNo : "") : "",
                                Amount_FC = row.AmountFc,
                                Amount = row.Amount
                            })
                            .ToList();

                if (receiptRegisterModelList==null)
                {
                    receiptRegisterModelList= new List<ReceiptRegisterModel>();
                }

                return receiptRegisterModelList; // returns.
            });
        }

    }
}

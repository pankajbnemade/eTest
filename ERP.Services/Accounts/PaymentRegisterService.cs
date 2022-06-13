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
    public class PaymentRegisterService : IPaymentRegister
    {
        private readonly ErpDbContext _dbContext;

        public PaymentRegisterService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<PaymentRegisterModel>> GetReport(SearchFilterPaymentRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<PaymentRegisterModel> paymentRegisterModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<PaymentRegisterModel> resultModel = new DataTableResultModel<PaymentRegisterModel>();

            if (null != paymentRegisterModelList && paymentRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<PaymentRegisterModel>();
                resultModel.ResultList = paymentRegisterModelList;
                resultModel.TotalResultCount = paymentRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PaymentRegisterModel>();
                resultModel.ResultList = new List<PaymentRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<PaymentRegisterModel>> GetList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<PaymentRegisterModel> paymentRegisterModelList = new List<PaymentRegisterModel>();

            IList<PaymentRegisterModel> paymentRegisterModelList_Trans = null;

            paymentRegisterModelList_Trans = await GetTransactionList(fromDate, toDate, financialYearId, companyId);

            if (paymentRegisterModelList_Trans==null)
            {
                paymentRegisterModelList_Trans= new List<PaymentRegisterModel>();
            }

            paymentRegisterModelList = paymentRegisterModelList_Trans;

            if (paymentRegisterModelList.Any())
            {
                paymentRegisterModelList.Add(new PaymentRegisterModel()
                {
                    SequenceNo = 3,
                    SrNo = paymentRegisterModelList.Max(w => w.SrNo)+1,
                    DocumentNo = "Total Amount",
                    DocumentDate = toDate,
                    Amount = paymentRegisterModelList.Sum(w => w.Amount),
                });

                return paymentRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return paymentRegisterModelList;
        }

        private async Task<IList<PaymentRegisterModel>> GetTransactionList(DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<PaymentRegisterModel> paymentRegisterModelList = null;

                paymentRegisterModelList = _dbContext
                            .Paymentvoucherdetails
                            .Include(i => i.ParticularLedger)
                            .Include(i => i.PurchaseInvoice)
                            .Include(i => i.DebitNote)
                            .Include(i => i.PaymentVoucher).ThenInclude(i => i.AccountLedger)
                            .Include(i => i.PaymentVoucher).ThenInclude(i => i.Currency)
                            .Where(w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved
                                && w.PaymentVoucher.FinancialYearId == financialYearId
                                && w.PaymentVoucher.CompanyId == companyId
                                && w.PaymentVoucher.VoucherDate >= fromDate
                                && w.PaymentVoucher.VoucherDate <= toDate
                                )
                            .ToList()
                            .Select((row, index) => new PaymentRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                DocumentDetId = row.PaymentVoucherDetId,
                                DocumentId = row.PaymentVoucherId,
                                DocumentNo = row.PaymentVoucher.VoucherNo,
                                DocumentDate = row.PaymentVoucher.VoucherDate,
                                ChequeNo = row.PaymentVoucher.ChequeNo,
                                ChequeDate = row.PaymentVoucher.ChequeDate,
                                BankName = (null != row.PaymentVoucher ? (null != row.PaymentVoucher.AccountLedger ? row.PaymentVoucher.AccountLedger.LedgerName : "") : ""),
                                CurrencyId = row.PaymentVoucher.CurrencyId,
                                CurrencyCode = (null != row.PaymentVoucher ? (null != row.PaymentVoucher.Currency ? row.PaymentVoucher.Currency.CurrencyCode : "") : ""),
                                ExchangeRate = row.PaymentVoucher.ExchangeRate,
                                ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                                Narration = row.Narration,
                                PurchaseInvoiceId = row.PurchaseInvoiceId,
                                DebitNoteId = row.DebitNoteId,
                                InvoiceType = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? "Purchase Invoice"
                                                : row.DebitNoteId != 0 && row.DebitNoteId != null ? "Debit Note" : "",
                                InvoiceNo = row.PurchaseInvoiceId != 0 && row.PurchaseInvoiceId != null ? (null != row.PurchaseInvoice ? row.PurchaseInvoice.InvoiceNo : "")
                                            : row.DebitNoteId != 0 && row.DebitNoteId != null ? (null != row.DebitNote ? row.DebitNote.DebitNoteNo : "") : "",
                                Amount_FC = row.AmountFc,
                                Amount = row.Amount
                            })
                            .ToList();

                if (paymentRegisterModelList==null)
                {
                    paymentRegisterModelList= new List<PaymentRegisterModel>();
                }

                return paymentRegisterModelList; // returns.
            });
        }

    }
}

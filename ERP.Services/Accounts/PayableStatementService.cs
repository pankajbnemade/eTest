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
    public class PayableStatementService : IPayableStatement
    {
        ErpDbContext dbContext;

        public PayableStatementService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<PayableStatementModel>> GetReport(SearchFilterPayableStatementModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<PayableStatementModel> payableStatementModelList = await GetList(searchFilterModel.LedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<PayableStatementModel> resultModel = new DataTableResultModel<PayableStatementModel>();

            if (null != payableStatementModelList && payableStatementModelList.Any())
            {
                resultModel = new DataTableResultModel<PayableStatementModel>();
                resultModel.ResultList = payableStatementModelList;
                resultModel.TotalResultCount = payableStatementModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PayableStatementModel>();
                resultModel.ResultList = new List<PayableStatementModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<PayableStatementModel>> GetList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<PayableStatementModel> payableStatementModelList = new List<PayableStatementModel>();

            IList<PayableStatementModel> payableStatementModelList_Trans = null;

            payableStatementModelList_Trans = await GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            if (payableStatementModelList_Trans==null)
            {
                payableStatementModelList_Trans= new List<PayableStatementModel>();
            }

            payableStatementModelList = payableStatementModelList_Trans;

            payableStatementModelList.Add(new PayableStatementModel()
            {
                SequenceNo = 3,
                SrNo = payableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Amount",
                InvoiceDate = null,
                NetAmount = payableStatementModelList.Sum(w => w.NetAmount),
                PaidAmount = payableStatementModelList.Sum(w => w.PaidAmount),
                OutstandingAmount = payableStatementModelList.Sum(w => w.OutstandingAmount),
            });

            decimal advanceamount = await GetAdvanceAmount(ledgerId, fromDate, toDate, financialYearId, companyId);

            payableStatementModelList.Add(new PayableStatementModel()
            {
                SequenceNo = 4,
                SrNo = payableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Advance Amount",
                InvoiceDate = null,
                NetAmount = 0,
                PaidAmount = advanceamount,
                OutstandingAmount = 0,
            });

            payableStatementModelList.Add(new PayableStatementModel()
            {
                SequenceNo = 5,
                SrNo = payableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Outstanding Amount",
                InvoiceDate = null,
                NetAmount = 0,
                PaidAmount = payableStatementModelList.Sum(w => w.PaidAmount),
                OutstandingAmount = payableStatementModelList.Where(w => w.SequenceNo==2).Sum(w => w.OutstandingAmount),
            });

            return payableStatementModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
        }

        private async Task<IList<PayableStatementModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {

                IList<PayableStatementModel> payableStatementModelList = null;
                try
                {
                    payableStatementModelList
                            = dbContext.Purchaseinvoices
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                        && w.CompanyId == companyId
                                        && w.SupplierLedgerId == ledgerId
                                        && w.InvoiceDate >= fromDate
                                        && w.InvoiceDate <= toDate
                                    )
                                .Include(i => i.Currency)
                                .Include(i => i.Paymentvoucherdetails)  //.ThenInclude(i => i.PaymentVoucher).Where(x => x.Paymentvoucherdetails.All(w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved))
                                .Include(i => i.Journalvoucherdetails)  //.Include(i => i.Journalvoucherdetails.AsQueryable().All(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved))
                                 .Include(i => i.Advanceadjustmentdetails)  //.Include(i => i.Advanceadjustmentdetails.AsQueryable().All(w => w.AdvanceAdjustment.StatusId == (int)DocumentStatus.Approved))
                                .ToList()
                                .Select((row, Index) => new PayableStatementModel
                                {
                                    SequenceNo=2,
                                    SrNo=Index,
                                    PurchaseInvoiceId=row.PurchaseInvoiceId,
                                    InvoiceType="Purchase Invoice",
                                    InvoiceNo=row.InvoiceNo,
                                    InvoiceDate=row.InvoiceDate,
                                    SupplierReferenceNo=row.SupplierReferenceNo,
                                    SupplierReferenceDate=row.SupplierReferenceDate,
                                    CreditLimitDays=row.CreditLimitDays,
                                    PaymentTerm=row.PaymentTerm,
                                    Remark=row.Remark,
                                    CurrencyId=row.CurrencyId,
                                    ExchangeRate=row.ExchangeRate,
                                    CurrencyCode=row.Currency.CurrencyCode,
                                    NetAmountFc=row.NetAmountFc,
                                    NetAmount=row.NetAmount,
                                    PaidAmount= (null != row.Paymentvoucherdetails ? row.Paymentvoucherdetails.Sum(s => s.Amount) : 0)
                                                + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.CreditAmount) : 0)
                                                + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                                ,
                                    OutstandingAmount = row.NetAmount -
                                                    (
                                                      (null != row.Paymentvoucherdetails ? row.Paymentvoucherdetails.Sum(s => s.Amount) : 0)
                                                    + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.CreditAmount) : 0)
                                                    + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                                    ),
                                    OutstandingDays= (int)(Convert.ToDateTime(DateTime.Now)-Convert.ToDateTime(row.InvoiceDate)).TotalDays,
                                    DueDate= Convert.ToDateTime(row.InvoiceDate).AddDays(row.CreditLimitDays),
                                })
                            .Union
                            (
                                dbContext.Debitnotes
                                 .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                        && w.CompanyId == companyId
                                        && w.PartyLedgerId == ledgerId
                                        && w.DebitNoteDate >= fromDate
                                        && w.DebitNoteDate <= toDate
                                    )
                                .Include(i => i.Currency)
                                .Include(i => i.Paymentvoucherdetails)  //.ThenInclude(i => i.PaymentVoucher).Where(x => x.Paymentvoucherdetails.All(w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved))
                                .Include(i => i.Journalvoucherdetails)  //.Include(i => i.Journalvoucherdetails.AsQueryable().All(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved))
                                .Include(i => i.Advanceadjustmentdetails)  //.Include(i => i.Advanceadjustmentdetails.AsQueryable().All(w => w.AdvanceAdjustment.StatusId == (int)DocumentStatus.Approved))
                                .ToList()
                                .Select((row, Index) => new PayableStatementModel
                                {
                                    SequenceNo=2,
                                    SrNo=Index,
                                    DebitNoteId=row.DebitNoteId,
                                    InvoiceType="Debit Note",
                                    InvoiceNo=row.DebitNoteNo,
                                    InvoiceDate=row.DebitNoteDate,
                                    SupplierReferenceNo=row.PartyReferenceNo,
                                    SupplierReferenceDate=row.PartyReferenceDate,
                                    CreditLimitDays=row.CreditLimitDays,
                                    PaymentTerm=row.PaymentTerm,
                                    Remark=row.Remark,
                                    CurrencyId=row.CurrencyId,
                                    ExchangeRate=row.ExchangeRate,
                                    CurrencyCode=row.Currency.CurrencyCode,
                                    NetAmountFc=row.NetAmountFc,
                                    NetAmount=row.NetAmount,
                                    PaidAmount= (null != row.Paymentvoucherdetails ? row.Paymentvoucherdetails.Sum(s => s.Amount) : 0)
                                                + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.CreditAmount) : 0)
                                                + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0),
                                    OutstandingAmount = row.NetAmount -
                                                    (
                                                      (null != row.Paymentvoucherdetails ? row.Paymentvoucherdetails.Sum(s => s.Amount) : 0)
                                                    + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.CreditAmount) : 0)
                                                    + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                                    ),
                                    OutstandingDays= EF.Functions.DateDiffDay(row.DebitNoteDate, DateTime.Now),
                                    DueDate= Convert.ToDateTime(row.DebitNoteDate).AddDays(row.CreditLimitDays),
                                })
                            )
                            .ToList();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message.ToString());
                }

                payableStatementModelList   =  payableStatementModelList
                                                .Where(w => w.OutstandingAmount != 0).ToList();

                if (payableStatementModelList==null)
                {
                    payableStatementModelList= new List<PayableStatementModel>();
                }

                return payableStatementModelList; // returns.
            });
        }


        private async Task<decimal> GetAdvanceAmount(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                decimal advanceAmount = 0;

                try
                {
                    var advanceList
                                = dbContext.Paymentvoucherdetails
                                    .Include(i => i.PaymentVoucher)
                                    .Where(w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved
                                            && w.PaymentVoucher.CompanyId == companyId
                                            && w.TransactionTypeId == (int)TransactionType.Advance
                                            && w.ParticularLedgerId == ledgerId
                                            //&& w.PaymentVoucher.VoucherDate >= fromDate
                                            && w.PaymentVoucher.VoucherDate <= toDate
                                        )
                                    .Include(i => i.Advanceadjustments)
                                    .ToList()
                                    .Select((row, Index) => new
                                    {
                                        VoucherType = "Payment Voucher",
                                        VoucherNo = row.PaymentVoucher.VoucherNo,
                                        AdvanceAmount = (row.Amount -
                                                         (null != row.Advanceadjustments ? row.Advanceadjustments.Sum(s => s.Amount) : 0)
                                                        ),
                                    })
                                .Union
                                (
                                    dbContext.Journalvoucherdetails
                                      .Include(i => i.JournalVoucher)
                                     .Where(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved
                                            && w.JournalVoucher.CompanyId == companyId
                                            && w.TransactionTypeId == (int)TransactionType.Advance
                                            && w.ParticularLedgerId == ledgerId
                                            && w.JournalVoucher.VoucherDate >= fromDate
                                            && w.JournalVoucher.VoucherDate <= toDate
                                        )
                                    .ToList()
                                    .Select((row, Index) => new
                                    {
                                        VoucherType = "Journal Voucher",
                                        VoucherNo = row.JournalVoucher.VoucherNo,
                                        AdvanceAmount = row.CreditAmount,
                                    })
                                )
                                .ToList();

                    advanceList = advanceList.Where(w => w.AdvanceAmount != 0).ToList();


                    if (advanceList==null)
                    {
                        advanceAmount= 0;
                    }
                    else
                    {
                        advanceAmount=advanceList.Sum(s => s.AdvanceAmount);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }

                return advanceAmount; // returns.
            });
        }

    }
}

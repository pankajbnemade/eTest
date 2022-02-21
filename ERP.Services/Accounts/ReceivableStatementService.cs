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
    public class ReceivableStatementService : IReceivableStatement
    {
        ErpDbContext dbContext;

        public ReceivableStatementService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<ReceivableStatementModel>> GetReport(SearchFilterReceivableStatementModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            //if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            //if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<ReceivableStatementModel> receivableStatementModelList = await GetList(searchFilterModel.LedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<ReceivableStatementModel> resultModel = new DataTableResultModel<ReceivableStatementModel>();

            if (null != receivableStatementModelList && receivableStatementModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceivableStatementModel>();
                resultModel.ResultList = receivableStatementModelList;
                resultModel.TotalResultCount = receivableStatementModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ReceivableStatementModel>();
                resultModel.ResultList = new List<ReceivableStatementModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<ReceivableStatementModel>> GetList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<ReceivableStatementModel> receivableStatementModelList = new List<ReceivableStatementModel>();

            IList<ReceivableStatementModel> receivableStatementModelList_Trans = null;

            receivableStatementModelList_Trans = await GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            if (receivableStatementModelList_Trans == null)
            {
                receivableStatementModelList_Trans = new List<ReceivableStatementModel>();
            }

            receivableStatementModelList = receivableStatementModelList_Trans;

            receivableStatementModelList.Add(new ReceivableStatementModel()
            {
                SequenceNo = 3,
                SrNo = receivableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Amount",
                InvoiceDate = null,
                NetAmount = receivableStatementModelList.Sum(w => w.NetAmount),
                ReceivedAmount = receivableStatementModelList.Sum(w => w.ReceivedAmount),
                OutstandingAmount = receivableStatementModelList.Sum(w => w.OutstandingAmount),
            });

            decimal advanceamount = await GetAdvanceAmount(ledgerId, fromDate, toDate, financialYearId, companyId);

            receivableStatementModelList.Add(new ReceivableStatementModel()
            {
                SequenceNo = 4,
                SrNo = receivableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Advance Amount",
                InvoiceDate = null,
                NetAmount = 0,
                ReceivedAmount = advanceamount,
                OutstandingAmount = 0,
            });

            receivableStatementModelList.Add(new ReceivableStatementModel()
            {
                SequenceNo = 5,
                SrNo = receivableStatementModelList.Max(w => w.SrNo) + 1,
                InvoiceNo = "Total Outstanding Amount",
                InvoiceDate = null,
                NetAmount = 0,
                ReceivedAmount = receivableStatementModelList.Sum(w => w.ReceivedAmount),
                OutstandingAmount = receivableStatementModelList.Where(w => w.SequenceNo==2).Sum(w => w.OutstandingAmount),
            });

            return receivableStatementModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
        }

        private async Task<IList<ReceivableStatementModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {

                IList<ReceivableStatementModel> receivableStatementModelList = null;
                //try
                //{
                receivableStatementModelList
                    = dbContext.Salesinvoices
                    .Where(w => w.StatusId == (int)DocumentStatus.Approved
                            && w.CompanyId == companyId
                            && w.CustomerLedgerId == ledgerId
                            && w.InvoiceDate >= fromDate
                            && w.InvoiceDate <= toDate
                        )
                    .Include(i => i.Currency)
                    .Include(i => i.Receiptvoucherdetails)  //.ThenInclude(i => i.ReceiptVoucher).Where(x => x.Paymentvoucherdetails.All(w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved))
                    .Include(i => i.Journalvoucherdetails)  //.Include(i => i.Journalvoucherdetails.AsQueryable().All(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved))
                    .Include(i => i.Advanceadjustmentdetails)  //.Include(i => i.Advanceadjustmentdetails.AsQueryable().All(w => w.AdvanceAdjustment.StatusId == (int)DocumentStatus.Approved))
                    .ToList()
                    .Select((row, Index) => new ReceivableStatementModel
                    {
                        SequenceNo=2,
                        SrNo=Index,
                        SalesInvoiceId=row.SalesInvoiceId,
                        InvoiceType="Sales Invoice",
                        InvoiceNo=row.InvoiceNo,
                        InvoiceDate=row.InvoiceDate,
                        CustomerReferenceNo=row.CustomerReferenceNo,
                        CustomerReferenceDate=row.CustomerReferenceDate,
                        CreditLimitDays=row.CreditLimitDays,
                        PaymentTerm=row.PaymentTerm,
                        Remark=row.Remark,
                        CurrencyId=row.CurrencyId,
                        ExchangeRate=row.ExchangeRate,
                        CurrencyCode=row.Currency.CurrencyCode,
                        NetAmountFc=row.NetAmountFc,
                        NetAmount=row.NetAmount,
                        ReceivedAmount = (null != row.Receiptvoucherdetails ? row.Receiptvoucherdetails.Sum(s => s.Amount) : 0)
                                    + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.DebitAmount) : 0)
                                    + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                    ,
                        OutstandingAmount = row.NetAmount -
                                        (
                                            (null != row.Receiptvoucherdetails ? row.Receiptvoucherdetails.Sum(s => s.Amount) : 0)
                                        + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.DebitAmount) : 0)
                                        + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                        ),
                        OutstandingDays = (int)(Convert.ToDateTime(DateTime.Now)-Convert.ToDateTime(row.InvoiceDate)).TotalDays,
                        DueDate = Convert.ToDateTime(row.InvoiceDate).AddDays(row.CreditLimitDays),
                    })
                    .Union
                    (
                        dbContext.Creditnotes
                        .Where(w => w.StatusId == (int)DocumentStatus.Approved
                            && w.CompanyId == companyId
                            && w.PartyLedgerId == ledgerId
                            && w.CreditNoteDate >= fromDate
                            && w.CreditNoteDate <= toDate
                        )
                        .Include(i => i.Currency)
                        .Include(i => i.Receiptvoucherdetails)  //.ThenInclude(i => i.ReceiptVoucher).Where(x => x.Paymentvoucherdetails.All(w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved))
                        .Include(i => i.Journalvoucherdetails)  //.Include(i => i.Journalvoucherdetails.AsQueryable().All(w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved))
                        .Include(i => i.Advanceadjustmentdetails)  //.Include(i => i.Advanceadjustmentdetails.AsQueryable().All(w => w.AdvanceAdjustment.StatusId == (int)DocumentStatus.Approved))
                        .ToList()
                        .Select((row, Index) => new ReceivableStatementModel
                        {
                            SequenceNo=2,
                            SrNo=Index,
                            CreditNoteId=row.CreditNoteId,
                            InvoiceType="Credit Note",
                            InvoiceNo=row.CreditNoteNo,
                            InvoiceDate=row.CreditNoteDate,
                            CustomerReferenceNo=row.PartyReferenceNo,
                            CustomerReferenceDate=row.PartyReferenceDate,
                            CreditLimitDays=row.CreditLimitDays,
                            PaymentTerm=row.PaymentTerm,
                            Remark=row.Remark,
                            CurrencyId=row.CurrencyId,
                            ExchangeRate=row.ExchangeRate,
                            CurrencyCode=row.Currency.CurrencyCode,
                            NetAmountFc=row.NetAmountFc,
                            NetAmount=row.NetAmount,
                            ReceivedAmount= (null != row.Receiptvoucherdetails ? row.Receiptvoucherdetails.Sum(s => s.Amount) : 0)
                                        + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.DebitAmount) : 0)
                                        + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0),
                            OutstandingAmount = row.NetAmount -
                                            (
                                                (null != row.Receiptvoucherdetails ? row.Receiptvoucherdetails.Sum(s => s.Amount) : 0)
                                            + (null != row.Journalvoucherdetails ? row.Journalvoucherdetails.Sum(s => s.DebitAmount) : 0)
                                            + (null != row.Advanceadjustmentdetails ? row.Advanceadjustmentdetails.Sum(s => s.Amount) : 0)
                                            ),
                            OutstandingDays= EF.Functions.DateDiffDay(row.CreditNoteDate, DateTime.Now),
                            DueDate= Convert.ToDateTime(row.CreditNoteDate).AddDays(row.CreditLimitDays),
                        })
                    )
                    .ToList();
                //}
                //catch (Exception ex)
                //{
                //    Console.Write(ex.Message.ToString());
                //}

                receivableStatementModelList = receivableStatementModelList.Where(w => w.OutstandingAmount != 0).ToList();

                if (receivableStatementModelList == null)
                {
                    receivableStatementModelList = new List<ReceivableStatementModel>();
                }

                return receivableStatementModelList; // returns.
            });
        }


        private async Task<decimal> GetAdvanceAmount(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                decimal advanceAmount = 0;

                //try
                //{
                var advanceList
                    = dbContext.Receiptvoucherdetails
                    .Include(i => i.ReceiptVoucher)
                    .Where(w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved
                            && w.ReceiptVoucher.CompanyId == companyId
                            && w.TransactionTypeId == (int)TransactionType.Advance
                            && w.ParticularLedgerId == ledgerId
                            //&& w.ReceiptVoucher.VoucherDate >= fromDate
                            && w.ReceiptVoucher.VoucherDate <= toDate
                        )
                    .Include(i => i.Advanceadjustments)
                    .ToList()
                    .Select((row, Index) => new
                    {
                        VoucherType = "Receipt Voucher",
                        VoucherNo = row.ReceiptVoucher.VoucherNo,
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
                            //&& w.JournalVoucher.VoucherDate >= fromDate
                            && w.JournalVoucher.VoucherDate <= toDate
                        )
                        .ToList()
                        .Select((row, Index) => new
                        {
                            VoucherType = "Journal Voucher",
                            VoucherNo = row.JournalVoucher.VoucherNo,
                            AdvanceAmount = row.DebitAmount,
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
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message.ToString());
                //}

                return advanceAmount; // returns.
            });
        }

    }
}

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
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TrialBalanceReportService : ITrialBalanceReport
    {
        private readonly ErpDbContext _dbContext;

        public TrialBalanceReportService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<TrialBalanceReportModel>> GetReport(SearchFilterTrialBalanceReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            DataTableResultModel<TrialBalanceReportModel> resultModel = new DataTableResultModel<TrialBalanceReportModel>();

            try
            {

                if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

                if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

                IList<TrialBalanceReportModel> trialBalanceReportModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.ReportType, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

                if (null != trialBalanceReportModelList && trialBalanceReportModelList.Any())
                {
                    resultModel = new DataTableResultModel<TrialBalanceReportModel>();
                    resultModel.ResultList = trialBalanceReportModelList;
                    resultModel.TotalResultCount = trialBalanceReportModelList.Count();
                }
                else
                {
                    resultModel = new DataTableResultModel<TrialBalanceReportModel>();
                    resultModel.ResultList = new List<TrialBalanceReportModel>();
                    resultModel.TotalResultCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return resultModel; // returns.
        }

        private async Task<IList<TrialBalanceReportModel>> GetList(DateTime fromDate, DateTime toDate, string reportType, int financialYearId, int companyId)
        {
            IList<TrialBalanceReportModel> trialBalanceReportModelList = new List<TrialBalanceReportModel>();

            IList<TrialBalanceReportModel> trialBalanceReportModelList_Trans = null;

            trialBalanceReportModelList_Trans = await GetTransactionList(fromDate, toDate, reportType, financialYearId, companyId);

            if (trialBalanceReportModelList_Trans == null)
            {
                trialBalanceReportModelList_Trans= new List<TrialBalanceReportModel>();
            }

            trialBalanceReportModelList = trialBalanceReportModelList_Trans;

            if (trialBalanceReportModelList.Any())
            {
                decimal debitAmount = 0;
                decimal creditAmount = 0;
                decimal debitAmount_diff = 0;
                decimal creditAmount_diff = 0;

                debitAmount = trialBalanceReportModelList.Sum(w => w.DebitAmount);
                creditAmount = trialBalanceReportModelList.Sum(w => w.CreditAmount);

                if (creditAmount > debitAmount)
                {
                    debitAmount_diff = creditAmount - debitAmount;
                }
                else
                {
                    creditAmount_diff = debitAmount - creditAmount;
                }

                trialBalanceReportModelList.Add(new TrialBalanceReportModel()
                {
                    SequenceNo = 2,
                    SrNo = 1,
                    LedgerCode = "",
                    ParticularLedgerName = "Opening Balance Difference",
                    DebitAmount = debitAmount_diff,
                    CreditAmount = creditAmount_diff,
                    GroupOrLedger = ""
                });

                trialBalanceReportModelList.Add(new TrialBalanceReportModel()
                {
                    SequenceNo = 3,
                    SrNo = 1,
                    LedgerCode = "",
                    ParticularLedgerName = "Total Amount",
                    DebitAmount = debitAmount + debitAmount_diff,
                    CreditAmount = creditAmount + creditAmount_diff,
                    GroupOrLedger = ""
                });

                return trialBalanceReportModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return trialBalanceReportModelList;
        }

        private async Task<IList<TrialBalanceReportModel>> GetTransactionList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {

            IList<TrialBalanceReportModel> trialBalanceReportModelList = null;

            IList<TrialBalanceReportModel> purchaseInvoiceModelList = null;

            IList<TrialBalanceReportModel> salesInvoiceModelList = null;

            IList<TrialBalanceReportModel> creditNoteModelList = null;

            IList<TrialBalanceReportModel> debitNoteModelList = null;

            IList<TrialBalanceReportModel> paymentVoucherDetailModelList = null;

            IList<TrialBalanceReportModel> receiptVoucherDetailModelList = null;

            IList<TrialBalanceReportModel> paymentVoucherModelList = null;

            IList<TrialBalanceReportModel> receiptVoucherModelList = null;

            IList<TrialBalanceReportModel> contraVoucherDetailModelList = null;

            IList<TrialBalanceReportModel> journalVoucherDetailModelList = null;


            //////-----------------------############################## Opening Balance

            IList<Ledgerfinancialyearbalance> ledgerFinancialYearBalance = await _dbContext.Ledgerfinancialyearbalances
                                                                   .Where(w => w.FinancialYearId == financialYearId && w.CompanyId == companyId)
                                                                   .ToListAsync();

            //////-----------------------############################## Transactions

            IList<Purchaseinvoice> purchaseInvoices = await _dbContext.Purchaseinvoices
                                                            .Where(w => w.FinancialYearId == financialYearId
                                                                        && w.CompanyId == companyId
                                                                        && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                        &&  w.InvoiceDate >= fromDate && w.InvoiceDate <= toDate
                                                                        )
                                                            .ToListAsync();

            IList<Salesinvoice> salesInvoices = await _dbContext.Salesinvoices
                                                            .Where(w => w.FinancialYearId == financialYearId
                                                                        && w.CompanyId == companyId
                                                                        && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                        &&  w.InvoiceDate >= fromDate && w.InvoiceDate <= toDate
                                                                        )
                                                            .ToListAsync();

            IList<Debitnote> debitNotes = await _dbContext.Debitnotes
                                                            .Where(w => w.FinancialYearId == financialYearId
                                                                        && w.CompanyId == companyId
                                                                        && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                        && w.DebitNoteDate >= fromDate && w.DebitNoteDate <= toDate
                                                                        )
                                                            .ToListAsync();

            IList<Creditnote> creditNotes = await _dbContext.Creditnotes
                                                            .Where(w => w.FinancialYearId == financialYearId
                                                                        && w.CompanyId == companyId
                                                                        && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                        && w.CreditNoteDate >= fromDate && w.CreditNoteDate <= toDate
                                                                        )
                                                            .ToListAsync();

            IList<Paymentvoucher> paymentVouchers = await _dbContext.Paymentvouchers
                                                           .Where(w => w.FinancialYearId == financialYearId
                                                                       && w.CompanyId == companyId
                                                                       && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                       && w.VoucherDate >= fromDate && w.VoucherDate <= toDate
                                                                       )
                                                           .ToListAsync();

            IList<Receiptvoucher> receiptVouchers = await _dbContext.Receiptvouchers
                                                           .Where(w => w.FinancialYearId == financialYearId
                                                                       && w.CompanyId == companyId
                                                                       && w.StatusId  ==  (int)DocumentStatus.Approved
                                                                       && w.VoucherDate >= fromDate && w.VoucherDate <= toDate
                                                                       )
                                                           .ToListAsync();

            IList<Paymentvoucherdetail> paymentVoucherDetails = await _dbContext.Paymentvoucherdetails
                                                            .Include(w => w.PaymentVoucher)
                                                           .Where(w => w.PaymentVoucher.FinancialYearId == financialYearId
                                                                       && w.PaymentVoucher.CompanyId == companyId
                                                                       && w.PaymentVoucher.StatusId  ==  (int)DocumentStatus.Approved
                                                                       && w.PaymentVoucher.VoucherDate >= fromDate && w.PaymentVoucher.VoucherDate <= toDate
                                                                       )
                                                           .ToListAsync();

            IList<Receiptvoucherdetail> receiptVoucherDetails = await _dbContext.Receiptvoucherdetails
                                                            .Include(w => w.ReceiptVoucher)
                                                           .Where(w => w.ReceiptVoucher.FinancialYearId == financialYearId
                                                                       && w.ReceiptVoucher.CompanyId == companyId
                                                                       && w.ReceiptVoucher.StatusId  ==  (int)DocumentStatus.Approved
                                                                       && w.ReceiptVoucher.VoucherDate >= fromDate && w.ReceiptVoucher.VoucherDate <= toDate
                                                                       )
                                                           .ToListAsync();

            IList<Contravoucherdetail> contraVoucherDetails = await _dbContext.Contravoucherdetails
                                                            .Include(w => w.ContraVoucher)
                                                          .Where(w => w.ContraVoucher.FinancialYearId == financialYearId
                                                                      && w.ContraVoucher.CompanyId == companyId
                                                                      && w.ContraVoucher.StatusId  ==  (int)DocumentStatus.Approved
                                                                      && w.ContraVoucher.VoucherDate >= fromDate && w.ContraVoucher.VoucherDate <= toDate
                                                                      )
                                                          .ToListAsync();

            IList<Journalvoucherdetail> journalVoucherDetails = await _dbContext.Journalvoucherdetails
                                                            .Include(w => w.JournalVoucher)
                                                          .Where(w => w.JournalVoucher.FinancialYearId == financialYearId
                                                                      && w.JournalVoucher.CompanyId == companyId
                                                                      && w.JournalVoucher.StatusId  ==  (int)DocumentStatus.Approved
                                                                      && w.JournalVoucher.VoucherDate >= fromDate && w.JournalVoucher.VoucherDate <= toDate
                                                                      )
                                                          .ToListAsync();


            if (null != ledgerFinancialYearBalance && ledgerFinancialYearBalance.Count > 0)
            {
                trialBalanceReportModelList = ledgerFinancialYearBalance
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.LedgerId,
                                        CreditAmount = row.CreditAmount,
                                        DebitAmount = row.DebitAmount,
                                    })
                                    .ToList();
            }


            if (null != purchaseInvoices && purchaseInvoices.Count > 0)
            {
                purchaseInvoiceModelList = purchaseInvoices
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.SupplierLedgerId,
                                        DocumentId = row.PurchaseInvoiceId,
                                        DocumentType = "Purchase Invoice",
                                        //DocumentNo = row.InvoiceNo,
                                        //DocumentDate = row.InvoiceDate,
                                        //Amount_FC = row.NetAmountFc,
                                        //Amount = row.NetAmount,
                                        //CreditAmount_FC = row.NetAmountFc,
                                        CreditAmount = row.NetAmount,
                                        //PurchaseInvoiceId = row.PurchaseInvoiceId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.SupplierReferenceNo,
                                    })
                                    .ToList();
            }

            if (null != salesInvoices && salesInvoices.Count > 0)
            {
                salesInvoiceModelList = salesInvoices
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.CustomerLedgerId,
                                        DocumentId = row.SalesInvoiceId,
                                        DocumentType = "Sales Invoice",
                                        //DocumentNo = row.InvoiceNo,
                                        //DocumentDate = row.InvoiceDate,
                                        //Amount_FC = row.NetAmountFc,
                                        //Amount = row.NetAmount,
                                        //DebitAmount_FC = row.NetAmountFc,
                                        DebitAmount = row.NetAmount,
                                        //SalesInvoiceId = row.SalesInvoiceId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.CustomerReferenceNo,
                                    })
                                    .ToList();
            }

            if (null != debitNotes && debitNotes.Count > 0)
            {
                debitNoteModelList = debitNotes
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.PartyLedgerId,
                                        DocumentId = row.DebitNoteId,
                                        DocumentType = "Debit Note",
                                        //DocumentNo = row.DebitNoteNo,
                                        //DocumentDate = row.DebitNoteDate,
                                        //Amount_FC = row.NetAmountFc,
                                        //Amount = row.NetAmount,
                                        //DebitAmount_FC = row.NetAmountFc,
                                        DebitAmount = row.NetAmount,
                                        //DebitNoteId = row.DebitNoteId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.PartyReferenceNo,
                                        //OurReferenceNo = row.OurReferenceNo,
                                    })
                                    .ToList();
            }

            if (null != creditNotes && creditNotes.Count > 0)
            {
                creditNoteModelList = creditNotes
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.PartyLedgerId,
                                        DocumentId = row.CreditNoteId,
                                        DocumentType = "Credit Note",
                                        //DocumentNo = row.CreditNoteNo,
                                        //DocumentDate = row.CreditNoteDate,
                                        //Amount_FC = row.NetAmountFc,
                                        //Amount = row.NetAmount,
                                        //CreditAmount_FC = row.NetAmountFc,
                                        CreditAmount = row.NetAmount,
                                        //DebitNoteId = row.CreditNoteId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.PartyReferenceNo,
                                        //OurReferenceNo = row.OurReferenceNo,
                                    })
                                    .ToList();
            }

            if (null != paymentVouchers && paymentVouchers.Count > 0)
            {
                paymentVoucherModelList = paymentVouchers
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.AccountLedgerId,
                                        DocumentId = row.PaymentVoucherId,
                                        DocumentType = "Payment Voucher",
                                        //DocumentNo = row.VoucherNo,
                                        //DocumentDate = row.VoucherDate,
                                        //Amount_FC = row.AmountFc,
                                        //Amount = row.Amount,
                                        //CreditAmount_FC = row.AmountFc,
                                        CreditAmount = row.Amount,
                                        //PaymentVoucherId = row.PaymentVoucherId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.ChequeNo,
                                    })
                                    .ToList();
            }

            if (null != receiptVouchers && receiptVouchers.Count > 0)
            {
                receiptVoucherModelList = receiptVouchers
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.AccountLedgerId,
                                        DocumentId = row.ReceiptVoucherId,
                                        DocumentType = "Receipt Voucher",
                                        //DocumentNo = row.VoucherNo,
                                        //DocumentDate = row.VoucherDate,
                                        //Amount_FC = row.AmountFc,
                                        //Amount = row.Amount,
                                        //DebitAmount_FC = row.AmountFc,
                                        DebitAmount = row.Amount,
                                        //PaymentVoucherId = row.ReceiptVoucherId,
                                        //CurrencyId = row.CurrencyId,
                                        //CurrencyCode = row.Currency.CurrencyCode,
                                        //ExchangeRate = row.ExchangeRate,
                                        //PartyReferenceNo = row.ChequeNo,
                                    })
                                    .ToList();
            }

            if (null != paymentVoucherDetails && paymentVoucherDetails.Count > 0)
            {
                paymentVoucherDetailModelList = paymentVoucherDetails
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.ParticularLedgerId,
                                        DocumentId = row.PaymentVoucher.PaymentVoucherId,
                                        //DocumentType = "Payment Voucher",
                                        //DocumentNo = row.PaymentVoucher.VoucherNo,
                                        //DocumentDate = row.PaymentVoucher.VoucherDate,
                                        //Amount_FC = row.AmountFc,
                                        //Amount = row.Amount,
                                        //DebitAmount_FC = row.AmountFc,
                                        DebitAmount = row.Amount,
                                        //PaymentVoucherId = row.PaymentVoucher.PaymentVoucherId,
                                        //CurrencyId = row.PaymentVoucher.CurrencyId,
                                        //CurrencyCode = row.PaymentVoucher.Currency.CurrencyCode,
                                        //ExchangeRate = row.PaymentVoucher.ExchangeRate,
                                        //PartyReferenceNo = row.PaymentVoucher.ChequeNo,
                                    })
                                    .ToList();
            }

            if (null != receiptVoucherDetails && receiptVoucherDetails.Count > 0)
            {
                receiptVoucherDetailModelList = receiptVoucherDetails
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.ParticularLedgerId,
                                        DocumentId = row.ReceiptVoucher.ReceiptVoucherId,
                                        DocumentType = "Receipt Voucher",
                                        //DocumentNo = row.ReceiptVoucher.VoucherNo,
                                        //DocumentDate = row.ReceiptVoucher.VoucherDate,
                                        //Amount_FC = row.AmountFc,
                                        //Amount = row.Amount,
                                        //CreditAmount_FC = row.AmountFc,
                                        CreditAmount = row.Amount,
                                        //ReceiptVoucherId = row.ReceiptVoucher.ReceiptVoucherId,
                                        //CurrencyId = row.ReceiptVoucher.CurrencyId,
                                        //CurrencyCode = row.ReceiptVoucher.Currency.CurrencyCode,
                                        //ExchangeRate = row.ReceiptVoucher.ExchangeRate,
                                        //PartyReferenceNo = row.ReceiptVoucher.ChequeNo,
                                    })
                                    .ToList();
            }

            if (null != contraVoucherDetails && contraVoucherDetails.Count > 0)
            {
                contraVoucherDetailModelList = contraVoucherDetails
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.ParticularLedgerId,
                                        DocumentId = row.ContraVoucher.ContraVoucherId,
                                        DocumentType = "Contra Voucher",
                                        //DocumentNo = row.ContraVoucher.VoucherNo,
                                        //DocumentDate = row.ContraVoucher.VoucherDate,
                                        //Amount_FC = row.DebitAmountFc  ==  0 ? row.CreditAmountFc : row.DebitAmountFc,
                                        //Amount = row.DebitAmount  ==  0 ? row.CreditAmount : row.DebitAmount,
                                        //DebitAmount_FC = row.DebitAmountFc,
                                        DebitAmount = row.DebitAmount,
                                        //CreditAmount_FC = row.CreditAmountFc,
                                        CreditAmount = row.CreditAmount,
                                        //ContraVoucherId = row.ContraVoucher.ContraVoucherId,
                                        //CurrencyId = row.ContraVoucher.CurrencyId,
                                        //CurrencyCode = row.ContraVoucher.Currency.CurrencyCode,
                                        //ExchangeRate = row.ContraVoucher.ExchangeRate,
                                    })
                                    .ToList();
            }

            if (null != journalVoucherDetails && journalVoucherDetails.Count > 0)
            {
                journalVoucherDetailModelList = journalVoucherDetails
                                    .Select((row, index) => new TrialBalanceReportModel
                                    {
                                        LedgerId = row.ParticularLedgerId,
                                        DocumentId = row.JournalVoucher.JournalVoucherId,
                                        DocumentType = "Journal Voucher",
                                        //DocumentNo = row.JournalVoucher.VoucherNo,
                                        //DocumentDate = row.JournalVoucher.VoucherDate,
                                        //Amount_FC = row.DebitAmountFc  ==  0 ? row.CreditAmountFc : row.DebitAmountFc,
                                        //Amount = row.DebitAmount  ==  0 ? row.CreditAmount : row.DebitAmount,
                                        //DebitAmount_FC = row.DebitAmountFc,
                                        DebitAmount = row.DebitAmount,
                                        //CreditAmount_FC = row.CreditAmountFc,
                                        CreditAmount = row.CreditAmount,
                                        //JournalVoucherId = row.JournalVoucher.JournalVoucherId,
                                        //CurrencyId = row.JournalVoucher.CurrencyId,
                                        //CurrencyCode = row.JournalVoucher.Currency.CurrencyCode,
                                        //ExchangeRate = row.JournalVoucher.ExchangeRate,
                                    })
                                    .ToList();
            }

            if (trialBalanceReportModelList  ==  null) { trialBalanceReportModelList = new List<TrialBalanceReportModel>(); }

            if (purchaseInvoiceModelList  ==  null) { purchaseInvoiceModelList = new List<TrialBalanceReportModel>(); }

            if (salesInvoiceModelList  ==  null) { salesInvoiceModelList = new List<TrialBalanceReportModel>(); }

            if (debitNoteModelList  ==  null) { debitNoteModelList = new List<TrialBalanceReportModel>(); }

            if (creditNoteModelList  ==  null) { creditNoteModelList = new List<TrialBalanceReportModel>(); }

            if (paymentVoucherModelList  ==  null) { paymentVoucherModelList = new List<TrialBalanceReportModel>(); }

            if (receiptVoucherModelList  ==  null) { receiptVoucherModelList = new List<TrialBalanceReportModel>(); }

            if (paymentVoucherDetailModelList  ==  null) { paymentVoucherDetailModelList = new List<TrialBalanceReportModel>(); }

            if (receiptVoucherDetailModelList  ==  null) { receiptVoucherDetailModelList = new List<TrialBalanceReportModel>(); }

            if (contraVoucherDetailModelList  ==  null) { contraVoucherDetailModelList = new List<TrialBalanceReportModel>(); }

            if (journalVoucherDetailModelList  ==  null) { journalVoucherDetailModelList = new List<TrialBalanceReportModel>(); }

            trialBalanceReportModelList = (
                                    trialBalanceReportModelList
                                    .Union(purchaseInvoiceModelList)
                                    .Union(salesInvoiceModelList)
                                    .Union(debitNoteModelList)
                                    .Union(creditNoteModelList)
                                    .Union(paymentVoucherModelList)
                                    .Union(receiptVoucherModelList)
                                    .Union(paymentVoucherDetailModelList)
                                    .Union(receiptVoucherDetailModelList)
                                    .Union(contraVoucherDetailModelList)
                                    .Union(journalVoucherDetailModelList)
                                    )
                                    .ToList()
                                    .GroupBy(c => new
                                    {
                                        c.LedgerId
                                    })
                                    .Select(groups => new TrialBalanceReportModel
                                    {
                                        LedgerId = groups.Key.LedgerId,
                                        DebitAmount = groups.Sum(s => s.DebitAmount),
                                        CreditAmount = groups.Sum(s => s.CreditAmount),
                                    })
                                    .ToList();



            //////////////////////////////
            ///
            var query = await _dbContext.Ledgers
                                        .Include(w => w.ParentGroup) //level 2
                                        .ThenInclude(w => w.ParentGroup) //level 1
                                                                         //.ThenInclude(w => w.ParentGroup) //0 lxevel
                                        .Where(w => w.ParentGroupId != null)
                                        .ToListAsync();

            IList<TrialBalanceReportModel> ledgerList = query
                                                        .Select(
                                                        (row, index) => new TrialBalanceReportModel
                                                        {
                                                            SequenceNo = 1,
                                                            LedgerId = row.LedgerId,
                                                            LedgerCode = row.LedgerCode,
                                                            ParticularLedgerName = row.LedgerName,
                                                            ParentGroupId = (int)row.ParentGroupId,
                                                            IsGroup = Convert.ToBoolean(row.IsGroup),
                                                            GroupOrLedger = Convert.ToBoolean(row.IsGroup) ? "G" : "L",
                                                            //TopGroupId = (int)
                                                            //            (
                                                            //                row.ParentGroupId == 0 ? 0
                                                            //                : (row.ParentGroup.ParentGroupId == null ? row.ParentGroupId // group parent group
                                                            //                    : (row.ParentGroup.ParentGroup.ParentGroupId == null ? row.ParentGroup.ParentGroupId // leger parent group
                                                            //                    : row.ParentGroup.ParentGroup.ParentGroupId) // ledger
                                                            //                )
                                                            //            ),
                                                            LevelNo =   (
                                                                            Convert.ToBoolean(row.IsGroup) == false ? 3
                                                                            : (row.ParentGroupId == null ? 0
                                                                                : (row.ParentGroup.ParentGroupId == null ? 1
                                                                                    : (row.ParentGroup.ParentGroup.ParentGroupId == null ? 2 : 3)
                                                                                )
                                                                            )
                                                                        )
                                                        })
                                                        .ToList();



            if (trialBalanceReportModelList  ==  null) { trialBalanceReportModelList = new List<TrialBalanceReportModel>(); }

            if (ledgerList  ==  null) { ledgerList = new List<TrialBalanceReportModel>(); }

            if (null != ledgerList && ledgerList.Count > 0)
            {

                ledgerList.Where(w => w.IsGroup == false  && w.LevelNo == 3).ToList()
                            .ForEach(w => w.ClosingAmount =  trialBalanceReportModelList
                                               .Where(t => t.LedgerId == w.LedgerId).Sum(t => t.CreditAmount - t.DebitAmount)
                            );

                ledgerList.Where(w => w.IsGroup == true && w.LevelNo == 2).ToList()
                            .ForEach(w => w.ClosingAmount =  ledgerList
                                                    .Where(t => t.ParentGroupId == w.LedgerId).Sum(t => t.CreditAmount - t.DebitAmount)
                            );

                ledgerList.Where(w => w.IsGroup == true && w.LevelNo == 1).ToList()
                            .ForEach(w => w.ClosingAmount =  ledgerList
                                                   .Where(t => t.ParentGroupId == w.LedgerId).Sum(t => t.CreditAmount - t.DebitAmount)
                           );

                ledgerList.Where(w => w.ClosingAmount < 0).ToList().ForEach(w => w.DebitAmount = Math.Abs(w.ClosingAmount));

                ledgerList.Where(w => w.ClosingAmount >= 0).ToList().ForEach(w => w.CreditAmount = Math.Abs(w.ClosingAmount));

                //for ordering 

                //IList<TrialBalanceReportModel> ledgerList_SrNo = ledgerList.Where(w => w.LevelNo == 1).OrderBy(w => w.ParticularLedgerName).ThenBy(w => w.ParticularLedgerName).ToList()
                //                                                           .Select((row, index) => new TrialBalanceReportModel
                //                                                           {
                //                                                               SrNo = index,
                //                                                               LedgerId = row.LedgerId
                //                                                           }).ToList();

                //ledgerList.Where(w => w.LevelNo == 1).ToList()
                //        .ForEach(w => w.SrNo = ledgerList_SrNo.Where(s => s.LedgerId == w.LedgerId).Select(s => s.SrNo).FirstOrDefault());

                //IList<TrialBalanceReportModel> ledgerList_SrNo = ledgerList
                //                                                .GroupBy(g => new { g.LevelNo, g.ParentGroupId })
                //                                                .SelectMany(g => g.OrderBy(o => o.LevelNo).ThenBy(o => o.ParentGroupId).ThenBy(o => o.ParticularLedgerName))
                //                                                .Select((row, index) => new TrialBalanceReportModel { LedgerId = row.LedgerId, SrNo = index + 1 })
                //                                                .ToList();

                //IList<TrialBalanceReportModel> ledgerList_SrNo = ledgerList
                //                                                .OrderBy(o => o.LevelNo).ThenBy(o => o.ParentGroupId).ThenBy(o => o.ParticularLedgerName)
                //                                                .Select((row, index) => new TrialBalanceReportModel { LedgerId = row.LedgerId, SrNo = index + 1 })
                //                                                .ToList();

                //ledgerList.ToList()
                //        .ForEach(w => w.SrNo = ledgerList_SrNo.Where(s => s.LedgerId == w.LedgerId).Select(s => s.SrNo).FirstOrDefault());


                try
                {
                    IList<TrialBalanceReportModel> ledgerList_Final = new List<TrialBalanceReportModel>();

                    IList<TrialBalanceReportModel> ledgerList_L1 = ledgerList.Where(w => w.LevelNo == 1).ToList();

                    if (ledgerList_L1 != null)
                    {
                        TrialBalanceReportModel trialBalanceReportModel_New;

                        IList<TrialBalanceReportModel> ledgerList_L2;
                        IList<TrialBalanceReportModel> ledgerList_L3;

                        foreach (TrialBalanceReportModel trialBalanceReportModel_L1 in ledgerList_L1)
                        {
                            trialBalanceReportModel_New = new TrialBalanceReportModel()
                            {
                                SequenceNo  = trialBalanceReportModel_L1.SequenceNo,
                                SrNo        = (ledgerList_Final.Any() ? ledgerList_Final.Max(w => w.SrNo) : 0) + 1,
                                DocumentId  = trialBalanceReportModel_L1.DocumentId,
                                DocumentType    = trialBalanceReportModel_L1.DocumentType,
                                LedgerId    = trialBalanceReportModel_L1.LedgerId,
                                LedgerCode  = trialBalanceReportModel_L1.LedgerCode,
                                ParticularLedgerName    = trialBalanceReportModel_L1.ParticularLedgerName,
                                IsGroup    = trialBalanceReportModel_L1.IsGroup,
                                GroupOrLedger   = trialBalanceReportModel_L1.GroupOrLedger,
                                ParentGroupId   = trialBalanceReportModel_L1.ParentGroupId,
                                TopGroupId  = trialBalanceReportModel_L1.TopGroupId,
                                LevelNo     = trialBalanceReportModel_L1.LevelNo,
                                DebitAmount    = trialBalanceReportModel_L1.DebitAmount,
                                CreditAmount   = trialBalanceReportModel_L1.CreditAmount,
                                ClosingAmount  = trialBalanceReportModel_L1.ClosingAmount
                            };

                            ledgerList_Final.Add(trialBalanceReportModel_New);

                            // if ledger added directly under level 1 group

                            ledgerList_L3 = ledgerList.Where(w => w.LevelNo == 3  && w.ParentGroupId == trialBalanceReportModel_L1.LedgerId).ToList();

                            if (ledgerList_L3 != null)
                            {
                                foreach (TrialBalanceReportModel trialBalanceReportModel_L3 in ledgerList_L3)
                                {
                                    trialBalanceReportModel_New = new TrialBalanceReportModel()
                                    {
                                        SequenceNo  = trialBalanceReportModel_L3.SequenceNo,
                                        SrNo        = (ledgerList_Final.Any() ? ledgerList_Final.Max(w => w.SrNo) : 0) + 1,
                                        DocumentId  = trialBalanceReportModel_L3.DocumentId,
                                        DocumentType    = trialBalanceReportModel_L3.DocumentType,
                                        LedgerId    = trialBalanceReportModel_L3.LedgerId,
                                        LedgerCode  = trialBalanceReportModel_L3.LedgerCode,
                                        ParticularLedgerName    = trialBalanceReportModel_L3.ParticularLedgerName,
                                        IsGroup    = trialBalanceReportModel_L3.IsGroup,
                                        GroupOrLedger   = trialBalanceReportModel_L3.GroupOrLedger,
                                        ParentGroupId   = trialBalanceReportModel_L3.ParentGroupId,
                                        TopGroupId  = trialBalanceReportModel_L3.TopGroupId,
                                        LevelNo     = trialBalanceReportModel_L3.LevelNo,
                                        DebitAmount    = trialBalanceReportModel_L3.DebitAmount,
                                        CreditAmount   = trialBalanceReportModel_L3.CreditAmount,
                                        ClosingAmount  = trialBalanceReportModel_L3.ClosingAmount
                                    };

                                    ledgerList_Final.Add(trialBalanceReportModel_New);

                                } // trialBalanceReportModel_L3

                            } //ledgerList_L3 != null

                            ledgerList_L2 = ledgerList.Where(w => w.LevelNo == 2 && w.ParentGroupId == trialBalanceReportModel_L1.LedgerId).ToList();

                            if (ledgerList_L2 != null)
                            {
                                foreach (TrialBalanceReportModel trialBalanceReportModel_L2 in ledgerList_L2)
                                {
                                    trialBalanceReportModel_New = new TrialBalanceReportModel()
                                    {
                                        SequenceNo  = trialBalanceReportModel_L2.SequenceNo,
                                        SrNo        = (ledgerList_Final.Any() ? ledgerList_Final.Max(w => w.SrNo) : 0) + 1,
                                        DocumentId  = trialBalanceReportModel_L2.DocumentId,
                                        DocumentType    = trialBalanceReportModel_L2.DocumentType,
                                        LedgerId    = trialBalanceReportModel_L2.LedgerId,
                                        LedgerCode  = trialBalanceReportModel_L2.LedgerCode,
                                        ParticularLedgerName    = trialBalanceReportModel_L2.ParticularLedgerName,
                                        IsGroup    = trialBalanceReportModel_L2.IsGroup,
                                        GroupOrLedger   = trialBalanceReportModel_L2.GroupOrLedger,
                                        ParentGroupId   = trialBalanceReportModel_L2.ParentGroupId,
                                        TopGroupId  = trialBalanceReportModel_L2.TopGroupId,
                                        LevelNo     = trialBalanceReportModel_L2.LevelNo,
                                        DebitAmount    = trialBalanceReportModel_L2.DebitAmount,
                                        CreditAmount   = trialBalanceReportModel_L2.CreditAmount,
                                        ClosingAmount  = trialBalanceReportModel_L2.ClosingAmount
                                    };

                                    ledgerList_Final.Add(trialBalanceReportModel_New);

                                    ledgerList_L3 = ledgerList.Where(w => w.LevelNo == 3  && w.ParentGroupId == trialBalanceReportModel_L2.LedgerId).ToList();

                                    if (ledgerList_L3 != null)
                                    {
                                        foreach (TrialBalanceReportModel trialBalanceReportModel_L3 in ledgerList_L3)
                                        {
                                            trialBalanceReportModel_New = new TrialBalanceReportModel()
                                            {
                                                SequenceNo  = trialBalanceReportModel_L3.SequenceNo,
                                                SrNo        = (ledgerList_Final.Any() ? ledgerList_Final.Max(w => w.SrNo) : 0) + 1,
                                                DocumentId  = trialBalanceReportModel_L3.DocumentId,
                                                DocumentType    = trialBalanceReportModel_L3.DocumentType,
                                                LedgerId    = trialBalanceReportModel_L3.LedgerId,
                                                LedgerCode  = trialBalanceReportModel_L3.LedgerCode,
                                                ParticularLedgerName    = trialBalanceReportModel_L3.ParticularLedgerName,
                                                IsGroup    = trialBalanceReportModel_L3.IsGroup,
                                                GroupOrLedger   = trialBalanceReportModel_L3.GroupOrLedger,
                                                ParentGroupId   = trialBalanceReportModel_L3.ParentGroupId,
                                                TopGroupId  = trialBalanceReportModel_L3.TopGroupId,
                                                LevelNo     = trialBalanceReportModel_L3.LevelNo,
                                                DebitAmount    = trialBalanceReportModel_L3.DebitAmount,
                                                CreditAmount   = trialBalanceReportModel_L3.CreditAmount,
                                                ClosingAmount  = trialBalanceReportModel_L3.ClosingAmount
                                            };

                                            ledgerList_Final.Add(trialBalanceReportModel_New);

                                        } // trialBalanceReportModel_L3

                                    } //ledgerList_L3 != null

                                } //trialBalanceReportModel_L2

                            } // ledgerList_L2 != null

                        } //trialBalanceReportModel_L1
                    }

                    //ledgerList= ledgerList_Final.OrderBy(o => o.SrNo).ToList(); // returns.

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return ledgerList; // returns.
        } //function



    }
}
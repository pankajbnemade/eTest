using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class OpeningBalanceTransferService : Repository<Ledgerfinancialyearbalance>, IOpeningBalanceTransfer
    {
        private readonly ErpDbContext _dbContext;

        public OpeningBalanceTransferService(ErpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UpdateOpeningBalanceTransfer(OpeningBalanceTransferModel openingBalanceTransferModel)
        {
            bool isUpdated = false;

            //try
            //{
                DateTime fromDate_FY;
                DateTime toDate_FY;

                Financialyear financialyear = _dbContext.Financialyears.Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId).FirstOrDefault();

                if (financialyear == null)
                {
                    return isUpdated;
                }

                fromDate_FY = (DateTime)financialyear.FromDate;
                toDate_FY = (DateTime)financialyear.ToDate;

                IList<OpeningBalanceLedgerModel> openingBalanceLedgerModelList = null;

                IList<OpeningBalanceLedgerModel> purchaseInvoiceModelList = null;

                IList<OpeningBalanceLedgerModel> salesInvoiceModelList = null;

                IList<OpeningBalanceLedgerModel> creditNoteModelList = null;

                IList<OpeningBalanceLedgerModel> debitNoteModelList = null;

                IList<OpeningBalanceLedgerModel> paymentVoucherDetailModelList = null;

                IList<OpeningBalanceLedgerModel> receiptVoucherDetailModelList = null;

                IList<OpeningBalanceLedgerModel> paymentVoucherModelList = null;

                IList<OpeningBalanceLedgerModel> receiptVoucherModelList = null;

                IList<OpeningBalanceLedgerModel> contraVoucherDetailModelList = null;

                IList<OpeningBalanceLedgerModel> journalVoucherDetailModelList = null;

                IList<OpeningBalanceLedgerModel> openingBalanceModelList = null;


                //////-----------------------############################## Opening Balance

                IList<Ledgerfinancialyearbalance> ledgerFinancialYearBalance = await _dbContext.Ledgerfinancialyearbalances
                                                                       .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId && w.CompanyId==openingBalanceTransferModel.CompanyId)
                                                                       .ToListAsync();

                //////-----------------------############################## Transactions

                IList<Purchaseinvoice> purchaseInvoices = await _dbContext.Purchaseinvoices
                                                                .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                            && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                            && w.StatusId == (int)DocumentStatus.Approved
                                                                            &&  w.InvoiceDate >= fromDate_FY && w.InvoiceDate <= toDate_FY
                                                                            )
                                                                .ToListAsync();

                IList<Salesinvoice> salesInvoices = await _dbContext.Salesinvoices
                                                                .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                            && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                            && w.StatusId == (int)DocumentStatus.Approved
                                                                            &&  w.InvoiceDate >= fromDate_FY && w.InvoiceDate <= toDate_FY
                                                                            )
                                                                .ToListAsync();

                IList<Debitnote> debitNotes = await _dbContext.Debitnotes
                                                                .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                            && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                            && w.StatusId == (int)DocumentStatus.Approved
                                                                            && w.DebitNoteDate >= fromDate_FY && w.DebitNoteDate <= toDate_FY
                                                                            )
                                                                .ToListAsync();

                IList<Creditnote> creditNotes = await _dbContext.Creditnotes
                                                                .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                            && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                            && w.StatusId == (int)DocumentStatus.Approved
                                                                            && w.CreditNoteDate >= fromDate_FY && w.CreditNoteDate <= toDate_FY
                                                                            )
                                                                .ToListAsync();

                IList<Paymentvoucher> paymentVouchers = await _dbContext.Paymentvouchers
                                                               .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                           && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                           && w.StatusId == (int)DocumentStatus.Approved
                                                                           && w.VoucherDate >= fromDate_FY && w.VoucherDate <= toDate_FY
                                                                           )
                                                               .ToListAsync();

                IList<Receiptvoucher> receiptVouchers = await _dbContext.Receiptvouchers
                                                               .Where(w => w.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                           && w.CompanyId==openingBalanceTransferModel.CompanyId
                                                                           && w.StatusId == (int)DocumentStatus.Approved
                                                                           && w.VoucherDate >= fromDate_FY && w.VoucherDate <= toDate_FY
                                                                           )
                                                               .ToListAsync();

                IList<Paymentvoucherdetail> paymentVoucherDetails = await _dbContext.Paymentvoucherdetails
                                                                .Include(w => w.PaymentVoucher)
                                                               .Where(w => w.PaymentVoucher.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                           && w.PaymentVoucher.CompanyId==openingBalanceTransferModel.CompanyId
                                                                           && w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved
                                                                           && w.PaymentVoucher.VoucherDate >= fromDate_FY && w.PaymentVoucher.VoucherDate <= toDate_FY
                                                                           )
                                                               .ToListAsync();

                IList<Receiptvoucherdetail> receiptVoucherDetails = await _dbContext.Receiptvoucherdetails
                                                                .Include(w => w.ReceiptVoucher)
                                                               .Where(w => w.ReceiptVoucher.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                           && w.ReceiptVoucher.CompanyId==openingBalanceTransferModel.CompanyId
                                                                           && w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved
                                                                           && w.ReceiptVoucher.VoucherDate >= fromDate_FY && w.ReceiptVoucher.VoucherDate <= toDate_FY
                                                                           )
                                                               .ToListAsync();

                IList<Contravoucherdetail> contraVoucherDetails = await _dbContext.Contravoucherdetails
                                                                .Include(w => w.ContraVoucher)
                                                              .Where(w => w.ContraVoucher.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                          && w.ContraVoucher.CompanyId==openingBalanceTransferModel.CompanyId
                                                                          && w.ContraVoucher.StatusId == (int)DocumentStatus.Approved
                                                                          && w.ContraVoucher.VoucherDate >= fromDate_FY && w.ContraVoucher.VoucherDate <= toDate_FY
                                                                          )
                                                              .ToListAsync();

                IList<Journalvoucherdetail> journalVoucherDetails = await _dbContext.Journalvoucherdetails
                                                                .Include(w => w.JournalVoucher)
                                                              .Where(w => w.JournalVoucher.FinancialYearId==openingBalanceTransferModel.FromYearId
                                                                          && w.JournalVoucher.CompanyId==openingBalanceTransferModel.CompanyId
                                                                          && w.JournalVoucher.StatusId == (int)DocumentStatus.Approved
                                                                          && w.JournalVoucher.VoucherDate >= fromDate_FY && w.JournalVoucher.VoucherDate <= toDate_FY
                                                                          )
                                                              .ToListAsync();


                if (null != ledgerFinancialYearBalance && ledgerFinancialYearBalance.Count > 0)
                {
                    openingBalanceModelList = ledgerFinancialYearBalance
                                        .Select((row, index) => new OpeningBalanceLedgerModel
                                        {
                                            LedgerId = row.LedgerId,
                                            DocumentType = "Opening Balance",
                                            CreditAmount = row.CreditAmount,
                                            DebitAmount = row.DebitAmount,
                                        })
                                        .ToList();
                }


                if (null != purchaseInvoices && purchaseInvoices.Count > 0)
                {
                    purchaseInvoiceModelList = purchaseInvoices
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
                                        {
                                            LedgerId = row.ParticularLedgerId,
                                            DocumentId = row.ContraVoucher.ContraVoucherId,
                                            DocumentType = "Contra Voucher",
                                            //DocumentNo = row.ContraVoucher.VoucherNo,
                                            //DocumentDate = row.ContraVoucher.VoucherDate,
                                            //Amount_FC = row.DebitAmountFc == 0 ? row.CreditAmountFc : row.DebitAmountFc,
                                            //Amount = row.DebitAmount == 0 ? row.CreditAmount : row.DebitAmount,
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
                                        .Select((row, index) => new OpeningBalanceLedgerModel
                                        {
                                            LedgerId = row.ParticularLedgerId,
                                            DocumentId = row.JournalVoucher.JournalVoucherId,
                                            DocumentType = "Journal Voucher",
                                            //DocumentNo = row.JournalVoucher.VoucherNo,
                                            //DocumentDate = row.JournalVoucher.VoucherDate,
                                            //Amount_FC = row.DebitAmountFc == 0 ? row.CreditAmountFc : row.DebitAmountFc,
                                            //Amount = row.DebitAmount == 0 ? row.CreditAmount : row.DebitAmount,
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

                if (openingBalanceModelList == null) { openingBalanceModelList = new List<OpeningBalanceLedgerModel>(); }

                if (purchaseInvoiceModelList == null) { purchaseInvoiceModelList = new List<OpeningBalanceLedgerModel>(); }

                if (salesInvoiceModelList == null) { salesInvoiceModelList = new List<OpeningBalanceLedgerModel>(); }

                if (debitNoteModelList == null) { debitNoteModelList = new List<OpeningBalanceLedgerModel>(); }

                if (creditNoteModelList == null) { creditNoteModelList = new List<OpeningBalanceLedgerModel>(); }

                if (paymentVoucherModelList == null) { paymentVoucherModelList = new List<OpeningBalanceLedgerModel>(); }

                if (receiptVoucherModelList == null) { receiptVoucherModelList = new List<OpeningBalanceLedgerModel>(); }

                if (paymentVoucherDetailModelList == null) { paymentVoucherDetailModelList = new List<OpeningBalanceLedgerModel>(); }

                if (receiptVoucherDetailModelList == null) { receiptVoucherDetailModelList = new List<OpeningBalanceLedgerModel>(); }

                if (contraVoucherDetailModelList == null) { contraVoucherDetailModelList = new List<OpeningBalanceLedgerModel>(); }

                if (journalVoucherDetailModelList == null) { journalVoucherDetailModelList = new List<OpeningBalanceLedgerModel>(); }

                openingBalanceLedgerModelList = (
                                        openingBalanceModelList
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
                                        .Select(groups => new OpeningBalanceLedgerModel
                                        {
                                            LedgerId = groups.Key.LedgerId,
                                            DebitAmount = groups.Sum(s => s.DebitAmount),
                                            CreditAmount = groups.Sum(s => s.CreditAmount),
                                        })
                                        .ToList();

                if (openingBalanceLedgerModelList == null) { openingBalanceLedgerModelList = new List<OpeningBalanceLedgerModel>(); }

                if (null != journalVoucherDetails && journalVoucherDetails.Count > 0)
                {
                    openingBalanceLedgerModelList.ToList().ForEach(w => w.ClosingAmount = w.CreditAmount - w.DebitAmount);

                    openingBalanceLedgerModelList = openingBalanceLedgerModelList.Where(w => w.ClosingAmount != 0).ToList();

                    openingBalanceLedgerModelList.Where(w => w.ClosingAmount < 0).ToList().ForEach(w => w.DebitAmount_OP = Math.Abs(w.ClosingAmount));

                    openingBalanceLedgerModelList.Where(w => w.ClosingAmount >= 0).ToList().ForEach(w => w.CreditAmount_OP = Math.Abs(w.ClosingAmount));
                }

                ////////////Remove all record

                //List<Ledgerfinancialyearbalance> newLedgerfinancialyearbalanceList = await _dbContext.Ledgerfinancialyearbalances
                //                                                                .Where(w => w.FinancialYearId==openingBalanceTransferModel.ToYearId).ToListAsync();

                _dbContext.Ledgerfinancialyearbalances.RemoveRange(_dbContext.Ledgerfinancialyearbalances.Where(w => w.FinancialYearId==openingBalanceTransferModel.ToYearId));
                _dbContext.SaveChanges();

                ////////////Add all record

                Ledgerfinancialyearbalance newLedgerfinancialyearbalance;

                foreach (OpeningBalanceLedgerModel openingBalanceLedgerModel in openingBalanceLedgerModelList)
                {
                    newLedgerfinancialyearbalance = new Ledgerfinancialyearbalance()
                    {
                        LedgerId = openingBalanceLedgerModel.LedgerId,
                        FinancialYearId = openingBalanceTransferModel.ToYearId,
                        CompanyId = openingBalanceTransferModel.CompanyId,
                        CreditAmount = openingBalanceLedgerModel.CreditAmount_OP,
                        DebitAmount = openingBalanceLedgerModel.DebitAmount_OP,
                    };

                    _dbContext.Ledgerfinancialyearbalances.Add(newLedgerfinancialyearbalance);
                }

                await _dbContext.SaveChangesAsync();

                isUpdated = true;

            //}
            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message.ToString());
            //}

            return isUpdated; // returns.
        }

    }
}

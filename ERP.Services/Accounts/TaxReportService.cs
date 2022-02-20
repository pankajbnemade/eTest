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
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TaxReportService : ITaxReport
    {
        ErpDbContext dbContext;
        IPurchaseInvoice purchaseInvoice;
        ISalesInvoice salesInvoice;
        ICreditNote creditNote;
        IDebitNote debitNote;

        IPaymentVoucher paymentVoucher;
        IReceiptVoucher receiptVoucher;

        IPaymentVoucherDetail paymentVoucherDetail;
        IReceiptVoucherDetail receiptVoucherDetail;
        IJournalVoucherDetail journalVoucherDetail;
        IContraVoucherDetail contraVoucherDetail;

        public TaxReportService(ErpDbContext _dbContext,
                                    IPurchaseInvoice _purchaseInvoice, ISalesInvoice _salesInvoice,
                                    ICreditNote _creditNote, IDebitNote _debitNote,
                                    IPaymentVoucher _paymentVoucher, IReceiptVoucher _receiptVoucher,
                                    IPaymentVoucherDetail _paymentVoucherDetail, IReceiptVoucherDetail _receiptVoucherDetail,
                                    IContraVoucherDetail _contraVoucherDetail, IJournalVoucherDetail _journalVoucherDetail
                                )
        {
            dbContext = _dbContext;
            purchaseInvoice = _purchaseInvoice;
            salesInvoice = _salesInvoice;
            creditNote = _creditNote;
            debitNote = _debitNote;
            paymentVoucher = _paymentVoucher;
            receiptVoucher = _receiptVoucher;
            paymentVoucherDetail = _paymentVoucherDetail;
            receiptVoucherDetail = _receiptVoucherDetail;
            contraVoucherDetail = _contraVoucherDetail;
            journalVoucherDetail = _journalVoucherDetail;
        }

        public async Task<DataTableResultModel<TaxReportModel>> GetReport(SearchFilterTaxReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            int ledgerId = (int)searchFilterModel.LedgerId;

            IList<TaxReportModel> generalLedgerModelList = await GetList(ledgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<TaxReportModel> resultModel = new DataTableResultModel<TaxReportModel>();

            if (null != generalLedgerModelList && generalLedgerModelList.Any())
            {
                resultModel = new DataTableResultModel<TaxReportModel>();
                resultModel.ResultList = generalLedgerModelList;
                resultModel.TotalResultCount = generalLedgerModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<TaxReportModel>();
                resultModel.ResultList = new List<TaxReportModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<TaxReportModel> GetBalanceAsOnDate(int ledgerId, DateTime date, int financialYearId, int companyId)
        {

            IList<TaxReportModel> generalLedgerModelList;

            date=date.AddDays(-1);

            DateTime fromDate_FY;

            Financialyear financialyear = dbContext.Financialyears.Where(w => w.FinancialYearId==financialYearId).FirstOrDefault();

            if (financialyear==null)
            {
                return null;
            }

            fromDate_FY=(DateTime)financialyear.FromDate;

            generalLedgerModelList= await GetTransactionList(ledgerId, fromDate_FY, date, financialYearId, companyId);

            if (generalLedgerModelList==null)
            {
                generalLedgerModelList=new List<TaxReportModel>();
            }

            Ledgerfinancialyearbalance ledgerFinancialYearBalance = dbContext.Ledgerfinancialyearbalances
                                                                    .Where(w => w.LedgerId==ledgerId
                                                                        && w.FinancialYearId==financialYearId
                                                                        && w.CompanyId==companyId)
                                                                    .FirstOrDefault();

            if (ledgerFinancialYearBalance!=null)
            {
                generalLedgerModelList.Add(new TaxReportModel()
                {
                    SequenceNo=1,
                    SrNo = 1,
                    DocumentId=0,
                    DocumentNo="",
                    DocumentType="Opening Balance",
                    DocumentDate = fromDate_FY,
                    CreditAmount = ledgerFinancialYearBalance.CreditAmount,
                    DebitAmount = ledgerFinancialYearBalance.DebitAmount,
                });
            }

            TaxReportModel generalLedgerModel = new TaxReportModel();

            generalLedgerModel.SequenceNo=1;
            generalLedgerModel.SrNo = 1;
            generalLedgerModel.DocumentId=0;
            generalLedgerModel.DocumentNo="";
            generalLedgerModel.DocumentType="Opening Balance";
            generalLedgerModel.DocumentNo="Opening Balance";
            generalLedgerModel.DocumentDate=fromDate_FY;

            generalLedgerModel.Amount=generalLedgerModelList.Sum(w => w.CreditAmount - w.DebitAmount);

            if (generalLedgerModel.Amount<0)
            {
                generalLedgerModel.DebitAmount=Math.Abs(generalLedgerModel.Amount);
            }
            else
            {
                generalLedgerModel.CreditAmount=Math.Abs(generalLedgerModel.Amount);
            }

            return generalLedgerModel; // returns.
        }

        private async Task<IList<TaxReportModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<TaxReportModel> generalLedgerModelList = null;

            IList<TaxReportModel> purchaseInvoiceTaxModelList = null;

            IList<TaxReportModel> salesInvoiceTaxModelList = null;

            IList<TaxReportModel> creditNoteTaxModelList = null;

            IList<TaxReportModel> debitNoteTaxModelList = null;

            IList<TaxReportModel> purchaseInvoiceDetTaxModelList = null;

            IList<TaxReportModel> salesInvoiceDetTaxModelList = null;

            IList<TaxReportModel> debitNoteDetTaxModelList = null;

            IList<TaxReportModel> creditNoteDetTaxModelList = null;

            purchaseInvoiceTaxModelList = dbContext.Purchaseinvoicetaxes
                                            .Include(i => i.PurchaseInvoice)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.PurchaseInvoice.StatusId == (int)DocumentStatus.Approved
                                                && w.PurchaseInvoice.FinancialYearId == financialYearId
                                                && w.PurchaseInvoice.CompanyId == companyId
                                                && w.PurchaseInvoice.InvoiceDate >= fromDate
                                                && w.PurchaseInvoice.InvoiceDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.PurchaseInvoice.PurchaseInvoiceId,
                                                DocumentType= "Purchase Invoice",
                                                DocumentNo= row.PurchaseInvoice.InvoiceNo,
                                                DocumentDate= row.PurchaseInvoice.InvoiceDate,
                                                PurchaseInvoiceId= row.PurchaseInvoice.PurchaseInvoiceId,
                                                CurrencyId= row.PurchaseInvoice.CurrencyId,
                                                CurrencyCode= row.PurchaseInvoice.Currency.CurrencyCode,
                                                ExchangeRate= row.PurchaseInvoice.ExchangeRate,
                                                PartyReferenceNo= row.PurchaseInvoice.SupplierReferenceNo,
                                                DebitAmount_FC= row.TaxAmountFc,
                                                DebitAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();


            salesInvoiceTaxModelList = dbContext.Salesinvoicetaxes
                                            .Include(i => i.SalesInvoice)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.SalesInvoice.StatusId == (int)DocumentStatus.Approved
                                                && w.SalesInvoice.FinancialYearId == financialYearId
                                                && w.SalesInvoice.CompanyId == companyId
                                                && w.SalesInvoice.InvoiceDate >= fromDate
                                                && w.SalesInvoice.InvoiceDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.SalesInvoice.SalesInvoiceId,
                                                DocumentType= "Sales Invoice",
                                                DocumentNo= row.SalesInvoice.InvoiceNo,
                                                DocumentDate= row.SalesInvoice.InvoiceDate,
                                                SalesInvoiceId= row.SalesInvoice.SalesInvoiceId,
                                                CurrencyId= row.SalesInvoice.CurrencyId,
                                                CurrencyCode= row.SalesInvoice.Currency.CurrencyCode,
                                                ExchangeRate= row.SalesInvoice.ExchangeRate,
                                                PartyReferenceNo= row.SalesInvoice.CustomerReferenceNo,
                                                CreditAmount_FC= row.TaxAmountFc,
                                                CreditAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();

            debitNoteTaxModelList = dbContext.Debitnotetaxes
                                            .Include(i => i.DebitNote)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.DebitNote.StatusId == (int)DocumentStatus.Approved
                                                && w.DebitNote.FinancialYearId == financialYearId
                                                && w.DebitNote.CompanyId == companyId
                                                && w.DebitNote.DebitNoteDate >= fromDate
                                                && w.DebitNote.DebitNoteDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.DebitNote.DebitNoteId,
                                                DocumentType= "Debit Note",
                                                DocumentNo= row.DebitNote.DebitNoteNo,
                                                DocumentDate= row.DebitNote.DebitNoteDate,
                                                DebitNoteId= row.DebitNote.DebitNoteId,
                                                CurrencyId= row.DebitNote.CurrencyId,
                                                CurrencyCode= row.DebitNote.Currency.CurrencyCode,
                                                ExchangeRate= row.DebitNote.ExchangeRate,
                                                PartyReferenceNo= row.DebitNote.PartyReferenceNo,
                                                DebitAmount_FC= row.TaxAmountFc,
                                                DebitAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();


            creditNoteTaxModelList = dbContext.Creditnotetaxes
                                            .Include(i => i.CreditNote)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.CreditNote.StatusId == (int)DocumentStatus.Approved
                                                && w.CreditNote.FinancialYearId == financialYearId
                                                && w.CreditNote.CompanyId == companyId
                                                && w.CreditNote.CreditNoteDate >= fromDate
                                                && w.CreditNote.CreditNoteDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.CreditNote.CreditNoteId,
                                                DocumentType= "Credit Note",
                                                DocumentNo= row.CreditNote.CreditNoteNo,
                                                DocumentDate= row.CreditNote.CreditNoteDate,
                                                CreditNoteId= row.CreditNote.CreditNoteId,
                                                CurrencyId= row.CreditNote.CurrencyId,
                                                CurrencyCode= row.CreditNote.Currency.CurrencyCode,
                                                ExchangeRate= row.CreditNote.ExchangeRate,
                                                PartyReferenceNo= row.CreditNote.PartyReferenceNo,
                                                CreditAmount_FC= row.TaxAmountFc,
                                                CreditAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();

            purchaseInvoiceDetTaxModelList = dbContext.Purchaseinvoicedetailtaxes
                                            .Include(i => i.PurchaseInvoiceDet)
                                            .ThenInclude(i => i.PurchaseInvoice)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.PurchaseInvoiceDet.PurchaseInvoice.StatusId == (int)DocumentStatus.Approved
                                                && w.PurchaseInvoiceDet.PurchaseInvoice.FinancialYearId == financialYearId
                                                && w.PurchaseInvoiceDet.PurchaseInvoice.CompanyId == companyId
                                                && w.PurchaseInvoiceDet.PurchaseInvoice.InvoiceDate >= fromDate
                                                && w.PurchaseInvoiceDet.PurchaseInvoice.InvoiceDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.PurchaseInvoiceDet.PurchaseInvoice.PurchaseInvoiceId,
                                                DocumentType= "Purchase Invoice",
                                                DocumentNo= row.PurchaseInvoiceDet.PurchaseInvoice.InvoiceNo,
                                                DocumentDate= row.PurchaseInvoiceDet.PurchaseInvoice.InvoiceDate,
                                                PurchaseInvoiceId= row.PurchaseInvoiceDet.PurchaseInvoice.PurchaseInvoiceId,
                                                CurrencyId= row.PurchaseInvoiceDet.PurchaseInvoice.CurrencyId,
                                                CurrencyCode= row.PurchaseInvoiceDet.PurchaseInvoice.Currency.CurrencyCode,
                                                ExchangeRate= row.PurchaseInvoiceDet.PurchaseInvoice.ExchangeRate,
                                                PartyReferenceNo= row.PurchaseInvoiceDet.PurchaseInvoice.SupplierReferenceNo,
                                                DebitAmount_FC= row.TaxAmountFc,
                                                DebitAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();


            salesInvoiceDetTaxModelList = dbContext.Salesinvoicedetailtaxes
                                            .Include(i => i.SalesInvoiceDet)
                                            .ThenInclude(i => i.SalesInvoice)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.SalesInvoiceDet.SalesInvoice.StatusId == (int)DocumentStatus.Approved
                                                && w.SalesInvoiceDet.SalesInvoice.FinancialYearId == financialYearId
                                                && w.SalesInvoiceDet.SalesInvoice.CompanyId == companyId
                                                && w.SalesInvoiceDet.SalesInvoice.InvoiceDate >= fromDate
                                                && w.SalesInvoiceDet.SalesInvoice.InvoiceDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.SalesInvoiceDet.SalesInvoice.SalesInvoiceId,
                                                DocumentType= "Sales Invoice",
                                                DocumentNo= row.SalesInvoiceDet.SalesInvoice.InvoiceNo,
                                                DocumentDate= row.SalesInvoiceDet.SalesInvoice.InvoiceDate,
                                                SalesInvoiceId= row.SalesInvoiceDet.SalesInvoice.SalesInvoiceId,
                                                CurrencyId= row.SalesInvoiceDet.SalesInvoice.CurrencyId,
                                                CurrencyCode= row.SalesInvoiceDet.SalesInvoice.Currency.CurrencyCode,
                                                ExchangeRate= row.SalesInvoiceDet.SalesInvoice.ExchangeRate,
                                                PartyReferenceNo= row.SalesInvoiceDet.SalesInvoice.CustomerReferenceNo,
                                                CreditAmount_FC= row.TaxAmountFc,
                                                CreditAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();

            debitNoteDetTaxModelList = dbContext.Debitnotedetailtaxes
                                            .Include(i => i.DebitNoteDet)
                                            .ThenInclude(i => i.DebitNote)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.DebitNoteDet.DebitNote.StatusId == (int)DocumentStatus.Approved
                                                && w.DebitNoteDet.DebitNote.FinancialYearId == financialYearId
                                                && w.DebitNoteDet.DebitNote.CompanyId == companyId
                                                && w.DebitNoteDet.DebitNote.DebitNoteDate >= fromDate
                                                && w.DebitNoteDet.DebitNote.DebitNoteDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.DebitNoteDet.DebitNote.DebitNoteId,
                                                DocumentType= "Debit Note",
                                                DocumentNo= row.DebitNoteDet.DebitNote.DebitNoteNo,
                                                DocumentDate= row.DebitNoteDet.DebitNote.DebitNoteDate,
                                                DebitNoteId= row.DebitNoteDet.DebitNote.DebitNoteId,
                                                CurrencyId= row.DebitNoteDet.DebitNote.CurrencyId,
                                                CurrencyCode= row.DebitNoteDet.DebitNote.Currency.CurrencyCode,
                                                ExchangeRate= row.DebitNoteDet.DebitNote.ExchangeRate,
                                                PartyReferenceNo= row.DebitNoteDet.DebitNote.PartyReferenceNo,
                                                DebitAmount_FC= row.TaxAmountFc,
                                                DebitAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();


            creditNoteDetTaxModelList = dbContext.Creditnotedetailtaxes
                                            .Include(i => i.CreditNoteDet)
                                            .ThenInclude(i => i.CreditNote)
                                            .ThenInclude(i => i.Currency)
                                            .Where(w => w.TaxLedgerId == ledgerId
                                                && w.CreditNoteDet.CreditNote.StatusId == (int)DocumentStatus.Approved
                                                && w.CreditNoteDet.CreditNote.FinancialYearId == financialYearId
                                                && w.CreditNoteDet.CreditNote.CompanyId == companyId
                                                && w.CreditNoteDet.CreditNote.CreditNoteDate >= fromDate
                                                && w.CreditNoteDet.CreditNote.CreditNoteDate <= toDate)
                                            .ToList()
                                            .Select((row, index) => new TaxReportModel
                                            {
                                                SequenceNo= 2,
                                                SrNo= index,
                                                DocumentId= row.CreditNoteDet.CreditNote.CreditNoteId,
                                                DocumentType= "Credit Note",
                                                DocumentNo= row.CreditNoteDet.CreditNote.CreditNoteNo,
                                                DocumentDate= row.CreditNoteDet.CreditNote.CreditNoteDate,
                                                CreditNoteId= row.CreditNoteDet.CreditNote.CreditNoteId,
                                                CurrencyId= row.CreditNoteDet.CreditNote.CurrencyId,
                                                CurrencyCode= row.CreditNoteDet.CreditNote.Currency.CurrencyCode,
                                                ExchangeRate= row.CreditNoteDet.CreditNote.ExchangeRate,
                                                PartyReferenceNo= row.CreditNoteDet.CreditNote.PartyReferenceNo,
                                                CreditAmount_FC= row.TaxAmountFc,
                                                CreditAmount= row.TaxAmount,
                                                Amount_FC= row.TaxAmountFc,
                                                Amount= row.TaxAmount,
                                            })
                                            .ToList();
            //-----------------------

            if (purchaseInvoiceTaxModelList == null) { purchaseInvoiceTaxModelList = new List<TaxReportModel>(); }

            if (salesInvoiceTaxModelList == null) { salesInvoiceTaxModelList = new List<TaxReportModel>(); }

            if (debitNoteTaxModelList == null) { debitNoteTaxModelList = new List<TaxReportModel>(); }

            if (creditNoteTaxModelList == null) { creditNoteTaxModelList = new List<TaxReportModel>(); }

            if (purchaseInvoiceDetTaxModelList == null) { purchaseInvoiceDetTaxModelList = new List<TaxReportModel>(); }

            if (salesInvoiceDetTaxModelList == null) { salesInvoiceDetTaxModelList = new List<TaxReportModel>(); }

            if (debitNoteDetTaxModelList == null) { debitNoteDetTaxModelList = new List<TaxReportModel>(); }

            if (creditNoteDetTaxModelList == null) { creditNoteDetTaxModelList = new List<TaxReportModel>(); }

            generalLedgerModelList = (purchaseInvoiceTaxModelList
                                    .Union(salesInvoiceTaxModelList)
                                    .Union(debitNoteTaxModelList)
                                    .Union(creditNoteTaxModelList)
                                    .Union(purchaseInvoiceDetTaxModelList)
                                    .Union(salesInvoiceDetTaxModelList)
                                    .Union(debitNoteDetTaxModelList)
                                    .Union(creditNoteDetTaxModelList)
                                    )
                                    .OrderBy(o => o.DocumentDate).ThenBy(o => o.DocumentType).ThenBy(o => o.DocumentNo)
                                    .Select((row, index) => new TaxReportModel
                                    {
                                        SequenceNo   =2,
                                        SrNo =index+1,
                                        DocumentId  =row.DocumentId,
                                        DocumentType    =row.DocumentType,
                                        DocumentNo  =row.DocumentNo,
                                        DocumentDate    =row.DocumentDate,
                                        PurchaseInvoiceId   =row.PurchaseInvoiceId,
                                        SalesInvoiceId  =row.SalesInvoiceId,
                                        CreditNoteId    =row.CreditNoteId,
                                        DebitNoteId     =row.DebitNoteId,
                                        CurrencyId  =row.CurrencyId,
                                        CurrencyCode    =row.CurrencyCode,
                                        PartyReferenceNo    =row.PartyReferenceNo,
                                        OurReferenceNo  =row.OurReferenceNo,
                                        ExchangeRate    =row.ExchangeRate,
                                        CreditAmount_FC     =row.CreditAmount_FC,
                                        DebitAmount_FC  =row.DebitAmount_FC,
                                        CreditAmount    =row.CreditAmount,
                                        DebitAmount     =row.DebitAmount,
                                        Amount_FC   =row.Amount_FC,
                                        Amount  =row.Amount,
                                        ClosingAmount   =row.ClosingAmount
                                    })
                                    .ToList();

            if (generalLedgerModelList==null)
            {
                generalLedgerModelList= new List<TaxReportModel>();
            }

            return await Task.Run(() =>
            {
                return generalLedgerModelList; // returns.
            });
        }

        private async Task<IList<TaxReportModel>> GetList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<TaxReportModel> generalLedgerModelList = new List<TaxReportModel>();

            //TaxReportModel generalLedgerModel_OB = await GetBalanceAsOnDate(ledgerId, fromDate, financialYearId, companyId);

            //if (generalLedgerModel_OB==null)
            //{
            //    generalLedgerModel_OB=new TaxReportModel();
            //}

            //generalLedgerModelList.Add(generalLedgerModel_OB);

            IList<TaxReportModel> generalLedgerModelList_Trans = null;

            generalLedgerModelList_Trans  = await GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            if (generalLedgerModelList_Trans==null)
            {
                generalLedgerModelList_Trans= new List<TaxReportModel>();
            }

            generalLedgerModelList = generalLedgerModelList
                                    .Union(generalLedgerModelList_Trans)
                                    .ToList();

            generalLedgerModelList.Add(new TaxReportModel()
            {
                SequenceNo=3,
                SrNo = generalLedgerModelList.Max(w => w.SrNo)+1,
                DocumentType = "Total - Closing Balance",
                DocumentNo = "Total - Closing Balance",
                DocumentDate = toDate,
                CreditAmount = generalLedgerModelList.Sum(w => w.CreditAmount),
                DebitAmount = generalLedgerModelList.Sum(w => w.DebitAmount),
                ClosingAmount=generalLedgerModelList.Sum(w => w.CreditAmount)-generalLedgerModelList.Sum(w => w.DebitAmount)
            });

            generalLedgerModelList= generalLedgerModelList
                                    .Select((row, index) => new TaxReportModel
                                    {
                                        SequenceNo   = row.SequenceNo,
                                        SrNo =index + 1,
                                        DocumentId  =row.DocumentId,
                                        DocumentType    =row.DocumentType,
                                        DocumentNo  =row.DocumentNo,
                                        DocumentDate    =row.DocumentDate,
                                        PurchaseInvoiceId   =row.PurchaseInvoiceId,
                                        SalesInvoiceId  =row.SalesInvoiceId,
                                        CreditNoteId    =row.CreditNoteId,
                                        DebitNoteId     =row.DebitNoteId,
                                        CurrencyId  =row.CurrencyId,
                                        CurrencyCode    =row.CurrencyCode,
                                        PartyReferenceNo    =row.PartyReferenceNo,
                                        OurReferenceNo  =row.OurReferenceNo,
                                        ExchangeRate    =row.ExchangeRate,
                                        CreditAmount_FC     =row.CreditAmount_FC,
                                        DebitAmount_FC  =row.DebitAmount_FC,
                                        CreditAmount    =row.CreditAmount,
                                        DebitAmount     =row.DebitAmount,
                                        Amount_FC   =row.Amount_FC,
                                        Amount  =row.Amount,
                                        ClosingAmount   =row.ClosingAmount
                                    })
                                    .OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();


            //TaxReportModel prvRow = null;

            //foreach (TaxReportModel currRow in generalLedgerModelList.OrderBy(o => o.SrNo))
            //{
            //    if (prvRow != null)
            //    {
            //        if (currRow.SequenceNo==2)
            //        {
            //            currRow.ClosingAmount = prvRow.ClosingAmount + (currRow.CreditAmount - currRow.DebitAmount);
            //        }
            //        else if (currRow.SequenceNo==3)
            //        {
            //            currRow.ClosingAmount = currRow.CreditAmount - currRow.DebitAmount;
            //        }
            //    }
            //    else
            //    {
            //        if (currRow.SequenceNo==1)
            //        {
            //            currRow.ClosingAmount = currRow.CreditAmount - currRow.DebitAmount;
            //        }
            //    }

            //    prvRow = currRow;
            //}

            return generalLedgerModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
        }

    }
}

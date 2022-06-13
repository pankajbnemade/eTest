using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class GeneralLedgerService : IGeneralLedger
    {
        private readonly ErpDbContext _dbContext;
        private readonly IPurchaseInvoice _purchaseInvoice;
        private readonly ISalesInvoice _salesInvoice;
        private readonly ICreditNote _creditNote;
        private readonly IDebitNote _debitNote;

        private readonly IPaymentVoucher _paymentVoucher;
        private readonly IReceiptVoucher _receiptVoucher;

        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly IReceiptVoucherDetail _receiptVoucherDetail;
        private readonly IJournalVoucherDetail _journalVoucherDetail;
        private readonly IContraVoucherDetail _contraVoucherDetail;

        public GeneralLedgerService(ErpDbContext dbContext,
                                    IPurchaseInvoice purchaseInvoice, ISalesInvoice salesInvoice,
                                    ICreditNote creditNote, IDebitNote debitNote,
                                    IPaymentVoucher paymentVoucher, IReceiptVoucher receiptVoucher,
                                    IPaymentVoucherDetail paymentVoucherDetail, IReceiptVoucherDetail receiptVoucherDetail,
                                    IContraVoucherDetail contraVoucherDetail, IJournalVoucherDetail journalVoucherDetail
                                )
        {
            _dbContext = dbContext;
            _purchaseInvoice = purchaseInvoice;
            _salesInvoice = salesInvoice;
            _creditNote = creditNote;
            _debitNote = debitNote;
            _paymentVoucher = paymentVoucher;
            _receiptVoucher = receiptVoucher;
            _paymentVoucherDetail = paymentVoucherDetail;
            _receiptVoucherDetail = receiptVoucherDetail;
            _contraVoucherDetail = contraVoucherDetail;
            _journalVoucherDetail = journalVoucherDetail;
        }

        public async Task<DataTableResultModel<GeneralLedgerModel>> GetReport(SearchFilterGeneralLedgerModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<GeneralLedgerModel> generalLedgerModelList = await GetList(searchFilterModel.LedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<GeneralLedgerModel> resultModel = new DataTableResultModel<GeneralLedgerModel>();

            if (null != generalLedgerModelList && generalLedgerModelList.Any())
            {
                resultModel = new DataTableResultModel<GeneralLedgerModel>();
                resultModel.ResultList = generalLedgerModelList;
                resultModel.TotalResultCount = generalLedgerModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<GeneralLedgerModel>();
                resultModel.ResultList = new List<GeneralLedgerModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<GeneralLedgerModel> GetBalanceAsOnDate(int ledgerId, DateTime date, int financialYearId, int companyId)
        {

            IList<GeneralLedgerModel> generalLedgerModelList;

            date=date.AddDays(-1);

            DateTime fromDate_FY;

            Financialyear financialyear = _dbContext.Financialyears.Where(w => w.FinancialYearId==financialYearId).FirstOrDefault();

            if (financialyear==null)
            {
                return null;
            }

            fromDate_FY=(DateTime)financialyear.FromDate;

            generalLedgerModelList= await GetTransactionList(ledgerId, fromDate_FY, date, financialYearId, companyId);

            if (generalLedgerModelList==null)
            {
                generalLedgerModelList=new List<GeneralLedgerModel>();
            }

            Ledgerfinancialyearbalance ledgerFinancialYearBalance = _dbContext.Ledgerfinancialyearbalances
                                                                    .Where(w => w.LedgerId==ledgerId && w.FinancialYearId==financialYearId && w.CompanyId==companyId)
                                                                    .FirstOrDefault();

            if (ledgerFinancialYearBalance!=null)
            {
                generalLedgerModelList.Add(new GeneralLedgerModel()
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

            GeneralLedgerModel generalLedgerModel = new GeneralLedgerModel();

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

        private async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            IList<GeneralLedgerModel> purchaseInvoiceModelList = null;

            IList<GeneralLedgerModel> salesInvoiceModelList = null;

            IList<GeneralLedgerModel> creditNoteModelList = null;

            IList<GeneralLedgerModel> debitNoteModelList = null;

            IList<GeneralLedgerModel> paymentVoucherDetailModelList = null;

            IList<GeneralLedgerModel> receiptVoucherDetailModelList = null;

            IList<GeneralLedgerModel> paymentVoucherModelList = null;

            IList<GeneralLedgerModel> receiptVoucherModelList = null;

            IList<GeneralLedgerModel> contraVoucherDetailModelList = null;

            IList<GeneralLedgerModel> journalVoucherDetailModelList = null;

            purchaseInvoiceModelList = await _purchaseInvoice.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            salesInvoiceModelList = await _salesInvoice.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            debitNoteModelList = await _debitNote.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            creditNoteModelList = await _creditNote.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            paymentVoucherModelList = await _paymentVoucher.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            receiptVoucherModelList = await _receiptVoucher.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            paymentVoucherDetailModelList = await _paymentVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            receiptVoucherDetailModelList = await _receiptVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            contraVoucherDetailModelList = await _contraVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);
            journalVoucherDetailModelList = await _journalVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            //-----------------------

            if (purchaseInvoiceModelList == null) { purchaseInvoiceModelList = new List<GeneralLedgerModel>(); }

            if (salesInvoiceModelList == null) { salesInvoiceModelList = new List<GeneralLedgerModel>(); }

            if (debitNoteModelList == null) { debitNoteModelList = new List<GeneralLedgerModel>(); }

            if (creditNoteModelList == null) { creditNoteModelList = new List<GeneralLedgerModel>(); }

            if (paymentVoucherModelList == null) { paymentVoucherModelList = new List<GeneralLedgerModel>(); }

            if (receiptVoucherModelList == null) { receiptVoucherModelList = new List<GeneralLedgerModel>(); }

            if (paymentVoucherDetailModelList == null) { paymentVoucherDetailModelList = new List<GeneralLedgerModel>(); }

            if (receiptVoucherDetailModelList == null) { receiptVoucherDetailModelList = new List<GeneralLedgerModel>(); }

            if (contraVoucherDetailModelList == null) { contraVoucherDetailModelList = new List<GeneralLedgerModel>(); }

            if (journalVoucherDetailModelList == null) { journalVoucherDetailModelList = new List<GeneralLedgerModel>(); }


            generalLedgerModelList = (purchaseInvoiceModelList
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
                                    .OrderBy(o => o.DocumentDate).ThenBy(o => o.DocumentType).ThenBy(o => o.DocumentNo)
                                    .Select((row, index) => new GeneralLedgerModel
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
                                        PaymentVoucherId    =row.PaymentVoucherId,
                                        ReceiptVoucherId    =row.ReceiptVoucherId,
                                        ContraVoucherId     =row.ContraVoucherId,
                                        JournalVoucherId    =row.JournalVoucherId,
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
                generalLedgerModelList= new List<GeneralLedgerModel>();
            }

            return generalLedgerModelList; // returns.
        }

        private async Task<IList<GeneralLedgerModel>> GetList(int ledgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = new List<GeneralLedgerModel>();

            GeneralLedgerModel generalLedgerModel_OB = await GetBalanceAsOnDate(ledgerId, fromDate, financialYearId, companyId);

            if (generalLedgerModel_OB==null)
            {
                generalLedgerModel_OB=new GeneralLedgerModel();
            }

            generalLedgerModelList.Add(generalLedgerModel_OB);

            IList<GeneralLedgerModel> generalLedgerModelList_Trans = null;

            generalLedgerModelList_Trans  = await GetTransactionList(ledgerId, fromDate, toDate, financialYearId, companyId);

            if (generalLedgerModelList_Trans==null)
            {
                generalLedgerModelList_Trans= new List<GeneralLedgerModel>();
            }

            generalLedgerModelList = generalLedgerModelList
                                    .Union(generalLedgerModelList_Trans)
                                    .ToList();

            generalLedgerModelList.Add(new GeneralLedgerModel()
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
                                    .Select((row, index) => new GeneralLedgerModel
                                    {
                                        SequenceNo   = row.SequenceNo,
                                        SrNo =index+1,
                                        DocumentId  =row.DocumentId,
                                        DocumentType    =row.DocumentType,
                                        DocumentNo  =row.DocumentNo,
                                        DocumentDate    =row.DocumentDate,
                                        PurchaseInvoiceId   =row.PurchaseInvoiceId,
                                        SalesInvoiceId  =row.SalesInvoiceId,
                                        CreditNoteId    =row.CreditNoteId,
                                        DebitNoteId     =row.DebitNoteId,
                                        PaymentVoucherId    =row.PaymentVoucherId,
                                        ReceiptVoucherId    =row.ReceiptVoucherId,
                                        ContraVoucherId     =row.ContraVoucherId,
                                        JournalVoucherId    =row.JournalVoucherId,
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


            GeneralLedgerModel prvRow = null;

            foreach (GeneralLedgerModel currRow in generalLedgerModelList.OrderBy(o => o.SrNo))
            {
                if (prvRow != null)
                {
                    if (currRow.SequenceNo==2)
                    {
                        currRow.ClosingAmount = prvRow.ClosingAmount + (currRow.CreditAmount - currRow.DebitAmount);
                    }
                    else if (currRow.SequenceNo==3)
                    {
                        currRow.ClosingAmount = currRow.CreditAmount - currRow.DebitAmount;
                    }
                }
                else
                {
                    if (currRow.SequenceNo==1)
                    {
                        currRow.ClosingAmount = currRow.CreditAmount - currRow.DebitAmount;
                    }
                }

                prvRow = currRow;
            }

            return generalLedgerModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
        }

    }
}

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
        IPurchaseInvoice purchaseInvoice;
        ISalesInvoice salesInvoice;
        ICreditNote creditNote;
        IDebitNote debitNote;

        IPaymentVoucherDetail paymentVoucherDetail;
        IReceiptVoucherDetail receiptVoucherDetail;
        IJournalVoucherDetail journalVoucherDetail;
        IContraVoucherDetail contraVoucherDetail;

        public GeneralLedgerService(IPurchaseInvoice _purchaseInvoice, ISalesInvoice _salesInvoice, ICreditNote _creditNote, IDebitNote _debitNote,
                                    IPaymentVoucherDetail _paymentVoucherDetail, IReceiptVoucherDetail _receiptVoucherDetail,
                                    IContraVoucherDetail _contraVoucherDetail, IJournalVoucherDetail _journalVoucherDetail)
        {
            purchaseInvoice = _purchaseInvoice;
            salesInvoice = _salesInvoice;
            creditNote = _creditNote;
            debitNote = _debitNote;
            paymentVoucherDetail = _paymentVoucherDetail;
            receiptVoucherDetail = _receiptVoucherDetail;
            contraVoucherDetail = _contraVoucherDetail;
            journalVoucherDetail = _journalVoucherDetail;
        }

        public async Task<DataTableResultModel<GeneralLedgerModel>> GetTransactionList(SearchFilterGeneralLedgerModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<GeneralLedgerModel> generalLedgerModelList = await GetTransactionList(searchFilterModel.LedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

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

        private async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            IList<GeneralLedgerModel> purchaseInvoiceModelList = null;

            IList<GeneralLedgerModel> salesInvoiceModelList = null;

            IList<GeneralLedgerModel> creditNoteModelList = null;

            IList<GeneralLedgerModel> debitNoteModelList = null;

            IList<GeneralLedgerModel> paymentVoucherModelList = null;

            IList<GeneralLedgerModel> receiptVoucherModelList = null;

            IList<GeneralLedgerModel> contraVoucherModelList = null;

            IList<GeneralLedgerModel> journalVoucherModelList = null;

            purchaseInvoiceModelList = await purchaseInvoice.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            salesInvoiceModelList = await salesInvoice.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            debitNoteModelList = await debitNote.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            creditNoteModelList = await creditNote.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);

            paymentVoucherModelList = await paymentVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            receiptVoucherModelList = await receiptVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            contraVoucherModelList = await contraVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);
            journalVoucherModelList = await journalVoucherDetail.GetTransactionList(ledgerId, fromDate, toDate, yearId, companyId);

            //-----------------------

            if (purchaseInvoiceModelList == null) { purchaseInvoiceModelList = new List<GeneralLedgerModel>(); }

            if (salesInvoiceModelList == null) { salesInvoiceModelList = new List<GeneralLedgerModel>(); }

            if (debitNoteModelList == null) { debitNoteModelList = new List<GeneralLedgerModel>(); }

            if (creditNoteModelList == null) { creditNoteModelList = new List<GeneralLedgerModel>(); }

            if (paymentVoucherModelList == null) { paymentVoucherModelList = new List<GeneralLedgerModel>(); }

            if (receiptVoucherModelList == null) { receiptVoucherModelList = new List<GeneralLedgerModel>(); }

            if (contraVoucherModelList == null) { contraVoucherModelList = new List<GeneralLedgerModel>(); }

            if (journalVoucherModelList == null) { journalVoucherModelList = new List<GeneralLedgerModel>(); }

            generalLedgerModelList = purchaseInvoiceModelList
                                    .Union(salesInvoiceModelList)
                                    .Union(debitNoteModelList)
                                    .Union(creditNoteModelList)
                                    .Union(paymentVoucherModelList)
                                    .Union(receiptVoucherModelList)
                                    .Union(contraVoucherModelList)
                                    .Union(journalVoucherModelList)
                                    .ToList();

            return generalLedgerModelList; // returns.
        }

    }
}

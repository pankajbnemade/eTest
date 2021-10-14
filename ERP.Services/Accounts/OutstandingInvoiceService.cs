using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class OutstandingInvoiceService : IOutstandingInvoice
    {
        IPurchaseInvoice purchaseInvoice;
        ISalesInvoice salesInvoice;
        ICreditNote creditNote;
        IDebitNote debitNote;

        IPaymentVoucherDetail paymentVoucherDetail;
        IReceiptVoucherDetail receiptVoucherDetail;
        IJournalVoucherDetail journalVoucherDetail;
        IAdvanceAdjustmentDetail advanceAdjustmentDetail;

        public OutstandingInvoiceService(IPurchaseInvoice _purchaseInvoice, ISalesInvoice _salesInvoice, ICreditNote _creditNote, IDebitNote _debitNote,
            IPaymentVoucherDetail _paymentVoucherDetail, IReceiptVoucherDetail _receiptVoucherDetail, IJournalVoucherDetail _journalVoucherDetail, IAdvanceAdjustmentDetail _advanceAdjustmentDetail)
        {
            purchaseInvoice = _purchaseInvoice;
            salesInvoice = _salesInvoice;
            creditNote = _creditNote;
            debitNote = _debitNote;
            paymentVoucherDetail = _paymentVoucherDetail;
            receiptVoucherDetail = _receiptVoucherDetail;
            journalVoucherDetail = _journalVoucherDetail;
            advanceAdjustmentDetail = _advanceAdjustmentDetail;
        }

        public async Task<IList<OutstandingInvoiceModel>> GetOutstandingInvoiceListByLedgerId(int ledgerId, string VoucherType, decimal ExchangeRate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await GetOutstandingInvoiceList(ledgerId, VoucherType, ExchangeRate);

            return outstandingInvoiceModelList; // returns.
        }

        private async Task<IList<OutstandingInvoiceModel>> GetOutstandingInvoiceList(int ledgerId, string VoucherType, decimal ExchangeRate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            IList<OutstandingInvoiceModel> purchaseInvoiceModel = await purchaseInvoice.GetPurchaseInvoiceListBySupplierLedgerId(ledgerId);

            IList<OutstandingInvoiceModel> salesInvoiceModel = await salesInvoice.GetSalesInvoiceListByCustomerLedgerId(ledgerId);

            IList<OutstandingInvoiceModel> creditNoteModel = await creditNote.GetCreditNoteListByPartyLedgerId(ledgerId);

            IList<OutstandingInvoiceModel> debitNoteModel = await debitNote.GetDebitNoteListByPartyLedgerId(ledgerId);

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModel = await paymentVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModel = await receiptVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<JournalVoucherDetailModel> journalVoucherDetailModel = await journalVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModel = await advanceAdjustmentDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            if (purchaseInvoiceModel != null)
            {
                foreach (var o in purchaseInvoiceModel)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (paymentVoucherDetailModel == null ? 0 : paymentVoucherDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.DebitAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (creditNoteModel != null)
            {
                foreach (var o in creditNoteModel)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.CreditNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.DebitAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.CreditNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (salesInvoiceModel != null)
            {
                foreach (var o in salesInvoiceModel)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (receiptVoucherDetailModel == null ? 0 : receiptVoucherDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.CreditAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (debitNoteModel != null)
            {
                foreach (var o in debitNoteModel)
                {
                     o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.DebitNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.CreditAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.DebitNoteId == o.DebitNoteId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            outstandingInvoiceModelList = purchaseInvoiceModel.Where(w => w.OutstandingAmount > 0)
                                        .Union(salesInvoiceModel.Where(w => w.OutstandingAmount > 0))
                                        .Union(debitNoteModel.Where(w => w.OutstandingAmount > 0))
                                        .Union(creditNoteModel.Where(w => w.OutstandingAmount > 0))
                                        .ToList();

            outstandingInvoiceModelList.ToList().ForEach(w =>
                                                {
                                                    w.OutstandingAmount_FC = w.OutstandingAmount * ExchangeRate;
                                                    w.InvoiceAmount_FC = w.InvoiceAmount * ExchangeRate;
                                                });

            return outstandingInvoiceModelList; // returns.
        }

    }
}

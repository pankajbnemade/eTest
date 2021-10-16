using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using System;
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

        public async Task<IList<OutstandingInvoiceModel>> GetOutstandingInvoiceListByLedgerId(int ledgerId, string voucherType, int voucherId, DateTime voucherDate, decimal exchangeRate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await GetOutstandingInvoiceList(ledgerId, voucherType, voucherId, voucherDate, exchangeRate);

            return outstandingInvoiceModelList; // returns.
        }

        private async Task<IList<OutstandingInvoiceModel>> GetOutstandingInvoiceList(int ledgerId, string voucherType, int voucherId, DateTime voucherDate, decimal exchangeRate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            IList<OutstandingInvoiceModel> purchaseInvoiceModelList = null;

            IList<OutstandingInvoiceModel> salesInvoiceModelList = null;

            IList<OutstandingInvoiceModel> creditNoteModelList = null;

            IList<OutstandingInvoiceModel> debitNoteModelList = null;


            if (voucherType == "Payment Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                purchaseInvoiceModelList = await purchaseInvoice.GetPurchaseInvoiceListBySupplierLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Receipt Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                salesInvoiceModelList = await salesInvoice.GetSalesInvoiceListByCustomerLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Payment Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                debitNoteModelList = await debitNote.GetDebitNoteListByPartyLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Receipt Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                creditNoteModelList = await creditNote.GetCreditNoteListByPartyLedgerId(ledgerId, voucherDate);
            }

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModel = await paymentVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModel = await receiptVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<JournalVoucherDetailModel> journalVoucherDetailModel = await journalVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModel = await advanceAdjustmentDetail.GetInvoiceListByParticularLedgerId(ledgerId);


            IList<PaymentVoucherDetailModel> paymentVoucherDetailModel_Current = await paymentVoucherDetail.GetPaymentVoucherDetailByVoucherId(voucherId, 0);

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModel_Current = await receiptVoucherDetail.GetReceiptVoucherDetailByVoucherId(voucherId, 0);

            IList<JournalVoucherDetailModel> journalVoucherDetailModel_Current = await journalVoucherDetail.GetJournalVoucherDetailByVoucherId(voucherId, 0);

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModel_Current = await advanceAdjustmentDetail.GetAdvanceAdjustmentDetailByAdjustmentId(voucherId, 0);


            if (purchaseInvoiceModelList != null)
            {
                if (paymentVoucherDetailModel_Current != null)
                {
                    purchaseInvoiceModelList = purchaseInvoiceModelList.Where(w => !paymentVoucherDetailModel_Current.Any(c => c.PurchaseInvoiceId == w.PurchaseInvoiceId)).ToList();
                }
                if (journalVoucherDetailModel_Current != null)
                {
                    purchaseInvoiceModelList = purchaseInvoiceModelList.Where(w => !journalVoucherDetailModel_Current.Any(c => c.PurchaseInvoiceId == w.PurchaseInvoiceId)).ToList();
                }
                if (advanceAdjustmentDetailModel_Current != null)
                {
                    purchaseInvoiceModelList = purchaseInvoiceModelList.Where(w => !advanceAdjustmentDetailModel_Current.Any(c => c.PurchaseInvoiceId == w.PurchaseInvoiceId)).ToList();
                }
            }

            if (debitNoteModelList != null)
            {
                if (paymentVoucherDetailModel_Current != null)
                {
                    debitNoteModelList = debitNoteModelList.Where(w => !paymentVoucherDetailModel_Current.Any(c => c.DebitNoteId == w.DebitNoteId)).ToList();
                }
                if (journalVoucherDetailModel_Current != null)
                {
                    debitNoteModelList = debitNoteModelList.Where(w => !journalVoucherDetailModel_Current.Any(c => c.DebitNoteId == w.DebitNoteId)).ToList();
                }
                if (advanceAdjustmentDetailModel_Current != null)
                {
                    debitNoteModelList = debitNoteModelList.Where(w => !advanceAdjustmentDetailModel_Current.Any(c => c.DebitNoteId == w.DebitNoteId)).ToList();
                }
            }


            if (salesInvoiceModelList != null)
            {
                if (receiptVoucherDetailModel_Current != null)
                {
                    salesInvoiceModelList = salesInvoiceModelList.Where(w => !receiptVoucherDetailModel_Current.Any(c => c.SalesInvoiceId == w.SalesInvoiceId)).ToList();
                }
                if (journalVoucherDetailModel_Current != null)
                {
                    salesInvoiceModelList = salesInvoiceModelList.Where(w => !journalVoucherDetailModel_Current.Any(c => c.SalesInvoiceId == w.SalesInvoiceId)).ToList();
                }
                if (advanceAdjustmentDetailModel_Current != null)
                {
                    salesInvoiceModelList = salesInvoiceModelList.Where(w => !advanceAdjustmentDetailModel_Current.Any(c => c.SalesInvoiceId == w.SalesInvoiceId)).ToList();
                }
            }

            if (creditNoteModelList != null)
            {
                if (receiptVoucherDetailModel_Current != null)
                {
                    creditNoteModelList = creditNoteModelList.Where(w => !receiptVoucherDetailModel_Current.Any(c => c.CreditNoteId == w.CreditNoteId)).ToList();
                }
                if (journalVoucherDetailModel_Current != null)
                {
                    creditNoteModelList = creditNoteModelList.Where(w => !journalVoucherDetailModel_Current.Any(c => c.CreditNoteId == w.CreditNoteId)).ToList();
                }
                if (advanceAdjustmentDetailModel_Current != null)
                {
                    creditNoteModelList = creditNoteModelList.Where(w => !advanceAdjustmentDetailModel_Current.Any(c => c.CreditNoteId == w.CreditNoteId)).ToList();
                }
            }


            //---------

            if (purchaseInvoiceModelList != null)
            {
                foreach (var o in purchaseInvoiceModelList)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (paymentVoucherDetailModel == null ? 0 : paymentVoucherDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.DebitAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.PurchaseInvoiceId == o.PurchaseInvoiceId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (debitNoteModelList != null)
            {
                foreach (var o in debitNoteModelList)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (paymentVoucherDetailModel == null ? 0 : paymentVoucherDetailModel.Where(w => w.DebitNoteId == o.DebitNoteId)
                                                                                   .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.DebitNoteId == o.CreditNoteId)
                                                                                   .Sum(w => w.DebitAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.DebitNoteId == o.DebitNoteId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (salesInvoiceModelList != null)
            {
                foreach (var o in salesInvoiceModelList)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (receiptVoucherDetailModel == null ? 0 : receiptVoucherDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.CreditAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.SalesInvoiceId == o.SalesInvoiceId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            if (creditNoteModelList != null)
            {
                foreach (var o in creditNoteModelList)
                {
                    o.OutstandingAmount = o.OutstandingAmount - (receiptVoucherDetailModel == null ? 0 : receiptVoucherDetailModel.Where(w => w.CreditNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.Amount));

                    o.OutstandingAmount = o.OutstandingAmount - (journalVoucherDetailModel == null ? 0 : journalVoucherDetailModel.Where(w => w.CreditNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.CreditAmount));

                    o.OutstandingAmount = o.OutstandingAmount - (advanceAdjustmentDetailModel == null ? 0 : advanceAdjustmentDetailModel.Where(w => w.CreditNoteId == o.CreditNoteId)
                                                                                    .Sum(w => w.Amount));
                }
            }

            //-----------------------

            if (purchaseInvoiceModelList == null)
            {
                purchaseInvoiceModelList = new List<OutstandingInvoiceModel>();
            }

            if (debitNoteModelList == null)
            {
                debitNoteModelList = new List<OutstandingInvoiceModel>();
            }

            if (salesInvoiceModelList == null)
            {
                salesInvoiceModelList = new List<OutstandingInvoiceModel>();
            }

            if (creditNoteModelList == null)
            {
                creditNoteModelList = new List<OutstandingInvoiceModel>();
            }

            outstandingInvoiceModelList = purchaseInvoiceModelList.Where(w => w.OutstandingAmount > 0)
                                        .Union(salesInvoiceModelList.Where(w => w.OutstandingAmount > 0))
                                        .Union(debitNoteModelList.Where(w => w.OutstandingAmount > 0))
                                        .Union(creditNoteModelList.Where(w => w.OutstandingAmount > 0))
                                        .ToList();

            outstandingInvoiceModelList.ToList().ForEach(w =>
                                                {
                                                    w.OutstandingAmount_FC = Math.Round(w.OutstandingAmount * exchangeRate, 4);
                                                    w.InvoiceAmount_FC = Math.Round(w.InvoiceAmount * exchangeRate, 4);
                                                });

            return outstandingInvoiceModelList; // returns.
        }

    }
}

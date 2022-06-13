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
        private readonly IPurchaseInvoice _purchaseInvoice;
        private readonly ISalesInvoice _salesInvoice;
        private readonly ICreditNote _creditNote;
        private readonly IDebitNote _debitNote;

        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly IReceiptVoucherDetail _receiptVoucherDetail;
        private readonly IJournalVoucherDetail _journalVoucherDetail;
        private readonly IAdvanceAdjustmentDetail _advanceAdjustmentDetail;

        public OutstandingInvoiceService(IPurchaseInvoice purchaseInvoice, ISalesInvoice salesInvoice, 
            ICreditNote creditNote, IDebitNote debitNote,
            IPaymentVoucherDetail paymentVoucherDetail, IReceiptVoucherDetail receiptVoucherDetail, 
            IJournalVoucherDetail journalVoucherDetail, IAdvanceAdjustmentDetail advanceAdjustmentDetail)
        {
            _purchaseInvoice = purchaseInvoice;
            _salesInvoice = salesInvoice;
            _creditNote = creditNote;
            _debitNote = debitNote;
            _paymentVoucherDetail = paymentVoucherDetail;
            _receiptVoucherDetail = receiptVoucherDetail;
            _journalVoucherDetail = journalVoucherDetail;
            _advanceAdjustmentDetail = advanceAdjustmentDetail;
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
                purchaseInvoiceModelList = await _purchaseInvoice.GetPurchaseInvoiceListBySupplierLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Receipt Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                salesInvoiceModelList = await _salesInvoice.GetSalesInvoiceListByCustomerLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Payment Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                debitNoteModelList = await _debitNote.GetDebitNoteListByPartyLedgerId(ledgerId, voucherDate);
            }

            if (voucherType == "Receipt Voucher" || voucherType == "Journal Voucher" || voucherType == "Advance Adjustment")
            {
                creditNoteModelList = await _creditNote.GetCreditNoteListByPartyLedgerId(ledgerId, voucherDate);
            }

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModel = await _paymentVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModel = await _receiptVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<JournalVoucherDetailModel> journalVoucherDetailModel = await _journalVoucherDetail.GetInvoiceListByParticularLedgerId(ledgerId);

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModel = await _advanceAdjustmentDetail.GetInvoiceListByParticularLedgerId(ledgerId);


            IList<PaymentVoucherDetailModel> paymentVoucherDetailModel_Current = null;
            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModel_Current = null;
            IList<JournalVoucherDetailModel> journalVoucherDetailModel_Current = null;
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModel_Current = null;

            if (voucherType == "Payment Voucher")
            {
                paymentVoucherDetailModel_Current = await _paymentVoucherDetail.GetPaymentVoucherDetailByVoucherId(voucherId, 0);
            }

            if (voucherType == "Receipt Voucher")
            {
                receiptVoucherDetailModel_Current = await _receiptVoucherDetail.GetReceiptVoucherDetailByVoucherId(voucherId, 0);
            }

            if (voucherType == "Journal Voucher")
            {
                journalVoucherDetailModel_Current = await _journalVoucherDetail.GetJournalVoucherDetailByVoucherId(voucherId, 0);
            }

            if (voucherType == "Advance Adjustment")
            {
                advanceAdjustmentDetailModel_Current = await _advanceAdjustmentDetail.GetAdvanceAdjustmentDetailByAdjustmentId(voucherId);
            }

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

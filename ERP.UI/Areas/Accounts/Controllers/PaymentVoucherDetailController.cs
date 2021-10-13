using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class PaymentVoucherDetailController : Controller
    {
        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IOutstandingInvoice _outstandingInvoice;
        private readonly IPaymentVoucher _paymentVoucher;

        public PaymentVoucherDetailController(IPaymentVoucherDetail paymentVoucherDetail, ILedger ledger, ICurrencyConversion currencyConversion,
            IOutstandingInvoice outstandingInvoice,
            IPaymentVoucher paymentVoucher)
        {
            this._paymentVoucherDetail = paymentVoucherDetail;
            this._ledger = ledger;
            this._currencyConversion = currencyConversion;
            this._outstandingInvoice = outstandingInvoice;
            this._paymentVoucher = paymentVoucher;
        }

        /// <summary>
        /// voucher detail.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> VoucherDetail(int paymentVoucherId, int addRow_Blank)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;

            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TransactionTypeList = EnumHelper.GetEnumListFor<TransactionType>();

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = await _paymentVoucherDetail.GetPaymentVoucherDetailByVoucherId(paymentVoucherId, addRow_Blank);

            return await Task.Run(() =>
            {
                return PartialView("_VoucherDetail", paymentVoucherDetailModelList);
            });
        }

        public async Task<IActionResult> MapOutstandingDetail(int particularLedgerId, int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            Decimal ExchangeRate = 0;
            Int32 currencyId = paymentVoucherModel.CurrencyId;
            DateTime voucherDate = (DateTime)paymentVoucherModel.VoucherDate;

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetExchangeRateByCurrencyId(currencyId, voucherDate);

            ExchangeRate = null != currencyConversionModel ? (decimal)currencyConversionModel.ExchangeRate : 0;

            ExchangeRate = 0 != ExchangeRate ? 1 / ExchangeRate : 0;

            //################

            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await _outstandingInvoice.GetOutstandingInvoiceListByLedgerId(particularLedgerId, "Payment Voucher", ExchangeRate);

            IList<PaymentVoucherOutstandingInvoiceModel> paymentVoucherOutstandingInvoiceModelList = new List<PaymentVoucherOutstandingInvoiceModel>(); ;

            foreach (OutstandingInvoiceModel outstandingInvoiceModel in outstandingInvoiceModelList)
            {
                paymentVoucherOutstandingInvoiceModelList.Add(new PaymentVoucherOutstandingInvoiceModel
                {
                    PaymentVoucherId = paymentVoucherId,
                    ParticularLedgerId = particularLedgerId,
                    TransactionTypeId = (int)TransactionType.Outstanding,
                    InvoiceId = outstandingInvoiceModel.InvoiceId,
                    InvoiceType = outstandingInvoiceModel.InvoiceType,
                    InvoiceNo = outstandingInvoiceModel.InvoiceNo,
                    InvoiceDate = outstandingInvoiceModel.InvoiceDate,
                    InvoiceAmount = outstandingInvoiceModel.InvoiceAmount,
                    OutstandingAmount = outstandingInvoiceModel.OutstandingAmount,
                    InvoiceAmount_FC = outstandingInvoiceModel.InvoiceAmount_FC,
                    OutstandingAmount_FC = outstandingInvoiceModel.OutstandingAmount_FC,
                    PurchaseInvoiceId = (int)outstandingInvoiceModel.PurchaseInvoiceId,
                    SalesInvoiceId = (int)outstandingInvoiceModel.SalesInvoiceId,
                    CreditNoteId = (int)outstandingInvoiceModel.CreditNoteId,
                    DebitNoteId = (int)outstandingInvoiceModel.DebitNoteId,
                    AmountFc = null,
                    Narration = "",
                });
            }

            return await Task.Run(() =>
            {
                return PartialView("_MapOutstandingDetail", paymentVoucherOutstandingInvoiceModelList);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveVoucherDetail(List<PaymentVoucherDetailModel> paymentVoucherDetailModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                foreach (PaymentVoucherDetailModel paymentVoucherDetailModel in paymentVoucherDetailModelList)
                {
                    if (paymentVoucherDetailModel.PaymentVoucherDetId > 0)
                    {
                        // update record.
                        if (true == await _paymentVoucherDetail.UpdatePaymentVoucherDetail(paymentVoucherDetailModel))
                        {
                            data.Result.Status = true;
                            data.Result.Data = paymentVoucherDetailModel.PaymentVoucherId;
                        }
                    }
                    else
                    {
                        // add new record.
                        if (await _paymentVoucherDetail.CreatePaymentVoucherDetail(paymentVoucherDetailModel) > 0)
                        {
                            data.Result.Status = true;
                            data.Result.Data = paymentVoucherDetailModel.PaymentVoucherId;
                        }
                    }
                }
            }

            return Json(data);
        }


        [HttpPost]
        public async Task<JsonResult> SaveOutstandingDetail(List<PaymentVoucherOutstandingInvoiceModel> paymentVoucherOutstandingInvoiceModelList)
        {

            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                PaymentVoucherDetailModel paymentVoucherDetailModel = null;

                foreach (PaymentVoucherOutstandingInvoiceModel paymentVoucherOutstandingInvoiceModel in paymentVoucherOutstandingInvoiceModelList)
                {
                    paymentVoucherDetailModel = new PaymentVoucherDetailModel
                    {
                        PaymentVoucherDetId = 0,
                        PaymentVoucherId = paymentVoucherOutstandingInvoiceModel.PaymentVoucherId,
                        ParticularLedgerId = paymentVoucherOutstandingInvoiceModel.ParticularLedgerId,
                        TransactionTypeId = paymentVoucherOutstandingInvoiceModel.TransactionTypeId,
                        PurchaseInvoiceId = paymentVoucherOutstandingInvoiceModel.PurchaseInvoiceId,
                        DebitNoteId = paymentVoucherOutstandingInvoiceModel.DebitNoteId,
                        AmountFc = paymentVoucherOutstandingInvoiceModel.AmountFc,
                        Narration = paymentVoucherOutstandingInvoiceModel.Narration,
                    };

                    if (paymentVoucherOutstandingInvoiceModel.PaymentVoucherId == 0
                        || paymentVoucherOutstandingInvoiceModel.ParticularLedgerId == 0
                        || paymentVoucherOutstandingInvoiceModel.TransactionTypeId == 0
                        || (paymentVoucherOutstandingInvoiceModel.PurchaseInvoiceId == 0 && paymentVoucherOutstandingInvoiceModel.DebitNoteId == 0)
                        || paymentVoucherOutstandingInvoiceModel.AmountFc == 0
                        || paymentVoucherOutstandingInvoiceModel.AmountFc == null
                        )
                    {
                        // skip as all required fields are not entered
                        continue; // Skip the remainder of this iteration. go back to foreach
                    }

                    // add new record.
                    if (await _paymentVoucherDetail.CreatePaymentVoucherDetail(paymentVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = paymentVoucherDetailModel.PaymentVoucherId;
                    }

                }

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherDetail(int paymentVoucherDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _paymentVoucherDetail.DeletePaymentVoucherDetail(paymentVoucherDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

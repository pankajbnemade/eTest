using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        /// <summary>
        /// constractor.
        /// </summary>
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
        public async Task<IActionResult> VoucherDetail(int paymentVoucherId)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;
           
            ViewBag.ParticularLedgerList = JsonConvert.SerializeObject(await _ledger.GetLedgerSelectList(0, true));
            ViewBag.TransactionTypeList = JsonConvert.SerializeObject(EnumHelper.GetEnumListFor<TransactionType>());

            return await Task.Run(() =>
            {
                return PartialView("_VoucherDetail");
            });
        }

        /// <summary>
        /// get payment voucher details list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPaymentVoucherDetailList(int paymentVoucherId, int addRow_Blank)
        {
            
            DataTableResultModel<PaymentVoucherDetailModel> resultModel = await _paymentVoucherDetail.GetPaymentVoucherDetailByPaymentVoucherId(paymentVoucherId, addRow_Blank);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    draw = "1",
                    recordsTotal = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// add voucher detail.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> MapOutstandingDetail(Int32 paymentVoucherId, Int32 particularLedgerId)
        {
            PaymentVoucherDetailModel paymentVoucherDetailModel = new PaymentVoucherDetailModel();

            paymentVoucherDetailModel.PaymentVoucherId = paymentVoucherId;
            paymentVoucherDetailModel.ParticularLedgerId = particularLedgerId;

            return await Task.Run(() =>
            {
                return PartialView("_MapOutstandingDetail", paymentVoucherDetailModel);
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetOutstandingInvoiceList(int ledgerId, int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            Decimal ExchangeRate = 0;
            Int32 currencyId = paymentVoucherModel.CurrencyId;
            DateTime voucherDate = (DateTime)paymentVoucherModel.VoucherDate;

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetExchangeRateByCurrencyId(currencyId, voucherDate);

            ExchangeRate = null != currencyConversionModel ? (decimal)currencyConversionModel.ExchangeRate : 0;

            ExchangeRate = 0 != ExchangeRate ? 1 / ExchangeRate : 0;

            //################

            DataTableResultModel<OutstandingInvoiceModel> resultModel = await _outstandingInvoice.GetOutstandingInvoiceListByLedgerId(ledgerId, "Payment Voucher", ExchangeRate);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    draw = "1",
                    recordsTotal = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// save payment voucher detail.
        /// </summary>
        /// <param name="paymentVoucherDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (paymentVoucherDetailModel.PaymentVoucherDetId > 0)
                {
                    // update record.
                    if (true == await _paymentVoucherDetail.UpdatePaymentVoucherDetail(paymentVoucherDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _paymentVoucherDetail.CreatePaymentVoucherDetail(paymentVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveVoucherDetailInline(Int32 paymentVoucherDetId, Int32 paymentVoucherId, Int32 particularLedgerId, Int32 transactionTypeId, decimal amountFc, string narration)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            PaymentVoucherDetailModel paymentVoucherDetailModel = null;
            Console.Write(paymentVoucherDetId);

            if (paymentVoucherId > 0 && particularLedgerId > 0 && transactionTypeId > 0 && amountFc > 0)
            {
                if (paymentVoucherDetId > 0)
                {
                    paymentVoucherDetailModel = await _paymentVoucherDetail.GetPaymentVoucherDetailById(paymentVoucherDetId, paymentVoucherId);

                    paymentVoucherDetailModel.ParticularLedgerId = particularLedgerId;
                    paymentVoucherDetailModel.TransactionTypeId = transactionTypeId;
                    paymentVoucherDetailModel.AmountFc = amountFc;
                    paymentVoucherDetailModel.Narration = narration;

                    // update record.
                    if (true == await _paymentVoucherDetail.UpdatePaymentVoucherDetail(paymentVoucherDetailModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = 1;
                    }
                }
                else
                {
                    paymentVoucherDetailModel = new PaymentVoucherDetailModel
                    {
                        PaymentVoucherDetId = paymentVoucherDetId,
                        PaymentVoucherId = paymentVoucherId,
                        ParticularLedgerId = particularLedgerId,
                        TransactionTypeId = transactionTypeId,
                        AmountFc = amountFc,
                        Narration = narration,
                    };

                    // add new record.
                    if (await _paymentVoucherDetail.CreatePaymentVoucherDetail(paymentVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = "0";
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveVoucherDetailOutstanding(Int32 paymentVoucherDetId, Int32 paymentVoucherId, Int32 particularLedgerId,
                Int32 transactionTypeId, Int32 purchaseInvoiceId,  Int32 debitNoteId, decimal amountFc, string narration)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            // deserilize string search filter

            PaymentVoucherDetailModel paymentVoucherDetailModel = new PaymentVoucherDetailModel
            {

                PaymentVoucherDetId = paymentVoucherDetId,
                PaymentVoucherId = paymentVoucherId,
                ParticularLedgerId = particularLedgerId,
                TransactionTypeId = transactionTypeId,
                PurchaseInvoiceId = purchaseInvoiceId,
                //CreditNoteId = creditNoteId,
                DebitNoteId = debitNoteId,
                AmountFc = amountFc,
                Narration = narration,
            };


            if (paymentVoucherId > 0 && particularLedgerId > 0 && transactionTypeId > 0 && amountFc > 0)
            {
                if (paymentVoucherDetailModel.PaymentVoucherDetId > 0)
                {
                    // update record.
                    if (true == await _paymentVoucherDetail.UpdatePaymentVoucherDetail(paymentVoucherDetailModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = 1;
                    }
                }
                else
                {
                    // add new record.
                    if (await _paymentVoucherDetail.CreatePaymentVoucherDetail(paymentVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = "0";
                    }
                }
            }

            return Json(data);
        }


        /// <summary>
        /// delete voucher detail.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
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

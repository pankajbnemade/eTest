using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class PaymentVoucherDetailController : Controller
    {
        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly ILedger _ledger;
        /// <summary>
        /// constractor.
        /// </summary>
        public PaymentVoucherDetailController(IPaymentVoucherDetail paymentVoucherDetail, ILedger ledger)
        {
            this._paymentVoucherDetail = paymentVoucherDetail;
            this._ledger = ledger;
        }

        /// <summary>
        /// voucher detail.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> VoucherDetail(int paymentVoucherId)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;

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
        public async Task<JsonResult> GetPaymentVoucherDetailList(int paymentVoucherId)
        {
            DataTableResultModel<PaymentVoucherDetailModel> resultModel = await _paymentVoucherDetail.GetPaymentVoucherDetailByPaymentVoucherId(paymentVoucherId);

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
        public async Task<IActionResult> AddVoucherDetail(int paymentVoucherId)
        {
            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0);
             ViewBag.TransactionTypeList = EnumHelper.GetEnumListFor<TransactionType>();

            PaymentVoucherDetailModel paymentVoucherDetailModel = new PaymentVoucherDetailModel();
            paymentVoucherDetailModel.PaymentVoucherId = paymentVoucherId;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherDetail", paymentVoucherDetailModel);
            });
        }

        /// <summary>
        /// edit voucher detail.
        /// </summary>
        /// <param name="paymentVoucherDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditVoucherDetail(int paymentVoucherDetId)
        {
            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TransactionTypeList = EnumHelper.GetEnumListFor<TransactionType>();

            PaymentVoucherDetailModel paymentVoucherDetailModel = await _paymentVoucherDetail.GetPaymentVoucherDetailById(paymentVoucherDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherDetail", paymentVoucherDetailModel);
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

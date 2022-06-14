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
    [Area("Accounts")]
    public class ContraVoucherDetailController : Controller
    {
        private readonly IContraVoucherDetail _contraVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IOutstandingInvoice _outstandingInvoice;
        private readonly IContraVoucher _contraVoucher;

        public ContraVoucherDetailController(IContraVoucherDetail contraVoucherDetail, ILedger ledger, ICurrencyConversion currencyConversion,
            IOutstandingInvoice outstandingInvoice,
            IContraVoucher contraVoucher)
        {
            this._contraVoucherDetail = contraVoucherDetail;
            this._ledger = ledger;
            this._currencyConversion = currencyConversion;
            this._outstandingInvoice = outstandingInvoice;
            this._contraVoucher = contraVoucher;
        }

        public async Task<IActionResult> VoucherDetail(int contraVoucherId, int addRow_Blank)
        {
            ViewBag.ContraVoucherId = contraVoucherId;

            ContraVoucherModel contraVoucherModel = await _contraVoucher.GetContraVoucherById(contraVoucherId);

            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, contraVoucherModel.CompanyId, true);

            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = await _contraVoucherDetail.GetContraVoucherDetailByVoucherId(contraVoucherId, addRow_Blank);

            return await Task.Run(() =>
            {
                return PartialView("_VoucherDetail", contraVoucherDetailModelList);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveVoucherDetail(List<ContraVoucherDetailModel> contraVoucherDetailModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int contraVoucherId = 0;

            if (ModelState.IsValid)
            {
                foreach (ContraVoucherDetailModel contraVoucherDetailModel in contraVoucherDetailModelList)
                {
                    contraVoucherId = contraVoucherDetailModel.ContraVoucherId;

                    if (contraVoucherDetailModel.ContraVoucherDetId > 0)
                    {
                        // update record.
                        if (true == await _contraVoucherDetail.UpdateContraVoucherDetail(contraVoucherDetailModel))
                        {
                            data.Result.Status = true;
                            data.Result.Data = contraVoucherDetailModel.ContraVoucherId;
                        }
                    }
                    else
                    {
                        // add new record.
                        if (await _contraVoucherDetail.CreateContraVoucherDetail(contraVoucherDetailModel) > 0)
                        {
                            data.Result.Status = true;
                            data.Result.Data = contraVoucherDetailModel.ContraVoucherId;
                        }
                    }

                }

                await _contraVoucher.UpdateContraVoucherMasterAmount(contraVoucherId);
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherDetail(int contraVoucherDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _contraVoucherDetail.DeleteContraVoucherDetail(contraVoucherDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

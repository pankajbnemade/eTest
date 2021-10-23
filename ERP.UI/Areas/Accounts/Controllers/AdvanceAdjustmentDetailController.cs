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
    public class AdvanceAdjustmentDetailController : Controller
    {
        private readonly IAdvanceAdjustmentDetail _advanceAdjustmentDetail;
        private readonly ILedger _ledger;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IOutstandingInvoice _outstandingInvoice;
        private readonly IAdvanceAdjustment _advanceAdjustment;

        public AdvanceAdjustmentDetailController(IAdvanceAdjustmentDetail advanceAdjustmentDetail, ILedger ledger, ICurrencyConversion currencyConversion,
            IOutstandingInvoice outstandingInvoice,
            IAdvanceAdjustment advanceAdjustment)
        {
            this._advanceAdjustmentDetail = advanceAdjustmentDetail;
            this._ledger = ledger;
            this._currencyConversion = currencyConversion;
            this._outstandingInvoice = outstandingInvoice;
            this._advanceAdjustment = advanceAdjustment;
        }

        public async Task<IActionResult> AdvanceAdjustmentDetail(int advanceAdjustmentId)
        {
            ViewBag.AdvanceAdjustmentId = advanceAdjustmentId;

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await _advanceAdjustmentDetail.GetAdvanceAdjustmentDetailByAdjustmentId(advanceAdjustmentId);

            return await Task.Run(() =>
            {
                return PartialView("_AdvanceAdjustmentDetail", advanceAdjustmentDetailModelList);
            });
        }

        public async Task<IActionResult> MapOutstandingDetail(int advanceAdjustmentId)
        {
            AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);

            Decimal exchangeRate = 0;
            Int32 currencyId = advanceAdjustmentModel.CurrencyId;
            DateTime advanceAdjustmentDate = (DateTime)advanceAdjustmentModel.AdvanceAdjustmentDate;

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetExchangeRateByCurrencyId(currencyId, advanceAdjustmentDate);

            exchangeRate = null != currencyConversionModel ? (decimal)currencyConversionModel.ExchangeRate : 0;

            //exchangeRate = 0 != exchangeRate ? 1 / exchangeRate : 0;

            //################

            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await _outstandingInvoice.GetOutstandingInvoiceListByLedgerId(advanceAdjustmentModel.ParticularLedgerId, "Advance Adjustment", advanceAdjustmentId, advanceAdjustmentDate, exchangeRate);

            IList<AdvanceAdjustmentOutstandingInvoiceModel> advanceAdjustmentOutstandingInvoiceModelList = new List<AdvanceAdjustmentOutstandingInvoiceModel>(); ;

            foreach (OutstandingInvoiceModel outstandingInvoiceModel in outstandingInvoiceModelList)
            {
                advanceAdjustmentOutstandingInvoiceModelList.Add(new AdvanceAdjustmentOutstandingInvoiceModel
                {
                    AdvanceAdjustmentId = advanceAdjustmentId,
                    ParticularLedgerId = advanceAdjustmentModel.ParticularLedgerId,
                    InvoiceId = outstandingInvoiceModel.InvoiceId,
                    InvoiceType = outstandingInvoiceModel.InvoiceType,
                    InvoiceNo = outstandingInvoiceModel.InvoiceNo,
                    InvoiceDate = outstandingInvoiceModel.InvoiceDate,
                    InvoiceAmount = outstandingInvoiceModel.InvoiceAmount,
                    OutstandingAmount = outstandingInvoiceModel.OutstandingAmount,
                    InvoiceAmount_FC = outstandingInvoiceModel.InvoiceAmount_FC,
                    OutstandingAmount_FC = outstandingInvoiceModel.OutstandingAmount_FC,
                    PurchaseInvoiceId = outstandingInvoiceModel.PurchaseInvoiceId,
                    SalesInvoiceId = outstandingInvoiceModel.SalesInvoiceId,
                    CreditNoteId = outstandingInvoiceModel.CreditNoteId,
                    DebitNoteId = outstandingInvoiceModel.DebitNoteId,
                    AmountFc = null,
                    Narration = "",
                });
            }

            return await Task.Run(() =>
            {
                return PartialView("_MapOutstandingDetail", advanceAdjustmentOutstandingInvoiceModelList);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAdvanceAdjustmentDetail(List<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int advanceAdjustmentId = 0;

            if (ModelState.IsValid)
            {
                foreach (AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel in advanceAdjustmentDetailModelList)
                {
                    advanceAdjustmentId = advanceAdjustmentDetailModel.AdvanceAdjustmentId;

                    if (advanceAdjustmentDetailModel.AdvanceAdjustmentDetId > 0)
                    {
                        // update record.
                        if (true == await _advanceAdjustmentDetail.UpdateAdvanceAdjustmentDetail(advanceAdjustmentDetailModel))
                        {
                            data.Result.Status = true;
                            data.Result.Data = advanceAdjustmentDetailModel.AdvanceAdjustmentId;
                        }
                    }
                    else
                    {
                        // add new record.
                        if (await _advanceAdjustmentDetail.CreateAdvanceAdjustmentDetail(advanceAdjustmentDetailModel) > 0)
                        {
                            data.Result.Status = true;
                            data.Result.Data = advanceAdjustmentDetailModel.AdvanceAdjustmentId;
                        }
                    }

                }

                await _advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentId);
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveOutstandingDetail(List<AdvanceAdjustmentOutstandingInvoiceModel> advanceAdjustmentOutstandingInvoiceModelList)
        {

            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int advanceAdjustmentId = 0;

            if (ModelState.IsValid)
            {
                AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel = null;

                foreach (AdvanceAdjustmentOutstandingInvoiceModel advanceAdjustmentOutstandingInvoiceModel in advanceAdjustmentOutstandingInvoiceModelList)
                {
                    advanceAdjustmentId = advanceAdjustmentOutstandingInvoiceModel.AdvanceAdjustmentId;

                    advanceAdjustmentDetailModel = new AdvanceAdjustmentDetailModel
                    {
                        AdvanceAdjustmentDetId = 0,
                        AdvanceAdjustmentId = advanceAdjustmentOutstandingInvoiceModel.AdvanceAdjustmentId,
                        SalesInvoiceId = advanceAdjustmentOutstandingInvoiceModel.SalesInvoiceId,
                        PurchaseInvoiceId = advanceAdjustmentOutstandingInvoiceModel.PurchaseInvoiceId,
                        DebitNoteId = advanceAdjustmentOutstandingInvoiceModel.DebitNoteId,
                        CreditNoteId = advanceAdjustmentOutstandingInvoiceModel.CreditNoteId,
                        AmountFc = advanceAdjustmentOutstandingInvoiceModel.AmountFc == null ? 0 : (decimal)advanceAdjustmentOutstandingInvoiceModel.AmountFc,
                        Narration = advanceAdjustmentOutstandingInvoiceModel.Narration,
                    };

                    if (advanceAdjustmentDetailModel.AdvanceAdjustmentId == 0
                        || (advanceAdjustmentDetailModel.PurchaseInvoiceId == 0 && advanceAdjustmentDetailModel.DebitNoteId == 0 && advanceAdjustmentDetailModel.PurchaseInvoiceId == 0 && advanceAdjustmentDetailModel.CreditNoteId == 0)
                        || advanceAdjustmentDetailModel.AmountFc == 0
                        )
                    {
                        // skip as all required fields are not entered
                        continue; // Skip the remainder of this iteration. go back to foreach
                    }

                    // add new record.
                    if (await _advanceAdjustmentDetail.CreateAdvanceAdjustmentDetail(advanceAdjustmentDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = advanceAdjustmentDetailModel.AdvanceAdjustmentId;
                    }

                }

                await _advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentId);

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAdvanceAdjustmentDetail(int advanceAdjustmentDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _advanceAdjustmentDetail.DeleteAdvanceAdjustmentDetail(advanceAdjustmentDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

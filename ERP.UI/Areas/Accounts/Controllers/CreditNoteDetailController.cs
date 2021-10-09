using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CreditNoteDetailController : Controller
    {
        private readonly ICreditNote _creditNote;
        private readonly ICreditNoteDetail _creditNoteDetail;
        private readonly ICreditNoteDetailTax _creditNoteDetailTax;
        private readonly ICreditNoteTax _creditNoteTax;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public CreditNoteDetailController(ICreditNote creditNote, ICreditNoteDetail creditNoteDetail,
                                                ICreditNoteDetailTax creditNoteDetailTax, ICreditNoteTax creditNoteTax,
                                                IUnitOfMeasurement unitOfMeasurement)
        {
            this._creditNote = creditNote;
            this._creditNoteDetail = creditNoteDetail;
            this._creditNoteDetailTax = creditNoteDetailTax;
            this._creditNoteTax = creditNoteTax;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// creditnote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        /// 
        public async Task<IActionResult> CreditNoteDetail(int creditNoteId)
        {
            ViewBag.CreditNoteId = creditNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_CreditNoteDetail");
            });
        }

        /// <summary>
        /// get credit note details list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCreditNoteDetailList(int creditNoteId)
        {
            DataTableResultModel<CreditNoteDetailModel> resultModel = await _creditNoteDetail.GetCreditNoteDetailByCreditNoteId(creditNoteId);

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
        /// add creditnote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddCreditNoteDetail(int creditNoteId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            CreditNoteDetailModel creditNoteDetailModel = new CreditNoteDetailModel();
            creditNoteDetailModel.CreditNoteId = creditNoteId;
            creditNoteDetailModel.SrNo = await _creditNoteDetail.GenerateSrNo(creditNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteDetail", creditNoteDetailModel);
            });
        }

        /// <summary>
        /// edit creditnote detail.
        /// </summary>
        /// <param name="creditNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditCreditNoteDetail(int creditNoteDetId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            CreditNoteDetailModel creditNoteDetailModel = await _creditNoteDetail.GetCreditNoteDetailById(creditNoteDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteDetail", creditNoteDetailModel);
            });
        }

        /// <summary>
        /// save credit note detail.
        /// </summary>
        /// <param name="creditNoteDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCreditNoteDetail(CreditNoteDetailModel creditNoteDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (creditNoteDetailModel.CreditNoteDetId > 0)
                {
                    // update record.
                    if (true == await _creditNoteDetail.UpdateCreditNoteDetail(creditNoteDetailModel))
                    {
                        await _creditNoteDetailTax.UpdateCreditNoteDetailTaxAmountOnDetailUpdate(creditNoteDetailModel.CreditNoteDetId);
                        await _creditNoteTax.UpdateCreditNoteTaxAmountAll(creditNoteDetailModel.CreditNoteId);
                        data.Result.Status = true;
                    }
                }
                else
                {
                    creditNoteDetailModel.CreditNoteDetId = await _creditNoteDetail.CreateCreditNoteDetail(creditNoteDetailModel);

                    // add new record.
                    if (creditNoteDetailModel.CreditNoteDetId > 0)
                    {
                        CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById((int)creditNoteDetailModel.CreditNoteId);
                        
                        if (creditNoteModel.TaxModelType == TaxModelType.LineWise.ToString())
                        {
                            await _creditNoteDetailTax.AddCreditNoteDetailTaxByCreditNoteDetId(creditNoteDetailModel.CreditNoteDetId, (int)creditNoteModel.TaxRegisterId);
                        }
                        await _creditNoteTax.UpdateCreditNoteTaxAmountAll(creditNoteDetailModel.CreditNoteId);

                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete creditnote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCreditNoteDetail(int creditNoteDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            CreditNoteDetailModel creditNoteDetailModel = await _creditNoteDetail.GetCreditNoteDetailById(creditNoteDetId);

            if (true == await _creditNoteDetail.DeleteCreditNoteDetail(creditNoteDetId))
            {
                await _creditNoteTax.UpdateCreditNoteTaxAmountAll(creditNoteDetailModel.CreditNoteId);
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

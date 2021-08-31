using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CreditNoteDetailController : Controller
    {
        private readonly ICreditNoteDetail _creditNoteDetail;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public CreditNoteDetailController(ICreditNoteDetail creditNoteDetail, IUnitOfMeasurement unitOfMeasurement)
        {
            this._creditNoteDetail = creditNoteDetail;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// creditNote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
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
        /// add creditNote detail.
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
        /// edit creditNote detail.
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
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _creditNoteDetail.CreateCreditNoteDetail(creditNoteDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete creditNote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCreditNoteDetail(int creditNoteDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _creditNoteDetail.DeleteCreditNoteDetail(creditNoteDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

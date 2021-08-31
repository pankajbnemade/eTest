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
    public class DebitNoteDetailController : Controller
    {
        private readonly IDebitNoteDetail _debitNoteDetail;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public DebitNoteDetailController(IDebitNoteDetail debitNoteDetail, IUnitOfMeasurement unitOfMeasurement)
        {
            this._debitNoteDetail = debitNoteDetail;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// debitNote detail.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DebitNoteDetail(int debitNoteId)
        {
            ViewBag.DebitNoteId = debitNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_DebitNoteDetail");
            });
        }

        /// <summary>
        /// get debit note details list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDebitNoteDetailList(int debitNoteId)
        {
            DataTableResultModel<DebitNoteDetailModel> resultModel = await _debitNoteDetail.GetDebitNoteDetailByDebitNoteId(debitNoteId);

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
        /// add debitNote detail.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddDebitNoteDetail(int debitNoteId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            DebitNoteDetailModel debitNoteDetailModel = new DebitNoteDetailModel();
            debitNoteDetailModel.DebitNoteId = debitNoteId;
            debitNoteDetailModel.SrNo = await _debitNoteDetail.GenerateSrNo(debitNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteDetail", debitNoteDetailModel);
            });
        }

        /// <summary>
        /// edit debitNote detail.
        /// </summary>
        /// <param name="debitNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDebitNoteDetail(int debitNoteDetId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            DebitNoteDetailModel debitNoteDetailModel = await _debitNoteDetail.GetDebitNoteDetailById(debitNoteDetId);
            
            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteDetail", debitNoteDetailModel);
            });
        }

        /// <summary>
        /// save debit note detail.
        /// </summary>
        /// <param name="debitNoteDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveDebitNoteDetail(DebitNoteDetailModel debitNoteDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (debitNoteDetailModel.DebitNoteDetId > 0)
                {
                    // update record.
                    if (true == await _debitNoteDetail.UpdateDebitNoteDetail(debitNoteDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _debitNoteDetail.CreateDebitNoteDetail(debitNoteDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete debitNote detail.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteDebitNoteDetail(int debitNoteDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _debitNoteDetail.DeleteDebitNoteDetail(debitNoteDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

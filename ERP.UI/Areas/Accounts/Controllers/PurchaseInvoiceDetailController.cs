using ERP.Models.Accounts;
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
    public class PurchaseInvoiceDetailController : Controller
    {
        private readonly IPurchaseInvoiceDetail _purchaseInvoiceDetail;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public PurchaseInvoiceDetailController(IPurchaseInvoiceDetail purchaseInvoiceDetail, IUnitOfMeasurement unitOfMeasurement)
        {
            this._purchaseInvoiceDetail = purchaseInvoiceDetail;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceDetail(int purchaseInvoiceId)
        {
            ViewBag.PurchaseInvoiceId = purchaseInvoiceId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceDetail");
            });
        }

        /// <summary>
        /// get purchase invoice details list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPurchaseInvoiceDetailList(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceDetailModel> resultModel = await _purchaseInvoiceDetail.GetPurchaseInvoiceDetailByPurchaseInvoiceId(purchaseInvoiceId);

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
        /// add invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceDetail(int purchaseInvoiceId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = new PurchaseInvoiceDetailModel();
            purchaseInvoiceDetailModel.PurchaseInvoiceId = purchaseInvoiceId;
            purchaseInvoiceDetailModel.SrNo = await _purchaseInvoiceDetail.GenerateSrNo(purchaseInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceDetail", purchaseInvoiceDetailModel);
            });
        }

        /// <summary>
        /// edit invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceDetail(int purchaseInvoiceDetId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = await _purchaseInvoiceDetail.GetPurchaseInvoiceDetailById(purchaseInvoiceDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceDetail", purchaseInvoiceDetailModel);
            });
        }

        /// <summary>
        /// save purchase invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (purchaseInvoiceDetailModel.PurchaseInvoiceDetId > 0)
                {
                    // update record.
                    if (true == await _purchaseInvoiceDetail.UpdatePurchaseInvoiceDetail(purchaseInvoiceDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _purchaseInvoiceDetail.CreatePurchaseInvoiceDetail(purchaseInvoiceDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceDetail(int purchaseInvoiceDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _purchaseInvoiceDetail.DeletePurchaseInvoiceDetail(purchaseInvoiceDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

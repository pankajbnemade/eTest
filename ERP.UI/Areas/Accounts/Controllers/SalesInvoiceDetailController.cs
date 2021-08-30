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
    public class SalesInvoiceDetailController : Controller
    {
        private readonly ISalesInvoiceDetail _salesInvoiceDetail;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public SalesInvoiceDetailController(ISalesInvoiceDetail salesInvoiceDetail, IUnitOfMeasurement unitOfMeasurement)
        {
            this._salesInvoiceDetail = salesInvoiceDetail;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceDetail(int salesInvoiceId)
        {
            ViewBag.SalesInvoiceId = salesInvoiceId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceDetail");
            });
        }

        /// <summary>
        /// get sales invoice details list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetSalesInvoiceDetailList(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceDetailModel> resultModel = await _salesInvoiceDetail.GetSalesInvoiceDetailBySalesInvoiceId(salesInvoiceId);

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
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceDetail(int salesInvoiceId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            SalesInvoiceDetailModel salesInvoiceDetailModel = new SalesInvoiceDetailModel();

            salesInvoiceDetailModel.SalesInvoiceId = salesInvoiceId;
            salesInvoiceDetailModel.SrNo = await _salesInvoiceDetail.GenerateSrNo(salesInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceDetail", salesInvoiceDetailModel);
            });
        }

        /// <summary>
        /// edit invoice detail.
        /// </summary>
        /// <param name="salesInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceDetail(int salesInvoiceDetId)
        {
            ViewBag.UnitOfMeasurementList = await _unitOfMeasurement.GetUnitOfMeasurementSelectList();

            SalesInvoiceDetailModel salesInvoiceDetailModel = await _salesInvoiceDetail.GetSalesInvoiceDetailById(salesInvoiceDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceDetail", salesInvoiceDetailModel);
            });
        }

        /// <summary>
        /// save sale invoice detail.
        /// </summary>
        /// <param name="salesInvoiceDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceDetailModel.SalesInvoiceDetId > 0)
                {
                    // update record.
                    if (true == await _salesInvoiceDetail.UpdateSalesInvoiceDetail(salesInvoiceDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _salesInvoiceDetail.CreateSalesInvoiceDetail(salesInvoiceDetailModel) > 0)
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
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceDetail(int salesInvoiceDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _salesInvoiceDetail.DeleteSalesInvoiceDetail(salesInvoiceDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

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
    public class PurchaseInvoiceDetailController : Controller
    {
        private readonly IPurchaseInvoice _purchaseInvoice;
        private readonly IPurchaseInvoiceDetail _purchaseInvoiceDetail;
        private readonly IPurchaseInvoiceDetailTax _purchaseInvoiceDetailTax;
        private readonly IPurchaseInvoiceTax _purchaseInvoiceTax;
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        public PurchaseInvoiceDetailController(IPurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetail purchaseInvoiceDetail,
                                                IPurchaseInvoiceDetailTax purchaseInvoiceDetailTax, IPurchaseInvoiceTax purchaseInvoiceTax,
                                                IUnitOfMeasurement unitOfMeasurement)
        {
            this._purchaseInvoice = purchaseInvoice;
            this._purchaseInvoiceDetail = purchaseInvoiceDetail;
            this._purchaseInvoiceDetailTax = purchaseInvoiceDetailTax;
            this._purchaseInvoiceTax = purchaseInvoiceTax;
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        /// 
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
                        await _purchaseInvoiceDetailTax.UpdatePurchaseInvoiceDetailTaxAmountOnDetailUpdate(purchaseInvoiceDetailModel.PurchaseInvoiceDetId);
                        await _purchaseInvoiceTax.UpdatePurchaseInvoiceTaxAmountAll(purchaseInvoiceDetailModel.PurchaseInvoiceId);
                        data.Result.Status = true;
                    }
                }
                else
                {
                    purchaseInvoiceDetailModel.PurchaseInvoiceDetId = await _purchaseInvoiceDetail.CreatePurchaseInvoiceDetail(purchaseInvoiceDetailModel);

                    // add new record.
                    if (purchaseInvoiceDetailModel.PurchaseInvoiceDetId > 0)
                    {
                        PurchaseInvoiceModel purchaseInvoiceModel = await _purchaseInvoice.GetPurchaseInvoiceById((int)purchaseInvoiceDetailModel.PurchaseInvoiceId);
                        
                        if (purchaseInvoiceModel.TaxModelType == TaxModelType.LineWise.ToString())
                        {
                            await _purchaseInvoiceDetailTax.AddPurchaseInvoiceDetailTaxByPurchaseInvoiceDetId(purchaseInvoiceDetailModel.PurchaseInvoiceDetId, (int)purchaseInvoiceModel.TaxRegisterId);
                        }
                        await _purchaseInvoiceTax.UpdatePurchaseInvoiceTaxAmountAll(purchaseInvoiceDetailModel.PurchaseInvoiceId);

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

            PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = await _purchaseInvoiceDetail.GetPurchaseInvoiceDetailById(purchaseInvoiceDetId);

            if (true == await _purchaseInvoiceDetail.DeletePurchaseInvoiceDetail(purchaseInvoiceDetId))
            {
                await _purchaseInvoiceTax.UpdatePurchaseInvoiceTaxAmountAll(purchaseInvoiceDetailModel.PurchaseInvoiceId);
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

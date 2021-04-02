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
    public class SalesInvoiceTaxDetailController : Controller
    {
        private readonly ISalesInvoiceDetailTax _salesInvoiceDetailTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public SalesInvoiceTaxDetailController(ISalesInvoiceDetailTax salesInvoiceDetailTax, ILedger ledger)
        {
            this._salesInvoiceDetailTax = salesInvoiceDetailTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// invoice tax detail.
        /// </summary>
        /// <param name="invoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxDetail(int invoiceDetId)
        {
            ViewBag.InvoiceDetId = invoiceDetId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceTaxDetail");
            });
        }

        /// <summary>
        /// view invoice tax detail.
        /// </summary>
        /// <param name="invoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewInvoiceTaxDetail(int invoiceDetId)
        {
            ViewBag.InvoiceDetId = invoiceDetId;

            return await Task.Run(() =>
            {
                return PartialView("_ViewInvoiceTaxDetail");
            });
        }

        /// <summary>
        /// get sales invoice tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetSalesInvoiceTaxDetailList(int invoiceDetId)
        {
            DataTableResultModel<SalesInvoiceDetailTaxModel> resultModel = await _salesInvoiceDetailTax.GetSalesInvoiceDetailTaxBySalesInvoiceDetailId(invoiceDetId);

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
        /// add invoice tax detail.
        /// </summary>
        /// <param name="invoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxDetail(int invoiceDetId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel();
            salesInvoiceDetailTaxModel.InvoiceDetId = invoiceDetId;

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail");
            });
        }

        ///// <summary>
        ///// save sale invoice master.
        ///// </summary>
        ///// <param name="cityModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<JsonResult> SaveInvoiceMaster(SalesInvoiceModel salesInvoiceModel)
        //{
        //    JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

        //    if (ModelState.IsValid)
        //    {
        //        if (salesInvoiceModel.InvoiceId > 0)
        //        {
        //            // update record.
        //            if (true == await _salesInvoice.UpdateSalesInvoice(salesInvoiceModel))
        //            {
        //                data.Result.Status = true;
        //            }
        //        }
        //        else
        //        {
        //            // add new record.
        //            if (await _salesInvoice.CreateSalesInvoice(salesInvoiceModel) > 0)
        //            {
        //                data.Result.Status = true;
        //            }
        //        }
        //    }

        //    return Json(data);
        //}

        /// <summary>
        /// delete invoice tax detail.
        /// </summary>
        /// <param name="invoiceDetTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxDetail(int invoiceDetTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _salesInvoiceDetailTax.DeleteSalesInvoiceDetailTax(invoiceDetTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

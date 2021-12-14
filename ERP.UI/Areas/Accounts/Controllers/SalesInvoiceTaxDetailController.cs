using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
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
        /// <param name="salesInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxDetail(int salesInvoiceDetId)
        {
            ViewBag.SalesInvoiceDetId = salesInvoiceDetId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceTaxDetail");
            });
        }

        ///// <summary>
        ///// view invoice tax detail.
        ///// </summary>
        ///// <param name="salesInvoiceDetId"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> ViewInvoiceTaxDetail(int salesInvoiceDetId)
        //{
        //    ViewBag.SalesInvoiceDetId = salesInvoiceDetId;

        //    return await Task.Run(() =>
        //    {
        //        return PartialView("_ViewInvoiceTaxDetail");
        //    });
        //}

        /// <summary>
        /// get sales invoice tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetSalesInvoiceTaxDetailList(int salesInvoiceDetId)
        {
            DataTableResultModel<SalesInvoiceDetailTaxModel> resultModel = await _salesInvoiceDetailTax.GetSalesInvoiceDetailTaxBySalesInvoiceDetailId(salesInvoiceDetId);

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
        /// <param name="salesInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxDetail(int salesInvoiceDetId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel();
            salesInvoiceDetailTaxModel.SalesInvoiceDetId = salesInvoiceDetId;
            salesInvoiceDetailTaxModel.SrNo = await _salesInvoiceDetailTax.GenerateSrNo(salesInvoiceDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", salesInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax detail.
        /// </summary>
        /// <param name="salesInvoiceDetTaxId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxDetail(int salesInvoiceDetTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = await _salesInvoiceDetailTax.GetSalesInvoiceDetailTaxById(salesInvoiceDetTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", salesInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// save sales invoice tax detail.
        /// </summary>
        /// <param name="salesInvoiceDetailTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxDetail(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceDetailTaxModel.SalesInvoiceDetTaxId > 0)
                {
                    // update record.
                    if (true == await _salesInvoiceDetailTax.UpdateSalesInvoiceDetailTax(salesInvoiceDetailTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _salesInvoiceDetailTax.CreateSalesInvoiceDetailTax(salesInvoiceDetailTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice tax detail.
        /// </summary>
        /// <param name="salesInvoiceDetTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxDetail(int salesInvoiceDetTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _salesInvoiceDetailTax.DeleteSalesInvoiceDetailTax(salesInvoiceDetTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

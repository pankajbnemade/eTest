using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
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

        ///// <summary>
        ///// view invoice tax detail.
        ///// </summary>
        ///// <param name="invoiceDetId"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> ViewInvoiceTaxDetail(int invoiceDetId)
        //{
        //    ViewBag.InvoiceDetId = invoiceDetId;

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
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel();
            salesInvoiceDetailTaxModel.InvoiceDetId = invoiceDetId;
            salesInvoiceDetailTaxModel.SrNo = await _salesInvoiceDetailTax.GenerateSrNo(invoiceDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", salesInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax detail.
        /// </summary>
        /// <param name="invoiceDetTaxId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxDetail(int invoiceDetTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = await _salesInvoiceDetailTax.GetSalesInvoiceDetailTaxById(invoiceDetTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", salesInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// save sale invoice tax detail.
        /// </summary>
        /// <param name="salesInvoiceDetailTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxDetail(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceDetailTaxModel.InvoiceDetTaxId > 0)
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

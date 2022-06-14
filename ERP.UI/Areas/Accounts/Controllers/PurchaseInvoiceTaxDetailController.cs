using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class PurchaseInvoiceTaxDetailController : Controller
    {
        private readonly IPurchaseInvoiceDetailTax _purchaseInvoiceDetailTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public PurchaseInvoiceTaxDetailController(IPurchaseInvoiceDetailTax purchaseInvoiceDetailTax, ILedger ledger)
        {
            this._purchaseInvoiceDetailTax = purchaseInvoiceDetailTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// invoice tax detail.
        /// </summary>
        /// <param name="purchaseInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxDetail(int purchaseInvoiceDetId)
        {
            ViewBag.PurchaseInvoiceDetId = purchaseInvoiceDetId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceTaxDetail");
            });
        }

        ///// <summary>
        ///// view invoice tax detail.
        ///// </summary>
        ///// <param name="purchaseInvoiceDetId"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> ViewInvoiceTaxDetail(int purchaseInvoiceDetId)
        //{
        //    ViewBag.PurchaseInvoiceDetId = purchaseInvoiceDetId;

        //    return await Task.Run(() =>
        //    {
        //        return PartialView("_ViewInvoiceTaxDetail");
        //    });
        //}

        /// <summary>
        /// get purchase invoice tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPurchaseInvoiceTaxDetailList(int purchaseInvoiceDetId)
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = await _purchaseInvoiceDetailTax.GetPurchaseInvoiceDetailTaxByPurchaseInvoiceDetailId(purchaseInvoiceDetId);

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
        /// <param name="purchaseInvoiceDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxDetail(int purchaseInvoiceDetId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel = new PurchaseInvoiceDetailTaxModel();

            purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetId = purchaseInvoiceDetId;
            purchaseInvoiceDetailTaxModel.SrNo = await _purchaseInvoiceDetailTax.GenerateSrNo(purchaseInvoiceDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", purchaseInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax detail.
        /// </summary>
        /// <param name="purchaseInvoiceDetTaxId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxDetail(int purchaseInvoiceDetTaxId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel = await _purchaseInvoiceDetailTax.GetPurchaseInvoiceDetailTaxById(purchaseInvoiceDetTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxDetail", purchaseInvoiceDetailTaxModel);
            });
        }

        /// <summary>
        /// save purchase invoice tax detail.
        /// </summary>
        /// <param name="purchaseInvoiceDetailTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxDetail(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetTaxId > 0)
                {
                    // update record.
                    if (true == await _purchaseInvoiceDetailTax.UpdatePurchaseInvoiceDetailTax(purchaseInvoiceDetailTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _purchaseInvoiceDetailTax.CreatePurchaseInvoiceDetailTax(purchaseInvoiceDetailTaxModel) > 0)
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
        /// <param name="purchaseInvoiceDetTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxDetail(int purchaseInvoiceDetTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _purchaseInvoiceDetailTax.DeletePurchaseInvoiceDetailTax(purchaseInvoiceDetTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

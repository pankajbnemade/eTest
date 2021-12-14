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
    public class PurchaseInvoiceTaxMasterController : Controller
    {
        private readonly IPurchaseInvoiceTax _purchaseInvoiceTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public PurchaseInvoiceTaxMasterController(IPurchaseInvoiceTax purchaseInvoiceTax, ILedger ledger)
        {
            this._purchaseInvoiceTax = purchaseInvoiceTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxMaster(int purchaseInvoiceId)
        {
            ViewBag.PurchaseInvoiceId = purchaseInvoiceId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceTaxMaster");
            });
        }

        /// <summary>
        /// get purchase invoice tax master list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPurchaseInvoiceTaxMasterList(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceTaxModel> resultModel = await _purchaseInvoiceTax.GetPurchaseInvoiceTaxByPurchaseInvoiceId(purchaseInvoiceId);

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
        /// add invoice tax master.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxMaster(int purchaseInvoiceId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            PurchaseInvoiceTaxModel purchaseInvoiceTaxModel = new PurchaseInvoiceTaxModel();
            purchaseInvoiceTaxModel.PurchaseInvoiceId = purchaseInvoiceId;
            purchaseInvoiceTaxModel.SrNo = await _purchaseInvoiceTax.GenerateSrNo(purchaseInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", purchaseInvoiceTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax master.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxMaster(int purchaseInvoiceTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            PurchaseInvoiceTaxModel purchaseInvoiceTaxModel = await _purchaseInvoiceTax.GetPurchaseInvoiceTaxById(purchaseInvoiceTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", purchaseInvoiceTaxModel);
            });
        }

        /// <summary>
        /// save purchase invoice tax master.
        /// </summary>
        /// <param name="purchaseInvoiceTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxMaster(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (purchaseInvoiceTaxModel.PurchaseInvoiceTaxId > 0)
                {
                    // update record.
                    if (true == await _purchaseInvoiceTax.UpdatePurchaseInvoiceTax(purchaseInvoiceTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _purchaseInvoiceTax.CreatePurchaseInvoiceTax(purchaseInvoiceTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice tax master.
        /// </summary>
        /// <param name="purchaseInvoiceTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxMaster(int purchaseInvoiceTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _purchaseInvoiceTax.DeletePurchaseInvoiceTax(purchaseInvoiceTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

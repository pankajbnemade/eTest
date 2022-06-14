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
    public class SalesInvoiceTaxMasterController : Controller
    {
        private readonly ISalesInvoiceTax _salesInvoiceTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public SalesInvoiceTaxMasterController(ISalesInvoiceTax salesInvoiceTax, ILedger ledger)
        {
            this._salesInvoiceTax = salesInvoiceTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxMaster(int salesInvoiceId)
        {
            ViewBag.SalesInvoiceId = salesInvoiceId;

            return await Task.Run(() =>
            {
                return PartialView("_InvoiceTaxMaster");
            });
        }

        /// <summary>
        /// get sales invoice tax master list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetSalesInvoiceTaxMasterList(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceTaxModel> resultModel = await _salesInvoiceTax.GetSalesInvoiceTaxBySalesInvoiceId(salesInvoiceId);

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
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxMaster(int salesInvoiceId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            SalesInvoiceTaxModel salesInvoiceTaxModel = new SalesInvoiceTaxModel();

            salesInvoiceTaxModel.SalesInvoiceId = salesInvoiceId;
            salesInvoiceTaxModel.SrNo = await _salesInvoiceTax.GenerateSrNo(salesInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", salesInvoiceTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax master.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxMaster(int salesInvoiceTaxId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            SalesInvoiceTaxModel salesInvoiceTaxModel = await _salesInvoiceTax.GetSalesInvoiceTaxById(salesInvoiceTaxId);

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", salesInvoiceTaxModel);
            });
        }

        /// <summary>
        /// save sales invoice tax master.
        /// </summary>
        /// <param name="salesInvoiceTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxMaster(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceTaxModel.SalesInvoiceTaxId > 0)
                {
                    // update record.
                    if (true == await _salesInvoiceTax.UpdateSalesInvoiceTax(salesInvoiceTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _salesInvoiceTax.CreateSalesInvoiceTax(salesInvoiceTaxModel) > 0)
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
        /// <param name="salesInvoiceTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxMaster(int salesInvoiceTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _salesInvoiceTax.DeleteSalesInvoiceTax(salesInvoiceTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

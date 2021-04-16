using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
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
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> InvoiceTaxMaster(int invoiceId)
        {
            ViewBag.InvoiceId = invoiceId;

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
        public async Task<JsonResult> GetSalesInvoiceTaxMasterList(int invoiceId)
        {
            DataTableResultModel<SalesInvoiceTaxModel> resultModel = await _salesInvoiceTax.GetSalesInvoiceTaxBySalesInvoiceId(invoiceId);

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
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceTaxMaster(int invoiceId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17);

            SalesInvoiceTaxModel salesInvoiceTaxModel = new SalesInvoiceTaxModel();
            salesInvoiceTaxModel.InvoiceId = invoiceId;
            salesInvoiceTaxModel.SrNo = await _salesInvoiceTax.GenerateSrNo(invoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", salesInvoiceTaxModel);
            });
        }

        /// <summary>
        /// edit invoice tax master.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceTaxMaster(int invoiceTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17);

            SalesInvoiceTaxModel salesInvoiceTaxModel = await _salesInvoiceTax.GetSalesInvoiceTaxById(invoiceTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceTaxMaster", salesInvoiceTaxModel);
            });
        }

        /// <summary>
        /// save sale invoice tax master.
        /// </summary>
        /// <param name="salesInvoiceTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceTaxMaster(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceTaxModel.InvoiceTaxId > 0)
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
        /// <param name="invoiceTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceTaxMaster(int invoiceTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _salesInvoiceTax.DeleteSalesInvoiceTax(invoiceTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

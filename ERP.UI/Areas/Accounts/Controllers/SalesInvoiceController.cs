using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private readonly ISalesInvoice _salesInvoice;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;

        /// <summary>
        /// constractor.
        /// </summary>
        public SalesInvoiceController(
            ISalesInvoice salesInvoice,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency)
        {
            this._salesInvoice = salesInvoice;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList(0);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search sales invoice result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetSalesInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterSalesInvoiceModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterSalesInvoiceModel>(searchFilter);
            // get data.
            DataTableResultModel<SalesInvoiceModel> resultModel = await _salesInvoice.GetSalesInvoiceList(dataTableAjaxPostModel, searchFilterModel);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    dataTableAjaxPostModel.draw,
                    recordsTotal = resultModel.TotalResultCount,
                    recordsFiltered = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// add new invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddInvoiceMaster()
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.BillToAddressList = await _ledgerAddress.GetLedgerAddressSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceMaster", salesInvoiceModel);
            });
        }

        /// <summary>
        /// edit invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceMaster(int invoiceId)
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.BillToAddressList = await _ledgerAddress.GetLedgerAddressSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            SalesInvoiceModel salesInvoiceModel = await _salesInvoice.GetSalesInvoiceById(invoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceMaster", salesInvoiceModel);
            });
        }

        /// <summary>
        /// save sale invoice master.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceMaster(SalesInvoiceModel salesInvoiceModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (salesInvoiceModel.InvoiceId > 0)
                {
                    // update record.
                    if (true == await _salesInvoice.UpdateSalesInvoice(salesInvoiceModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _salesInvoice.CreateSalesInvoice(salesInvoiceModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// manage invoice.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManageInvoice(int invoiceId)
        {
            ViewBag.InvoiceId = invoiceId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewInvoiceMaster(int invoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = await _salesInvoice.GetSalesInvoiceById(invoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewInvoiceMaster", salesInvoiceModel);
            });
        }

        /// <summary>
        /// delete invoice master.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceMaster(int invoiceId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _salesInvoice.DeleteSalesInvoice(invoiceId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

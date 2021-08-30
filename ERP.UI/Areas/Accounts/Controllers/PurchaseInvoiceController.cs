using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class PurchaseInvoiceController : Controller
    {
        private readonly IPurchaseInvoice _purchaseInvoice;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        public PurchaseInvoiceController(
            IPurchaseInvoice purchaseInvoice,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._purchaseInvoice = purchaseInvoice;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetPurchaseInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterPurchaseInvoiceModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPurchaseInvoiceModel>(searchFilter);
            // get data.
            DataTableResultModel<PurchaseInvoiceModel> resultModel = await _purchaseInvoice.GetPurchaseInvoiceList(dataTableAjaxPostModel, searchFilterModel);

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
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            PurchaseInvoiceModel purchaseInvoiceModel = new PurchaseInvoiceModel();
            purchaseInvoiceModel.CompanyId = userSession.CompanyId;
            purchaseInvoiceModel.FinancialYearId = userSession.FinancialYearId;

            // generate no.
            GenerateNoModel generateNoModel = await _purchaseInvoice.GenerateInvoiceNo(userSession.CompanyId, userSession.FinancialYearId);
            purchaseInvoiceModel.InvoiceNo = generateNoModel.VoucherNo;
            purchaseInvoiceModel.InvoiceDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceMaster", purchaseInvoiceModel);
            });
        }

        /// <summary>
        /// edit invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditInvoiceMaster(int purchaseInvoiceId)
        {
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            PurchaseInvoiceModel purchaseInvoiceModel = await _purchaseInvoice.GetPurchaseInvoiceById(purchaseInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_AddInvoiceMaster", purchaseInvoiceModel);
            });
        }

        /// <summary>
        /// get bill to address based on ledgerId
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> GetBillToAddressByLedgerId(int ledgerId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList = await _ledgerAddress.GetLedgerAddressSelectList(ledgerId);
            if (null != selectList && selectList.Any())
            {
                data.Result.Status = true;
                data.Result.Data = selectList;
            }
            else
            {
                data.Result.Message = "NoItems";
            }

            return Json(data); // returns.
        }

        [HttpPost]
        public async Task<JsonResult> GetExchangeRateByCurrencyId(int currencyId, DateTime invoiceDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            CurrencyConversionModel currencyConversionModel = await
                _currencyConversion.GetExchangeRateByCurrencyId(currencyId, invoiceDate);

            if (null != currencyConversionModel)
            {
                data.Result.Status = true;
                data.Result.Data = currencyConversionModel.ExchangeRate;
            }
            else
            {
                data.Result.Data = "0";
            }

            return Json(data); // returns.
        }

        /// <summary>
        /// save sale invoice master.
        /// </summary>
        /// <param name="purchaseInvoiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveInvoiceMaster(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (purchaseInvoiceModel.PurchaseInvoiceId > 0)
                {
                    // update record.
                    if (true == await _purchaseInvoice.UpdatePurchaseInvoice(purchaseInvoiceModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _purchaseInvoice.CreatePurchaseInvoice(purchaseInvoiceModel) > 0)
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
        public async Task<IActionResult> ManageInvoice(int purchaseInvoiceId)
        {
            ViewBag.PurchaseInvoiceId = purchaseInvoiceId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewInvoiceMaster(int purchaseInvoiceId)
        {
            PurchaseInvoiceModel purchaseInvoiceModel = await _purchaseInvoice.GetPurchaseInvoiceById(purchaseInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewInvoiceMaster", purchaseInvoiceModel);
            });
        }

        /// <summary>
        /// view invoice summary.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewInvoiceSummary(int purchaseInvoiceId)
        {
            PurchaseInvoiceModel purchaseInvoiceModel = await _purchaseInvoice.GetPurchaseInvoiceById(purchaseInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewInvoiceSummary", purchaseInvoiceModel);
            });
        }

        /// <summary>
        /// delete invoice master.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceMaster(int purchaseInvoiceId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _purchaseInvoice.DeletePurchaseInvoice(purchaseInvoiceId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

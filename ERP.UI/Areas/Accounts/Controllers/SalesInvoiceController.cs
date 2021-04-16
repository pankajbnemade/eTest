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
    public class SalesInvoiceController : Controller
    {
        private readonly ISalesInvoice _salesInvoice;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        public SalesInvoiceController(
            ISalesInvoice salesInvoice,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._salesInvoice = salesInvoice;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor);

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
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor);
            ViewBag.BankLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.BankAccount);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();

            salesInvoiceModel.CompanyId = userSession.CompanyId;
            salesInvoiceModel.FinancialYearId = userSession.FinancialYearId;
            // generate no.
            GenerateNoModel generateNoModel = await _salesInvoice.GenerateInvoiceNo(userSession.CompanyId, userSession.FinancialYearId);
            salesInvoiceModel.InvoiceNo = generateNoModel.VoucherNo;
            salesInvoiceModel.InvoiceDate = DateTime.Now;

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
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor);
            ViewBag.BankLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.BankAccount);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
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

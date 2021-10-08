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
        private readonly ISalesInvoiceDetailTax _salesInvoiceDetailTax;
        private readonly ISalesInvoiceTax _salesInvoiceTax;
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
            ICurrencyConversion currencyConversion,
            ISalesInvoiceDetailTax salesInvoiceDetailTax,
            ISalesInvoiceTax salesInvoiceTax)
        {
            this._salesInvoice = salesInvoice;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
            this._salesInvoiceDetailTax = salesInvoiceDetailTax;
            this._salesInvoiceTax = salesInvoiceTax;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);

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
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
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
        public async Task<IActionResult> EditInvoiceMaster(int salesInvoiceId)
        {
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            SalesInvoiceModel salesInvoiceModel = await _salesInvoice.GetSalesInvoiceById(salesInvoiceId);

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
                if (salesInvoiceModel.SalesInvoiceId > 0)
                {
                    SalesInvoiceModel salesInvoiceModel_Old = await _salesInvoice.GetSalesInvoiceById(salesInvoiceModel.SalesInvoiceId);

                    // update record.
                    if (true == await _salesInvoice.UpdateSalesInvoice(salesInvoiceModel))
                    {
                        if (salesInvoiceModel_Old.TaxModelType != salesInvoiceModel.TaxModelType)
                        {
                            await _salesInvoiceDetailTax.DeleteSalesInvoiceDetailTaxBySalesInvoiceId(salesInvoiceModel.SalesInvoiceId);
                            await _salesInvoiceTax.DeleteSalesInvoiceTaxBySalesInvoiceId(salesInvoiceModel.SalesInvoiceId);

                            if (salesInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString())
                            {
                                await _salesInvoiceTax.AddSalesInvoiceTaxBySalesInvoiceId(salesInvoiceModel.SalesInvoiceId, (int)salesInvoiceModel.TaxRegisterId);
                            }
                            else if (salesInvoiceModel.TaxModelType == TaxModelType.LineWise.ToString())
                            {
                                await _salesInvoiceDetailTax.AddSalesInvoiceDetailTaxBySalesInvoiceId(salesInvoiceModel.SalesInvoiceId, (int)salesInvoiceModel.TaxRegisterId);
                            }
                        }
                        else
                        {
                            await _salesInvoiceTax.UpdateSalesInvoiceTaxAmountAll(salesInvoiceModel.SalesInvoiceId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = salesInvoiceModel.SalesInvoiceId;
                    }
                }
                else
                {
                    salesInvoiceModel.SalesInvoiceId = await _salesInvoice.CreateSalesInvoice(salesInvoiceModel);
                    // add new record.
                    if (salesInvoiceModel.SalesInvoiceId > 0)
                    {
                        if (salesInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString())
                        {
                            await _salesInvoiceTax.AddSalesInvoiceTaxBySalesInvoiceId(salesInvoiceModel.SalesInvoiceId, (int)salesInvoiceModel.TaxRegisterId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = salesInvoiceModel.SalesInvoiceId;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// manage invoice.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManageInvoice(int salesInvoiceId)
        {
            ViewBag.SalesInvoiceId = salesInvoiceId;

            SalesInvoiceModel salesInvoiceModel = await _salesInvoice.GetSalesInvoiceById(salesInvoiceId);

            ViewBag.IsTaxMasterVisible = salesInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString() ? true : false;
            ViewBag.IsApprovalRequestVisible = salesInvoiceModel.StatusId == 1 || salesInvoiceModel.StatusId == 3 ? true : false;
            ViewBag.IsApproveVisible = salesInvoiceModel.StatusId == 2 ? true : false;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view invoice master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewInvoiceMaster(int salesInvoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = await _salesInvoice.GetSalesInvoiceById(salesInvoiceId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewInvoiceMaster", salesInvoiceModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusInvoiceMaster(int salesInvoiceId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (salesInvoiceId > 0)
            {
                if (true == await _salesInvoice.UpdateStatusSalesInvoice(salesInvoiceId, statusId))
                {
                    data.Result.Status = true;
                    data.Result.Data = salesInvoiceId;
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice master.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteInvoiceMaster(int salesInvoiceId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _salesInvoice.DeleteSalesInvoice(salesInvoiceId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

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
        private readonly IPurchaseInvoiceDetailTax _purchaseInvoiceDetailTax;
        private readonly IPurchaseInvoiceTax _purchaseInvoiceTax;
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
            ICurrencyConversion currencyConversion,
            IPurchaseInvoiceDetailTax purchaseInvoiceDetailTax,
            IPurchaseInvoiceTax purchaseInvoiceTax)
        {
            this._purchaseInvoice = purchaseInvoice;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
            this._purchaseInvoiceDetailTax = purchaseInvoiceDetailTax;
            this._purchaseInvoiceTax = purchaseInvoiceTax;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);

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
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
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
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
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
                    PurchaseInvoiceModel purchaseInvoiceModel_Old = await _purchaseInvoice.GetPurchaseInvoiceById(purchaseInvoiceModel.PurchaseInvoiceId);

                    // update record.
                    if (true == await _purchaseInvoice.UpdatePurchaseInvoice(purchaseInvoiceModel))
                    {
                        if (purchaseInvoiceModel_Old.TaxModelType != purchaseInvoiceModel.TaxModelType)
                        {
                            await _purchaseInvoiceDetailTax.DeletePurchaseInvoiceDetailTaxByPurchaseInvoiceId(purchaseInvoiceModel.PurchaseInvoiceId);
                            await _purchaseInvoiceTax.DeletePurchaseInvoiceTaxByPurchaseInvoiceId(purchaseInvoiceModel.PurchaseInvoiceId);

                            if (purchaseInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString())
                            {
                                await _purchaseInvoiceTax.AddPurchaseInvoiceTaxByPurchaseInvoiceId(purchaseInvoiceModel.PurchaseInvoiceId, (int)purchaseInvoiceModel.TaxRegisterId);
                            }
                            else if (purchaseInvoiceModel.TaxModelType == TaxModelType.LineWise.ToString())
                            {
                                await _purchaseInvoiceDetailTax.AddPurchaseInvoiceDetailTaxByPurchaseInvoiceId(purchaseInvoiceModel.PurchaseInvoiceId, (int)purchaseInvoiceModel.TaxRegisterId);
                            }
                        }
                        else
                        {
                            await _purchaseInvoiceTax.UpdatePurchaseInvoiceTaxAmountAll(purchaseInvoiceModel.PurchaseInvoiceId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = purchaseInvoiceModel.PurchaseInvoiceId;
                    }
                }
                else
                {
                    purchaseInvoiceModel.PurchaseInvoiceId = await _purchaseInvoice.CreatePurchaseInvoice(purchaseInvoiceModel);
                    // add new record.
                    if (purchaseInvoiceModel.PurchaseInvoiceId > 0)
                    {
                        if (purchaseInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString())
                        {
                            await _purchaseInvoiceTax.AddPurchaseInvoiceTaxByPurchaseInvoiceId(purchaseInvoiceModel.PurchaseInvoiceId, (int)purchaseInvoiceModel.TaxRegisterId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = purchaseInvoiceModel.PurchaseInvoiceId;
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

            PurchaseInvoiceModel purchaseInvoiceModel = await _purchaseInvoice.GetPurchaseInvoiceById(purchaseInvoiceId);

            ViewBag.IsTaxMasterVisible = purchaseInvoiceModel.TaxModelType == TaxModelType.SubTotal.ToString() ? true : false;
            //ViewBag.IsApprovalRequestVisible = purchaseInvoiceModel.StatusId == 1 || purchaseInvoiceModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = purchaseInvoiceModel.StatusId == 2 ? true : false;

            ViewBag.IsApprovalRequestVisible = purchaseInvoiceModel.StatusId == (int)DocumentStatus.Inprocess || purchaseInvoiceModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false;
            ViewBag.IsApproveVisible = purchaseInvoiceModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false;
            ViewBag.IsCancelVisible = purchaseInvoiceModel.StatusId != (int)DocumentStatus.Cancelled ? true : false;

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

        [HttpPost]
        public async Task<JsonResult> UpdateStatusInvoiceMaster(int purchaseInvoiceId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (purchaseInvoiceId > 0)
            {
                if (true == await _purchaseInvoice.UpdateStatusPurchaseInvoice(purchaseInvoiceId, statusId))
                {
                    data.Result.Status = true;
                    data.Result.Data = purchaseInvoiceId;
                }
            }

            return Json(data);
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

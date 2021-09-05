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
    public class PaymentVoucherController : Controller
    {
        private readonly IPaymentVoucher _paymentVoucher;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        public PaymentVoucherController(
            IPaymentVoucher paymentVoucher,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._paymentVoucher = paymentVoucher;
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
        /// get search payment voucher result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetPaymentVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterPaymentVoucherModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPaymentVoucherModel>(searchFilter);
            // get data.
            DataTableResultModel<PaymentVoucherModel> resultModel = await _paymentVoucher.GetPaymentVoucherList(dataTableAjaxPostModel, searchFilterModel);

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
        /// add new voucher master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddVoucherMaster()
        {
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
            paymentVoucherModel.CompanyId = userSession.CompanyId;
            paymentVoucherModel.FinancialYearId = userSession.FinancialYearId;

            // generate no.
            GenerateNoModel generateNoModel = await _paymentVoucher.GeneratePaymentVoucherNo(userSession.CompanyId, userSession.FinancialYearId);
            paymentVoucherModel.VoucherNo = generateNoModel.VoucherNo;
            paymentVoucherModel.VoucherDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", paymentVoucherModel);
            });
        }

        /// <summary>
        /// edit voucher master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditVoucherMaster(int paymentVoucherId)
        {
            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", paymentVoucherModel);
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
        public async Task<JsonResult> GetExchangeRateByCurrencyId(int currencyId, DateTime voucherDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            CurrencyConversionModel currencyConversionModel = await
                _currencyConversion.GetExchangeRateByCurrencyId(currencyId, voucherDate);

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
        /// save sale voucher master.
        /// </summary>
        /// <param name="paymentVoucherModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveVoucherMaster(PaymentVoucherModel paymentVoucherModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (paymentVoucherModel.PaymentVoucherId > 0)
                {
                    // update record.
                    if (true == await _paymentVoucher.UpdatePaymentVoucher(paymentVoucherModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _paymentVoucher.CreatePaymentVoucher(paymentVoucherModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// manage voucher.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManageVoucher(int paymentVoucherId)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view voucher master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewVoucherMaster(int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherMaster", paymentVoucherModel);
            });
        }

        /// <summary>
        /// view voucher summary.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewVoucherSummary(int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherSummary", paymentVoucherModel);
            });
        }

        /// <summary>
        /// delete voucher master.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteVoucherMaster(int paymentVoucherId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _paymentVoucher.DeletePaymentVoucher(paymentVoucherId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

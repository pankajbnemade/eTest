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
    public class ContraVoucherController : Controller
    {
        private readonly IContraVoucher _contraVoucher;
        private readonly IContraVoucherDetail _contraVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        public ContraVoucherController(
            IContraVoucher contraVoucher,
            IContraVoucherDetail contraVoucherDetail,
            ILedger ledger,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._contraVoucher = contraVoucher;
            this._contraVoucherDetail = contraVoucherDetail;
            this._ledger = ledger;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetContraVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterContraVoucherModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterContraVoucherModel>(searchFilter);
            // get data.
            DataTableResultModel<ContraVoucherModel> resultModel = await _contraVoucher.GetContraVoucherList(dataTableAjaxPostModel, searchFilterModel);

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

        public async Task<IActionResult> ManageVoucher(int contraVoucherId)
        {
            ViewBag.ContraVoucherId = contraVoucherId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> MasterButtons(int contraVoucherId)
        {
            ContraVoucherModel contraVoucherModel = await _contraVoucher.GetContraVoucherById(contraVoucherId);

            ContraVoucherMasterButtonsModel contraVoucherMasterButtonsModel = null;

            contraVoucherMasterButtonsModel = new ContraVoucherMasterButtonsModel()
            {
                ContraVoucherId = contraVoucherId,
                IsApprovalRequestVisible = contraVoucherModel.StatusId == (int)DocumentStatus.Inprocess || contraVoucherModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false,
                IsApproveVisible = contraVoucherModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false,
                IsCancelVisible = contraVoucherModel.StatusId != (int)DocumentStatus.Cancelled ? true : false,
            };

            return await Task.Run(() =>
            {
                return PartialView("_MasterButtons", contraVoucherMasterButtonsModel);
            });
        }

        public async Task<IActionResult> ViewVoucherMaster(int contraVoucherId)
        {
            ContraVoucherModel contraVoucherModel = await _contraVoucher.GetContraVoucherById(contraVoucherId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherMaster", contraVoucherModel);
            });
        }

        public async Task<IActionResult> AddVoucherMaster()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ContraVoucherModel contraVoucherModel = new ContraVoucherModel();
            contraVoucherModel.CompanyId = userSession.CompanyId;
            contraVoucherModel.FinancialYearId = userSession.FinancialYearId;
            contraVoucherModel.NoOfLineItems = 0;

            // generate no.
            GenerateNoModel generateNoModel = await _contraVoucher.GenerateContraVoucherNo(userSession.CompanyId, userSession.FinancialYearId);
            contraVoucherModel.VoucherNo = generateNoModel.VoucherNo;
            contraVoucherModel.VoucherDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", contraVoucherModel);
            });
        }

        public async Task<IActionResult> EditVoucherMaster(int contraVoucherId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            ContraVoucherModel contraVoucherModel = await _contraVoucher.GetContraVoucherById(contraVoucherId);

            DataTableResultModel<ContraVoucherDetailModel> resultModel = await _contraVoucherDetail.GetContraVoucherDetailByContraVoucherId(contraVoucherId, 0);

            contraVoucherModel.NoOfLineItems = resultModel.TotalResultCount;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", contraVoucherModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveVoucherMaster(ContraVoucherModel contraVoucherModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (contraVoucherModel.ContraVoucherId > 0)
                {
                    // update record.
                    if (true == await _contraVoucher.UpdateContraVoucher(contraVoucherModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = contraVoucherModel.ContraVoucherId;
                    }
                }
                else
                {
                    // add new record.
                    contraVoucherModel.ContraVoucherId = await _contraVoucher.CreateContraVoucher(contraVoucherModel);

                    if (contraVoucherModel.ContraVoucherId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = contraVoucherModel.ContraVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusVoucherMaster(int contraVoucherId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (contraVoucherId > 0)
            {
                ContraVoucherModel contraVoucherModel = await _contraVoucher.GetContraVoucherById(contraVoucherId);

                if (contraVoucherModel.AmountFc == 0
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Amount FC should be getter than 0. Please update Amount FC";
                }
                else if (contraVoucherModel.CreditAmountFc != contraVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Credit Amount FC != Amount FC. Please update particular details Credit Amount FC Or change Amount FC";
                }
                else if (contraVoucherModel.DebitAmountFc != contraVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Debit Amount FC != Amount FC. Please update particular details Debit Amount FC Or change Amount FC";
                }
                else
                {
                    if (true == await _contraVoucher.UpdateStatusContraVoucher(contraVoucherId, statusId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = contraVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherMaster(int contraVoucherId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _contraVoucher.DeleteContraVoucher(contraVoucherId))
            {
                data.Result.Status = true;
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

    }
}

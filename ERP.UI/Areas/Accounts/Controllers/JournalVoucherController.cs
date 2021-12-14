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
    [Area("Accounts")]
    public class JournalVoucherController : Controller
    {
        private readonly IJournalVoucher _journalVoucher;
        private readonly IJournalVoucherDetail _journalVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        public JournalVoucherController(
            IJournalVoucher journalVoucher,
            IJournalVoucherDetail journalVoucherDetail,
            ILedger ledger,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._journalVoucher = journalVoucher;
            this._journalVoucherDetail = journalVoucherDetail;
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
        public async Task<IActionResult> GetJournalVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterJournalVoucherModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterJournalVoucherModel>(searchFilter);
            // get data.
            DataTableResultModel<JournalVoucherModel> resultModel = await _journalVoucher.GetJournalVoucherList(dataTableAjaxPostModel, searchFilterModel);

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

        public async Task<IActionResult> ManageVoucher(int journalVoucherId)
        {
            ViewBag.JournalVoucherId = journalVoucherId;

            //JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            //ViewBag.IsApprovalRequestVisible = journalVoucherModel.StatusId == 1 || journalVoucherModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = journalVoucherModel.StatusId == 2 ? true : false;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> MasterButtons(int journalVoucherId)
        {
            JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            JournalVoucherMasterButtonsModel journalVoucherMasterButtonsModel = null;

            journalVoucherMasterButtonsModel = new JournalVoucherMasterButtonsModel()
            {
                JournalVoucherId = journalVoucherId,
                IsApprovalRequestVisible = journalVoucherModel.StatusId == (int)DocumentStatus.Inprocess || journalVoucherModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false,
                IsApproveVisible = journalVoucherModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false,
                IsCancelVisible = journalVoucherModel.StatusId != (int)DocumentStatus.Cancelled ? true : false,
            };

            //ViewBag.IsApprovalRequestVisible = journalVoucherModel.StatusId == 1 || journalVoucherModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = journalVoucherModel.StatusId == 2 ? true : false;

            return await Task.Run(() =>
            {
                return PartialView("_MasterButtons", journalVoucherMasterButtonsModel);
            });
        }

        public async Task<IActionResult> ViewVoucherMaster(int journalVoucherId)
        {
            JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherMaster", journalVoucherModel);
            });
        }

        public async Task<IActionResult> AddVoucherMaster()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            JournalVoucherModel journalVoucherModel = new JournalVoucherModel();
            journalVoucherModel.CompanyId = userSession.CompanyId;
            journalVoucherModel.FinancialYearId = userSession.FinancialYearId;
            journalVoucherModel.NoOfLineItems = 0;

            // generate no.
            GenerateNoModel generateNoModel = await _journalVoucher.GenerateJournalVoucherNo(userSession.CompanyId, userSession.FinancialYearId);
            journalVoucherModel.VoucherNo = generateNoModel.VoucherNo;
            journalVoucherModel.VoucherDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", journalVoucherModel);
            });
        }

        public async Task<IActionResult> EditVoucherMaster(int journalVoucherId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            DataTableResultModel<JournalVoucherDetailModel> resultModel = await _journalVoucherDetail.GetJournalVoucherDetailByJournalVoucherId(journalVoucherId, 0);

            journalVoucherModel.NoOfLineItems = resultModel.TotalResultCount;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", journalVoucherModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveVoucherMaster(JournalVoucherModel journalVoucherModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (journalVoucherModel.JournalVoucherId > 0)
                {
                    // update record.
                    if (true == await _journalVoucher.UpdateJournalVoucher(journalVoucherModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = journalVoucherModel.JournalVoucherId;
                    }
                }
                else
                {
                    // add new record.
                    journalVoucherModel.JournalVoucherId = await _journalVoucher.CreateJournalVoucher(journalVoucherModel);

                    if (journalVoucherModel.JournalVoucherId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = journalVoucherModel.JournalVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusVoucherMaster(int journalVoucherId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (journalVoucherId > 0)
            {
                JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

                if (journalVoucherModel.AmountFc == 0
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Amount FC should be getter than 0. Please update Amount FC";
                }
                else if (journalVoucherModel.CreditAmountFc != journalVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Credit Amount FC != Amount FC. Please update particular details Credit Amount FC Or change Amount FC";
                }
                else if (journalVoucherModel.DebitAmountFc != journalVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Debit Amount FC != Amount FC. Please update particular details Debit Amount FC Or change Amount FC";
                }
                else
                {
                    if (true == await _journalVoucher.UpdateStatusJournalVoucher(journalVoucherId, statusId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = journalVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherMaster(int journalVoucherId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _journalVoucher.DeleteJournalVoucher(journalVoucherId))
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

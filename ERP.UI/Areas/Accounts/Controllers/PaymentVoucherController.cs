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
    public class PaymentVoucherController : Controller
    {
        private readonly IPaymentVoucher _paymentVoucher;
        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        public PaymentVoucherController(
            IPaymentVoucher paymentVoucher,
            IPaymentVoucherDetail paymentVoucherDetail,
            ILedger ledger,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._paymentVoucher = paymentVoucher;
            this._paymentVoucherDetail = paymentVoucherDetail;
            this._ledger = ledger;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
        }

        public async Task<IActionResult> Index()
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.TypeCorBList = EnumHelper.GetEnumListFor<TypeCorB>();
            ViewBag.LedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetPaymentVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterPaymentVoucherModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPaymentVoucherModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

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

        public async Task<IActionResult> ManageVoucher(int paymentVoucherId)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;

            //PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            //ViewBag.IsApprovalRequestVisible = paymentVoucherModel.StatusId == 1 || paymentVoucherModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = paymentVoucherModel.StatusId == 2 ? true : false;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> MasterButtons(int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            PaymentVoucherMasterButtonsModel paymentVoucherMasterButtonsModel = null;

            paymentVoucherMasterButtonsModel = new PaymentVoucherMasterButtonsModel()
            {
                PaymentVoucherId = paymentVoucherId,
                IsApprovalRequestVisible = paymentVoucherModel.StatusId == (int)DocumentStatus.Inprocess || paymentVoucherModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false,
                IsApproveVisible = paymentVoucherModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false,
                IsCancelVisible = paymentVoucherModel.StatusId != (int)DocumentStatus.Cancelled ? true : false,
            };

            //ViewBag.IsApprovalRequestVisible = paymentVoucherModel.StatusId == 1 || paymentVoucherModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = paymentVoucherModel.StatusId == 2 ? true : false;

            return await Task.Run(() =>
            {
                return PartialView("_MasterButtons", paymentVoucherMasterButtonsModel);
            });
        }

        public async Task<IActionResult> ViewVoucherMaster(int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            LedgerModel ledgerModel = await _ledger.GetClosingBalanceByAccountLedgerId((int)paymentVoucherModel.AccountLedgerId, (DateTime)paymentVoucherModel.VoucherDate);

            if (null != ledgerModel)
            {
                paymentVoucherModel.ClosingBalance = ledgerModel.ClosingBalance;
            }
            else
            {
                paymentVoucherModel.ClosingBalance = 0;
            }

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherMaster", paymentVoucherModel);
            });
        }

        public async Task<IActionResult> AddVoucherMaster()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TypeCorBList = EnumHelper.GetEnumListFor<TypeCorB>();
            ViewBag.PaymentTypeList = EnumHelper.GetEnumListFor<PaymentType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
            paymentVoucherModel.CompanyId = userSession.CompanyId;
            paymentVoucherModel.FinancialYearId = userSession.FinancialYearId;
            paymentVoucherModel.NoOfLineItems = 0;

            // generate no.
            GenerateNoModel generateNoModel = await _paymentVoucher.GeneratePaymentVoucherNo(userSession.CompanyId, userSession.FinancialYearId);
            paymentVoucherModel.VoucherNo = generateNoModel.VoucherNo;
            paymentVoucherModel.VoucherDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", paymentVoucherModel);
            });
        }

        public async Task<IActionResult> EditVoucherMaster(int paymentVoucherId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TypeCorBList = EnumHelper.GetEnumListFor<TypeCorB>();
            ViewBag.PaymentTypeList = EnumHelper.GetEnumListFor<PaymentType>();

            PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

            DataTableResultModel<PaymentVoucherDetailModel> resultModel = await _paymentVoucherDetail.GetPaymentVoucherDetailByPaymentVoucherId(paymentVoucherId, 0);

            paymentVoucherModel.NoOfLineItems = resultModel.TotalResultCount;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", paymentVoucherModel);
            });
        }

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
                        data.Result.Data = paymentVoucherModel.PaymentVoucherId;
                    }
                }
                else
                {
                    // add new record.
                    paymentVoucherModel.PaymentVoucherId = await _paymentVoucher.CreatePaymentVoucher(paymentVoucherModel);

                    if (paymentVoucherModel.PaymentVoucherId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = paymentVoucherModel.PaymentVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusVoucherMaster(int paymentVoucherId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (paymentVoucherId > 0)
            {
                PaymentVoucherModel paymentVoucherModel = await _paymentVoucher.GetPaymentVoucherById(paymentVoucherId);

                if (paymentVoucherModel.ChequeAmountFc == 0
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Cheque Amount FC should be getter than 0. Please update Cheque Amount FC";
                }
                else if (paymentVoucherModel.ChequeAmountFc != paymentVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Cheque Amount FC != Amount FC. Please update particular details Amount FC Or change cheque Amount FC";
                }
                else
                {
                    if (true == await _paymentVoucher.UpdateStatusPaymentVoucher(paymentVoucherId, statusId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = paymentVoucherId;
                    }
                }
            }

            return Json(data);
        }

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

        [HttpPost]
        public async Task<JsonResult> GetAccountLedgerByTypeCorB(string typeCorB, int companyId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList;

            if (typeCorB == "C")
            {
                selectList = await _ledger.GetLedgerSelectList((int)LedgerName.CashAccount, companyId, true);
            }
            else
            {
                selectList = await _ledger.GetLedgerSelectList((int)LedgerName.BankAccount, companyId, true);
            }

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
        public async Task<JsonResult> GetClosingBalanceByAccountLedgerId(int accountLedgerId, DateTime voucherDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            LedgerModel ledgerModel = await _ledger.GetClosingBalanceByAccountLedgerId(accountLedgerId, voucherDate);

            if (null != ledgerModel)
            {
                data.Result.Status = true;
                data.Result.Data = ledgerModel.ClosingBalance;
            }
            else
            {
                data.Result.Data = "0";
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

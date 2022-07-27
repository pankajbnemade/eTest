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
    public class ReceiptVoucherController : Controller
    {
        private readonly IReceiptVoucher _receiptVoucher;
        private readonly IReceiptVoucherDetail _receiptVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        public ReceiptVoucherController(
            IReceiptVoucher receiptVoucher,
            IReceiptVoucherDetail receiptVoucherDetail,
            ILedger ledger,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._receiptVoucher = receiptVoucher;
            this._receiptVoucherDetail = receiptVoucherDetail;
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
        public async Task<IActionResult> GetReceiptVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterReceiptVoucherModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterReceiptVoucherModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<ReceiptVoucherModel> resultModel = await _receiptVoucher.GetReceiptVoucherList(dataTableAjaxPostModel, searchFilterModel);

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

        public async Task<IActionResult> ManageVoucher(int receiptVoucherId)
        {
            ViewBag.ReceiptVoucherId = receiptVoucherId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> MasterButtons(int receiptVoucherId)
        {
            ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

            ReceiptVoucherMasterButtonsModel receiptVoucherMasterButtonsModel = null;

            receiptVoucherMasterButtonsModel = new ReceiptVoucherMasterButtonsModel()
            {
                ReceiptVoucherId = receiptVoucherId,
                IsApprovalRequestVisible = receiptVoucherModel.StatusId == (int)DocumentStatus.Inprocess || receiptVoucherModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false,
                IsApproveVisible = receiptVoucherModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false,
                IsCancelVisible = receiptVoucherModel.StatusId != (int)DocumentStatus.Cancelled ? true : false,
                IsPDCProcessedVisible = receiptVoucherModel.StatusId == (int)DocumentStatus.Approved
                                            && receiptVoucherModel.TypeCorB == TypeCorB.B.ToString()
                                            && receiptVoucherModel.PaymentTypeId == (int)PaymentType.PDC
                                            && receiptVoucherModel.IsPDCProcessed == false
                                        ? true : false,
            };

            return await Task.Run(() =>
            {
                return PartialView("_MasterButtons", receiptVoucherMasterButtonsModel);
            });
        }

        public async Task<IActionResult> ViewVoucherMaster(int receiptVoucherId)
        {
            ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

            LedgerModel ledgerModel = await _ledger.GetClosingBalanceByAccountLedgerId((int)receiptVoucherModel.AccountLedgerId, (DateTime)receiptVoucherModel.VoucherDate);

            if (null != ledgerModel)
            {
                receiptVoucherModel.ClosingBalance = ledgerModel.ClosingBalance;
            }
            else
            {
                receiptVoucherModel.ClosingBalance = 0;
            }

            return await Task.Run(() =>
            {
                return PartialView("_ViewVoucherMaster", receiptVoucherModel);
            });
        }

        public async Task<IActionResult> AddVoucherMaster()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TypeCorBList = EnumHelper.GetEnumListFor<TypeCorB>();
            ViewBag.PaymentTypeList = EnumHelper.GetEnumListFor<PaymentType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ReceiptVoucherModel receiptVoucherModel = new ReceiptVoucherModel();
            receiptVoucherModel.CompanyId = userSession.CompanyId;
            receiptVoucherModel.FinancialYearId = userSession.FinancialYearId;
            receiptVoucherModel.NoOfLineItems = 0;

            // generate no.
            GenerateNoModel generateNoModel = await _receiptVoucher.GenerateReceiptVoucherNo(userSession.CompanyId, userSession.FinancialYearId);
            receiptVoucherModel.VoucherNo = generateNoModel.VoucherNo;
            receiptVoucherModel.VoucherDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", receiptVoucherModel);
            });
        }

        public async Task<IActionResult> EditVoucherMaster(int receiptVoucherId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TypeCorBList = EnumHelper.GetEnumListFor<TypeCorB>();
            ViewBag.PaymentTypeList = EnumHelper.GetEnumListFor<PaymentType>();

            ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

            DataTableResultModel<ReceiptVoucherDetailModel> resultModel = await _receiptVoucherDetail.GetReceiptVoucherDetailByReceiptVoucherId(receiptVoucherId, 0);

            receiptVoucherModel.NoOfLineItems = resultModel.TotalResultCount;

            return await Task.Run(() =>
            {
                return PartialView("_AddVoucherMaster", receiptVoucherModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveVoucherMaster(ReceiptVoucherModel receiptVoucherModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (receiptVoucherModel.ReceiptVoucherId > 0)
                {
                    // update record.
                    if (true == await _receiptVoucher.UpdateReceiptVoucher(receiptVoucherModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = receiptVoucherModel.ReceiptVoucherId;
                    }
                }
                else
                {
                    // add new record.
                    receiptVoucherModel.ReceiptVoucherId = await _receiptVoucher.CreateReceiptVoucher(receiptVoucherModel);

                    if (receiptVoucherModel.ReceiptVoucherId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = receiptVoucherModel.ReceiptVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusVoucherMaster(int receiptVoucherId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (receiptVoucherId > 0)
            {
                ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

                if (receiptVoucherModel.ChequeAmountFc == 0
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Cheque Amount FC should be getter than 0. Please update Cheque Amount FC";
                }
                else if (receiptVoucherModel.ChequeAmountFc != receiptVoucherModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Cheque Amount FC != Amount FC. Please update particular details Amount FC Or change cheque Amount FC";
                }
                else
                {
                    if (true == await _receiptVoucher.UpdateStatusReceiptVoucher(receiptVoucherId, statusId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = receiptVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePDCProcessed(int receiptVoucherId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (receiptVoucherId > 0)
            {
                ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

                if (receiptVoucherModel.StatusId != (int)DocumentStatus.Approved)
                {
                    data.Result.Data = "Voucher Status should be approved to mark PDC processed";
                }
                else
                {
                    if (true == await _receiptVoucher.UpdatePDCProcessed(receiptVoucherId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = receiptVoucherId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherMaster(int receiptVoucherId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _receiptVoucher.DeleteReceiptVoucher(receiptVoucherId))
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
                selectList = await _ledger.GetLedgerSelectList((int)LedgerName.Cash, companyId, true);
            }
            else
            {
                selectList = await _ledger.GetLedgerSelectList((int)LedgerName.Bank, companyId, true);
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

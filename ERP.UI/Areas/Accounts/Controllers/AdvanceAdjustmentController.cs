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
    public class AdvanceAdjustmentController : Controller
    {
        private readonly IAdvanceAdjustment _advanceAdjustment;
        private readonly IAdvanceAdjustmentDetail _advanceAdjustmentDetail;
        private readonly ILedger _ledger;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        private readonly IPaymentVoucherDetail _paymentVoucherDetail;
        private readonly IReceiptVoucherDetail _receiptVoucherDetail;

        public AdvanceAdjustmentController(
            IAdvanceAdjustment advanceAdjustment,
            IAdvanceAdjustmentDetail advanceAdjustmentDetail,
            ILedger ledger,
            ICurrency currency,
            ICurrencyConversion currencyConversion,
            IPaymentVoucherDetail paymentVoucherDetail,
            IReceiptVoucherDetail receiptVoucherDetail)
        {
            this._advanceAdjustment = advanceAdjustment;
            this._advanceAdjustmentDetail = advanceAdjustmentDetail;
            this._ledger = ledger;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
            this._paymentVoucherDetail = paymentVoucherDetail;
            this._receiptVoucherDetail = receiptVoucherDetail;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, true);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetAdvanceAdjustmentList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterAdvanceAdjustmentModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterAdvanceAdjustmentModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<AdvanceAdjustmentModel> resultModel = await _advanceAdjustment.GetAdvanceAdjustmentList(dataTableAjaxPostModel, searchFilterModel);

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

        public async Task<IActionResult> ManageAdvanceAdjustment(int advanceAdjustmentId)
        {
            ViewBag.AdvanceAdjustmentId = advanceAdjustmentId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> MasterButtons(int advanceAdjustmentId)
        {
            AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);

            AdvanceAdjustmentMasterButtonsModel advanceAdjustmentMasterButtonsModel = null;

            advanceAdjustmentMasterButtonsModel = new AdvanceAdjustmentMasterButtonsModel()
            {
                AdvanceAdjustmentId = advanceAdjustmentId,
                IsApprovalRequestVisible = advanceAdjustmentModel.StatusId == (int)DocumentStatus.Inprocess || advanceAdjustmentModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false,
                IsApproveVisible = advanceAdjustmentModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false,
                IsCancelVisible = advanceAdjustmentModel.StatusId != (int)DocumentStatus.Cancelled ? true : false,
            };

            return await Task.Run(() =>
            {
                return PartialView("_MasterButtons", advanceAdjustmentMasterButtonsModel);
            });
        }

        public async Task<IActionResult> ViewAdvanceAdjustmentMaster(int advanceAdjustmentId)
        {
            AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);

            LedgerModel ledgerModel = await _ledger.GetLedgerById(advanceAdjustmentModel.ParticularLedgerId);

            if (ledgerModel.ParentGroupId == (int)LedgerName.SundryDebtor)
            {
                advanceAdjustmentModel.VoucherAvailableAmountFc = await _receiptVoucherDetail.GetVoucherAvailableAmount((Int32)advanceAdjustmentModel.ReceiptVoucherDetId);
            }
            else if (ledgerModel.ParentGroupId == (int)LedgerName.SundryCreditor)
            {
                advanceAdjustmentModel.VoucherAvailableAmountFc = await _paymentVoucherDetail.GetVoucherAvailableAmount((Int32)advanceAdjustmentModel.PaymentVoucherDetId);
            }

            advanceAdjustmentModel.VoucherAvailableAmountFc = advanceAdjustmentModel.VoucherAvailableAmountFc + advanceAdjustmentModel.AmountFc;

            return await Task.Run(() =>
            {
                return PartialView("_ViewAdvanceAdjustmentMaster", advanceAdjustmentModel);
            });
        }

        public async Task<IActionResult> AddAdvanceAdjustmentMaster()
        {
            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, true);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            AdvanceAdjustmentModel advanceAdjustmentModel = new AdvanceAdjustmentModel();
            advanceAdjustmentModel.CompanyId = userSession.CompanyId;
            advanceAdjustmentModel.FinancialYearId = userSession.FinancialYearId;
            advanceAdjustmentModel.NoOfLineItems = 0;

            // generate no.
            GenerateNoModel generateNoModel = await _advanceAdjustment.GenerateAdvanceAdjustmentNo(userSession.CompanyId, userSession.FinancialYearId);
            advanceAdjustmentModel.AdvanceAdjustmentNo = generateNoModel.VoucherNo;
            advanceAdjustmentModel.AdvanceAdjustmentDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddAdvanceAdjustmentMaster", advanceAdjustmentModel);
            });
        }

        public async Task<IActionResult> EditAdvanceAdjustmentMaster(int advanceAdjustmentId)
        {
            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, true);

            AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);

            DataTableResultModel<AdvanceAdjustmentDetailModel> resultModel = await _advanceAdjustmentDetail.GetAdvanceAdjustmentDetailByAdvanceAdjustmentId(advanceAdjustmentId, 0);

            advanceAdjustmentModel.NoOfLineItems = resultModel.TotalResultCount;

            return await Task.Run(() =>
            {
                return PartialView("_AddAdvanceAdjustmentMaster", advanceAdjustmentModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveAdvanceAdjustmentMaster(AdvanceAdjustmentModel advanceAdjustmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            LedgerModel ledgerModel = await _ledger.GetLedgerById(advanceAdjustmentModel.ParticularLedgerId);

            AdvanceAdjustmentVoucherDetailModel advanceAdjustmentVoucherDetailModel = null;

            if (ledgerModel != null)
            {
                if (ledgerModel.ParentGroupId == (int)LedgerName.SundryDebtor)
                {
                    advanceAdjustmentModel.ReceiptVoucherDetId = advanceAdjustmentModel.VoucherDetId;
                    advanceAdjustmentModel.PaymentVoucherDetId = null;
                    advanceAdjustmentVoucherDetailModel = await _receiptVoucherDetail.GetVoucherDetail(advanceAdjustmentModel.VoucherDetId);
                }
                else if (ledgerModel.ParentGroupId == (int)LedgerName.SundryCreditor)
                {
                    advanceAdjustmentModel.ReceiptVoucherDetId = null;
                    advanceAdjustmentModel.PaymentVoucherDetId = advanceAdjustmentModel.VoucherDetId;
                    advanceAdjustmentVoucherDetailModel = await _paymentVoucherDetail.GetVoucherDetail(advanceAdjustmentModel.VoucherDetId);
                }
            }

            advanceAdjustmentModel.CurrencyId = advanceAdjustmentVoucherDetailModel.CurrencyId;
            advanceAdjustmentModel.ExchangeRate = advanceAdjustmentVoucherDetailModel.ExchangeRate;

            if (ModelState.IsValid)
            {
                if (advanceAdjustmentModel.AdvanceAdjustmentId > 0)
                {
                    // update record.
                    if (true == await _advanceAdjustment.UpdateAdvanceAdjustment(advanceAdjustmentModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = advanceAdjustmentModel.AdvanceAdjustmentId;
                    }
                }
                else
                {
                    // add new record.
                    advanceAdjustmentModel.AdvanceAdjustmentId = await _advanceAdjustment.CreateAdvanceAdjustment(advanceAdjustmentModel);

                    if (advanceAdjustmentModel.AdvanceAdjustmentId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = advanceAdjustmentModel.AdvanceAdjustmentId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusAdvanceAdjustmentMaster(int advanceAdjustmentId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (advanceAdjustmentId > 0)
            {
                AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);

                LedgerModel ledgerModel = await _ledger.GetLedgerById(advanceAdjustmentModel.ParticularLedgerId);

                if (ledgerModel.ParentGroupId == (int)LedgerName.SundryDebtor)
                {
                    advanceAdjustmentModel.VoucherAvailableAmountFc = await _receiptVoucherDetail.GetVoucherAvailableAmount((Int32)advanceAdjustmentModel.ReceiptVoucherDetId);
                }
                else if (ledgerModel.ParentGroupId == (int)LedgerName.SundryCreditor)
                {
                    advanceAdjustmentModel.VoucherAvailableAmountFc = await _paymentVoucherDetail.GetVoucherAvailableAmount((Int32)advanceAdjustmentModel.PaymentVoucherDetId);
                }

                advanceAdjustmentModel.VoucherAvailableAmountFc = advanceAdjustmentModel.VoucherAvailableAmountFc + advanceAdjustmentModel.AmountFc;

                if (advanceAdjustmentModel.AmountFc == 0
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Amount FC should be getter than 0. Please update Amount FC";
                }
                else if (advanceAdjustmentModel.VoucherAvailableAmountFc < advanceAdjustmentModel.AmountFc
                    && (statusId == (int)DocumentStatus.Approved || statusId == (int)DocumentStatus.ApprovalRequested)
                    )
                {
                    data.Result.Data = "Voucher Available Amount FC < Adjusted Amount FC. Please update adjusted Amount FC ";
                }
                else
                {
                    if (true == await _advanceAdjustment.UpdateStatusAdvanceAdjustment(advanceAdjustmentId, statusId))
                    {
                        data.Result.Status = true;
                        data.Result.Data = advanceAdjustmentId;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAdvanceAdjustmentMaster(int advanceAdjustmentId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _advanceAdjustment.DeleteAdvanceAdjustment(advanceAdjustmentId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

        [HttpPost]
        public async Task<JsonResult> GetVoucherListByParticularId(int advanceAdjustmentId, int particularLedgerId, DateTime advanceAdjustmentDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList = null;

            LedgerModel ledgerModel = await _ledger.GetLedgerById(particularLedgerId);

            int voucherDetId = 0;
            decimal amountFc = 0;

            if (advanceAdjustmentId > 0)
            {
                AdvanceAdjustmentModel advanceAdjustmentModel = await _advanceAdjustment.GetAdvanceAdjustmentById(advanceAdjustmentId);
                voucherDetId = advanceAdjustmentModel.VoucherDetId;
                amountFc = advanceAdjustmentModel.AmountFc;
            }

            if (ledgerModel.ParentGroupId == (int)LedgerName.SundryDebtor)
            {
                selectList = await _receiptVoucherDetail.GetVocuherSelectList(particularLedgerId, advanceAdjustmentDate, voucherDetId, amountFc);
            }
            else if (ledgerModel.ParentGroupId == (int)LedgerName.SundryCreditor)
            {
                selectList = await _paymentVoucherDetail.GetVocuherSelectList(particularLedgerId, advanceAdjustmentDate, voucherDetId, amountFc);
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

    }
}

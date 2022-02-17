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
    public class LedgerController : Controller
    {
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ILedgerFinancialYearBalance _ledgerFinancialYearBalance;

        public LedgerController(
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ILedgerFinancialYearBalance ledgerFinancialYearBalance
            )
        {
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._ledgerFinancialYearBalance = ledgerFinancialYearBalance;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ParentGroupList = await _ledger.GetGroupSelectList(0);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetLedgerList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterLedgerModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterLedgerModel>(searchFilter);
            // get data.
            DataTableResultModel<LedgerModel> resultModel = await _ledger.GetLedgerList(dataTableAjaxPostModel, searchFilterModel);

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

        public async Task<IActionResult> AddLedgerMaster()
        {
            ViewBag.ParentGroupList = await _ledger.GetGroupSelectList(0);

            LedgerModel ledgerModel = new LedgerModel();

            // generate no.
            GenerateNoModel generateNoModel = await _ledger.GenerateLedgerCode();

            ledgerModel.LedgerCode = generateNoModel.VoucherNo;

            return await Task.Run(() =>
            {
                return PartialView("_AddLedgerMaster", ledgerModel);
            });
        }

        public async Task<IActionResult> EditLedgerMaster(int ledgerId)
        {
            ViewBag.ParentGroupList = await _ledger.GetGroupSelectList(0);

            LedgerModel ledgerModel = await _ledger.GetLedgerById(ledgerId);

            return await Task.Run(() =>
            {
                return PartialView("_AddLedgerMaster", ledgerModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveLedgerMaster(LedgerModel ledgerModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                //To check number of levels

                if (ledgerModel.IsGroup==true)
                {
                    LedgerModel ledgerModel_Parent = await _ledger.GetLedgerById((int)ledgerModel.ParentGroupId);

                    if (ledgerModel_Parent.IsMasterGroup==false)
                    {
                        data.Result.Status = false;
                        data.Result.Data = ledgerModel.LedgerId;
                        data.Result.Message = "You can not add/update this group. Selected group's parent group is not master group.";

                        return Json(data);
                    }
                }


                if (ledgerModel.LedgerId > 0)
                {
                    if (ledgerModel.IsGroup==false)
                    {
                        DataTableResultModel<LedgerModel> resultModel = await _ledger.GetLedgerListByParentGroupId(ledgerModel.LedgerId);

                        if (resultModel.TotalResultCount!=0)
                        {
                            data.Result.Status = false;
                            data.Result.Data = ledgerModel.LedgerId;
                            data.Result.Message = "You can not remove 'Is Group'. Group is referred as parent group in other Group/Ledger";

                            return Json(data);
                        }

                    }

                    // update record.
                    if (true == await _ledger.UpdateLedger(ledgerModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = ledgerModel.LedgerId;
                    }
                }
                else
                {
                    // generate no.
                    GenerateNoModel generateNoModel = await _ledger.GenerateLedgerCode();

                    ledgerModel.LedgerCode = generateNoModel.VoucherNo;
                    ledgerModel.MaxNo = generateNoModel.MaxNo;

                    ledgerModel.LedgerId = await _ledger.CreateLedger(ledgerModel);
                    // add new record.
                    if (ledgerModel.LedgerId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = ledgerModel.LedgerId;
                    }
                }
            }

            return Json(data);
        }

        public async Task<IActionResult> ManageLedger(int ledgerId)
        {
            ViewBag.LedgerId = ledgerId;

            LedgerModel ledgerModel = await _ledger.GetLedgerById(ledgerId);

            ViewBag.IsAddressVisible = false;
            ViewBag.IsEditVisible =false;
            ViewBag.IsUpdateBalanceVisible =false;

            if (ledgerModel.ParentGroupId == (int)LedgerName.SundryDebtor || ledgerModel.ParentGroupId == (int)LedgerName.SundryCreditor)
            {
                ViewBag.IsAddressVisible = true;
            }

            if (ledgerModel.IsMasterGroup == false)
            {
                ViewBag.IsEditVisible = true;
            }

            if (ledgerModel.IsGroup == false)
            {
                ViewBag.IsUpdateBalanceVisible = true;
            }

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> ViewLedgerMaster(int ledgerId)
        {
            LedgerModel ledgerModel = await _ledger.GetLedgerById(ledgerId);

            if (ledgerModel==null)
            {
                return NotFound("Record not exists");
            }

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel = await _ledgerFinancialYearBalance.GetLedgerFinancialYearBalance(ledgerId, userSession.CompanyId, userSession.FinancialYearId);

            if (ledgerFinancialYearBalanceModel != null)
            {
                ledgerModel.CreditAmountOpBal=ledgerFinancialYearBalanceModel.CreditAmount;
                ledgerModel.DebitAmountOpBal=ledgerFinancialYearBalanceModel.DebitAmount;
            }


            return await Task.Run(() =>
            {
                return PartialView("_ViewLedgerMaster", ledgerModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLedgerMaster(int ledgerId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _ledger.DeleteLedger(ledgerId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

        public async Task<IActionResult> EditLedgerBalance(int ledgerId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel = await _ledgerFinancialYearBalance.GetLedgerFinancialYearBalance(ledgerId, userSession.CompanyId, userSession.FinancialYearId);

            if (ledgerFinancialYearBalanceModel == null)
            {
                ledgerFinancialYearBalanceModel=new LedgerFinancialYearBalanceModel();

                ledgerFinancialYearBalanceModel.CompanyId=userSession.CompanyId;
                ledgerFinancialYearBalanceModel.FinancialYearId=userSession.FinancialYearId;
            }

            return await Task.Run(() =>
            {
                return PartialView("_UpdateLedgerBalance", ledgerFinancialYearBalanceModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveUpdateLedgerBalance(LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (ledgerFinancialYearBalanceModel.LedgerBalanceId > 0)
                {
                    // update record.
                    if (true == await _ledgerFinancialYearBalance.UpdateLedgerFinancialYearBalance(ledgerFinancialYearBalanceModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = ledgerFinancialYearBalanceModel.LedgerId;
                    }
                }
                else
                {
                    ledgerFinancialYearBalanceModel.LedgerBalanceId = await _ledgerFinancialYearBalance.CreateLedgerFinancialYearBalance(ledgerFinancialYearBalanceModel);

                    // add new record.
                    if (ledgerFinancialYearBalanceModel.LedgerBalanceId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = ledgerFinancialYearBalanceModel.LedgerId;
                    }
                }
            }

            return Json(data);
        }


    }
}

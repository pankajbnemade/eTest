﻿using ERP.Models.Accounts;
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
    public class LedgerController : Controller
    {
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;

        public LedgerController(
            ILedger ledger,
            ILedgerAddress ledgerAddress
            )
        {
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
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
                if (ledgerModel.LedgerId > 0)
                {
                    LedgerModel ledgerModel_Old = await _ledger.GetLedgerById(ledgerModel.LedgerId);

                    // update record.
                    if (true == await _ledger.UpdateLedger(ledgerModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = ledgerModel.LedgerId;
                    }
                }
                else
                {
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

            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> ViewLedgerMaster(int ledgerId)
        {
            LedgerModel ledgerModel = await _ledger.GetLedgerById(ledgerId);

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
    }
}

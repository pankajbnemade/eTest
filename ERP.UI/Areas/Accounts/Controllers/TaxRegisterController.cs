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
    public class TaxRegisterController : Controller
    {
        private readonly ITaxRegister _taxRegister;

        /// <summary>
        /// constractor.
        /// </summary>
        public TaxRegisterController(ITaxRegister taxRegister)
        {
            this._taxRegister = taxRegister;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search sales TaxRegister result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetTaxRegisterList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterTaxRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterTaxRegisterModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<TaxRegisterModel> resultModel = await _taxRegister.GetTaxRegisterList(dataTableAjaxPostModel, searchFilterModel);

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


        public async Task<IActionResult> AddTaxRegisterMaster()
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            TaxRegisterModel taxRegisterModel = new TaxRegisterModel();
            //taxRegisterModel.CompanyId = userSession.CompanyId;
            //taxRegisterModel.FinancialYearId = userSession.FinancialYearId;

            // generate no.
            //GenerateNoModel generateNoModel = await _taxRegister.GenerateTaxRegisterNo(userSession.CompanyId, userSession.FinancialYearId);
            //taxRegisterModel.TaxRegisterNo = generateNoModel.VoucherNo;
            //taxRegisterModel.TaxRegisterDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddTaxRegisterMaster", taxRegisterModel);
            });
        }


        public async Task<IActionResult> EditTaxRegisterMaster(int taxRegisterId)
        {
            TaxRegisterModel taxRegisterModel = await _taxRegister.GetTaxRegisterById(taxRegisterId);

            return await Task.Run(() =>
            {
                return PartialView("_AddTaxRegisterMaster", taxRegisterModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveTaxRegisterMaster(TaxRegisterModel taxRegisterModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (taxRegisterModel.TaxRegisterId > 0)
                {
                    TaxRegisterModel taxRegisterModel_Old = await _taxRegister.GetTaxRegisterById(taxRegisterModel.TaxRegisterId);

                    // update record.
                    if (true == await _taxRegister.UpdateTaxRegister(taxRegisterModel))
                    {
                        data.Result.Status = true;
                        data.Result.Data = taxRegisterModel.TaxRegisterId;
                    }
                }
                else
                {
                    taxRegisterModel.TaxRegisterId = await _taxRegister.CreateTaxRegister(taxRegisterModel);
                    // add new record.
                    if (taxRegisterModel.TaxRegisterId > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = taxRegisterModel.TaxRegisterId;
                    }
                }
            }

            return Json(data);
        }

        public async Task<IActionResult> ManageTaxRegister(int taxRegisterId)
        {
            ViewBag.TaxRegisterId = taxRegisterId;

            //TaxRegisterModel taxRegisterModel = await _taxRegister.GetTaxRegisterById(taxRegisterId);

            //ViewBag.IsTaxMasterVisible = taxRegisterModel.TaxModelType == TaxModelType.SubTotal.ToString() ? true : false;


            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view TaxRegister master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewTaxRegisterMaster(int taxRegisterId)
        {
            TaxRegisterModel taxRegisterModel = await _taxRegister.GetTaxRegisterById(taxRegisterId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewTaxRegisterMaster", taxRegisterModel);
            });
        }

        //[HttpPost]
        //public async Task<JsonResult> UpdateStatusTaxRegisterMaster(int taxRegisterId, string action)
        //{
        //    JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

        //    int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

        //    if (taxRegisterId > 0)
        //    {
        //        if (true == await _taxRegister.UpdateStatusTaxRegister(taxRegisterId, statusId))
        //        {
        //            data.Result.Status = true;
        //            data.Result.Data = taxRegisterId;
        //        }
        //    }

        //    return Json(data);
        //}

        
        [HttpPost]
        public async Task<JsonResult> DeleteTaxRegisterMaster(int taxRegisterId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _taxRegister.DeleteTaxRegister(taxRegisterId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

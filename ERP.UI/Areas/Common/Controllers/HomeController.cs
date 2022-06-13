using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using ERP.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Common.Controllers
{
    [Authorize]
    [Area("Common")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICompany _company;
        private readonly IFinancialYear _financialYear;
        private readonly IFinancialYearCompanyRelation _financialYearCompanyRelation;

        public HomeController(ILogger<HomeController> logger, ICompany company, IFinancialYear financialYear, IFinancialYearCompanyRelation financialYearCompanyRelation)
        {
            _logger = logger;
            _company = company;
            _financialYear = financialYear;
            _financialYearCompanyRelation = financialYearCompanyRelation;
        }

        public IActionResult Index()
        {
            UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            if (userSessionModel.CompanyId==0)
            {
                return RedirectToAction("ChangeCompany", "Home", new { area = "" });
            }

            if (userSessionModel.FinancialYearId==0)
            {
                return RedirectToAction("ChangeYear", "Home", new { area = "" });
            }

            return View();
        }

        public IActionResult FAQ()
        {
            //UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ChangeCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetCompanyList()
        {
            DataTableResultModel<CompanyModel> resultModel = await _company.GetCompanyList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// choose company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChooseCompany(int companyId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            CompanyModel companyModel = await _company.GetCompanyById(companyId);

            UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            if (userSessionModel.FinancialYearId==0)
            {
                userSessionModel.CompanyId = companyModel.CompanyId;
                userSessionModel.CompanyName = companyModel.CompanyName;

                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                //return RedirectToAction("ChangeYear", "Home", new { area = "" });
                data.Result.Data = 1;
                data.Result.Status = true;

                return Json(data);
            }

            FinancialYearCompanyRelationModel financialYearCompanyRelationModel = await _financialYearCompanyRelation.GetFinancialYearCompanyRelation(userSessionModel.FinancialYearId, companyId);

            if (financialYearCompanyRelationModel.RelationId > 0)
            {
                userSessionModel.CompanyId = companyModel.CompanyId;
                userSessionModel.CompanyName = companyModel.CompanyName;

                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                data.Result.Status=true;
                data.Result.Message="Company Changed Successfully";
                data.Result.Data = 2;
            }
            else
            {
                data.Result.Status = false;
                data.Result.Message = $"{companyModel.CompanyName}{" & "}{userSessionModel.FinancialYearName}{" dont have any relation."}";
            }

            return Json(data);
        }

        public IActionResult ChangeYear()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetFinancialYearList()
        {
            DataTableResultModel<FinancialYearModel> resultModel = await _financialYear.GetFinancialYearList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// choose financial year.
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChooseFinancialYear(int financialYearId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(financialYearId);

            UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            if (userSessionModel.CompanyId==0)
            {
                userSessionModel.FinancialYearId = financialYearModel.FinancialYearId;
                userSessionModel.FinancialYearName = financialYearModel.FinancialYearName;

                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                //return RedirectToAction("ChangeCompany", "Home", new { area = "" });

                data.Result.Data = 1;
                data.Result.Status = true;
                //data.Result.Message = "Financial Year Changed Successfully";

                return Json(data);
            }

            FinancialYearCompanyRelationModel financialYearCompanyRelationModel = await _financialYearCompanyRelation.GetFinancialYearCompanyRelation(financialYearId, userSessionModel.CompanyId);

            if (financialYearCompanyRelationModel.RelationId > 0)
            {
                userSessionModel.FinancialYearId = financialYearModel.FinancialYearId;
                userSessionModel.FinancialYearName = financialYearModel.FinancialYearName;

                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                data.Result.Data = 2;
                data.Result.Status=true;
                data.Result.Message="Financial Year Changed Successfully";
            }
            else
            {
                data.Result.Status = false;
                data.Result.Message = $"{userSessionModel.CompanyName}{" & "}{financialYearModel.FinancialYearName}{" dont have any relation."}";
            }

            return Json(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using ERP.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Common.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICompany _company;
        private readonly IFinancialYear _financialYear;

        public HomeController(ILogger<HomeController> logger, ICompany company, IFinancialYear financialYear)
        {
            _logger = logger;
            _company = company;
            _financialYear = financialYear;
        }

        public IActionResult Index()
        {

            UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
           
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
        public async Task<JsonResult> ChooseCommpany(int companyId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            return await Task.Run(() =>
            {
                UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
                userSessionModel.CompanyId = companyId;
                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                return Json(data);
            });
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
        public async Task<JsonResult> ChooseFinancialYear(int financialYearId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            return await Task.Run(() =>
            {
                UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
                userSessionModel.FinancialYearId = financialYearId;
                SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                return Json(data);
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

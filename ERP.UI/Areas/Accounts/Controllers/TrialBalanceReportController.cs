using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class TrialBalanceReportController : Controller
    {

        private readonly ITrialBalanceReport _trialBalanceReport;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public TrialBalanceReportController(ILedger ledger, IFinancialYear financialYear, ITrialBalanceReport trialBalanceReport)
        {
            this._trialBalanceReport = trialBalanceReport;
            this._ledger = ledger;
            this._financialYear = financialYear;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        public async Task<IActionResult> Search(string searchFilter)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.LedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, userSession.CompanyId, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterTrialBalanceReportModel searchFilterTrialBalanceReportModel = new SearchFilterTrialBalanceReportModel();

            if (searchFilter==null)
            {
                searchFilterTrialBalanceReportModel.FromDate = financialYearModel.FromDate;
                searchFilterTrialBalanceReportModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterTrialBalanceReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterTrialBalanceReportModel>(searchFilter);

                searchFilterTrialBalanceReportModel.ReportType = searchFilterModel.ReportType;
                searchFilterTrialBalanceReportModel.FromDate = searchFilterModel.FromDate;
                searchFilterTrialBalanceReportModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterTrialBalanceReportModel);
            });
        }

        public async Task<IActionResult> Detail()
        {
            return await Task.Run(() =>
            {
                return PartialView("_Detail");
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetTransactionList(string searchFilter)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            // deserilize string search filter.
            SearchFilterTrialBalanceReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterTrialBalanceReportModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<TrialBalanceReportModel> resultModel = await _trialBalanceReport.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    draw = "1",
                    recordsTotal = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }



    }
}

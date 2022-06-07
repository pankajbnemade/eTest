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
    public class ProfitAndLossReportController : Controller
    {

        private readonly IProfitAndLossReport _profitAndLossReport;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public ProfitAndLossReportController(ILedger ledger, IFinancialYear financialYear, IProfitAndLossReport profitAndLossReport)
        {
            this._profitAndLossReport = profitAndLossReport;
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
            ViewBag.LedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterProfitAndLossReportModel searchFilterProfitAndLossReportModel = new SearchFilterProfitAndLossReportModel();

            if (searchFilter==null)
            {
                searchFilterProfitAndLossReportModel.FromDate = financialYearModel.FromDate;
                searchFilterProfitAndLossReportModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterProfitAndLossReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterProfitAndLossReportModel>(searchFilter);

                searchFilterProfitAndLossReportModel.ReportType = searchFilterModel.ReportType;
                searchFilterProfitAndLossReportModel.FromDate = searchFilterModel.FromDate;
                searchFilterProfitAndLossReportModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterProfitAndLossReportModel);
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
            SearchFilterProfitAndLossReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterProfitAndLossReportModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<ProfitAndLossReportModel> resultModel = await _profitAndLossReport.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

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

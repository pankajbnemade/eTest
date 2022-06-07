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
    public class BalanceSheetReportController : Controller
    {

        private readonly IBalanceSheetReport _balanceSheetReport;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public BalanceSheetReportController(ILedger ledger, IFinancialYear financialYear, IBalanceSheetReport balanceSheetReport)
        {
            this._balanceSheetReport = balanceSheetReport;
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

            SearchFilterBalanceSheetReportModel searchFilterBalanceSheetReportModel = new SearchFilterBalanceSheetReportModel();

            if (searchFilter==null)
            {
                searchFilterBalanceSheetReportModel.FromDate = financialYearModel.FromDate;
                searchFilterBalanceSheetReportModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterBalanceSheetReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterBalanceSheetReportModel>(searchFilter);

                searchFilterBalanceSheetReportModel.ReportType = searchFilterModel.ReportType;
                searchFilterBalanceSheetReportModel.FromDate = searchFilterModel.FromDate;
                searchFilterBalanceSheetReportModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterBalanceSheetReportModel);
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
            SearchFilterBalanceSheetReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterBalanceSheetReportModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<BalanceSheetReportModel> resultModel = await _balanceSheetReport.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

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

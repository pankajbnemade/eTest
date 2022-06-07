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
    public class PDCReportController : Controller
    {
       
        private readonly IPDCReport _pdcReport;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public PDCReportController(ILedger ledger, IFinancialYear financialYear, IPDCReport pdcReport)
        {
            this._pdcReport = pdcReport;
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

            SearchFilterPDCReportModel searchFilterPDCReportModel = new SearchFilterPDCReportModel();

            if (searchFilter==null)
            {
                searchFilterPDCReportModel.FromDate = financialYearModel.FromDate;
                searchFilterPDCReportModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterPDCReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPDCReportModel>(searchFilter);

                searchFilterPDCReportModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterPDCReportModel.PDCType = searchFilterModel.PDCType;
                searchFilterPDCReportModel.FromDate = searchFilterModel.FromDate;
                searchFilterPDCReportModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterPDCReportModel);
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
            SearchFilterPDCReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPDCReportModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<PDCReportModel> resultModel = await _pdcReport.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

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

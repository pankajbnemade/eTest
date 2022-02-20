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
    public class TaxReportController : Controller
    {
        private readonly ITaxReport _taxReport;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public TaxReportController(ILedger ledger, IFinancialYear financialYear, ITaxReport taxReport)
        {
            this._taxReport = taxReport;
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
            ViewBag.LedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterTaxReportModel searchFilterTaxReportModel = new SearchFilterTaxReportModel();


            if (searchFilter==null)
            {
                searchFilterTaxReportModel.FromDate = financialYearModel.FromDate;
                searchFilterTaxReportModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterTaxReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterTaxReportModel>(searchFilter);

                searchFilterTaxReportModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterTaxReportModel.FromDate = searchFilterModel.FromDate;
                searchFilterTaxReportModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterTaxReportModel);
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
            SearchFilterTaxReportModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterTaxReportModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<TaxReportModel> resultModel = await _taxReport.GetReport(searchFilterModel,
                                                                                            financialYearModel.FromDate, financialYearModel.ToDate);

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

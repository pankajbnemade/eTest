using ERP.Models.Accounts;
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
    public class GeneralLedgerController : Controller
    {
        private readonly IGeneralLedger _generalLedger;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public GeneralLedgerController(ILedger ledger, IFinancialYear financialYear, IGeneralLedger generalLedger)
        {
            this._generalLedger = generalLedger;
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

            ViewBag.LedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterGeneralLedgerModel searchFilterGeneralLedgerModel = new SearchFilterGeneralLedgerModel();


            if (searchFilter==null)
            {
                searchFilterGeneralLedgerModel.FromDate = financialYearModel.FromDate;
                searchFilterGeneralLedgerModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterGeneralLedgerModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterGeneralLedgerModel>(searchFilter);

                searchFilterGeneralLedgerModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterGeneralLedgerModel.FromDate = searchFilterModel.FromDate;
                searchFilterGeneralLedgerModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterGeneralLedgerModel);
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
            SearchFilterGeneralLedgerModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterGeneralLedgerModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<GeneralLedgerModel> resultModel = await _generalLedger.GetReport(searchFilterModel,
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

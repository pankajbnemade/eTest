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
    public class JournalRegisterController : Controller
    {
        private readonly IJournalRegister _journalRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public JournalRegisterController(ILedger ledger, IFinancialYear financialYear, IJournalRegister journalRegister)
        {
            this._journalRegister = journalRegister;
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
            //ViewBag.LedgerList = await _ledger.GetLedgerSelectList(0, true);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterJournalRegisterModel searchFilterJournalRegisterModel = new SearchFilterJournalRegisterModel();

            if (searchFilter==null)
            {
                searchFilterJournalRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterJournalRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterJournalRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterJournalRegisterModel>(searchFilter);

                //searchFilterJournalRegisterModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterJournalRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterJournalRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterJournalRegisterModel);
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
            SearchFilterJournalRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterJournalRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<JournalRegisterModel> resultModel = await _journalRegister.GetReport(searchFilterModel,
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

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
    public class ReceivableStatementController : Controller
    {
        private readonly IReceivableStatement _receivableStatement;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public ReceivableStatementController(ILedger ledger, IFinancialYear financialYear, IReceivableStatement receivableStatement)
        {
            this._receivableStatement = receivableStatement;
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

            ViewBag.LedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, userSession.CompanyId, true);

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterReceivableStatementModel searchFilterReceivableStatementModel = new SearchFilterReceivableStatementModel();

            if (searchFilter==null)
            {
                searchFilterReceivableStatementModel.FromDate = financialYearModel.FromDate;
                searchFilterReceivableStatementModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterReceivableStatementModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterReceivableStatementModel>(searchFilter);

                searchFilterReceivableStatementModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterReceivableStatementModel.FromDate = searchFilterModel.FromDate;
                searchFilterReceivableStatementModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterReceivableStatementModel);
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
            SearchFilterReceivableStatementModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterReceivableStatementModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<ReceivableStatementModel> resultModel = await _receivableStatement.GetReport(searchFilterModel,
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

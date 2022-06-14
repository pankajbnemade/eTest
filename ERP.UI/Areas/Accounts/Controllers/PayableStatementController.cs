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
    public class PayableStatementController : Controller
    {
        private readonly IPayableStatement _payableStatement;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public PayableStatementController(ILedger ledger, IFinancialYear financialYear, IPayableStatement payableStatement)
        {
            this._payableStatement = payableStatement;
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

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterPayableStatementModel searchFilterPayableStatementModel = new SearchFilterPayableStatementModel();

            if (searchFilter==null)
            {
                searchFilterPayableStatementModel.FromDate = financialYearModel.FromDate;
                searchFilterPayableStatementModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterPayableStatementModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPayableStatementModel>(searchFilter);

                searchFilterPayableStatementModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterPayableStatementModel.FromDate = searchFilterModel.FromDate;
                searchFilterPayableStatementModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterPayableStatementModel);
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
            SearchFilterPayableStatementModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPayableStatementModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<PayableStatementModel> resultModel = await _payableStatement.GetReport(searchFilterModel,
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

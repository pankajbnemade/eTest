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
    public class SalesRegisterController : Controller
    {
        private readonly ISalesRegister _salesRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public SalesRegisterController(ILedger ledger, IFinancialYear financialYear, ISalesRegister salesRegister)
        {
            this._salesRegister = salesRegister;
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
            ViewBag.CustomerList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterSalesRegisterModel searchFilterSalesRegisterModel = new SearchFilterSalesRegisterModel();

            if (searchFilter==null)
            {
                searchFilterSalesRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterSalesRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterSalesRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterSalesRegisterModel>(searchFilter);

                searchFilterSalesRegisterModel.CustomerLedgerId = searchFilterModel.CustomerLedgerId;
                searchFilterSalesRegisterModel.AccountLedgerId = searchFilterModel.AccountLedgerId;
                searchFilterSalesRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterSalesRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterSalesRegisterModel);
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
            SearchFilterSalesRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterSalesRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<SalesRegisterModel> resultModel = await _salesRegister.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

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

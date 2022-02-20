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
    public class ContraRegisterController : Controller
    {
        private readonly IContraRegister _contraRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public ContraRegisterController(ILedger ledger, IFinancialYear financialYear, IContraRegister contraRegister)
        {
            this._contraRegister = contraRegister;
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

            SearchFilterContraRegisterModel searchFilterContraRegisterModel = new SearchFilterContraRegisterModel();

            if (searchFilter==null)
            {
                searchFilterContraRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterContraRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterContraRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterContraRegisterModel>(searchFilter);

                //searchFilterContraRegisterModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterContraRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterContraRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterContraRegisterModel);
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
            SearchFilterContraRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterContraRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<ContraRegisterModel> resultModel = await _contraRegister.GetReport(searchFilterModel,
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

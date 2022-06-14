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
    public class PurchaseRegisterController : Controller
    {
        private readonly IPurchaseRegister _purchaseRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public PurchaseRegisterController(ILedger ledger, IFinancialYear financialYear, IPurchaseRegister purchaseRegister)
        {
            this._purchaseRegister = purchaseRegister;
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

            ViewBag.SupplierList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, userSession.CompanyId, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);

            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(userSession.FinancialYearId);

            SearchFilterPurchaseRegisterModel searchFilterPurchaseRegisterModel = new SearchFilterPurchaseRegisterModel();

            if (searchFilter==null)
            {
                searchFilterPurchaseRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterPurchaseRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterPurchaseRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPurchaseRegisterModel>(searchFilter);

                searchFilterPurchaseRegisterModel.SupplierLedgerId = searchFilterModel.SupplierLedgerId;
                searchFilterPurchaseRegisterModel.AccountLedgerId = searchFilterModel.AccountLedgerId;
                searchFilterPurchaseRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterPurchaseRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterPurchaseRegisterModel);
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
            SearchFilterPurchaseRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPurchaseRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<PurchaseRegisterModel> resultModel = await _purchaseRegister.GetReport(searchFilterModel, financialYearModel.FromDate, financialYearModel.ToDate);

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

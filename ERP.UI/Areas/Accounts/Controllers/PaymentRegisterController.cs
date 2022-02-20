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
    public class PaymentRegisterController : Controller
    {
        private readonly IPaymentRegister _paymentRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public PaymentRegisterController(ILedger ledger, IFinancialYear financialYear, IPaymentRegister paymentRegister)
        {
            this._paymentRegister = paymentRegister;
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

            SearchFilterPaymentRegisterModel searchFilterPaymentRegisterModel = new SearchFilterPaymentRegisterModel();

            if (searchFilter==null)
            {
                searchFilterPaymentRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterPaymentRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterPaymentRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPaymentRegisterModel>(searchFilter);

                //searchFilterPaymentRegisterModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterPaymentRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterPaymentRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterPaymentRegisterModel);
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
            SearchFilterPaymentRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterPaymentRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<PaymentRegisterModel> resultModel = await _paymentRegister.GetReport(searchFilterModel,
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

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
    public class ReceiptRegisterController : Controller
    {
        private readonly IReceiptRegister _receiptRegister;
        private readonly IFinancialYear _financialYear;
        private readonly ILedger _ledger;

        public ReceiptRegisterController(ILedger ledger, IFinancialYear financialYear, IReceiptRegister receiptRegister)
        {
            this._receiptRegister = receiptRegister;
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

            SearchFilterReceiptRegisterModel searchFilterReceiptRegisterModel = new SearchFilterReceiptRegisterModel();

            if (searchFilter==null)
            {
                searchFilterReceiptRegisterModel.FromDate = financialYearModel.FromDate;
                searchFilterReceiptRegisterModel.ToDate = financialYearModel.ToDate;
            }
            else
            {
                // deserilize string search filter.

                SearchFilterReceiptRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterReceiptRegisterModel>(searchFilter);

                //searchFilterReceiptRegisterModel.LedgerId = searchFilterModel.LedgerId;
                searchFilterReceiptRegisterModel.FromDate = searchFilterModel.FromDate;
                searchFilterReceiptRegisterModel.ToDate = searchFilterModel.ToDate;
            }


            return await Task.Run(() =>
            {
                return PartialView("_Search", searchFilterReceiptRegisterModel);
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
            SearchFilterReceiptRegisterModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterReceiptRegisterModel>(searchFilter);

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

            // get data.
            DataTableResultModel<ReceiptRegisterModel> resultModel = await _receiptRegister.GetReport(searchFilterModel,
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

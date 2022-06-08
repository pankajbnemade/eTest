using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class OpeningBalanceTransferController : Controller
    {
        private readonly IFinancialYear _financialYear;
        private readonly IOpeningBalanceTransfer _openingBalanceTransfer;

        public OpeningBalanceTransferController(IOpeningBalanceTransfer openingBalanceTransfer, IFinancialYear financialYear)
        {
            this._openingBalanceTransfer = openingBalanceTransfer;
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
            ViewBag.FromYearList = await _financialYear.GetFinancialYearList();
            ViewBag.ToYearList = await _financialYear.GetFinancialYearList();

            // deserilize string search filter.
            OpeningBalanceTransferModel openingBalanceTransferModel = JsonConvert.DeserializeObject<OpeningBalanceTransferModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            openingBalanceTransferModel.CompanyId=userSession.CompanyId;
            openingBalanceTransferModel.FinancialYearId=userSession.FinancialYearId;

            return await Task.Run(() =>
            {
                return PartialView("_Search", openingBalanceTransferModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveOpeningBalanceTransfer(OpeningBalanceTransferModel openingBalanceTransferModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (openingBalanceTransferModel.CompanyId > 0)
                {
                    // update record.
                    if (true == await _openingBalanceTransfer.UpdateOpeningBalanceTransfer(openingBalanceTransferModel))
                    {
                        data.Result.Status = true;
                        data.Result.Message = "Opening Balance Saved Successfully";
                    }
                    else
                    {
                        data.Result.Status = false;
                        data.Result.Message = "Opening Balance Not Saved";
                    }
                }
                
            }

            return Json(data);
        }


    }
}

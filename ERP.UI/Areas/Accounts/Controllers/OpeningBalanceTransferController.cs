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

        public async Task<IActionResult> OpeningBalanceTransfer()
        {
            ViewBag.FromYearList = await _financialYear.GetFinancialYearSelectList();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            OpeningBalanceTransferModel openingBalanceTransferModel = new OpeningBalanceTransferModel();

            openingBalanceTransferModel.CompanyId=userSession.CompanyId;
            openingBalanceTransferModel.FinancialYearId=userSession.FinancialYearId;

            openingBalanceTransferModel.ToYearId=userSession.FinancialYearId;
            openingBalanceTransferModel.ToYearName=userSession.FinancialYearName;

            return await Task.Run(() =>
            {
                return PartialView("_OpeningBalanceTransfer", openingBalanceTransferModel);
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
                        data.Result.Message = "Opening Balance Transfer Saved Successfully";
                    }
                    else
                    {
                        data.Result.Status = false;
                        data.Result.Message = "Opening Balance Transfer Not Saved. Please Try Again.";
                    }
                }
            }

            return Json(data);
        }


    }
}

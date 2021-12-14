using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class FinancialYearController : Controller
    {
        private readonly IFinancialYear _financialYear;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="financialYear"></param>
        public FinancialYearController(IFinancialYear financialYear)
        {
            this._financialYear = financialYear;
        }

        /// <summary>
        /// financialYear list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get financialYear list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFinancialYearList()
        {
            DataTableResultModel<FinancialYearModel> resultModel = await _financialYear.GetFinancialYearList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new financialYear.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddFinancialYear()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddFinancialYear", new FinancialYearModel());
            });
        }

        /// <summary>
        /// edit financialYear.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditFinancialYear(int financialYearId)
        {
            FinancialYearModel financialYearModel = await _financialYear.GetFinancialYearById(financialYearId);

            return PartialView("_AddFinancialYear", financialYearModel);
        }

        /// <summary>
        /// save financialYear.
        /// </summary>
        /// <param name="financialYearModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveFinancialYear(FinancialYearModel financialYearModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (financialYearModel.FinancialYearId > 0)
                {
                    // update record.
                    if (true == await _financialYear.UpdateFinancialYear(financialYearModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _financialYear.CreateFinancialYear(financialYearModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete financialYear by financialYearid.
        /// </summary>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteFinancialYear(int financialYearId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _financialYear.DeleteFinancialYear(financialYearId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrency _currency;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="currency"></param>
        public CurrencyController(ICurrency currency)
        {
            this._currency = currency;
        }

        /// <summary>
        /// currency list.
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
        /// get currency list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCurrencyList()
        {
            DataTableResultModel<CurrencyModel> resultModel = await _currency.GetCurrencyList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new currency.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddCurrency()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddCurrency", new CurrencyModel());
            });
        }

        /// <summary>
        /// edit currency.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditCurrency(int currencyId)
        {
            CurrencyModel currencyModel = await _currency.GetCurrencyById(currencyId);

            return PartialView("_AddCurrency", currencyModel);
        }

        /// <summary>
        /// save currency.
        /// </summary>
        /// <param name="currencyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCurrency(CurrencyModel currencyModel)
        {

            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (currencyModel.CurrencyId > 0)
                {
                    // update record.
                    if (true == await _currency.UpdateCurrency(currencyModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _currency.CreateCurrency(currencyModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete currency by currencyid.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCurrency(int currencyId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _currency.DeleteCurrency(currencyId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

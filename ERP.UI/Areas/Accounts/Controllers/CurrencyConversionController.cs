using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ERP.Services.Master.Interface;
using ERP.Models.Admin;
using ERP.Models.Extension;
using System;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class CurrencyConversionController : Controller
    {
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        public CurrencyConversionController(ICurrencyConversion currencyConversion, ICurrency currency)
        {
            this._currencyConversion = currencyConversion;
            this._currency = currency;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetCurrencyConversionList()
        {
            DataTableResultModel<CurrencyConversionModel> resultModel = await _currencyConversion.GetCurrencyConversionList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> AddCurrencyConversion()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            
            CurrencyConversionModel currencyConversionModel = new CurrencyConversionModel();
            currencyConversionModel.CompanyId = userSession.CompanyId;
            currencyConversionModel.EffectiveDateTime = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddCurrencyConversion", currencyConversionModel);
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> EditCurrencyConversion(int currencyConversionId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetCurrencyConversionById(currencyConversionId);

            return PartialView("_AddCurrencyConversion", currencyConversionModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCurrencyConversion(CurrencyConversionModel currencyConversionModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (currencyConversionModel.ConversionId > 0)
                {
                    // update record.
                    if (true == await _currencyConversion.UpdateCurrencyConversion(currencyConversionModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _currencyConversion.CreateCurrencyConversion(currencyConversionModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCurrencyConversion(int conversionId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _currencyConversion.DeleteCurrencyConversion(conversionId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }


    }
}

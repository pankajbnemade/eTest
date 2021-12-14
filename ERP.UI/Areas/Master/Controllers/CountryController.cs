using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    [Area("Master")]
    public class CountryController : Controller
    {
        private readonly ICountry _country;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="country"></param>
        public CountryController(ICountry country)
        {
            this._country = country;
        }

        /// <summary>
        /// country list.
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
        /// get country list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCountryList()
        {
            DataTableResultModel<CountryModel> resultModel = await _country.GetCountryList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new country.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddCountry()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddCountry", new CountryModel());
            });
        }

        /// <summary>
        /// edit country.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditCountry(int countryId)
        {
            CountryModel countryModel = await _country.GetCountryById(countryId);

            return PartialView("_AddCountry", countryModel);
        }

        /// <summary>
        /// save country.
        /// </summary>
        /// <param name="countryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCountry(CountryModel countryModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (countryModel.CountryId > 0)
                {
                    // update record.
                    if (true == await _country.UpdateCountry(countryModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _country.CreateCountry(countryModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete country by countryid.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCountry(int countryId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _country.DeleteCountry(countryId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

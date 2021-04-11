using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    public class CityController : Controller
    {
        private readonly ICity _city;
        private readonly ICountry _country;
        private readonly IState _state;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        public CityController(ICity city, ICountry country, IState state)
        {
            this._city = city;
            this._state = state;
            this._country = country;
        }

        /// <summary>
        /// city list.
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
        /// get city list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCityList()
        {
            DataTableResultModel<CityModel> resultModel = await _city.GetCityList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new city.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddCity()
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            return View("_AddCity", new CityModel());
        }

        /// <summary>
        /// edit city.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditCity(int cityId)
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            CityModel cityModel = await _city.GetCityById(cityId);

            return View("_AddCity", cityModel);
        }

        /// <summary>
        /// save city.
        /// </summary>
        /// <param name="cityModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCity(CityModel cityModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (cityModel.CityId > 0)
                {
                    // update record.
                    if (true == await _city.UpdateCity(cityModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _city.CreateCity(cityModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// get state list based on countryId
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> GetStateByCountryId(int countryId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<StateModel> stateModelList = await _state.GetStateByCountryId(countryId);
            if (null != stateModelList && stateModelList.Any())
            {
                data.Result.Status = true;
                data.Result.Data = stateModelList;
            }
            else
            {
                data.Result.Message = "NoItems";
            }

            return Json(data); // returns.
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCity(int cityId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _city.DeleteCity(cityId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    public class StateController : Controller
    {
        private readonly ICountry _country;
        private readonly IState _state;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="state"></param>
        public StateController(ICountry country, IState state)
        {
            this._state = state;
            this._country = country;
        }

        /// <summary>
        /// state list.
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
        /// get state list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetStateList()
        {
            DataTableResultModel<StateModel> resultModel = await _state.GetStateList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new state.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddState()
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            return PartialView("_AddState", new StateModel());
        }

        /// <summary>
        /// edit state.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditState(int stateId)
        {
            ViewBag.CountryList = await _country.GetCountrySelectList();

            StateModel stateModel = await _state.GetStateById(stateId);

            return PartialView("_AddState", stateModel);
        }

        /// <summary>
        /// save state.
        /// </summary>
        /// <param name="stateModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveState(StateModel stateModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (stateModel.StateId > 0)
                {
                    // update record.
                    if (true == await _state.UpdateState(stateModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _state.CreateState(stateModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete state by stateid.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteState(int stateId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _state.DeleteState(stateId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

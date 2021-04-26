using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    public class UnitOfMeasurementController : Controller
    {
        private readonly IUnitOfMeasurement _unitOfMeasurement;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="unitOfMeasurement"></param>
        public UnitOfMeasurementController(IUnitOfMeasurement unitOfMeasurement)
        {
            this._unitOfMeasurement = unitOfMeasurement;
        }

        /// <summary>
        /// unitOfMeasurement list.
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
        /// get unitOfMeasurement list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetUnitOfMeasurementList()
        {
            DataTableResultModel<UnitOfMeasurementModel> resultModel = await _unitOfMeasurement.GetUnitOfMeasurementList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new unitOfMeasurement.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddUnitOfMeasurement()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddUnitOfMeasurement", new UnitOfMeasurementModel());
            });
        }

        /// <summary>
        /// edit unitOfMeasurement.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditUnitOfMeasurement(int unitOfMeasurementId)
        {
            UnitOfMeasurementModel unitOfMeasurementModel = await _unitOfMeasurement.GetUnitOfMeasurementById(unitOfMeasurementId);

            return PartialView("_AddUnitOfMeasurement", unitOfMeasurementModel);
        }

        /// <summary>
        /// save unitOfMeasurement.
        /// </summary>
        /// <param name="unitOfMeasurementModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveUnitOfMeasurement(UnitOfMeasurementModel unitOfMeasurementModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (unitOfMeasurementModel.UnitOfMeasurementId > 0)
                {
                    // update record.
                    if (true == await _unitOfMeasurement.UpdateUnitOfMeasurement(unitOfMeasurementModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _unitOfMeasurement.CreateUnitOfMeasurement(unitOfMeasurementModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete unitOfMeasurement by unitOfMeasurementid.
        /// </summary>
        /// <param name="unitOfMeasurementId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteUnitOfMeasurement(int unitOfMeasurementId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _unitOfMeasurement.DeleteUnitOfMeasurement(unitOfMeasurementId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

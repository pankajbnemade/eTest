using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    [Area("Master")]
    public class DesignationController : Controller
    {
        private readonly IDesignation _designation;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="designation"></param>
        public DesignationController(IDesignation designation)
        {
            this._designation = designation;
        }

        /// <summary>
        /// designation list.
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
        /// get designation list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDesignationList()
        {
            DataTableResultModel<DesignationModel> resultModel = await _designation.GetDesignationList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new designation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddDesignation()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddDesignation", new DesignationModel());
            });
        }

        /// <summary>
        /// edit designation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditDesignation(int designationId)
        {
            DesignationModel designationModel = await _designation.GetDesignationById(designationId);

            return PartialView("_AddDesignation", designationModel);
        }

        /// <summary>
        /// save designation.
        /// </summary>
        /// <param name="designationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveDesignation(DesignationModel designationModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (designationModel.DesignationId > 0)
                {
                    // update record.
                    if (true == await _designation.UpdateDesignation(designationModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _designation.CreateDesignation(designationModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete designation by designationid.
        /// </summary>
        /// <param name="designationId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteDesignation(int designationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _designation.DeleteDesignation(designationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

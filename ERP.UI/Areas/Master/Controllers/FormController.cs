using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    [Area("Master")]
    public class FormController : Controller
    {
        private readonly IModule _module;
        private readonly IForm _form;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="form"></param>
        public FormController(IModule module, IForm form)
        {
            this._form = form;
            this._module = module;
        }

        /// <summary>
        /// form list.
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
        /// get form list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFormList()
        {
            DataTableResultModel<FormModel> resultModel = await _form.GetFormList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new form.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddForm()
        {
            ViewBag.ModuleList = await _module.GetModuleSelectList();

            return PartialView("_AddForm", new FormModel());
        }

        /// <summary>
        /// edit form.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditForm(int formId)
        {
            ViewBag.ModuleList = await _module.GetModuleSelectList();

            FormModel formModel = await _form.GetFormById(formId);

            return PartialView("_AddForm", formModel);
        }

        /// <summary>
        /// save form.
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveForm(FormModel formModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (formModel.FormId > 0)
                {
                    // update record.
                    if (true == await _form.UpdateForm(formModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _form.CreateForm(formModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete form by formid.
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteForm(int formId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _form.DeleteForm(formId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}

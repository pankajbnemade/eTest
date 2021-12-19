using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AssignRoleController : Controller
    {

        private readonly IAssignRole _assignRole;

        public AssignRoleController(IApplicationRole assignRole)
        {
            _assignRole = assignRole;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        //[HttpPost]
        //public async Task<JsonResult> GetRoleList()
        //{
        //    DataTableResultModel<ApplicationRoleModel> resultModel = await _assignRole.GetApplicationRoleList();

        //    return await Task.Run(() =>
        //    {
        //        return Json(new { draw = 1, data = resultModel.ResultList });
        //    });
        //}


        //[HttpGet]
        //public async Task<PartialViewResult> AddRole()
        //{
        //    return await Task.Run(() =>
        //    {
        //        return PartialView("_AddRole", new ApplicationRoleModel());
        //    });
        //}

        //[HttpGet]
        //public async Task<PartialViewResult> EditRole(int roleId)
        //{
        //    ApplicationRoleModel applicationRoleModel = await _assignRole.GetApplicationRoleById(roleId);

        //    return PartialView("_AddRole", applicationRoleModel);
        //}

        //[HttpPost]
        //public async Task<JsonResult> SaveRole(ApplicationRoleModel applicationRoleModel)
        //{
        //    JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

        //    if (ModelState.IsValid)
        //    {
        //        if (applicationRoleModel.Id > 0)
        //        {
        //            // update record.
        //            if (true == await _assignRole.UpdateRole(applicationRoleModel))
        //            {
        //                data.Result.Status = true;
        //            }
        //        }
        //        else
        //        {
        //            // add new record.
        //            if (await _assignRole.CreateRole(applicationRoleModel) > 0)
        //            {
        //                data.Result.Status = true;
        //            }
        //        }
        //    }

        //    return Json(data);
        //}

        //[HttpPost]
        //public async Task<JsonResult> DeleteRole(int roleId)
        //{
        //    JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

        //    if (true == await _assignRole.DeleteRole(roleId))
        //    {
        //        data.Result.Status = true;
        //    }

        //    return Json(data); // returns.
        //}


    }
}

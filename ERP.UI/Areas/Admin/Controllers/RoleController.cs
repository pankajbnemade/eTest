using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        
        private readonly IApplicationRole _role;

        public RoleController(IApplicationRole role)
        {
            _role=role;
        }
        
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetRoleList()
        {
            DataTableResultModel<ApplicationRoleModel> resultModel = await _role.GetApplicationRoleList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }


        [HttpGet]
        public async Task<PartialViewResult> AddRole()
        {
            return await Task.Run(() =>
            {
                return PartialView("_AddRole", new ApplicationRoleModel());
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> EditRole(int roleId)
        {
            ApplicationRoleModel applicationRoleModel = await _role.GetApplicationRoleById(roleId);

            return PartialView("_AddRole", applicationRoleModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveRole(ApplicationRoleModel applicationRoleModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (applicationRoleModel.Id > 0)
                {
                    // update record.
                    if (true == await _role.UpdateRole(applicationRoleModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _role.CreateRole(applicationRoleModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRole(int roleId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _role.DeleteRole(roleId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

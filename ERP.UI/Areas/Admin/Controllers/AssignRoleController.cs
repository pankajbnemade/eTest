using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Admin.Interface;
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
        private readonly IApplicationIdentityUser _user;
        private readonly IApplicationRole _role;

        public AssignRoleController(IAssignRole assignRole, IApplicationIdentityUser user, IApplicationRole role)
        {
            _assignRole = assignRole;
            _user = user;
            _role = role;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetUserRoleList()
        {
            DataTableResultModel<AssignRoleModel> resultModel = await _assignRole.GetUserRoleList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        [HttpGet]
        public async Task<PartialViewResult> AddUserRole()
        {
            ViewBag.UserList = await _user.GetUserSelectList();
            ViewBag.RoleList = await _role.GetRoleSelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddUserRole", new AssignRoleModel());
            });
        }

        [HttpPost]
        public async Task<JsonResult> SaveUserRole(AssignRoleModel assignRoleModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                // add new record.
                if (await _assignRole.AddUserRole(assignRoleModel))
                {
                    data.Result.Status = true;
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUserRole(int userId, int roleId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _assignRole.DeleteUserRole(userId, roleId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}

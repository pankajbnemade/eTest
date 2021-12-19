using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IApplicationIdentityUser _aplicationIdentityUser;
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(
                            IApplicationIdentityUser aplicationIdentityUser,
                            ILogger<UserController> logger,
                            IWebHostEnvironment hostEnvironment
                            )
        {
            _aplicationIdentityUser = aplicationIdentityUser;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetUserList()
        {
            DataTableResultModel<ApplicationIdentityUserModel> resultModel = await _aplicationIdentityUser.GetApplicationIdentityUserList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock(int id)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (id > 0)
            {
                // update record.
                if (true == await _aplicationIdentityUser.LockUnlock(id))
                {
                    data.Result.Status = true;
                    data.Result.Message = "Action successful ";
                }
            }

            return Json(data); // returns.
        }

        public IActionResult UserInformation()
        {
            return View();
        }

    }
}

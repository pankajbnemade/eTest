using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly IApplicationIdentityUser _aplicationIdentityUser;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(UserManager<ApplicationIdentityUser> userManager,
                            SignInManager<ApplicationIdentityUser> signInManager,
                            IApplicationIdentityUser aplicationIdentityUser,
                            ILogger<UserController> logger,
                            IEmailSender emailSender,
                            IWebHostEnvironment hostEnvironment
                            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _aplicationIdentityUser = aplicationIdentityUser;
            _logger = logger;
            _emailSender = emailSender;
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

            ApplicationIdentityUserModel userModel = await _aplicationIdentityUser.GetApplicationIdentityUserByUserId(id);

            if (userModel == null)
            {
                data.Result.Status = false;
                data.Result.Message = "Error while Locking/Unlocking";
                return Json(data);
            }
            if (userModel.LockoutEnd != null && userModel.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                userModel.LockoutEnd = DateTime.Now;
            }
            else
            {
                userModel.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            if (true == await _aplicationIdentityUser.UpdateUser(userModel))
            {
                data.Result.Status = true;
                data.Result.Message = "Action successful ";
            }
            
            return Json(data); // returns.
        }

        public IActionResult UserInformation()
        {
            return View();
        }

    }
}

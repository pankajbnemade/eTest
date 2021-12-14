using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Models.Extension;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using ERP.Models.Common;

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

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
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

        public IActionResult UserInformation()
        {
            return View();
        }

    }
}

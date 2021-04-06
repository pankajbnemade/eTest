using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Services.Admin.Interface;
using ERP.UI.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly IApplicationIdentityUser _aplicationIdentityUser;

        public UserController(UserManager<ApplicationIdentityUser> userManager,
                              SignInManager<ApplicationIdentityUser> signInManager,
                              IApplicationIdentityUser aplicationIdentityUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _aplicationIdentityUser = aplicationIdentityUser;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home", new { area = "Common" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    ApplicationIdentityUserModel applicationUser = await _aplicationIdentityUser.GetApplicationIdentityUserListByEmail(user.Email);
                    UserSessionModel userSessionModel = new UserSessionModel();
                    userSessionModel.UserId = applicationUser.Id;
                    userSessionModel.Username = applicationUser.UserName;
                    SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                    return RedirectToAction("Index", "Home", new { area = "Common" });
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult UserInformation()
        {
            return View();
        }

    }
}

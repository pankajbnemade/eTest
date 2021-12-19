using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ERP.DataAccess.Entity;
using ERP.Models.Extension;
using ERP.Models.Admin;
using ERP.Services.Admin.Interface;

namespace ERP.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IApplicationIdentityUser _aplicationIdentityUser;

        public LoginModel(SignInManager<ApplicationIdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationIdentityUser> userManager,
            IApplicationIdentityUser aplicationIdentityUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _aplicationIdentityUser = aplicationIdentityUser;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    ApplicationIdentityUserModel applicationUser = await _aplicationIdentityUser.GetApplicationIdentityUserByEmail(Input.Email);

                    ///Added to set default session -- By Pankaj

                    UserSessionModel userSessionModel = new UserSessionModel();

                    userSessionModel.UserId = applicationUser.Id;
                    userSessionModel.UserName = applicationUser.UserName;

                    //start temporary avoid branch/financial year  selection

                    userSessionModel.CompanyId = 1;
                    userSessionModel.FinancialYearId = 1;

                    //temporary avoid branch/financial year  selection

                    SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                    return LocalRedirect(returnUrl);
                }



                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                //    return Page();
                //}
                else
                {
                    var user = await _userManager.FindByNameAsync(Input.Email);

                    //Add this to check if the email was confirmed.
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "You need to confirm your email.");
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        return Page();
                    }

                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

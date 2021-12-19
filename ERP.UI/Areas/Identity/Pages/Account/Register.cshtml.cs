using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Models.Helpers;
using ERP.Services.Admin.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ERP.UI.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IEmployee _employee;
        private readonly IApplicationIdentityUser _aplicationIdentityUser;

        public RegisterModel(
            UserManager<ApplicationIdentityUser> userManager,
            SignInManager<ApplicationIdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IEmployee employee,
            IApplicationIdentityUser aplicationIdentityUser)
        {
            _employee = employee;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _aplicationIdentityUser = aplicationIdentityUser;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            //public int UserId { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Employee")]
            public int EmployeeId { get; set; }

            public IList<SelectListModel> EmployeeList { get; set; }
        }

        //public async Task OnGetAsync(int? userId, string returnUrl = null)
        public async Task OnGetAsync(string returnUrl = null)
        {
            //if (userId != null)
            //{
            //    ApplicationIdentityUserModel userMode = await _aplicationIdentityUser.GetApplicationIdentityUserByUserId((int)userId);

            //    Input.UserId = userMode.Id;
            //    Input.Email = userMode.Email;
            //}

            ReturnUrl = returnUrl;

            Input = new InputModel()
            {
                EmployeeList = await _employee.GetEmployeeSelectList()
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Random Password

            Input.Password = Input.Email.Substring(1, 2).ToUpper() + "@" + Input.Email.Substring(1, 2).ToLower() + "2" + Guid.NewGuid().ToString().Substring(2, 4);

            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser { UserName = Input.Email, Email = Input.Email, EmployeeId = Input.EmployeeId };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.SetLockoutEnabledAsync(user, true);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." +
                                    "</br> Your password is <b>" + Input.Password + "<b>"
                                    );

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToPage("RegisterConfirmation", new
                        {
                            email = Input.Email,
                            returnUrl = returnUrl
                        });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);

                        return RedirectToAction("Index", "User", new { area = "Admin" });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            ReturnUrl = returnUrl;

            Input = new InputModel()
            {
                EmployeeList = await _employee.GetEmployeeSelectList()
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }

    }



    #region OriginalCode


    //[AllowAnonymous]
    //public class RegisterModel : PageModel
    //{
    //    private readonly SignInManager<ApplicationIdentityUser> _signInManager;
    //    private readonly UserManager<ApplicationIdentityUser> _userManager;
    //    private readonly ILogger<RegisterModel> _logger;
    //    private readonly IEmailSender _emailSender;

    //    public RegisterModel(
    //        UserManager<ApplicationIdentityUser> userManager,
    //        SignInManager<ApplicationIdentityUser> signInManager,
    //        ILogger<RegisterModel> logger,
    //        IEmailSender emailSender)
    //    {
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //        _logger = logger;
    //        _emailSender = emailSender;
    //    }

    //    [BindProperty]
    //    public InputModel Input { get; set; }

    //    public string ReturnUrl { get; set; }

    //    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    //    public class InputModel
    //    {
    //        [Required]
    //        [EmailAddress]
    //        [Display(Name = "Email")]
    //        public string Email { get; set; }

    //        [Required]
    //        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    //        [DataType(DataType.Password)]
    //        [Display(Name = "Password")]
    //        public string Password { get; set; }

    //        [DataType(DataType.Password)]
    //        [Display(Name = "Confirm password")]
    //        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //        public string ConfirmPassword { get; set; }
    //    }

    //    public async Task OnGetAsync(string returnUrl = null)
    //    {
    //        ReturnUrl = returnUrl;
    //        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    //    }

    //    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    //    {
    //        returnUrl = returnUrl ?? Url.Content("~/");
    //        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    //        if (ModelState.IsValid)
    //        {
    //            var user = new ApplicationIdentityUser { UserName = Input.Email, Email = Input.Email };
    //            var result = await _userManager.CreateAsync(user, Input.Password);
    //            if (result.Succeeded)
    //            {
    //                _logger.LogInformation("User created a new account with password.");

    //                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    //                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

    //                var callbackUrl = Url.Page(
    //                    "/Account/ConfirmEmail",
    //                    pageHandler: null,
    //                    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
    //                    protocol: Request.Scheme);

    //                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
    //                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

    //                if (_userManager.Options.SignIn.RequireConfirmedAccount)
    //                {
    //                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
    //                }
    //                else
    //                {
    //                    await _signInManager.SignInAsync(user, isPersistent: false);
    //                    return LocalRedirect(returnUrl);
    //                }
    //            }
    //            foreach (var error in result.Errors)
    //            {
    //                ModelState.AddModelError(string.Empty, error.Description);
    //            }
    //        }

    //        // If we got this far, something failed, redisplay form
    //        return Page();
    //    }
    //}


    #endregion

}

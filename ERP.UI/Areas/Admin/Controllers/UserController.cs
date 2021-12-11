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

namespace ERP.UI.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly IApplicationIdentityUser _aplicationIdentityUser;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(UserManager<ApplicationIdentityUser> userManager,
                            SignInManager<ApplicationIdentityUser> signInManager,
                            IApplicationIdentityUser aplicationIdentityUser,
                            ILogger logger,
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

        public IActionResult AccessDenied()
        {
            AccessDeniedModel model = new AccessDeniedModel();

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            ConfirmEmailModel model = new ConfirmEmailModel();

            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            model.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            ConfirmEmailChangeModel model = new ConfirmEmailChangeModel();

            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ChangeEmailAsync(user, email, code);

            if (!result.Succeeded)
            {
                model.StatusMessage = "Error changing email.";
                return View(model);
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);


            if (!setUserNameResult.Succeeded)
            {
                model.StatusMessage = "Error changing user name.";
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            model.StatusMessage = "Thank you for confirming your email change.";

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ExternalLogin()
        {
            ExternalLoginModel model = new ExternalLoginModel();

            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallbackAsync", "User", new { area = "Admin", returnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            ExternalLoginModel model = new ExternalLoginModel();

            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                model.ErrorMessage = $"Error from external provider: {remoteError}";
                //return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                model.ErrorMessage = "Error loading external login information.";
                //return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                //return RedirectToPage("./Lockout");
                return RedirectToAction("Lockout", "User");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                model.ReturnUrl = returnUrl;
                model.ProviderDisplayName = info.ProviderDisplayName;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    model = new ExternalLoginModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return View(model);
            }

            //return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLoginConfirmationAsync(ExternalLoginModel model, string returnUrl = null)
        {
            //ExternalLoginModel model = new ExternalLoginModel();

            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                model.ErrorMessage = "Error loading external login information during confirmation.";

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        //var callbackUrl = Url.Page(
                        //    "/Account/ConfirmEmail",
                        //    pageHandler: null,
                        //    values: new { area = "Identity", userId = userId, code = code },
                        //    protocol: Request.Scheme);

                        var callbackUrl = Url.Action("ConfirmEmail", "User", new { area = "Admin", userId = userId, code = code });

                        await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToAction("RegisterConfirmation", new { Email = model.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.ProviderDisplayName = info.ProviderDisplayName;
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "User", new { area = "Admin" });
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Action("ResetPassword", "User", new { area = "Admin", code = code });

                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToAction("ForgotPasswordConfirmation", "User", new { area = "Admin" });

            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            ForgotPasswordConfirmationModel model = new ForgotPasswordConfirmationModel();

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Lockout()
        {
            LockoutModel model = new LockoutModel();

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            LoginModel model = new LoginModel();

            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    ApplicationIdentityUserModel applicationUser = await _aplicationIdentityUser.GetApplicationIdentityUserListByEmail(model.Email);
                    UserSessionModel userSessionModel = new UserSessionModel();

                    userSessionModel.UserId = applicationUser.Id;
                    userSessionModel.UserName = applicationUser.UserName;

                    //start temporary avoid branch/financial year  selection

                    userSessionModel.CompanyId = 1;
                    userSessionModel.FinancialYearId = 1;

                    //temporary avoid branch/financial year  selection

                    SessionExtension.SetComplexData(HttpContext.Session, "UserSession", userSessionModel);

                    return RedirectToAction("Index", "Home", new { area = "Common" });

                    //return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", "User", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");

                    return RedirectToAction("Lockout", "User", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);


        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            LoginWith2faModel model = new LoginWith2faModel();

            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            model.ReturnUrl = returnUrl;
            model.RememberMe = rememberMe;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return RedirectToAction("Index", "Home", new { area = "Common" });
                //return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToAction("Lockout", "User", new { area = "Admin" });
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            LoginWithRecoveryCodeModel model = new LoginWithRecoveryCodeModel();

            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);

                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);

                return RedirectToAction("Lockout", "User", new { area = "Admin" });
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            RegisterModel model = new RegisterModel();

            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

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
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { area = "Admin", code = code });

                    var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                   + "Templates" + Path.DirectorySeparatorChar.ToString() + "MailTemplates"
                                   + Path.DirectorySeparatorChar.ToString() + "Confirm_Account_Registration.html";

                    var subject = "Confirm Account Registration";

                    string HtmlBody = "";

                    using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
                    {
                        HtmlBody = streamReader.ReadToEnd();
                    }

                    //{0} : Subject  
                    //{1} : DateTime  
                    //{2} : Name  
                    //{3} : Email  
                    //{4} : Message  
                    //{5} : callbackURL  

                    string Message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                    string messageBody = string.Format(HtmlBody,
                        subject,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        user.UserName,
                        user.Email,
                        Message,
                        callbackUrl
                        );

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email", messageBody);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index", "Home", new { area = "Common" });
                    }

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string email, string returnUrl = null)
        {
            RegisterConfirmationModel model = new RegisterConfirmationModel();

            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            model.Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            model.DisplayConfirmAccountLink = true;

            if (model.DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                model.EmailConfirmationUrl = Url.Action("ConfirmEmail", "User", new { area = "Admin", userId = userId, code = code, returnUrl = returnUrl });
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                ResetPasswordModel model = new ResetPasswordModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };

                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "User", new { area = "Admin" });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "User", new { area = "Admin" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult UserInformation()
        {
            return View();
        }

    }
}

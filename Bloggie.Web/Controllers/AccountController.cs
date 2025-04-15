using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;

        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            // If something fails, show form again
            ModelState.AddModelError("", "Registration failed. Please try again.");
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl // Capture the ReturnUrl from the query string
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid login details.";
                return View(loginViewModel);
            }

            var user = await userManager.FindByNameAsync(loginViewModel.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id) // 👈 Add this line
                    
                };


                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Save token in a cookie
                HttpContext.Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = token.ValidTo,
                    SameSite = SameSiteMode.Strict // Or Lax, depending on your needs
                });

                var returnUrl = loginViewModel.ReturnUrl ?? "/Home/Index";
                return Redirect(returnUrl); // Redirect to the page the user was originally trying to access
            }

            TempData["ErrorMessage"] = "Invalid username or password.";
            return View(loginViewModel);
        }




        [HttpGet]
        public IActionResult Logout()
        {
            // Remove the JWT cookie
            Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        // ➤ Show Forgot Password Page
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "⚠️ Email not found.";
                return View();
            }

            // Generate reset token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // Create reset link
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token, email = model.Email }, Request.Scheme);

            // Normally, send this via email (for now, redirect to the reset link)
            return Redirect(resetLink);
        }






        // ➤ Show Reset Password Page
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }


        // ➤ Handle Password Reset
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "⚠️ Invalid request.";
                return View();
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "✅ Password reset successful!";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "⚠️ Error resetting password.";
            return View();
        }






        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "✅ Password updated successfully!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = "❌ Incorrect current password or invalid new password.";
            }

            return View(model);
        }








    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{

    public class Account1Controller : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender1 _emailSender;

        public Account1Controller(UserManager<ApplicationUser> userManager, IEmailSender1 emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                /*if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("ForgotPassword");
                }*/
                if (user == null)
                {
                    return View("NoResults");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account1", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Thay đổi mật khẩu", $"Vui lòng nhấn vào liên kết phía dưới để thay đổi mật khẩu <a href='{callbackUrl}'>Tại đây</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token = null)
        {
            if (token == null)
            {
                return BadRequest("A token must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}

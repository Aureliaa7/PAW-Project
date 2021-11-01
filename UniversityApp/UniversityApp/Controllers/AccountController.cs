using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signManager;
        private readonly UserManager<Users> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ILoginService loginService;
        private readonly IImageService imageService;

        public AccountController(SignInManager<Users> signManager, UserManager<Users> userManager, IHttpContextAccessor contextAccessor, 
            ILoginService loginService, IImageService imageService)
        {
            this.signManager = signManager;
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.loginService = loginService;
            this.imageService = imageService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin userModel)
        {
            int studentRole = 0, teacherRole = 0, secretaryRole = 0;

            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await userManager.FindByEmailAsync(userModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                await loginService.Login(user, userManager);

                foreach (string role in roles)
                {
                    if (String.Equals(role, "Student"))
                    {
                        studentRole++;
                    }
                    else if (String.Equals(role, "Teacher"))
                    {
                        teacherRole++;
                    }
                    else if (String.Equals(role, "Secretary"))
                    {
                        secretaryRole++;
                    }

                }
                if (studentRole > 0)
                {
                    return RedirectToLocal("/Students/Home");
                }
                else if (teacherRole > 0)
                {
                    return RedirectToLocal("/Teachers/Home");
                }
                else
                {
                    return RedirectToLocal("/Secretaries/Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            contextAccessor.HttpContext.Session.Clear();
            await signManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public JsonResult GetUserImage()
        {
            var user = userManager.GetUserAsync(User);
            string retVal = "";
            if (user != null)
            {
                retVal = imageService.GetUserProfileImage(user.Result); 
            }
            return Json(retVal);
        }
    }
}
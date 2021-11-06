using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Core;
using System.Linq;

namespace UniversityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signManager;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ILoginService loginService;
        private readonly IImageService imageService;

        public AccountController(SignInManager<User> signManager, UserManager<User> userManager, IHttpContextAccessor contextAccessor, 
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
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await userManager.FindByEmailAsync(userModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                await loginService.Login(user, userManager);

                RedirectToSpecificPage(role);
            }
            else
            {
                ModelState.AddModelError("", "Wrong credentials!");
            }
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToSpecificPage(string role)
        {
            if (role == null)
            {
                return View();
            }

            if (role != Constants.SecretaryRole)
            {
                return RedirectToLocal($"/{role}s/Home");
            }

            if (role == Constants.SecretaryRole)
            {
                return RedirectToLocal($"/Secretaries/Home");
            }

            return View();
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
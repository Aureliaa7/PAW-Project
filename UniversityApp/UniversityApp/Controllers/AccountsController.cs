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
using UniversityApp.Core.Exceptions;

namespace UniversityApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IAccountService accountService;
        private readonly IImageService imageService;

        public AccountsController(
            UserManager<User> userManager, 
            IHttpContextAccessor contextAccessor, 
            IAccountService accountService, 
            IImageService imageService)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.accountService = accountService;
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

            try {
                await accountService.LoginAsync(userModel);
                string path = await GetPageRouteAsync(userModel.Email);
                if (!string.IsNullOrEmpty(path))
                {
                    return RedirectToLocal(path);
                }
            }
            catch (IncorrectCredentialsException)
            {
                ModelState.AddModelError("", "Wrong credentials!");
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private async Task<string> GetPageRouteAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            if (role != Constants.SecretaryRole)
            {
                return $"/{role}s/Home";
            }

            if (role == Constants.SecretaryRole)
            {
                return $"/Secretaries/Home";
            }

            return string.Empty;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<JsonResult> GetCurrentUserProfileImage()
        {
            var user = await userManager.GetUserAsync(User);
            string base64Image = null;
            if (user != null)
            {
                base64Image = imageService.GetUserProfileImage(user); 
            }
            return Json(base64Image);
        }
    }
}
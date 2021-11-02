using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class SecretariesController : Controller
    {
        private readonly ISecretaryService secretaryService;
        private SignInManager<User> signManager;
        private UserManager<User> userManager;
        private readonly IUserService userService;

        public SecretariesController(ISecretaryService secretaryService, SignInManager<User> signManager, UserManager<User> userManager, IUserService userService)
        {
            this.secretaryService = secretaryService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {
            return View((await secretaryService.GetAsync()).Include(t => t.User).ToList());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = (await secretaryService.GetAsync(s => s.SecretaryId == id)).FirstOrDefault();

            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile>Image, SecretaryRegistrationViewModel secretary)
        {
            var user = userService.CreateUser(Image, String.Concat(secretary.LastName, secretary.FirstName), secretary.Email, secretary.PhoneNumber);
            var result = await userManager.CreateAsync(user, secretary.Password);

            if (result.Succeeded)
            {
                secretary.Image = user.Image;
                await secretaryService.AddAsync(secretary, user.Id);
                await userManager.AddToRoleAsync(user, secretary.Role);
                return RedirectToAction("Index", "Secretaries");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = (await secretaryService.GetAsync(s => s.SecretaryId == id)).FirstOrDefault();
            return View(secretary);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SecretaryId,Cnp,FirstName,LastName,UserId,PhoneNumber,Email")] Secretary secretary)
        {
            if (id != secretary.SecretaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var secretaryFound = (await secretaryService.GetAsync(s => s.SecretaryId == secretary.SecretaryId)).FirstOrDefault();
                if (secretaryFound == null)
                {
                    return NotFound();
                }
                await secretaryService.UpdateAsync(secretary);
                return RedirectToAction(nameof(Index));
            }
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var secretary = (await secretaryService.GetAsync(s => s.SecretaryId == id)).FirstOrDefault();

            if (secretary == null)
            {
                return NotFound();
            }
            return View(secretary);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var secretary = (await secretaryService.GetAsync(s => s.SecretaryId == id)).FirstOrDefault();
            var user = (await userService.GetAsync(u => String.Equals(u.Id, secretary.UserId))).FirstOrDefault();
            await secretaryService.DeleteByCnpAsync(secretary.Cnp);
            await userService.DeleteAsync(new Guid(user.Id));  // i'm not sure about this one
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteByCnp()
        {
            ViewData["SecretaryCnp"] = new SelectList(await secretaryService.GetAsync(), "Cnp", "Cnp");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteByCnp(DeleteSecretaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await secretaryService.DeleteByCnpAsync(model.Cnp);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["SecretaryCnp"] = new SelectList(await secretaryService.GetAsync(), "Cnp", "Cnp");
            return View(model);
        }

        public async Task<IActionResult> GetSecretaryNameByCnp(string cnp)
        {
            var secretaryName = "secretary not found";
            var searchedSecretary = (await secretaryService.GetAsync(s => String.Equals(s.Cnp, cnp))).FirstOrDefault();
            if(searchedSecretary != null)
            {
                secretaryName = searchedSecretary.LastName + " " + searchedSecretary.FirstName;
            }
            return Json(secretaryName);
        }

        [Authorize]
        public async Task<IActionResult> Home([FromServices] IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                // search the student based on his user id
                var secretary = (await secretaryService.GetAsync(s => String.Equals(userId, s.UserId))).FirstOrDefault();

                if (secretary != null)
                {
                    return View(secretary);
                }
            }
            return View();
        }
    }
}

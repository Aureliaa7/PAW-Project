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
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Controllers
{
    public class SecretariesController : Controller
    {
        private readonly ISecretaryService secretaryService;
        private SignInManager<Users> signManager;
        private UserManager<Users> userManager;
        private readonly IUserService userService;

        public SecretariesController(ISecretaryService secretaryService, SignInManager<Users> signManager, UserManager<Users> userManager, IUserService userService)
        {
            this.secretaryService = secretaryService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Index()
        {
            return View(secretaryService.SecretaryRepository.FindAll().Include(t => t.User).ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = secretaryService.SecretaryRepository.FindByCondition(s => s.SecretaryId == id).FirstOrDefault();

            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }


        [Authorize(Roles = "Secretary")]
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
                secretaryService.RegisterSecretary(secretary, user.Id);
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = secretaryService.SecretaryRepository.FindByCondition(s => s.SecretaryId == id).First();
            return View(secretary);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("SecretaryId,Cnp,FirstName,LastName,UserId,PhoneNumber,Email")] Secretaries secretary)
        {
            if (id != secretary.SecretaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var secretaryFound = secretaryService.SecretaryRepository.FindByCondition(s => s.SecretaryId == secretary.SecretaryId);
                if (secretaryFound == null)
                {
                    return NotFound();
                }
                secretaryService.SecretaryRepository.Update(secretary);
                return RedirectToAction(nameof(Index));
            }
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var secretary = secretaryService.SecretaryRepository.FindByCondition(s => s.SecretaryId == id).FirstOrDefault();

            if (secretary == null)
            {
                return NotFound();
            }
            return View(secretary);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var secretary = secretaryService.SecretaryRepository.FindByCondition(s => s.SecretaryId == id).First();
            var user = secretaryService.UserRepository.FindByCondition(u => String.Equals(u.Id, secretary.UserId)).First();
            secretaryService.SecretaryRepository.Delete(secretary);
            secretaryService.UserRepository.Delete(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteByCnp()
        {
            ViewData["SecretaryCnp"] = new SelectList(secretaryService.SecretaryRepository.FindAll(), "Cnp", "Cnp");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteByCnp(DeleteSecretaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                secretaryService.DeleteByCnp(model.Cnp);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["SecretaryCnp"] = new SelectList(secretaryService.SecretaryRepository.FindAll(), "Cnp", "Cnp");
            return View(model);
        }

        public IActionResult GetSecretaryNameByCnp(string cnp)
        {
            var secretaryName = "secretary not found";
            var searchedSecretary = secretaryService.SecretaryRepository.FindByCondition(s => String.Equals(s.Cnp, cnp)).FirstOrDefault();
            if(searchedSecretary != null)
            {
                secretaryName = searchedSecretary.LastName + " " + searchedSecretary.FirstName;
            }
            return Json(secretaryName);
        }

        [Authorize]
        public IActionResult Home([FromServices] IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                // search the student based on his user id
                var secretary = secretaryService.SecretaryRepository.FindByCondition(s => String.Equals(userId, s.UserId)).FirstOrDefault();

                if (secretary != null)
                {
                    return View(secretary);
                }
            }
            return View();
        }
    }
}

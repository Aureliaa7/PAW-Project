using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Presentation.Controllers;

namespace UniversityApp.Controllers
{
    public class SecretariesController : Controller
    {
        private readonly ISecretaryService secretaryService;
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public SecretariesController(
            ISecretaryService secretaryService,
            IMapper mapper,
            IImageService imageService)
        {
            this.secretaryService = secretaryService;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View(await secretaryService.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await ReturnSecretaryView<SecretaryDto>(id);
        }

        [HttpGet]
        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SecretaryRegistrationDto secretaryDto)
        {
            var secretary = mapper.Map<Secretary>(secretaryDto);
            secretary.Image = imageService.GetBytes(secretaryDto.Image);

            try
            {
                await secretaryService.AddAsync(secretary, secretaryDto.Password);
                return RedirectToAction("Index", "Secretaries");
            }
            catch (FailedUserRegistrationException ex)
            {
                //TODO do smth with the ex. message
            }
            return View(secretaryDto);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await ReturnSecretaryView<EditSecretaryDto>(id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Cnp,FirstName,LastName,PhoneNumber,Email")] EditSecretaryDto secretary)
        {
            if (id != secretary.Id)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }

            if (ModelState.IsValid)
            {
                var secretaryFound = await secretaryService.GetFirstOrDefaultAsync(s => s.Id == id);
                if (secretaryFound == null)
                {
                    return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
                }
                await secretaryService.UpdateAsync(mapper.Map<Secretary>(secretary));
                return RedirectToAction(nameof(Index));
            }
            return View(secretary);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await ReturnSecretaryView<SecretaryDto>(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await secretaryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteByCnp()
        {
            ViewData["SecretaryCnp"] = new SelectList(await secretaryService.GetAllAsync(), "Cnp", "Cnp");
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
            ViewData["SecretaryCnp"] = new SelectList(await secretaryService.GetAllAsync(), "Cnp", "Cnp");
            return View(model);
        }

        public async Task<IActionResult> GetSecretaryNameByCnp(string cnp)
        {
            var secretaryName = "secretary not found";
            var searchedSecretary = await secretaryService.GetFirstOrDefaultAsync(s => s.Cnp == cnp);
            if(searchedSecretary != null)
            {
                secretaryName = searchedSecretary.LastName + " " + searchedSecretary.FirstName;
            }
            return Json(secretaryName);
        }

        [Authorize]
        public async Task<IActionResult> Home([FromServices] ILoggedInUserService findUserService)
        {
            var userId = findUserService.GetCurrentUserId();
            if (userId != null)
            {
                var secretary = await secretaryService.GetFirstOrDefaultAsync(s => s.Id.ToString() == userId);

                if (secretary != null)
                {
                    return View(secretary);
                }
            }
            return View();
        }

        private async Task<IActionResult> ReturnSecretaryView<T>(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            var secretary = await secretaryService.GetFirstOrDefaultAsync(s => s.Id == id);

            if (secretary == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            return View(mapper.Map<T>(secretary));
        }
    }
}

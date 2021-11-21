using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Presentation.Controllers;

namespace UniversityApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public TeachersController(
            ITeacherService teacherService, 
            IMapper mapper,
            IImageService imageService)
        {
            this.teacherService = teacherService;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            var teachers = (await teacherService.GetAsync()).OrderBy(t => t.LastName);
            return View(mapper.Map<IEnumerable<TeacherDto>>(teachers));
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherRegistrationViewModel teacherModel)
        {
            var mappedTeacher = mapper.Map<Teacher>(teacherModel);
            mappedTeacher.Image = imageService.GetBytes(teacherModel.Image);

            try
            {
                await teacherService.AddAsync(mappedTeacher, teacherModel.Password);

                return RedirectToAction("Index", "Teachers");
            }
            catch (FailedUserRegistrationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(teacherModel);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await GetTeacherView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,PhoneNumber,Email,Cnp,Degree")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                await teacherService.UpdateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await GetTeacherView(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            await teacherService.DeleteAsync(teacher.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetCourses(Guid id, [FromServices] ITeachedCourseService teachedCourseService)
        {
            var courses = await teachedCourseService.GetTeachedCoursesAsync(id);
            return View(mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> Home([FromServices]ILoggedInUserService findUserService)
        {
            var userId = findUserService.GetCurrentUserId();
            if (userId != null)
            {
                var teacher = await teacherService.GetFirstOrDefaultAsync(s => s.Id.ToString() == userId);

                if (teacher != null)
                {
                    return View(mapper.Map<TeacherDto>(teacher));
                }
            }
            return View();
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> TeachedCourses([FromServices]ITeachedCourseService teachedCourseService)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await teachedCourseService.GetTeachedCoursesAsync(new Guid(teacherId));
            return View(mapper.Map<IEnumerable<CourseDto>>(courses));
        }  

        private async Task<IActionResult> GetTeacherView(Guid? id)
        {
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            return View(mapper.Map<TeacherDto>(teacher));
        }
    }
}

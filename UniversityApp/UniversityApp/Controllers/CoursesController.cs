using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IMapper mapper;

        public CoursesController(ICourseService courseService, IMapper mapper)
        {
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<IEnumerable<CourseDto>>(await courseService.GetAllAsync()));
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,CourseTitle,NoCredits,Year,Semester")] CourseDto course)
        {
            if (ModelState.IsValid)
            {
                await courseService.AddAsync(mapper.Map<Course>(course));
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await courseService.GetFirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            return View(mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CourseTitle,NoCredits,Year,Semester")] CourseDto course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await courseService.UpdateAsync(mapper.Map<Course>(course));
                var courseFound = await courseService.GetFirstOrDefaultAsync(c => c.Id == course.Id);
                if (courseFound == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await courseService.GetFirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(mapper.Map<CourseDto>(course));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await courseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

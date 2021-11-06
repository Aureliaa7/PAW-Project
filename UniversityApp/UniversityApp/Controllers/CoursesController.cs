using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View(await courseService.GetAsync());
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Course courses)
        {
            if (ModelState.IsValid)
            {
                await courseService.AddAsync(courses);
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
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
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await courseService.UpdateAsync(course);
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
            return View(course);
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

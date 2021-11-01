using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {
            return View(await courseService.GetAsync());
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                await courseService.AddAsync(courses);
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await courseService.GetFirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Courses course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await courseService.UpdateAsync(course);
                var courseFound = await courseService.GetFirstOrDefaultAsync(c => c.CourseId == course.CourseId);
                if (courseFound == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles="Secretary")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await courseService.GetFirstOrDefaultAsync(c => c.CourseId == id);
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

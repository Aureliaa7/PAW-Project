using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Interfaces;
using UniversityApp.Models;

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
        public IActionResult Index()
        {
            return View(courseService.CourseRepository.FindAll().ToList().OrderBy(c => c.CourseTitle));
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                courseService.CourseRepository.Create(courses);
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = courseService.CourseRepository.FindByCondition(c => c.CourseId == id).First();

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CourseId,CourseTitle,NoCredits,Year,Semester")] Courses course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                courseService.CourseRepository.Update(course);
                var courseFound = courseService.CourseRepository.FindByCondition(c => c.CourseId == course.CourseId);
                if (courseFound == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles="Secretary")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = courseService.CourseRepository.FindByCondition(c => c.CourseId == id).FirstOrDefault();
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = courseService.CourseRepository.FindByCondition(c => c.CourseId == id).First();
            courseService.CourseRepository.Delete(course);
            return RedirectToAction(nameof(Index));
        }
    }
}

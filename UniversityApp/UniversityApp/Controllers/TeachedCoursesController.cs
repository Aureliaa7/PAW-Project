using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class TeachedCoursesController : Controller
    {
        private readonly ITeachedCourseService teachedCourseService;
        private readonly ICourseService courseService;
        private readonly ITeacherService teacherService;

        public TeachedCoursesController(
            ITeachedCourseService teachedCourseService,
            ICourseService courseService,
            ITeacherService teacherService)
        {
            this.teachedCourseService = teachedCourseService;
            this.courseService = courseService;
            this.teacherService = teacherService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAsync(), "CourseTitle", "CourseTitle");
            ViewData["TeacherCnp"] = new SelectList(await teacherService.GetAsync(), "Cnp", "Cnp");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await teachedCourseService.AssignCourseAsync(model);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAsync(), "CourseTitle", "CourseTitle");
            ViewData["TeacherCnp"] = new SelectList(await teacherService.GetAsync(), "Cnp", "Cnp");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachedCourses = (await teachedCourseService.GetAsync(tc => tc.Id == id)).FirstOrDefault();
            if (teachedCourses == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(await courseService.GetAsync(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(await teacherService.GetAsync(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TeacherId,CourseId")] TeachedCourses teachedCourses)
        {
            if (id != teachedCourses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await teachedCourseService.UpdateAsync(teachedCourses);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseId"] = new SelectList(await courseService.GetAsync(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(await teacherService.GetAsync(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }


        // this function will print a message if the course is already assigned to the same teacher i want to assign the course 
        public async Task<IActionResult> GetInfo(string courseTitle, string teacherCnp)
        {
            var teacher = (await teacherService.GetAsync(t => String.Equals(t.Cnp, teacherCnp))).FirstOrDefault();
            var course = (await courseService.GetAsync(c => String.Equals(c.CourseTitle, courseTitle))).FirstOrDefault();
            string info = " ";
            if(teacher != null && course != null)
            {
                var teachedCourse = (await teachedCourseService.GetAsync(tc => (tc.CourseId == course.CourseId) && (tc.TeacherId == teacher.TeacherId))).FirstOrDefault();
                if(teachedCourse != null)
                {
                    info += "this item already exists!";
                }
            }
            return Json(info);
        }
    }
}

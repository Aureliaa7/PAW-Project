using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Presentation.Controllers;

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
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
            //it would be better/easier for user to see the teacher's name instead of their cnp
            // TODO make this change 
            var teachers = (await teacherService.GetAsync()).ToList();
            teachers.ForEach(t => t.FullName = $"{t.LastName} {t.FirstName}");
            ViewData["TeacherCnp"] = new SelectList(teachers, "Cnp", "FullName");
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
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
            ViewData["TeacherCnp"] = new SelectList(await teacherService.GetAsync(), "Cnp", "Cnp");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }

            var teachedCourses = await teachedCourseService.GetFirstOrDefaultAsync(tc => tc.Id == id);
            if (teachedCourses == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            //TODO replace course id and teacher id with the names
            ViewData["CourseId"] = new SelectList(await courseService.GetAllAsync(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(await teacherService.GetAsync(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TeacherId,CourseId")] TeachedCourse teachedCourses)
        {
            if (id != teachedCourses.Id)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }

            if (ModelState.IsValid)
            {
                await teachedCourseService.UpdateAsync(teachedCourses);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseId"] = new SelectList(await courseService.GetAllAsync(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(await teacherService.GetAsync(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }

        // this function will print a message if the course is already assigned to the same teacher i want to assign the course 
        public async Task<IActionResult> GetInfo(string courseTitle, string teacherCnp)
        {
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => String.Equals(t.Cnp, teacherCnp));
            var course = await courseService.GetFirstOrDefaultAsync(c => String.Equals(c.CourseTitle, courseTitle));
            string info = " ";
            if(teacher != null && course != null)
            {
                var teachedCourse = await teachedCourseService.GetFirstOrDefaultAsync(
                    tc => (tc.CourseId == course.Id) && (tc.TeacherId == teacher.Id));
                if(teachedCourse != null)
                {
                    info += "this item already exists!";
                }
            }
            return Json(info);
        }

        public async Task<IActionResult> GetTeachersByCourseId(string courseId)
        {
            var teachers = await teachedCourseService.GetTeachersByCourseIdAsync(new Guid(courseId));
            var result = teachers.Select(x => new
            {
                Id = x.Id,
                FullName = $"{x.LastName} {x.FirstName}"
            });
            return Json(result);
        }
    }
}

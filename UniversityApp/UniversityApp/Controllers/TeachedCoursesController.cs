using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Controllers
{
    public class TeachedCoursesController : Controller
    {
        private readonly ITeachedCourseService teachedCourseService;

        public TeachedCoursesController(ITeachedCourseService teachedCourseService)
        {
            this.teachedCourseService = teachedCourseService;
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewData["CourseTitle"] = new SelectList(teachedCourseService.CourseRepository.FindAll(), "CourseTitle", "CourseTitle");
            ViewData["TeacherCnp"] = new SelectList(teachedCourseService.TeacherRepository.FindAll(), "Cnp", "Cnp");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                teachedCourseService.AssignCourse(model);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseTitle"] = new SelectList(teachedCourseService.CourseRepository.FindAll(), "CourseTitle", "CourseTitle");
            ViewData["TeacherCnp"] = new SelectList(teachedCourseService.TeacherRepository.FindAll(), "Cnp", "Cnp");
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachedCourses = teachedCourseService.TeachedCourseRepository.FindByCondition(tc => tc.Id == id).FirstOrDefault();
            if (teachedCourses == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(teachedCourseService.CourseRepository.FindAll(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(teachedCourseService.TeacherRepository.FindAll(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,TeacherId,CourseId")] TeachedCourses teachedCourses)
        {
            if (id != teachedCourses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    teachedCourseService.TeachedCourseRepository.Update(teachedCourses);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachedCoursesExists(teachedCourses.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseId"] = new SelectList(teachedCourseService.CourseRepository.FindAll(), "CourseId", "CourseId", teachedCourses.CourseId);
            ViewData["TeacherId"] = new SelectList(teachedCourseService.TeacherRepository.FindAll(), "TeacherId", "TeacherId", teachedCourses.TeacherId);
            return View(teachedCourses);
        }


        // this function will print a message if the course is already assigned to the same teacher i want to assign the course 
        public IActionResult GetInfo(string courseTitle, string teacherCnp)
        {
            var teacher = teachedCourseService.TeacherRepository.FindByCondition(t => String.Equals(t.Cnp, teacherCnp)).FirstOrDefault();
            var course = teachedCourseService.CourseRepository.FindByCondition(c => String.Equals(c.CourseTitle, courseTitle)).FirstOrDefault();
            string info = " ";
            if(teacher != null && course != null)
            {
                var teachedCourse = teachedCourseService.TeachedCourseRepository.FindByCondition(tc => (tc.CourseId == course.CourseId) && (tc.TeacherId == teacher.TeacherId)).FirstOrDefault();
                if(teachedCourse != null)
                {
                    info += "this item already exists!";
                }
            }
            return Json(info);
        }

        private bool TeachedCoursesExists(int id)
        {
            return teachedCourseService.TeachedCourseRepository.FindAll().Any(e => e.Id == id);
        }
    }
}

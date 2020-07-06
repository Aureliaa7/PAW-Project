using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Interfaces;
using UniversityApp.ViewModels;

namespace UniversityApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService enrollmentService;


        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            this.enrollmentService = enrollmentService;
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Index()
        {
            var enrollments = enrollmentService.GetAllEnrollments();
            if (enrollments != null)
            {
                return View(enrollments);
            }
            return View();
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Create()
        {
            ViewData["CourseTitle"] = new SelectList(enrollmentService.CourseRepository.FindAll(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(enrollmentService.StudentRepository.FindAll().ToList(), "Cnp", "Cnp");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EnrollmentViewModel enrollmentModel)
        {
            if (ModelState.IsValid)
            {
                enrollmentService.CreateEnrollment(enrollmentModel);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseTitle"] = new SelectList(enrollmentService.CourseRepository.FindAll(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(enrollmentService.StudentRepository.FindAll(), "Cnp", "Cnp");
            return View(enrollmentModel);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Delete()
        {
            ViewData["CourseTitle"] = new SelectList(enrollmentService.CourseRepository.FindAll(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(enrollmentService.StudentRepository.FindAll(), "Cnp", "Cnp");
            return View();
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(DeleteEnrollmentViewModel model)
        {
            enrollmentService.DeleteEnrollment(model);
            return RedirectToAction("Home", "Secretaries");
        }

        // MOVE THIS TO A SERVICE
        public IActionResult GetEnrolledStudentName(string courseTitle, string studentCnp)
        {
            var course = enrollmentService.CourseRepository.FindByCondition(c => String.Equals(c.CourseTitle, courseTitle)).FirstOrDefault();
            var student = enrollmentService.StudentRepository.FindByCondition(s => String.Equals(s.Cnp, studentCnp)).FirstOrDefault();
            var studentName = "student not enrolled to selected course...";
            if(course != null && student != null)
            {
                var enrollment = enrollmentService.EnrollmentRepository.FindByCondition(e => (e.CourseId == course.CourseId) && (e.StudentId == student.StudentId)).FirstOrDefault();
                if(enrollment != null)
                {
                    studentName = student.LastName + " " + student.FirstName;
                }
            }
            return Json(studentName);
        }
    }
}

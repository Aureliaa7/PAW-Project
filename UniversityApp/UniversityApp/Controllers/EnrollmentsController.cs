using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Core;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService enrollmentService;
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;


        public EnrollmentsController(
            IEnrollmentService enrollmentService,
            IStudentService studentService,
            ICourseService courseService)
        {
            this.enrollmentService = enrollmentService;
            this.studentService = studentService;
            this.courseService = courseService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            var enrollments = await enrollmentService.GetAllEnrollmentsAsync();
            if (enrollments != null)
            {
                return View(enrollments);
            }
            return View();
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Create()
        {
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAsync(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel enrollmentModel)
        {
            if (ModelState.IsValid)
            {
                await enrollmentService.CreateEnrollmentAsync(enrollmentModel);
                return RedirectToAction("Home", "Secretaries");
            }
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAsync(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View(enrollmentModel);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete()
        {
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAsync(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View();
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteEnrollmentViewModel model)
        {
            await enrollmentService.DeleteEnrollmentAsync(model);
            return RedirectToAction("Home", "Secretaries");
        }

        // MOVE THIS TO A SERVICE
        public async Task<IActionResult> GetEnrolledStudentName(string courseTitle, string studentCnp)
        {
            var course = await courseService.GetFirstOrDefaultAsync(c => String.Equals(c.CourseTitle, courseTitle));
            var student = (await studentService.GetAsync(s => String.Equals(s.Cnp, studentCnp))).FirstOrDefault();
            var studentName = "student not enrolled to selected course...";
            if(course != null && student != null)
            {
                var enrollment = await enrollmentService.GetFirstOrDefaultAsync(e => (e.CourseId == course.Id) && (e.StudentId == student.Id));
                if(enrollment != null)
                {
                    studentName = student.LastName + " " + student.FirstName;
                }
            }
            return Json(studentName);
        }
    }
}

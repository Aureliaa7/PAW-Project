using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public EnrollmentsController(
            IEnrollmentService enrollmentService,
            IStudentService studentService,
            ICourseService courseService,
            IMapper mapper)
        {
            this.enrollmentService = enrollmentService;
            this.studentService = studentService;
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            var enrollments = await enrollmentService.GetAllEnrollmentsAsync();
            return View(enrollments);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Create()
        {
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
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
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View(enrollmentModel);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Delete()
        {
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
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

        public async Task<IActionResult> GetEnrolledStudentName(string courseTitle, string studentCnp)
        {
            var student = await studentService.GetEnrolledStudentAsync(courseTitle, studentCnp);

            if (student != null)
            {
                return Json($"{student.LastName} {student.FirstName}");
            }
            return Json("student not enrolled to selected course...");
        }
    }
}

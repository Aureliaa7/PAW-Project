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
            return View(enrollments);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Create()
        {
            ViewData["CourseTitle"] = new SelectList((await courseService.GetAllAsync()).OrderBy(x => x.CourseTitle), "Id", "CourseTitle");
            ViewData["StudentNames"] = new SelectList((await studentService.GetAsync()).OrderBy(x => x.FullName), "Cnp", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEnrollmentViewModel enrollmentModel)
        {
            if (ModelState.IsValid)
            {
                await enrollmentService.CreateEnrollmentAsync(enrollmentModel);
                return RedirectToAction("Index", "Enrollments");
            }
            ViewData["CourseTitle"] = new SelectList(await courseService.GetAllAsync(), "CourseTitle", "CourseTitle");
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "FullName");
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

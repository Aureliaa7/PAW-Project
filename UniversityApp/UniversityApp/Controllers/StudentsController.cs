using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Presentation.Controllers;

namespace UniversityApp.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentService studentService;
        private SignInManager<User> signManager;
        private UserManager<User> userManager;
        private IImageService imageService;
        private IMapper mapper;

        public StudentsController(
            IStudentService studentService, 
            SignInManager<User> signManager, 
            UserManager<User> userManager, 
            IImageService imageService,
            IMapper mapper)
        {
            this.studentService = studentService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View((await studentService.GetAsync()).OrderBy(s => s.StudyYear));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await GetStudentView(id);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        [HttpPost]
        public async Task<IActionResult> Create(StudentRegistrationViewModel student)
        {
            var mappedStudent = mapper.Map<Student>(student);
            mappedStudent.Image = imageService.GetBytes(student.Image);

            try
            {
                await studentService.AddAsync(mappedStudent, student.Password);

                return RedirectToAction("Index", "Students");
            }
            catch (FailedUserRegistrationException ex)
            {
                //TODO display these messages
            }
            return View(student);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await GetStudentView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,FirstName,LastName,Cnp,PhoneNumber,Email,StudyYear,Section,GroupName")] Student student)
        {
            if (id != student.Id)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedStudent = await studentService.UpdateAsync(student);
                    return RedirectToAction(nameof(Index)); //TODO redirect to Details page
                }
                catch (EntityNotFoundException)
                {
                    return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
                }
            }
            return View(student);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await GetStudentView(id);
        }

        public async Task<IActionResult> DeleteSelectedStudent()
        {
            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSelectedStudent(DeleteStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await studentService.DeleteAsync(model);
                return RedirectToAction("Home", "Secretaries");
            }

            ViewData["StudentCnp"] = new SelectList(await studentService.GetAsync(), "Cnp", "Cnp");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {        
            await studentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetStudentToBeDeleted(int studyYear, string sectionName, string cnp)
        {
            var studentName = "student not found";
            var searchedStudent = await studentService.GetFirstOrDefaultAsync(s => s.StudyYear == studyYear
                && s.Section == sectionName && s.Cnp == cnp);
            if (searchedStudent != null)
            {
                studentName = searchedStudent.LastName + " " + searchedStudent.FirstName;
            }
            return Json(studentName);
        }

        [Authorize(Roles = Constants.StudentRole)]
        public async Task<IActionResult> Home()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            User applicationUser =  await userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;
            if(applicationUser != null)
            {
                // search the student based on his user id
                var student = await studentService.GetFirstOrDefaultAsync(s => userId == s.Id.ToString());
                
                if (student != null)
                {
                    return View(student);
                }
            }
            return View();
        }

        [Authorize(Roles = Constants.StudentRole)]
        public async Task<IActionResult> Grades([FromServices] IGradeService gradeService, [FromServices] ILoggedInUserService findUserService)
        {
            var userId = findUserService.GetCurrentUserId();
            if (userId != null)
            {
                var student = await studentService.GetFirstOrDefaultAsync(s => userId == s.Id.ToString());
                var grades = await gradeService.GetGradesForStudentAsync(student.Id);
                return View(grades);
            }
            return RedirectToAction("Home","Students");
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> EnrolledStudents(Guid courseId, [FromServices] ILoggedInUserService userService, [FromServices] IEnrollmentService enrollmentService)
        {
            var currentTeacherId = userService.GetCurrentUserId();
            var enrolledStudents = await enrollmentService.GetEnrolledStudentsByCourseAndTeacherIdAsync(new Guid(currentTeacherId), courseId);
            return View(enrolledStudents);
        }

        private async Task<IActionResult> GetStudentView(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            var student = await studentService.GetFirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            return View(student);
        }
    }
}

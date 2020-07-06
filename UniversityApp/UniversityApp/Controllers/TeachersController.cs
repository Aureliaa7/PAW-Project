using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService teacherService;
        private SignInManager<Users> signManager;
        private UserManager<Users> userManager;
        private readonly IUserService userService;
        public TeachersController(ITeacherService teacherService, SignInManager<Users> signManager, UserManager<Users> userManager, IUserService userService)
        {
            this.teacherService = teacherService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Index()
        {
            return View(teacherService.TeacherRepository.FindAll().Include(t => t.User).ToList().OrderBy(t => t.LastName));
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> Image, TeacherRegistrationViewModel teacherModel)
        {
            var user = userService.CreateUser(Image, String.Concat(teacherModel.LastName, teacherModel.FirstName), teacherModel.Email, teacherModel.PhoneNumber);
            var result = await userManager.CreateAsync(user, teacherModel.Password);
            if (result.Succeeded)
            {
                teacherModel.Image = user.Image;
                teacherService.RegisterTeacher(teacherModel, user.Id);
                await userManager.AddToRoleAsync(user, teacherModel.Role);
                return RedirectToAction("Create", "TeachedCourses");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(teacherModel);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = teacherService.TeacherRepository.FindByCondition(t => t.TeacherId == id).First();

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TeacherId,FirstName,LastName,PhoneNumber,Email,Cnp,UserId,Degree")] Teachers teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                teacherService.TeacherRepository.Update(teacher);
                var teacherFound = teacherService.TeacherRepository.FindByCondition(t => t.TeacherId == teacher.TeacherId);
                if (teacherFound == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = teacherService.TeacherRepository.FindByCondition(t => t.TeacherId == id).FirstOrDefault();
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var teacher = teacherService.TeacherRepository.FindByCondition(t => t.TeacherId == id).First();
            var user = teacherService.UserRepository.FindByCondition(u => String.Equals(u.Id, teacher.UserId)).First();
            teacherService.TeacherRepository.Delete(teacher);
            teacherService.UserRepository.Delete(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetCourses(int id, [FromServices] ITeachedCourseService teachedCourseService)
        {
            var courses = teachedCourseService.GetTeachedCourses(id);
            return View(courses);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Home([FromServices]IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                // search the student based on his user id
                var teacher = teacherService.TeacherRepository.FindByCondition(s => String.Equals(userId, s.UserId)).FirstOrDefault();

                if (teacher != null)
                {
                    return View(teacher);
                }
            }
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult TeachedCourses([FromServices]ITeachedCourseService teachedCourseService)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = teacherService.TeacherRepository.FindByCondition(t => String.Equals(t.UserId, userId)).FirstOrDefault();
            int teacherId = teacher.TeacherId;
            var courses = teachedCourseService.GetTeachedCourses(teacherId);
            return View(courses);
        }

        public IActionResult GetStudents(int id, [FromServices] ICourseService courseService, [FromServices] IEnrollmentService enrollmentService)
        {
            var students = courseService.GetEnrolledStudents(id);
            var studentsToBeReturned = new List<EnrolledStudentViewModel>();
            foreach(var student in students)
            {
                var enrollment = enrollmentService.EnrollmentRepository.FindByCondition(e => e.StudentId == student.StudentId && e.CourseId == id).FirstOrDefault();
                studentsToBeReturned.Add(new EnrolledStudentViewModel
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    CNP = student.Cnp,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    Section = student.Section,
                    GroupName = student.GroupName,
                    EnrollmentID = enrollment.EnrollmentId
                });
            }
            return View(studentsToBeReturned);
        }
    }
}

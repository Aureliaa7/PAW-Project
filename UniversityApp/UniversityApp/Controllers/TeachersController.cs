﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService teacherService;
        private SignInManager<User> signManager;
        private UserManager<User> userManager;
        private readonly IUserService userService;
        public TeachersController(
            ITeacherService teacherService, 
            SignInManager<User> signManager, 
            UserManager<User> userManager, 
            IUserService userService)
        {
            this.teacherService = teacherService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View((await teacherService.GetAsync()).ToList().OrderBy(t => t.LastName));
        }

        [Authorize(Roles = Constants.SecretaryRole)]
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
                await teacherService.AddAsync(teacherModel, user.Id);
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

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TeacherId,FirstName,LastName,PhoneNumber,Email,Cnp,UserId,Degree")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await teacherService.UpdateAsync(teacher);
                var teacherFound = await teacherService.GetFirstOrDefaultAsync(t => t.Id == teacher.Id);
                if (teacherFound == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            var user = (await userService.GetAsync(u => String.Equals(u.Id, teacher.Id))).FirstOrDefault();
            await teacherService.DeleteAsync(teacher.Id);
            await userService.DeleteAsync(teacher.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetCourses(Guid id, [FromServices] ITeachedCourseService teachedCourseService)
        {
            var courses = await teachedCourseService.GetTeachedCoursesAsync(id);
            return View(courses);
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> Home([FromServices]IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                var teacher = await teacherService.GetFirstOrDefaultAsync(s => String.Equals(userId, s.Id));

                if (teacher != null)
                {
                    return View(teacher);
                }
            }
            return View();
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> TeachedCourses([FromServices]ITeachedCourseService teachedCourseService)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => String.Equals(t.Id, userId));
            Guid teacherId = teacher.Id;
            var courses = await teachedCourseService.GetTeachedCoursesAsync(teacherId);
            return View(courses);
        }

        public async Task<IActionResult> GetStudents(Guid id, [FromServices] ICourseService courseService, [FromServices] IEnrollmentService enrollmentService)
        {
            var students = await courseService.GetEnrolledStudents(id);
            var studentsToBeReturned = new List<EnrolledStudentViewModel>();
            foreach(var student in students)
            {
                var enrollment = await enrollmentService.GetFirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == id);
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

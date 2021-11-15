using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Presentation.Controllers;

namespace UniversityApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly SignInManager<User> signManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public TeachersController(
            ITeacherService teacherService, 
            SignInManager<User> signManager, 
            UserManager<User> userManager,
            IMapper mapper,
            IImageService imageService)
        {
            this.teacherService = teacherService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.imageService = imageService;
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
        public async Task<IActionResult> Create(TeacherRegistrationViewModel teacherModel)
        {
            var mappedTeacher = mapper.Map<Teacher>(teacherModel);
            mappedTeacher.Image = imageService.GetBytes(teacherModel.Image);

            try
            {
                await teacherService.AddAsync(mappedTeacher, teacherModel.Password);

                return RedirectToAction("Index", "Teachers");
            }
            catch (FailedUserRegistrationException ex)
            {
                //TODO display these messages
            }
            return View(teacherModel);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await GetTeacherView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,PhoneNumber,Email,Cnp,UserId,Degree")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                await teacherService.UpdateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await GetTeacherView(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            await teacherService.DeleteAsync(teacher.Id);
            return RedirectToAction(nameof(Index));
        }

        //this does not belong here.
        //TODO check if is called from a js file and if not, delete it
        public async Task<IActionResult> GetCourses(Guid id, [FromServices] ITeachedCourseService teachedCourseService)
        {
            var courses = await teachedCourseService.GetTeachedCoursesAsync(id);
            return View(mapper.Map<IEnumerable<CourseDto>>(courses));
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
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await teachedCourseService.GetTeachedCoursesAsync(new Guid(teacherId));
            return View(mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        //TODO move this to StudentsController
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

        private async Task<IActionResult> GetTeacherView(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            var teacher = await teacherService.GetFirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            return View(teacher);
        }
    }
}

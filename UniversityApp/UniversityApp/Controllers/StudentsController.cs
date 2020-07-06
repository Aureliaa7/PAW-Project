using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentService studentService;
        private SignInManager<Users> signManager;
        private UserManager<Users> userManager;
        private IUserService userService;
        public StudentsController(IStudentService studentService, SignInManager<Users> signManager, UserManager<Users> userManager, IUserService userService)
        {
            this.studentService = studentService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Index()
        {
            return View(studentService.StudentRepository.FindAll().ToList().OrderBy(s => s.StudyYear));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = studentService.StudentRepository.FindByCondition(s => s.StudentId == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [Authorize(Roles="Secretary")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Secretary")]
        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> Image, StudentRegistrationViewModel student)
        {
            var user = userService.CreateUser(Image, String.Concat(student.LastName, student.FirstName), student.Email, student.PhoneNumber);
            if(user != null)
            {
                var result = await userManager.CreateAsync(user, student.Password);

                if (result.Succeeded)
                {
                    student.Image = user.Image;
                    await userManager.AddToRoleAsync(user, student.Role);
                    studentService.RegisterStudent(student, user.Id);

                    return RedirectToAction("Create", "Enrollments");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            

            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = studentService.StudentRepository.FindByCondition(s => s.StudentId == id).First();

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("StudentId,UserId,FirstName,LastName,Cnp,PhoneNumber,Email,StudyYear,Section,GroupName")] Students student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                studentService.StudentRepository.Update(student);
                // WHY DO I DO THIS?!
                var studentFound = studentService.StudentRepository.FindByCondition(s => s.StudentId == student.StudentId);
                if (studentFound == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var students = studentService.StudentRepository.FindByCondition(s => s.StudentId == id).FirstOrDefault();
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
        }

        public IActionResult DeleteSelectedStudent()
        {
            ViewData["StudentCnp"] = new SelectList(studentService.StudentRepository.FindAll(), "Cnp", "Cnp");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteSelectedStudent(DeleteStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                studentService.DeleteSelectedStudent(model);
                return RedirectToAction("Home", "Secretaries");
            }

            ViewData["StudentCnp"] = new SelectList(studentService.StudentRepository.FindAll(), "Cnp", "Cnp");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = studentService.StudentRepository.FindByCondition(s => s.StudentId == id).First();
            var user = studentService.UserRepository.FindByCondition(u => String.Equals(u.Id, student.UserId)).First();
        
            studentService.StudentRepository.Delete(student);
            studentService.UserRepository.Delete(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetStudentToBeDeleted(int studyYear, string sectionName, string cnp)
        {
            var studentName = "student not found";
            var searchedStudent = studentService.StudentRepository.FindByCondition(s => (s.StudyYear == studyYear)
            && (String.Equals(s.Section, sectionName)) && (String.Equals(s.Cnp, cnp))).FirstOrDefault();
            if (searchedStudent != null)
            {
                studentName = searchedStudent.LastName + " " + searchedStudent.FirstName;
            }
            return Json(studentName);
        }

        [Authorize(Roles ="Student")]
        public async Task<IActionResult> Home()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            Users applicationUser =  await userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;
            if(applicationUser != null)
            {
                // search the student based on his user id
                var student = studentService.StudentRepository.FindByCondition(s => String.Equals(userId, s.UserId)).FirstOrDefault();
                
                if (student != null)
                {
                    return View(student);
                }
            }
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Grades([FromServices] IGradeService gradeService, [FromServices] IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                var student = studentService.StudentRepository.FindByCondition(s => String.Equals(userId, s.UserId)).FirstOrDefault();
                var grades = gradeService.GetGradesForStudent(student.StudentId);
                return View(grades);
            }
            return RedirectToAction("Home","Students");
    }
    }
}

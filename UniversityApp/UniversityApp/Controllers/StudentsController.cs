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
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentService studentService;
        private SignInManager<User> signManager;
        private UserManager<User> userManager;
        private IUserService userService;

        public StudentsController(
            IStudentService studentService, 
            SignInManager<User> signManager, 
            UserManager<User> userManager, 
            IUserService userService)
        {
            this.studentService = studentService;
            this.signManager = signManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Index()
        {
            return View((await studentService.GetAsync()).OrderBy(s => s.StudyYear));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await studentService.GetFirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> Image, StudentRegistrationViewModel student)
        {
            var user = userService.CreateUser(Image, String.Concat(student.LastName, student.FirstName), student.Email, student.PhoneNumber);
            user.Cnp = student.Cnp;
            if(user != null)
            {
                var result = await userManager.CreateAsync(user, student.Password);

                if (result.Succeeded)
                {
                    student.Image = user.Image;
                    await userManager.AddToRoleAsync(user, student.Role);
                    await studentService.AddAsync(student, user.Id);

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

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = (await studentService.GetAsync(s => s.Id == id)).FirstOrDefault();

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StudentId,UserId,FirstName,LastName,Cnp,PhoneNumber,Email,StudyYear,Section,GroupName")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await studentService.UpdateAsync(student); ;
                // WHY DO I DO THIS?!
                // IDK
                var studentFound = (await studentService.GetAsync(s => s.Id == student.Id)).FirstOrDefault();
                if (studentFound == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = Constants.SecretaryRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var students = (await studentService.GetAsync(s => s.Id == id)).FirstOrDefault();
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
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
            var student = (await studentService.GetAsync(s => s.Id == id)).FirstOrDefault();
            var user = (await userService.GetAsync(u => String.Equals(u.Id, student.Id))).FirstOrDefault();
        
            await studentService.DeleteAsync(student.Id);
            await userService.DeleteAsync(user.Id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetStudentToBeDeleted(int studyYear, string sectionName, string cnp)
        {
            var studentName = "student not found";
            var searchedStudent = (await studentService.GetAsync(s => (s.StudyYear == studyYear)
            && (String.Equals(s.Section, sectionName)) && (String.Equals(s.Cnp, cnp)))).FirstOrDefault();
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
                var student = (await studentService.GetAsync(s => String.Equals(userId, s.Id))).FirstOrDefault();
                
                if (student != null)
                {
                    return View(student);
                }
            }
            return View();
        }

        [Authorize(Roles = Constants.StudentRole)]
        public async Task<IActionResult> Grades([FromServices] IGradeService gradeService, [FromServices] IFindLoggedInUser findUserService)
        {
            var userId = findUserService.GetIdLoggedInUser();
            if (userId != null)
            {
                var student = (await studentService.GetAsync(s => String.Equals(userId, s.Id))).FirstOrDefault();
                var grades = gradeService.GetGradesForStudentAsync(student.Id);
                return View(grades);
            }
            return RedirectToAction("Home","Students");
    }
    }
}

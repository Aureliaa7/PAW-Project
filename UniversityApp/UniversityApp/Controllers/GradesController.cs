using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeService gradeService;
        private readonly IEnrollmentService enrollmentService;

        public GradesController(IGradeService gradeService, IEnrollmentService enrollmentService)
        {
            this.gradeService = gradeService;
            this.enrollmentService = enrollmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await gradeService.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = await gradeService.GetFirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create()
        {
            ViewData["EnrollmentId"] = new SelectList(
                await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradeId,EnrollmentId,Value,Date")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                await gradeService.AddAsync(grade);
                return RedirectToAction("Home", "Teachers");
            }
            ViewData["EnrollmentId"] = new SelectList(await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = await gradeService.GetFirstOrDefaultAsync(g => g.GradeId == id);

            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GradeId,EnrollmentId,Value,Date")] Grade grade)
        {
            if (id != grade.GradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await gradeService.UpdateAsync(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await GradesExistsAsync(grade.GradeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentId"] = new SelectList(await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = await gradeService.GetFirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await gradeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GradesExistsAsync(Guid id)
        {
            var grade = await gradeService.GetFirstOrDefaultAsync(g => g.GradeId == id);

            if (grade != null)
            {
                return true;
            }

            return false;
        }
    }
}

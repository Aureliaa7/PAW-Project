using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Presentation.Controllers;

namespace UniversityApp.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeService gradeService;
        private readonly IEnrollmentService enrollmentService;
        private readonly IMapper mapper;

        public GradesController(
            IGradeService gradeService,
            IEnrollmentService enrollmentService,
            IMapper mapper)
        {
            this.gradeService = gradeService;
            this.enrollmentService = enrollmentService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<IEnumerable<GradeDto>>(await gradeService.GetAllAsync()));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await ReturnGradeView(id);
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> Create()
        {
            ViewData["EnrollmentId"] = new SelectList(
                await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradeId,EnrollmentId,Value,Date")] GradeDto grade)
        {
            if (ModelState.IsValid)
            {
                await gradeService.AddAsync(mapper.Map<Grade>(grade));
                return RedirectToAction("Home", "Teachers");
            }
            ViewData["EnrollmentId"] = new SelectList(await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await ReturnGradeView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GradeId,EnrollmentId,Value,Date")] GradeDto grade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await gradeService.UpdateAsync(mapper.Map<Grade>(grade));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await gradeService.ExistsAsync(g => g.GradeId == id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentId"] = new SelectList(
                await enrollmentService.GetAllEnrollmentsAsync(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = Constants.TeacherRole)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await ReturnGradeView(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await gradeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ReturnGradeView(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            var grade = await gradeService.GetFirstOrDefaultAsync(g => g.GradeId == id);

            if (grade == null)
            {
                return RedirectToAction(nameof(ErrorsController.EntityNotFound), "Errors");
            }
            return View(mapper.Map<GradeDto>(grade));
        }
    }
}

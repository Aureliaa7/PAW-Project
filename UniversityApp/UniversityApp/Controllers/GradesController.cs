using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Interfaces;
using UniversityApp.Models;

namespace UniversityApp.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeService gradeService;

        public GradesController(IGradeService gradeService)
        {
            this.gradeService = gradeService;
        }

        public IActionResult Index()
        {
            return View(gradeService.GradeRepository.FindAll().ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = gradeService.GradeRepository.FindByCondition(g => g.GradeId == id).FirstOrDefault();
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["EnrollmentId"] = new SelectList(gradeService.EnrollmentRepository.FindAll(), "EnrollmentId", "EnrollmentId");
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("GradeId,EnrollmentId,Value,Date")] Grades grade)
        {
            if (ModelState.IsValid)
            {
                gradeService.GradeRepository.Create(grade);
                return RedirectToAction("Home", "Teachers");
            }
            ViewData["EnrollmentId"] = new SelectList(gradeService.EnrollmentRepository.FindAll(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = gradeService.GradeRepository.FindByCondition(g => g.GradeId == id).First();

            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("GradeId,EnrollmentId,Value,Date")] Grades grade)
        {
            if (id != grade.GradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    gradeService.GradeRepository.Update(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradesExists(grade.GradeId))
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
            ViewData["EnrollmentId"] = new SelectList(gradeService.EnrollmentRepository.FindAll(), "EnrollmentId", "EnrollmentId", grade.EnrollmentId);
            return View(grade);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = gradeService.GradeRepository.FindByCondition(g => g.GradeId == id).FirstOrDefault();
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var grade = gradeService.GradeRepository.FindByCondition(g => g.GradeId == id).First();
            gradeService.GradeRepository.Delete(grade);
            return RedirectToAction(nameof(Index));
        }

        private bool GradesExists(int id)
        {
            //return _context.Grades.Any(e => e.GradeId == id);
            return gradeService.GradeRepository.FindByCondition(g => g.GradeId == id).Any();
        }
    }
}

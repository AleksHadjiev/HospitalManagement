using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models;

namespace HospitalManagement.Controllers
{
    public class PatientsController : BaseController
    {
        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var hospitalManagement_DbContext = _context.Patients.Include(p => p.Department).Include(p => p.Doctor);

            ViewBag.PatientNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DepartmentSortParm = sortOrder == "department" ? "department_desc" : "department";

            var patients = from p in this._context.Patients
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients
                    .Where(p => p.FullName.Contains(searchString) || p.Department.DepartmentName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(m => m.FullName);
                    break;
                case "department":
                    patients = patients.OrderBy(m => m.Department);
                    break;
                case "department_desc":
                    patients = patients.OrderByDescending(m => m.Department);
                    break;
                default:
                    patients = patients.OrderBy(m => m.FullName);
                    break;
            }

            return View(patients.Include(p => p.Department).Include(p => p.Doctor));
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Department)
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            if (this.IsAdminUser())
            {
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName");
                ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartmentId,DoctorId,FullName,Age,PersonalId,EntryDate,Insurance")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.Id = Guid.NewGuid();
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", patient.DepartmentId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", patient.DoctorId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", patient.DepartmentId);
                ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", patient.DoctorId);
                return View(patient);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DepartmentId,DoctorId,FullName,Age,PersonalId,EntryDate,Insurance")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", patient.DepartmentId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", patient.DoctorId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var patient = await _context.Patients
                    .Include(p => p.Department)
                    .Include(p => p.Doctor)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (patient == null)
                {
                    return NotFound();
                }

                return View(patient);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}

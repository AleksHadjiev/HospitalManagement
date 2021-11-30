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
    public class DoctorsController : BaseController
    {
        // GET: Doctors
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.DoctorNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SpecializationSortParm = sortOrder == "specialization" ? "specialization_desc" : "specialization";

            var doctors = from d in this._context.Doctors
                          select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors
                    .Where(d => d.FullName.Contains(searchString) || d.Specialization.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(m => m.FullName);
                    break;
                case "specialization":
                    doctors = doctors.OrderBy(m => m.Specialization);
                    break;
                case "specialization_desc":
                    doctors = doctors.OrderByDescending(m => m.Specialization);
                    break;
                default:
                    doctors = doctors.OrderBy(m => m.FullName);
                    break;
            }

            return View(doctors);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            if (this.IsAdminUser())
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Age,PersonalId,Specialization,ExperienceStartDate")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.Id = Guid.NewGuid();
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                {
                    return NotFound();
                }
                return View(doctor);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FullName,Age,PersonalId,Specialization,ExperienceStartDate")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (doctor == null)
                {
                    return NotFound();
                }

                return View(doctor);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor.Patients.Count < 1)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            // if doctors have patients delete is not possible and the user is redirected back to the index
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(Guid id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}

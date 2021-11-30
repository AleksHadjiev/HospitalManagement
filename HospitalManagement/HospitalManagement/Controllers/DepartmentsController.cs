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
    public class DepartmentsController : BaseController
    {
        // GET: Departments
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.DepartmentNameSortParm = String.IsNullOrEmpty(sortOrder) ? "departmentName_desc" : "";
            ViewBag.CapacitySortParm = sortOrder == "capacity" ? "capacity_desc" : "capacity";

            var departments = from d in this._context.Departments
                              select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments
                    .Where(d => d.DepartmentName.Contains(searchString) || d.Capacity.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "departmentName_desc":
                    departments = departments.OrderByDescending(m => m.DepartmentName);
                    break;
                case "capacity":
                    departments = departments.OrderBy(m => m.Capacity);
                    break;
                case "capacity_desc":
                    departments = departments.OrderByDescending(m => m.Capacity);
                    break;
                default:
                    departments = departments.OrderBy(m => m.DepartmentName);
                    break;
            }

            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            if (this.IsAdminUser())
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartmentName,Capacity,DepartmentNumber,HeadNurse,NursesCount")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.Id = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return View(department);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DepartmentName,Capacity,DepartmentNumber,HeadNurse,NursesCount")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (this.IsAdminUser())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var department = await _context.Departments
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (department == null)
                {
                    return NotFound();
                }

                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            var patients = this._context.Patients.Where(x => x.DepartmentId == id);
            if (patients.ToList().Count < 1)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            // if department have patients delete is not possible and the user is redirected back to the index
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(Guid id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}

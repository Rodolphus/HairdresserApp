using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HairdresserApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly HairdresserAppContext _context;
        public EmployeesController(HairdresserAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employee = await _context.Employees.Include(l => l.Location).ToListAsync();
            return View(employee);
        }

        public IActionResult Create()
        {
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, LocationId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, LocationId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}

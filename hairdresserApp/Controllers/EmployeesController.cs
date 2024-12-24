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
            var employee = await _context.Employees
                                        .Include(l => l.Location)
                                        .Include(es => es.EmployeeServices)
                                        .ThenInclude(s => s.Service)
                                        .ToListAsync();
            return View(employee);
        }

        public IActionResult Create()
        {
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["Services"] = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, LocationId")] Employee employee, int[] selectedServices)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                if (selectedServices.Length > 0)
                {
                    foreach (var serviceId in selectedServices)
                    {
                        _context.EmployeeServices.Add(new EmployeeService{EmployeeId = employee.Id, ServiceId = serviceId});
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["Services"] = _context.Services.ToList();
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                                        .Include(es => es.EmployeeServices)
                                        .ThenInclude(s => s.Service)
                                        .FirstOrDefaultAsync(x => x.Id == id);

            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["Services"] = _context.Services.ToList();
            ViewData["SelectedServices"] = employee.EmployeeServices.Select(s => s.ServiceId).ToList();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, LocationId")] Employee employee, int[] selectedServices)
        {
            if (ModelState.IsValid)
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();

                var existingServices = _context.EmployeeServices.Where(es => es.EmployeeId == id).ToList();
                _context.EmployeeServices.RemoveRange(existingServices);
                await _context.SaveChangesAsync();

                if (selectedServices.Length > 0)
                {
                    foreach (var serviceId in selectedServices)
                    {
                        _context.EmployeeServices.Add(new EmployeeService { EmployeeId = employee.Id, ServiceId = serviceId });
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["Services"] = _context.Services.ToList();
            ViewData["SelectedServices"] = selectedServices;
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
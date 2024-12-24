using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HairdresserApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly HairdresserAppContext _context;
        public ServicesController(HairdresserAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var service = await _context.Services
                                        .Include(es => es.EmployeeServices)
                                        .ThenInclude(e => e.Employee)
                                        .ToListAsync();
            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price, ProcessTimeInMinutes")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, ProcessTimeInMinutes")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Update(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }

}
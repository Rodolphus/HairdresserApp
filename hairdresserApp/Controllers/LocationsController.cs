using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HairdresserApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly HairdresserAppContext _context;
        public LocationsController(HairdresserAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var location = await _context.Locations.Include(e => e.Employees).ToListAsync();
            return View(location);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Address, OpeningTime, ClosingTime")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Address, OpeningTime, ClosingTime")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }

}
using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HairdresserApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly HairdresserAppContext _context;
        public AppointmentsController(HairdresserAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointment = await _context.Appointments
                                            .Include(u => u.User)
                                            .Include(e => e.Employee)
                                            .Include(l => l.Location)
                                            .Include(s => s.Service).ToListAsync();
            return View(appointment);
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "FirstName", "LastName", "Email");
            ViewData["Services"] = new SelectList(_context.Services, "Id", "Name", "Price", "ProcessTimeInMinutes");
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name", "Address");
            return View();
        }

        

    }
}

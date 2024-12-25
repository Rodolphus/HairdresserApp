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
                                        .Include(s => s.Service)
                                        .Include(e => e.Employee)
                                        .ThenInclude(e => e.Location)
                                        .Include(u => u.User)
                                        .ToListAsync();

            return View(appointment);
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users
                .Select(u => new { u.Id, Info = u.FirstName + " " + u.LastName + ", " + u.Email }),
                "Id",
                "Info"
            );
            ViewData["Services"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, ServiceId, EmployeeId, UserId, AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.Now;
                appointment.Confirmed = false;

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Users"] = new SelectList(_context.Users
                .Select(u => new { u.Id, Info = u.FirstName + " " + u.LastName + ", " + u.Email }),
                "Id",
                "Info"
            );
            ViewData["Services"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var appointment = await _context.Appointments.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);
            return View(appointment);
        }

        [HttpPost, ActionName("Confirmation")]
        public async Task<IActionResult> ConfirmationConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                if(appointment.Confirmed == false) 
                    appointment.Confirmed = true;
                else
                    appointment.Confirmed = false;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);
            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments
                                            .Include(s => s.Service)
                                            .Include(e => e.Employee)
                                            .ThenInclude(e => e.Location)
                                            .Include(u => u.User)
                                            .FirstOrDefaultAsync(x => x.Id == id);

            ViewData["Users"] = new SelectList(_context.Users
                .Select(u => new { u.Id, Info = u.FirstName + " " + u.LastName + ", " + u.Email }),
                "Id",
                "Info"
            );
            ViewData["Services"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, ServiceId, EmployeeId, UserId, AppointmentDate")] Appointment appointment)
        {  
            if (ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.Now;
                appointment.Confirmed = false;
                _context.Update(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Users"] = new SelectList(_context.Users
                .Select(u => new { u.Id, Info = u.FirstName + " " + u.LastName + ", " + u.Email }),
                "Id",
                "Info"
            );
            ViewData["Services"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["Locations"] = new SelectList(_context.Locations, "Id", "Name");

            return View(appointment);
        }


    }

}

using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HairdresserApp.Controllers
{
    [Route("api/appointments")]
    public class AppointmentsApiController : Controller
    {
        private readonly HairdresserAppContext _context;
        public AppointmentsApiController(HairdresserAppContext context)
        {
            _context = context;
        }

        [HttpGet("available-times/{employeeId}/{serviceId}/{selectedDate}")]
        public async Task<IActionResult> GetAvailableTimes(int employeeId, int serviceId, DateTime selectedDate)
        {
            var employee = await _context.Employees
                                   .Include(e => e.Location)
                                   .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return NotFound(new { Message = "Çalışan bilgisi bulunamadı." });

            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound(new { Message = "Hizmet bilgisi bulunamadı." });

            var openingTime = employee.Location.OpeningTime;
            var closingTime = employee.Location.ClosingTime;

            var existingAppointments = await _context.Appointments
                                        .Where(a => a.EmployeeId == employeeId && a.AppointmentDate.Date == selectedDate.Date)
                                        .Select(a => new
                                        {
                                            StartTime = a.AppointmentDate.TimeOfDay,
                                            EndTime = a.AppointmentDate.TimeOfDay.Add(TimeSpan.FromMinutes(a.Service.ProcessTimeInMinutes))
                                        })
                                        .ToListAsync();

            var availableSlots = new List<object>();

            for (var time = openingTime; time.Add(TimeSpan.FromMinutes(service.ProcessTimeInMinutes)) <= closingTime; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                var endTime = time.Add(TimeSpan.FromMinutes(service.ProcessTimeInMinutes));

                bool isAvailable = !existingAppointments.Any(appt =>
                    (time >= appt.StartTime && time < appt.EndTime) ||          // Yeni randevu mevcut bir randevuya başlıyor
                    (endTime > appt.StartTime && endTime <= appt.EndTime) ||    // Yeni randevu mevcut bir randevunun içinde bitiyor
                    (time <= appt.StartTime && endTime >= appt.EndTime)         // Yeni randevu mevcut bir randevuyu kapsıyor
                ); 

                if (isAvailable)
                {
                    availableSlots.Add(new
                    {
                        StartTime = time,
                        EndTime = endTime
                    });
                }
            }

            return Ok(availableSlots);
        }

    }
}

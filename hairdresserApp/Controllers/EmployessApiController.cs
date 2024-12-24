using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;
using HairdresserApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HairdresserApp.Controllers
{
    [Route("api/employees")]
    public class EmployeesApiController : Controller
    {
        private readonly HairdresserAppContext _context;
        public EmployeesApiController(HairdresserAppContext context)
        {
            _context = context;
        }

        [HttpGet("by-location-and-service/{locationId}/{serviceId}")]
        public async Task<IActionResult> GetEmployeesByLocationAndService(int locationId, int serviceId)
        {
            var employees = await _context.Employees
                                        .Include(es => es.EmployeeServices)
                                        .Where(e => e.LocationId == locationId && e.EmployeeServices.Any(s => s.ServiceId == serviceId))
                                        .Select(e => new { e.Id, e.FirstName, e.LastName })
                                        .ToListAsync();

            if (!employees.Any())
                return NotFound(new { Message = "Seçili şubede seçili hizmeti veren çalışan bulunamadı." });

            return Ok(employees);
        }

    }
}

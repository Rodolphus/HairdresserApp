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

        [HttpGet("by-location/{locationId}")]
        public async Task<IActionResult> GetEmployeesByLocation(int locationId)
        {
            var employees = await _context.Employees
                                          .Where(e => e.LocationId == locationId)
                                          .Select(e => new { e.Id, e.FirstName, e.LastName })
                                          .ToListAsync();

            if (!employees.Any())
                return NotFound(new { Message = "Bu şubeye ait çalışan bulunamadı." });

            return Ok(employees);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using HairdresserApp.Models;

namespace HairdresserApp.Controllers
{
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            var location = new Location() { Name = "Kartal Şubesi" };
            return View(location);
        }


    }
}

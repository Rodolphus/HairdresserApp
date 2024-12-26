using HairdresserApp.Areas.Identity.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairdresserApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public HairdresserAppUser? User { get; set; }
        public bool Confirmed { get; set; }

    }
}

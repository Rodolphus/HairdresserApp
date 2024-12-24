using System.ComponentModel.DataAnnotations.Schema;

namespace HairdresserApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
        public List<EmployeeService>? EmployeeServices { get; set; }
    }
}
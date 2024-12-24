namespace HairdresserApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int ProcessTimeInMinutes { get; set; }
        public List<EmployeeService>? EmployeeServices { get; set; }
    }
}
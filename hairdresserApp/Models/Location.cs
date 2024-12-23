namespace HairdresserApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}

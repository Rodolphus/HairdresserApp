using Microsoft.EntityFrameworkCore;

namespace HairdresserApp.Data
{
    public class HairdresserAppContext : DbContext
    {
        public HairdresserAppContext(DbContextOptions<HairdresserAppContext> options) : base(options) { }


    }
}

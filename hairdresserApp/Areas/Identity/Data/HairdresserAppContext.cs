using HairdresserApp.Areas.Identity.Data;
using HairdresserApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HairdresserApp.Data;

public class HairdresserAppContext : IdentityDbContext<HairdresserAppUser>
{
    public HairdresserAppContext(DbContextOptions<HairdresserAppContext> options)
        : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

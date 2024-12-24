using HairdresserApp.Areas.Identity.Data;
using HairdresserApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HairdresserApp.Data;

public class HairdresserAppContext : IdentityDbContext<HairdresserAppUser>
{
    public HairdresserAppContext(DbContextOptions<HairdresserAppContext> options)
        : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeService> EmployeeServices { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Employees)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull); //Might be Cascade

        builder.Entity<EmployeeService>().HasKey(es => new { es.EmployeeId, es.ServiceId });

        builder.Entity<EmployeeService>()
            .HasOne(e => e.Employee)
            .WithMany(es => es.EmployeeServices)
            .HasForeignKey(e => e.EmployeeId);

        builder.Entity<EmployeeService>()
            .HasOne(s => s.Service)
            .WithMany(es => es.EmployeeServices)
            .HasForeignKey(s => s.ServiceId);

    }
}

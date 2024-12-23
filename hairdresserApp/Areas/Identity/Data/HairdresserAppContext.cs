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
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Employees)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Appointment>()
            .HasOne(a => a.Location)
            .WithMany()
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Appointment>()
            .HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);


    }
}

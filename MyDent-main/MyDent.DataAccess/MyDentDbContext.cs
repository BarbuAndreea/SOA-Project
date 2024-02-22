using Microsoft.EntityFrameworkCore;
using MyDent.Domain.Models;

namespace MyDent.DataAccess
{
    public class MyDentDbContext : DbContext
    {
        public MyDentDbContext(DbContextOptions<MyDentDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Medic> Medics { get; set; }

        public DbSet<Intervention> Interventions { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<ClinicAdmin> ClinicAdmins { get; set;}

        public DbSet<Radiography> Radiographies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.PersonalCode).IsUnique();
        }
    }
}

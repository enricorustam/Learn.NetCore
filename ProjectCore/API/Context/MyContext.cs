using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserRole>().HasKey(sc => new { sc.UserId, sc.RoleId });
            modelBuilder.Entity<UserRole>().HasKey(sc => sc.UserId);
            modelBuilder.Entity<Employee>().HasKey(sc => sc.EmpId);

            modelBuilder.Entity<UserRole>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.userRoles)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<Employee>()
                .HasOne<User>(sc => sc.User)
                .WithOne(s => s.Employee)
                .HasForeignKey<Employee>(sc => sc.EmpId);

            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}

using Learn.NetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.NetCore.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(UR => new { UR.UserId, UR.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne<User>(UR => UR.Users)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(UR => UR.UserId);


            modelBuilder.Entity<UserRole>()
                .HasOne<Role>(UR => UR.Roles)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(UR => UR.RoleId);
        }
    }
}

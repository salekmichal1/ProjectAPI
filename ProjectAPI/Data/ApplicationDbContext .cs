using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; } 

        /// <summary>
        /// Dodawania użytkowników przy tworzeni bazy
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "Kubek",
                    Price = 10.00,
                    Quantity = 10,
                    Available = true
                },
                new Product()
                {
                    Id = 2,
                    Name = "Dlugopis",
                    Price = 20.00,
                    Quantity = 0,
                    Available = false
                });
        }
    }
}

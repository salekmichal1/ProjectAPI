using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAPIAuth.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectAPIAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 

        /// <summary>
        /// Dodawania użytkowników przy tworzeni bazy
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

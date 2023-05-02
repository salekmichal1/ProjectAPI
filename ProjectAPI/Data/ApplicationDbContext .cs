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
        public DbSet<Produkt> Produkts { get; set; }
        public DbSet<Urzytkownik> Urzytkownicy { get; set; } 

        /// <summary>
        /// Dodawania użytkowników przy tworzeni bazy
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produkt>().HasData(
                new Produkt()
                {
                    Id = 1,
                    Nazwa = "Kubek",
                    Cena = 10.00,
                    Ilosc = 10,
                    Dostepny = true
                },
                new Produkt()
                {
                    Id = 2,
                    Nazwa = "Dlugopis",
                    Cena = 20.00,
                    Ilosc = 0,
                    Dostepny = false
                });
        }
    }
}

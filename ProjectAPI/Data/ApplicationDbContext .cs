using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Produkt> Produkts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

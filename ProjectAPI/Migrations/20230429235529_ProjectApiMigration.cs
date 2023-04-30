using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProjectApiMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produkts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    Dostepny = table.Column<bool>(type: "bit", nullable: false),
                    Utworzony = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Zaktualizowany = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produkts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Produkts",
                columns: new[] { "Id", "Cena", "Dostepny", "Ilosc", "Nazwa", "Utworzony", "Zaktualizowany" },
                values: new object[,]
                {
                    { 1, 10.0, true, 10, "Kubek", null, null },
                    { 2, 20.0, false, 0, "Dlugopis", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produkts");
        }
    }
}

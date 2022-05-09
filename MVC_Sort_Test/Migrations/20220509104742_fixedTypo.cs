using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Sort_Test.Migrations
{
    public partial class fixedTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrigonalCSV",
                table: "SortEntry",
                newName: "OriginalCSV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalCSV",
                table: "SortEntry",
                newName: "OrigonalCSV");
        }
    }
}

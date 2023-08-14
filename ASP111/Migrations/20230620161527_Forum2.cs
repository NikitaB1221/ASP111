using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP111.Migrations
{
    /// <inheritdoc />
    public partial class Forum2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDt",
                schema: "asp111",
                table: "Sections",
                newName: "CreateDt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDt",
                schema: "asp111",
                table: "Sections",
                newName: "CreatedDt");
        }
    }
}

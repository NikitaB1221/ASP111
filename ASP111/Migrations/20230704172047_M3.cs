using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP111.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Topics_AuthorId",
                schema: "asp111",
                table: "Topics",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_AuthorId",
                schema: "asp111",
                table: "Sections",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Users_AuthorId",
                schema: "asp111",
                table: "Sections",
                column: "AuthorId",
                principalSchema: "asp111",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Users_AuthorId",
                schema: "asp111",
                table: "Topics",
                column: "AuthorId",
                principalSchema: "asp111",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Users_AuthorId",
                schema: "asp111",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_AuthorId",
                schema: "asp111",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_AuthorId",
                schema: "asp111",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Sections_AuthorId",
                schema: "asp111",
                table: "Sections");
        }
    }
}

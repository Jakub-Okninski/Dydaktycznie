using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorIdToQuizm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresentationFile",
                table: "Presentations");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Presentations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Presentations");

            migrationBuilder.AddColumn<byte[]>(
                name: "PresentationFile",
                table: "Presentations",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}

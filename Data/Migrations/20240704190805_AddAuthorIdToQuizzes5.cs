using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorIdToQuizzes5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PresentationFile",
                table: "Presentations",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresentationFile",
                table: "Presentations");
        }
    }
}

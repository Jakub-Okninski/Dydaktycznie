using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class ss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorID",
                table: "Presentations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorID",
                table: "Presentations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

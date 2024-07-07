using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorIdToQuizzes1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorID",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_AuthorID",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Quizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorID",
                table: "Quizzes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AuthorID",
                table: "Quizzes",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorID",
                table: "Quizzes",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

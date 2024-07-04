using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class userquiz1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Quizzes",
                newName: "AuthorID");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes",
                newName: "IX_Quizzes_AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorID",
                table: "Quizzes",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorID",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Quizzes",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_AuthorID",
                table: "Quizzes",
                newName: "IX_Quizzes_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AuthorId",
                table: "Quizzes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

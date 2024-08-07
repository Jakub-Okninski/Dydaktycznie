﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dydaktycznie.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorIdToQuizm324234dsada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorID",
                table: "Presentations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Presentations_AuthorID",
                table: "Presentations",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_AspNetUsers_AuthorID",
                table: "Presentations");

            migrationBuilder.DropIndex(
                name: "IX_Presentations_AuthorID",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Presentations");
        }
    }
}

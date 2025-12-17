using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

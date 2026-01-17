using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllowDeleteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

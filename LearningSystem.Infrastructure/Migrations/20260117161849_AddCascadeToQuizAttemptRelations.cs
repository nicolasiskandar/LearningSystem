using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeToQuizAttemptRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonCompleteds_AspNetUsers_UserId",
                table: "LessonCompleteds");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonCompleteds_AspNetUsers_UserId",
                table: "LessonCompleteds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonCompleteds_AspNetUsers_UserId",
                table: "LessonCompleteds");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonCompleteds_AspNetUsers_UserId",
                table: "LessonCompleteds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParentLessonRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentLessonId",
                table: "Lessons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ParentLessonId",
                table: "Lessons",
                column: "ParentLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_ParentLessonId",
                table: "Lessons",
                column: "ParentLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_ParentLessonId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ParentLessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ParentLessonId",
                table: "Lessons");
        }
    }
}

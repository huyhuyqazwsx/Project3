using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LessonCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CategoryId",
                table: "Lessons",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonCategories_CategoryId",
                table: "Lessons",
                column: "CategoryId",
                principalTable: "LessonCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonCategories_CategoryId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "LessonCategories");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CategoryId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Lessons");

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
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToVocabulary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Vocabularies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Vocabularies");
        }
    }
}

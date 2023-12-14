using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaApp.Migrations
{
    /// <inheritdoc />
    public partial class addedPrime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrime",
                table: "productsImage",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrime",
                table: "productsImage");
        }
    }
}

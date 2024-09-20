using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helpme.Migrations
{
    /// <inheritdoc />
    public partial class ImageRemove3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescripcionDonacion",
                table: "Organizaciones");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionDonacion",
                table: "Publicacion",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescripcionDonacion",
                table: "Publicacion");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionDonacion",
                table: "Organizaciones",
                type: "text",
                nullable: true);
        }
    }
}

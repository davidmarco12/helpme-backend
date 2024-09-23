using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace helpme.Migrations
{
    /// <inheritdoc />
    public partial class MercagoPago22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MercadoPagoCode",
                table: "Organizaciones",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MercadoPagoCode",
                table: "Organizaciones");
        }
    }
}

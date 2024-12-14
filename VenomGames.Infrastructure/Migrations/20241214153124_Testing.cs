using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenomGames.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Testing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "GameOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GameOrders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenomGames.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategory_Categories_CategoryId",
                table: "GameCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_GameCategory_Games_GameId",
                table: "GameCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOrder_Games_GameId",
                table: "GameOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOrder_Orders_OrderId",
                table: "GameOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOrder",
                table: "GameOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategory",
                table: "GameCategory");

            migrationBuilder.RenameTable(
                name: "GameOrder",
                newName: "GameOrders");

            migrationBuilder.RenameTable(
                name: "GameCategory",
                newName: "GameCategories");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrder_OrderId",
                table: "GameOrders",
                newName: "IX_GameOrders_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrder_GameId",
                table: "GameOrders",
                newName: "IX_GameOrders_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategory_GameId",
                table: "GameCategories",
                newName: "IX_GameCategories_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategory_CategoryId",
                table: "GameCategories",
                newName: "IX_GameCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOrders",
                table: "GameOrders",
                columns: new[] { "OrderId", "GameId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategories",
                table: "GameCategories",
                columns: new[] { "CategoryId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategories_Categories_CategoryId",
                table: "GameCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategories_Games_GameId",
                table: "GameCategories",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOrders_Games_GameId",
                table: "GameOrders",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOrders_Orders_OrderId",
                table: "GameOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategories_Categories_CategoryId",
                table: "GameCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_GameCategories_Games_GameId",
                table: "GameCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOrders_Games_GameId",
                table: "GameOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOrders_Orders_OrderId",
                table: "GameOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOrders",
                table: "GameOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategories",
                table: "GameCategories");

            migrationBuilder.RenameTable(
                name: "GameOrders",
                newName: "GameOrder");

            migrationBuilder.RenameTable(
                name: "GameCategories",
                newName: "GameCategory");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrders_OrderId",
                table: "GameOrder",
                newName: "IX_GameOrder_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOrders_GameId",
                table: "GameOrder",
                newName: "IX_GameOrder_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategories_GameId",
                table: "GameCategory",
                newName: "IX_GameCategory_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategories_CategoryId",
                table: "GameCategory",
                newName: "IX_GameCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOrder",
                table: "GameOrder",
                columns: new[] { "OrderId", "GameId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategory",
                table: "GameCategory",
                columns: new[] { "CategoryId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategory_Categories_CategoryId",
                table: "GameCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategory_Games_GameId",
                table: "GameCategory",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOrder_Games_GameId",
                table: "GameOrder",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOrder_Orders_OrderId",
                table: "GameOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

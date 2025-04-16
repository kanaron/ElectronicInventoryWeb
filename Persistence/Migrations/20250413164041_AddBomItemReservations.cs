using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBomItemReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0125e9b2-c77b-4c15-a987-73a24296edb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f054527c-2db2-41a8-8fb3-221c26e379f0");

            migrationBuilder.CreateTable(
                name: "BomItemReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BomItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BomItemReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BomItemReservations_BomItems_BomItemId",
                        column: x => x.BomItemId,
                        principalTable: "BomItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BomItemReservations_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d84fdd3-ed16-4870-82fa-3f290c42f47f", null, "User", "USER" },
                    { "9821a475-6db6-45a3-b938-71d929bf72ab", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BomItemReservations_BomItemId",
                table: "BomItemReservations",
                column: "BomItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BomItemReservations_InventoryItemId",
                table: "BomItemReservations",
                column: "InventoryItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BomItemReservations");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d84fdd3-ed16-4870-82fa-3f290c42f47f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9821a475-6db6-45a3-b938-71d929bf72ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0125e9b2-c77b-4c15-a987-73a24296edb0", null, "User", "USER" },
                    { "f054527c-2db2-41a8-8fb3-221c26e379f0", null, "Admin", "ADMIN" }
                });
        }
    }
}

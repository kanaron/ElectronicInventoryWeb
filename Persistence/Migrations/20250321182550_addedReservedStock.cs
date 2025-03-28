using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedReservedStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "099d1e96-c882-4bfb-9402-bc26d31cdb56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e78bb84e-490d-4023-b0cb-59b92989b89e");

            migrationBuilder.AddColumn<int>(
                name: "ReservedForProjects",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a94143a-ab2b-48bd-8bc2-1151d6f01d3d", null, "User", "USER" },
                    { "69a918ed-84a9-4e15-a2e9-f3a7be6ec0a1", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a94143a-ab2b-48bd-8bc2-1151d6f01d3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69a918ed-84a9-4e15-a2e9-f3a7be6ec0a1");

            migrationBuilder.DropColumn(
                name: "ReservedForProjects",
                table: "InventoryItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "099d1e96-c882-4bfb-9402-bc26d31cdb56", null, "Admin", "ADMIN" },
                    { "e78bb84e-490d-4023-b0cb-59b92989b89e", null, "User", "USER" }
                });
        }
    }
}

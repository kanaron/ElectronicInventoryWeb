using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchingInventoryItemIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "073569f4-3b7e-4bae-8b46-43d3fe7a84e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ac3ee02-a7ef-4bcc-afc0-3e942d6b88cd");

            migrationBuilder.AddColumn<string>(
                name: "MatchingInventoryItemIds",
                table: "BomItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "099d1e96-c882-4bfb-9402-bc26d31cdb56", null, "Admin", "ADMIN" },
                    { "e78bb84e-490d-4023-b0cb-59b92989b89e", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "099d1e96-c882-4bfb-9402-bc26d31cdb56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e78bb84e-490d-4023-b0cb-59b92989b89e");

            migrationBuilder.DropColumn(
                name: "MatchingInventoryItemIds",
                table: "BomItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "073569f4-3b7e-4bae-8b46-43d3fe7a84e5", null, "Admin", "ADMIN" },
                    { "5ac3ee02-a7ef-4bcc-afc0-3e942d6b88cd", null, "User", "USER" }
                });
        }
    }
}

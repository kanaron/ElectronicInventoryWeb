using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class normalizedValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbb2053b-7095-45b6-9897-ac5d2e38076e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f63f025a-51e0-47f5-b7b1-e57830d995b2");

            migrationBuilder.AddColumn<string>(
                name: "StandardUnit",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "StandardValue",
                table: "InventoryItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "StandardUnit",
                table: "BomItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "StandardValue",
                table: "BomItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "073569f4-3b7e-4bae-8b46-43d3fe7a84e5", null, "Admin", "ADMIN" },
                    { "5ac3ee02-a7ef-4bcc-afc0-3e942d6b88cd", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "073569f4-3b7e-4bae-8b46-43d3fe7a84e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ac3ee02-a7ef-4bcc-afc0-3e942d6b88cd");

            migrationBuilder.DropColumn(
                name: "StandardUnit",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "StandardValue",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "StandardUnit",
                table: "BomItems");

            migrationBuilder.DropColumn(
                name: "StandardValue",
                table: "BomItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dbb2053b-7095-45b6-9897-ac5d2e38076e", null, "User", "USER" },
                    { "f63f025a-51e0-47f5-b7b1-e57830d995b2", null, "Admin", "ADMIN" }
                });
        }
    }
}

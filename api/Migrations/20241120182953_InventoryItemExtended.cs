using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ElectronicInventoryWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class InventoryItemExtended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83d6437a-1f5c-4958-9884-bef6fabd051a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad2c58a3-7bc5-4c26-82cc-03a197a01476");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "InventoryItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "InventoryItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MinStockLevel",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24b730ed-1cdd-43d2-af8c-c878f1cdbefe", null, "Admin", "ADMIN" },
                    { "b68c8a33-7c64-4198-99f6-1b9b899683de", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24b730ed-1cdd-43d2-af8c-c878f1cdbefe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b68c8a33-7c64-4198-99f6-1b9b899683de");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "InventoryItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83d6437a-1f5c-4958-9884-bef6fabd051a", null, "Admin", "ADMIN" },
                    { "ad2c58a3-7bc5-4c26-82cc-03a197a01476", null, "User", "USER" }
                });
        }
    }
}

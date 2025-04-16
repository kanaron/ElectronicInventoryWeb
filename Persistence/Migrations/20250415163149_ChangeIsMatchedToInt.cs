using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsMatchedToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d84fdd3-ed16-4870-82fa-3f290c42f47f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9821a475-6db6-45a3-b938-71d929bf72ab");

            migrationBuilder.AlterColumn<int>(
                name: "IsMatched",
                table: "BomItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42dec592-84c9-4429-ad09-586f8661e467", null, "User", "USER" },
                    { "a8808e12-3e5f-43d5-ab96-66402235e361", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42dec592-84c9-4429-ad09-586f8661e467");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8808e12-3e5f-43d5-ab96-66402235e361");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMatched",
                table: "BomItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d84fdd3-ed16-4870-82fa-3f290c42f47f", null, "User", "USER" },
                    { "9821a475-6db6-45a3-b938-71d929bf72ab", null, "Admin", "ADMIN" }
                });
        }
    }
}

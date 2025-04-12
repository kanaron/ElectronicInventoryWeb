using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsMatchedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d2e84fe-e344-487f-988a-290c51053fb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7c30c5b-7bbb-4f88-a9a0-ef5200be5b91");

            migrationBuilder.AddColumn<bool>(
                name: "IsMatched",
                table: "BomItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0125e9b2-c77b-4c15-a987-73a24296edb0", null, "User", "USER" },
                    { "f054527c-2db2-41a8-8fb3-221c26e379f0", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0125e9b2-c77b-4c15-a987-73a24296edb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f054527c-2db2-41a8-8fb3-221c26e379f0");

            migrationBuilder.DropColumn(
                name: "IsMatched",
                table: "BomItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d2e84fe-e344-487f-988a-290c51053fb0", null, "User", "USER" },
                    { "c7c30c5b-7bbb-4f88-a9a0-ef5200be5b91", null, "Admin", "ADMIN" }
                });
        }
    }
}

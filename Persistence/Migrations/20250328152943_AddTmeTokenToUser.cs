using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTmeTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a94143a-ab2b-48bd-8bc2-1151d6f01d3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69a918ed-84a9-4e15-a2e9-f3a7be6ec0a1");

            migrationBuilder.AddColumn<string>(
                name: "tmeToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c388f96-e99c-414b-9822-a39940f602ba", null, "User", "USER" },
                    { "9e44495d-72b1-4b95-8713-f90dd17a31ec", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c388f96-e99c-414b-9822-a39940f602ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e44495d-72b1-4b95-8713-f90dd17a31ec");

            migrationBuilder.DropColumn(
                name: "tmeToken",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a94143a-ab2b-48bd-8bc2-1151d6f01d3d", null, "User", "USER" },
                    { "69a918ed-84a9-4e15-a2e9-f3a7be6ec0a1", null, "Admin", "ADMIN" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReferencesToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38180c6a-bf89-4494-8b45-e737d09b6350");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d80eb1d-fcee-446e-810e-2e92aeeeb274");

            migrationBuilder.AlterColumn<string>(
                name: "References",
                table: "BomItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c3cbf48-c1cd-4168-a261-1a4054e38636", null, "User", "USER" },
                    { "64886a01-7742-4fc6-9d8c-fe4b4ffe36d8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c3cbf48-c1cd-4168-a261-1a4054e38636");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64886a01-7742-4fc6-9d8c-fe4b4ffe36d8");

            migrationBuilder.AlterColumn<string>(
                name: "References",
                table: "BomItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38180c6a-bf89-4494-8b45-e737d09b6350", null, "User", "USER" },
                    { "7d80eb1d-fcee-446e-810e-2e92aeeeb274", null, "Admin", "ADMIN" }
                });
        }
    }
}

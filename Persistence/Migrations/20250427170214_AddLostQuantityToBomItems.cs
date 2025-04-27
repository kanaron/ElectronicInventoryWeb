using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLostQuantityToBomItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42dec592-84c9-4429-ad09-586f8661e467");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8808e12-3e5f-43d5-ab96-66402235e361");

            migrationBuilder.AddColumn<int>(
                name: "LostQuantity",
                table: "BomItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38180c6a-bf89-4494-8b45-e737d09b6350", null, "User", "USER" },
                    { "7d80eb1d-fcee-446e-810e-2e92aeeeb274", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38180c6a-bf89-4494-8b45-e737d09b6350");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d80eb1d-fcee-446e-810e-2e92aeeeb274");

            migrationBuilder.DropColumn(
                name: "LostQuantity",
                table: "BomItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42dec592-84c9-4429-ad09-586f8661e467", null, "User", "USER" },
                    { "a8808e12-3e5f-43d5-ab96-66402235e361", null, "Admin", "ADMIN" }
                });
        }
    }
}

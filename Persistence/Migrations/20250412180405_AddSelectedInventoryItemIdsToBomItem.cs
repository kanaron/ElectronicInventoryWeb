using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedInventoryItemIdsToBomItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f427fef-2760-4e82-aa30-818ef52fb532");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "773c2a48-a11a-4238-ae74-14acee0f7d4c");

            migrationBuilder.AddColumn<string>(
                name: "SelectedInventoryItemIds",
                table: "BomItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d2e84fe-e344-487f-988a-290c51053fb0", null, "User", "USER" },
                    { "c7c30c5b-7bbb-4f88-a9a0-ef5200be5b91", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d2e84fe-e344-487f-988a-290c51053fb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7c30c5b-7bbb-4f88-a9a0-ef5200be5b91");

            migrationBuilder.DropColumn(
                name: "SelectedInventoryItemIds",
                table: "BomItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f427fef-2760-4e82-aa30-818ef52fb532", null, "User", "USER" },
                    { "773c2a48-a11a-4238-ae74-14acee0f7d4c", null, "Admin", "ADMIN" }
                });
        }
    }
}

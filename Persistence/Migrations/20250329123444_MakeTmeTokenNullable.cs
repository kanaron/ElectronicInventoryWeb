using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeTmeTokenNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c388f96-e99c-414b-9822-a39940f602ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e44495d-72b1-4b95-8713-f90dd17a31ec");

            migrationBuilder.RenameColumn(
                name: "tmeToken",
                table: "AspNetUsers",
                newName: "TmeToken");

            migrationBuilder.AlterColumn<string>(
                name: "TmeToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f427fef-2760-4e82-aa30-818ef52fb532", null, "User", "USER" },
                    { "773c2a48-a11a-4238-ae74-14acee0f7d4c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f427fef-2760-4e82-aa30-818ef52fb532");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "773c2a48-a11a-4238-ae74-14acee0f7d4c");

            migrationBuilder.RenameColumn(
                name: "TmeToken",
                table: "AspNetUsers",
                newName: "tmeToken");

            migrationBuilder.AlterColumn<string>(
                name: "tmeToken",
                table: "AspNetUsers",
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
                    { "4c388f96-e99c-414b-9822-a39940f602ba", null, "User", "USER" },
                    { "9e44495d-72b1-4b95-8713-f90dd17a31ec", null, "Admin", "ADMIN" }
                });
        }
    }
}

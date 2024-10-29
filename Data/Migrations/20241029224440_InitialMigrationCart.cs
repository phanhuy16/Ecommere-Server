using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10047e8e-e7cd-4447-80c3-4db794baef1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63771023-c1d7-4d8f-a458-46536fbd2dee");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "029c8a67-07a0-434c-8674-7eee637d1954", null, "Admin", "ADMIN" },
                    { "5a56b525-d4cf-4791-b368-9a48002f7a98", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "029c8a67-07a0-434c-8674-7eee637d1954");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a56b525-d4cf-4791-b368-9a48002f7a98");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "carts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10047e8e-e7cd-4447-80c3-4db794baef1b", null, "Admin", "ADMIN" },
                    { "63771023-c1d7-4d8f-a458-46536fbd2dee", null, "User", "USER" }
                });
        }
    }
}

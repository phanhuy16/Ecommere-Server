using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class TableCartImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b339cf3-215c-4d4c-b23a-4b4027769d8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "beb540c1-e285-4832-ae78-b704df67ba5c");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10047e8e-e7cd-4447-80c3-4db794baef1b", null, "Admin", "ADMIN" },
                    { "63771023-c1d7-4d8f-a458-46536fbd2dee", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10047e8e-e7cd-4447-80c3-4db794baef1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63771023-c1d7-4d8f-a458-46536fbd2dee");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "carts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b339cf3-215c-4d4c-b23a-4b4027769d8c", null, "Admin", "ADMIN" },
                    { "beb540c1-e285-4832-ae78-b704df67ba5c", null, "User", "USER" }
                });
        }
    }
}

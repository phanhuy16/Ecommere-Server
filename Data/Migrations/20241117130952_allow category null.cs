using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class allowcategorynull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db46110e-663f-4edd-95fa-8e17d569b309");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e638015f-a0f9-4764-85ae-264b230ac590");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2faa6493-7e2d-4654-ab84-79769ec25578", null, "Admin", "ADMIN" },
                    { "7d6ae6a5-3e97-445f-ba54-92b098cd20e8", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2faa6493-7e2d-4654-ab84-79769ec25578");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d6ae6a5-3e97-445f-ba54-92b098cd20e8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "db46110e-663f-4edd-95fa-8e17d569b309", null, "Admin", "ADMIN" },
                    { "e638015f-a0f9-4764-85ae-264b230ac590", null, "User", "USER" }
                });
        }
    }
}

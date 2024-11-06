using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Editsupplierimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05084348-7a6a-4639-8ced-3f582df294d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "436f28b1-84e2-4f78-8dd5-b41d7ac1c26c");

            migrationBuilder.RenameColumn(
                name: "Imgae",
                table: "Suppliers",
                newName: "Image");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31e13739-a84d-419f-a2c0-9fb7db93c6c8", null, "Admin", "ADMIN" },
                    { "bb360186-d3fd-45ee-8647-5f856e91e940", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31e13739-a84d-419f-a2c0-9fb7db93c6c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb360186-d3fd-45ee-8647-5f856e91e940");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Suppliers",
                newName: "Imgae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05084348-7a6a-4639-8ced-3f582df294d1", null, "User", "USER" },
                    { "436f28b1-84e2-4f78-8dd5-b41d7ac1c26c", null, "Admin", "ADMIN" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addslugpromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31e13739-a84d-419f-a2c0-9fb7db93c6c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb360186-d3fd-45ee-8647-5f856e91e940");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "562c4c0e-119d-422e-be82-51ca8845c016", null, "Admin", "ADMIN" },
                    { "98f8d183-2f22-4d5e-b3b1-efc4d06d1214", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "562c4c0e-119d-422e-be82-51ca8845c016");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98f8d183-2f22-4d5e-b3b1-efc4d06d1214");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Promotions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31e13739-a84d-419f-a2c0-9fb7db93c6c8", null, "Admin", "ADMIN" },
                    { "bb360186-d3fd-45ee-8647-5f856e91e940", null, "User", "USER" }
                });
        }
    }
}

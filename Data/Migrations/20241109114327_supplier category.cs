using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class suppliercategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "562c4c0e-119d-422e-be82-51ca8845c016");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98f8d183-2f22-4d5e-b3b1-efc4d06d1214");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c32607d-0437-47e0-af8b-7079a5f7cb2f", null, "Admin", "ADMIN" },
                    { "b61985af-3c27-4e67-9f96-56578deb4fc3", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c32607d-0437-47e0-af8b-7079a5f7cb2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b61985af-3c27-4e67-9f96-56578deb4fc3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "562c4c0e-119d-422e-be82-51ca8845c016", null, "Admin", "ADMIN" },
                    { "98f8d183-2f22-4d5e-b3b1-efc4d06d1214", null, "User", "USER" }
                });
        }
    }
}

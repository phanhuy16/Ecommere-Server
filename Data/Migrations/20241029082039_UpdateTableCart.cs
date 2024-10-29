using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60483836-e8dc-464d-9222-7008b1cc3152");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8f957a0-2123-4e8e-982c-60bd874819ca");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b71656b-6230-4a91-ab8a-fe67a1536097", null, "Admin", "ADMIN" },
                    { "89bca7be-acd5-464f-9dda-6476b8ea3826", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b71656b-6230-4a91-ab8a-fe67a1536097");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89bca7be-acd5-464f-9dda-6476b8ea3826");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60483836-e8dc-464d-9222-7008b1cc3152", null, "Admin", "ADMIN" },
                    { "d8f957a0-2123-4e8e-982c-60bd874819ca", null, "User", "USER" }
                });
        }
    }
}

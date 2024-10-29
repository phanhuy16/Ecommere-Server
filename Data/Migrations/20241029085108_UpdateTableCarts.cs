using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "b9857418-cbba-4060-a0fb-9d1a1bccb030", null, "Admin", "ADMIN" },
                    { "fed9c766-fd33-40a8-99f2-cfc67aed13ea", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9857418-cbba-4060-a0fb-9d1a1bccb030");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed9c766-fd33-40a8-99f2-cfc67aed13ea");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b71656b-6230-4a91-ab8a-fe67a1536097", null, "Admin", "ADMIN" },
                    { "89bca7be-acd5-464f-9dda-6476b8ea3826", null, "User", "USER" }
                });
        }
    }
}

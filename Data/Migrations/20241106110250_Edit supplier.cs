using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Editsupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f428893-0a4e-4323-888b-8b7ef40f2bcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b230d83e-19b4-4477-8421-ce1dfea476be");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05084348-7a6a-4639-8ced-3f582df294d1", null, "User", "USER" },
                    { "436f28b1-84e2-4f78-8dd5-b41d7ac1c26c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05084348-7a6a-4639-8ced-3f582df294d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "436f28b1-84e2-4f78-8dd5-b41d7ac1c26c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9f428893-0a4e-4323-888b-8b7ef40f2bcf", null, "User", "USER" },
                    { "b230d83e-19b4-4477-8421-ce1dfea476be", null, "Admin", "ADMIN" }
                });
        }
    }
}

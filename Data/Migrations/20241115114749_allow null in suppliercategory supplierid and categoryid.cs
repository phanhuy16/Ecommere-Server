using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class allownullinsuppliercategorysupplieridandcategoryid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d747d7b-a08f-491c-ae11-1abbc1541427");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb36f821-4878-4bf4-ab36-8cc37cf52bf2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3487cf9a-e3e8-486e-aef8-601f72bd78b6", null, "User", "USER" },
                    { "3f029815-10ca-438f-9eae-8ec9030e64cc", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3487cf9a-e3e8-486e-aef8-601f72bd78b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f029815-10ca-438f-9eae-8ec9030e64cc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9d747d7b-a08f-491c-ae11-1abbc1541427", null, "User", "USER" },
                    { "fb36f821-4878-4bf4-ab36-8cc37cf52bf2", null, "Admin", "ADMIN" }
                });
        }
    }
}

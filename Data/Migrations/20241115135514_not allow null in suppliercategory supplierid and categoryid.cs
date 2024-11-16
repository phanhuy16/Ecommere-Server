using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class notallownullinsuppliercategorysupplieridandcategoryid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "079cb971-2603-4597-8f46-1c28d19640ef", null, "Admin", "ADMIN" },
                    { "a1453f68-783d-42c9-b091-17c6c816716a", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "079cb971-2603-4597-8f46-1c28d19640ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1453f68-783d-42c9-b091-17c6c816716a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3487cf9a-e3e8-486e-aef8-601f72bd78b6", null, "User", "USER" },
                    { "3f029815-10ca-438f-9eae-8ec9030e64cc", null, "Admin", "ADMIN" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubProductDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "469335db-997a-4237-b286-14b0118866f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6b9a602-20da-4993-aab5-e9419209e8b5");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "SubProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25722b87-43a0-4d9b-b42a-7d025715e83b", null, "Admin", "ADMIN" },
                    { "b4cedf44-5911-4322-8a45-32dc746297b3", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25722b87-43a0-4d9b-b42a-7d025715e83b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4cedf44-5911-4322-8a45-32dc746297b3");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "SubProducts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "469335db-997a-4237-b286-14b0118866f7", null, "Admin", "ADMIN" },
                    { "e6b9a602-20da-4993-aab5-e9419209e8b5", null, "User", "USER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePromotionNumOfAvaible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "839fcca4-aae1-47ac-9e28-dc48078f4461");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3f6252e-4b23-4294-bc30-8adbc8350ba8");

            migrationBuilder.AddColumn<int>(
                name: "NumOfAvailable",
                table: "Promotion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "469335db-997a-4237-b286-14b0118866f7", null, "Admin", "ADMIN" },
                    { "e6b9a602-20da-4993-aab5-e9419209e8b5", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "469335db-997a-4237-b286-14b0118866f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6b9a602-20da-4993-aab5-e9419209e8b5");

            migrationBuilder.DropColumn(
                name: "NumOfAvailable",
                table: "Promotion");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "839fcca4-aae1-47ac-9e28-dc48078f4461", null, "Admin", "ADMIN" },
                    { "f3f6252e-4b23-4294-bc30-8adbc8350ba8", null, "User", "USER" }
                });
        }
    }
}

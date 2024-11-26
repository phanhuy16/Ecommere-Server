using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Renameisusedinrefreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03456a3d-0ea8-46a1-8caf-00ef2820910a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c76fa2-e509-4010-9a24-404edf174236");

            migrationBuilder.RenameColumn(
                name: "IsUesd",
                table: "RefreshTokens",
                newName: "IsUsed");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7f7e10a5-c8a7-43e6-a2e4-65f72d751314", null, "User", "USER" },
                    { "d5093ae6-22a1-445f-b998-60d2975f8c5f", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f7e10a5-c8a7-43e6-a2e4-65f72d751314");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5093ae6-22a1-445f-b998-60d2975f8c5f");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "RefreshTokens",
                newName: "IsUesd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03456a3d-0ea8-46a1-8caf-00ef2820910a", null, "Admin", "ADMIN" },
                    { "69c76fa2-e509-4010-9a24-404edf174236", null, "User", "USER" }
                });
        }
    }
}

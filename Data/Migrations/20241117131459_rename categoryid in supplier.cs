using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class renamecategoryidinsupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_suppliers_Categories_CategoyId",
                table: "suppliers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "371a24b8-7242-4be7-bde5-f4112288b780");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58174234-a73c-410c-b491-6f37b21e2c06");

            migrationBuilder.RenameColumn(
                name: "CategoyId",
                table: "suppliers",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_suppliers_CategoyId",
                table: "suppliers",
                newName: "IX_suppliers_CategoryId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03456a3d-0ea8-46a1-8caf-00ef2820910a", null, "Admin", "ADMIN" },
                    { "69c76fa2-e509-4010-9a24-404edf174236", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_suppliers_Categories_CategoryId",
                table: "suppliers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_suppliers_Categories_CategoryId",
                table: "suppliers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03456a3d-0ea8-46a1-8caf-00ef2820910a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c76fa2-e509-4010-9a24-404edf174236");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "suppliers",
                newName: "CategoyId");

            migrationBuilder.RenameIndex(
                name: "IX_suppliers_CategoryId",
                table: "suppliers",
                newName: "IX_suppliers_CategoyId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "371a24b8-7242-4be7-bde5-f4112288b780", null, "User", "USER" },
                    { "58174234-a73c-410c-b491-6f37b21e2c06", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_suppliers_Categories_CategoyId",
                table: "suppliers",
                column: "CategoyId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

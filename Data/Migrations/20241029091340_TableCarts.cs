using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class TableCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carts_SubProducts_SubProductId",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_SubProductId",
                table: "carts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9857418-cbba-4060-a0fb-9d1a1bccb030");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed9c766-fd33-40a8-99f2-cfc67aed13ea");

            migrationBuilder.DropColumn(
                name: "SubProductId",
                table: "carts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a87665a-852f-4a40-b680-e47df8defbfb", null, "Admin", "ADMIN" },
                    { "a1a39551-4479-43ef-a687-2b76411dd86d", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a87665a-852f-4a40-b680-e47df8defbfb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1a39551-4479-43ef-a687-2b76411dd86d");

            migrationBuilder.AddColumn<Guid>(
                name: "SubProductId",
                table: "carts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b9857418-cbba-4060-a0fb-9d1a1bccb030", null, "Admin", "ADMIN" },
                    { "fed9c766-fd33-40a8-99f2-cfc67aed13ea", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_carts_SubProductId",
                table: "carts",
                column: "SubProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_carts_SubProducts_SubProductId",
                table: "carts",
                column: "SubProductId",
                principalTable: "SubProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

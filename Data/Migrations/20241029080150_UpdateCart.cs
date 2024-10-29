using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_carts",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Cart_Id",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86388e43-aeff-46aa-b29e-3c3c568d5ac3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8dda805-4389-47a4-977e-f8db91e02ded");

            migrationBuilder.DropColumn(
                name: "Cart_Id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Cart_Id",
                table: "carts",
                newName: "ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "carts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "carts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_carts",
                table: "carts",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "28dcd687-44ec-4b75-aff6-1e9a128ed374", null, "User", "USER" },
                    { "4e85cce1-bb2f-40cb-8007-32fdeb4a101a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_carts_ProductId",
                table: "carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_carts_UserId",
                table: "carts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_carts_AspNetUsers_UserId",
                table: "carts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_carts_Products_ProductId",
                table: "carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carts_AspNetUsers_UserId",
                table: "carts");

            migrationBuilder.DropForeignKey(
                name: "FK_carts_Products_ProductId",
                table: "carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_carts",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_ProductId",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_UserId",
                table: "carts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28dcd687-44ec-4b75-aff6-1e9a128ed374");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e85cce1-bb2f-40cb-8007-32fdeb4a101a");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "carts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "carts",
                newName: "Cart_Id");

            migrationBuilder.AddColumn<Guid>(
                name: "Cart_Id",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_carts",
                table: "carts",
                column: "Cart_Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86388e43-aeff-46aa-b29e-3c3c568d5ac3", null, "User", "USER" },
                    { "d8dda805-4389-47a4-977e-f8db91e02ded", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cart_Id",
                table: "AspNetUsers",
                column: "Cart_Id");

        }
    }
}

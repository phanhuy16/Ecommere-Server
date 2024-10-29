using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_carts_AspNetUsers_User_Id",
            //     table: "carts");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_carts_Cart_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Cart_Id",
                table: "orders");

            // migrationBuilder.DropIndex(
            //     name: "IX_carts_User_Id",
            //     table: "carts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87575734-2d07-4d82-95dd-67036bcb8fcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6a00681-09a0-4a63-bee3-90d450b5601e");

            migrationBuilder.DropColumn(
                name: "Cart_Id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "carts");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "carts",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubProductId",
                table: "carts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Cart_Id",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86388e43-aeff-46aa-b29e-3c3c568d5ac3", null, "User", "USER" },
                    { "d8dda805-4389-47a4-977e-f8db91e02ded", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_carts_SubProductId",
                table: "carts",
                column: "SubProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cart_Id",
                table: "AspNetUsers",
                column: "Cart_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_carts_SubProducts_SubProductId",
                table: "carts",
                column: "SubProductId",
                principalTable: "SubProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_carts_SubProducts_SubProductId",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_SubProductId",
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
                name: "Color",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "SubProductId",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Cart_Id",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "Cart_Id",
                table: "orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "87575734-2d07-4d82-95dd-67036bcb8fcc", null, "User", "USER" },
                    { "a6a00681-09a0-4a63-bee3-90d450b5601e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_Cart_Id",
                table: "orders",
                column: "Cart_Id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_carts_User_Id",
            //     table: "carts",
            //     column: "User_Id",
            //     unique: true);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_carts_AspNetUsers_User_Id",
            //     table: "carts",
            //     column: "User_Id",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_carts_Cart_Id",
                table: "orders",
                column: "Cart_Id",
                principalTable: "carts",
                principalColumn: "Cart_Id");
        }
    }
}

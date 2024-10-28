using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74df34ed-5c4c-47f1-a180-1fbfaeaaed93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6365bb8-6721-4c7c-8dda-93874577cbce");

            migrationBuilder.AlterColumn<Guid>(
                name: "Order_Id",
                table: "SubProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "87575734-2d07-4d82-95dd-67036bcb8fcc", null, "User", "USER" },
                    { "a6a00681-09a0-4a63-bee3-90d450b5601e", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87575734-2d07-4d82-95dd-67036bcb8fcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6a00681-09a0-4a63-bee3-90d450b5601e");

            migrationBuilder.AlterColumn<Guid>(
                name: "Order_Id",
                table: "SubProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "74df34ed-5c4c-47f1-a180-1fbfaeaaed93", null, "Admin", "ADMIN" },
                    { "b6365bb8-6721-4c7c-8dda-93874577cbce", null, "User", "USER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fdcc1f7-47cc-4c5b-85ca-6279a2d01dd1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2b7960c-d54b-40cd-b9ac-494849116304");

            migrationBuilder.AddColumn<string>(
                name: "FisrtName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cd3dab3c-add2-420e-b340-b2cb4a5fb174", null, "Admin", "ADMIN" },
                    { "f81c32a4-0155-4edb-8bf6-ed6dd56fe99d", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd3dab3c-add2-420e-b340-b2cb4a5fb174");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f81c32a4-0155-4edb-8bf6-ed6dd56fe99d");

            migrationBuilder.DropColumn(
                name: "FisrtName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fdcc1f7-47cc-4c5b-85ca-6279a2d01dd1", null, "User", "USER" },
                    { "f2b7960c-d54b-40cd-b9ac-494849116304", null, "Admin", "ADMIN" }
                });
        }
    }
}

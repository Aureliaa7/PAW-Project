using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityApp.Infrastructure.Migrations
{
    public partial class ModifiedEnrollmentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("55cd610d-ab0e-4fe1-a514-d30362885cd1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6d26792c-1787-48c1-8831-7106b8d4bec3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8fa57a30-78d4-478b-a4d6-ab83bf024369"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Enrollments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("41619129-d8e6-4665-852f-8ba5e015aeda"), "2cfdc90e-9765-4825-a5af-cf4a1147af9e", "Secretary", "SECRETARY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("0724619a-6a21-47b9-a6d0-cfdc0701976e"), "5742eacd-41a1-44d5-8b81-19b61ea6f011", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("6be48982-66a3-4824-9b0e-a8d21e3b1aa1"), "ac40922c-cd2c-4f05-85af-55757a82dc27", "Teacher", "TEACHER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0724619a-6a21-47b9-a6d0-cfdc0701976e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("41619129-d8e6-4665-852f-8ba5e015aeda"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6be48982-66a3-4824-9b0e-a8d21e3b1aa1"));

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Enrollments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("6d26792c-1787-48c1-8831-7106b8d4bec3"), "7fdb7ba4-ff0b-428e-bc13-58e9ee14bcd7", "Secretary", "SECRETARY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("55cd610d-ab0e-4fe1-a514-d30362885cd1"), "a07160e1-bcaa-44b3-8974-18f47ddd9a55", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("8fa57a30-78d4-478b-a4d6-ab83bf024369"), "7f772427-48bd-46b6-9e49-b3d66dbca12e", "Teacher", "TEACHER" });
        }
    }
}

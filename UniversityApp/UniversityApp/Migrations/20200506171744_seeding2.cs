using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityApp.Migrations
{
    public partial class seeding2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "fc8d18b3-a0d2-4e2e-85b0-330f9f44c6cd", "SECRETARY" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "bdc1ef6d-b97a-426e-b7e8-312ef7628c4c", "STUDENT" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "02440754-daf5-43c6-8e4b-3bb044103b78", "TEACHER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "e0334422-f6f9-4cad-9ea1-b879f4bc3fbd", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "f223a1b3-01e8-4a89-8ec2-bc157e8b08d4", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "f61754eb-d61c-4457-aa05-255abcc7a4e6", null });
        }
    }
}

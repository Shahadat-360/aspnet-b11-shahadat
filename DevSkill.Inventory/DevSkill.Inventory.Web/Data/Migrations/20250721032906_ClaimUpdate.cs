using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClaimUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRoleAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRoleUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRoleDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRolePage" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserType", "UserTypeAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserType", "UserTypeUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserType", "UserTypeDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserType", "UserTypePage" });
        }
    }
}

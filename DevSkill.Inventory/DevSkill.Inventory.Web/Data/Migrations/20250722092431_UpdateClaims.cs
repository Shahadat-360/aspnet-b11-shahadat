using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "User", "UserAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "User", "UserUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22,
                column: "ClaimValue",
                value: "UserDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23,
                column: "ClaimValue",
                value: "UserPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Category", "CategoryAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Category", "CategoryUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26,
                column: "ClaimValue",
                value: "CategoryDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27,
                column: "ClaimValue",
                value: "CategoryPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Unit", "UnitAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Unit", "UnitUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30,
                column: "ClaimValue",
                value: "UnitDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31,
                column: "ClaimValue",
                value: "UnitPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Department", "DepartmentAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Department", "DepartmentUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34,
                column: "ClaimValue",
                value: "DepartmentDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35,
                column: "ClaimValue",
                value: "DepartmentPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "CashAccount", "CashAccountAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "CashAccount", "CashAccountUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38,
                column: "ClaimValue",
                value: "CashAccountDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39,
                column: "ClaimValue",
                value: "CashAccountPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "BankAccount", "BankAccountAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "BankAccount", "BankAccountUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                column: "ClaimValue",
                value: "BankAccountDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                column: "ClaimValue",
                value: "BankAccountPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "MobileAccount", "MobileAccountAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "MobileAccount", "MobileAccountUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                column: "ClaimValue",
                value: "MobileAccountDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                column: "ClaimValue",
                value: "MobileAccountPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRoleAdd" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "UserRole", "UserRoleUpdate" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                column: "ClaimValue",
                value: "UserRoleDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                column: "ClaimValue",
                value: "UserRolePage");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                column: "ClaimValue",
                value: "UserRoleAssign");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                column: "ClaimValue",
                value: "UserRoleRemove");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Employee", "EmployeeRoleAssign" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Employee", "EmployeeRoleRemove" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22,
                column: "ClaimValue",
                value: "UserAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23,
                column: "ClaimValue",
                value: "UserUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "User", "UserDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "User", "UserPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26,
                column: "ClaimValue",
                value: "CategoryAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27,
                column: "ClaimValue",
                value: "CategoryUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Category", "CategoryDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Category", "CategoryPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30,
                column: "ClaimValue",
                value: "UnitAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31,
                column: "ClaimValue",
                value: "UnitUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Unit", "UnitDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Unit", "UnitPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34,
                column: "ClaimValue",
                value: "DepartmentAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35,
                column: "ClaimValue",
                value: "DepartmentUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Department", "DepartmentDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "Department", "DepartmentPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38,
                column: "ClaimValue",
                value: "CashAccountAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39,
                column: "ClaimValue",
                value: "CashAccountUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "CashAccount", "CashAccountDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "CashAccount", "CashAccountPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42,
                column: "ClaimValue",
                value: "BankAccountAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43,
                column: "ClaimValue",
                value: "BankAccountUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "BankAccount", "BankAccountDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "BankAccount", "BankAccountPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46,
                column: "ClaimValue",
                value: "MobileAccountAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47,
                column: "ClaimValue",
                value: "MobileAccountUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "MobileAccount", "MobileAccountDelete" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ClaimType", "ClaimValue" },
                values: new object[] { "MobileAccount", "MobileAccountPage" });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50,
                column: "ClaimValue",
                value: "UserRoleAdd");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51,
                column: "ClaimValue",
                value: "UserRoleUpdate");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52,
                column: "ClaimValue",
                value: "UserRoleDelete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53,
                column: "ClaimValue",
                value: "UserRolePage");
        }
    }
}

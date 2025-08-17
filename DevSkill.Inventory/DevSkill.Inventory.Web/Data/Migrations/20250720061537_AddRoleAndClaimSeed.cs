using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndClaimSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf"), "10/07/2025 01:01:01 AM", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Customer", "CustomerAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 2, "Customer", "CustomerUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 3, "Customer", "CustomerDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 4, "Customer", "CustomerPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 5, "Product", "ProductAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 6, "Product", "ProductUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 7, "Product", "ProductDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 8, "Product", "ProductPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 9, "Sale", "SaleAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 10, "Sale", "SaleUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 11, "Sale", "SaleDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 12, "Sale", "SalePage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 13, "BalanceTransfer", "BalanceTransferAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 14, "BalanceTransfer", "BalanceTransferDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 15, "BalanceTransfer", "BalanceTransferPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 16, "Staff", "StaffAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 17, "Staff", "StaffUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 18, "Staff", "StaffDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 19, "Staff", "StaffPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 20, "Staff", "StaffRoleAssign", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 21, "Staff", "StaffRoleRemove", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 22, "User", "UserAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 23, "User", "UserUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 24, "User", "UserDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 25, "User", "UserPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 26, "Category", "CategoryAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 27, "Category", "CategoryUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 28, "Category", "CategoryDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 29, "Category", "CategoryPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 30, "Unit", "UnitAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 31, "Unit", "UnitUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 32, "Unit", "UnitDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 33, "Unit", "UnitPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 34, "Department", "DepartmnetAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 35, "Department", "DepartmnetUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 36, "Department", "DepartmnetDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 37, "Department", "DepartmnetPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 38, "CashAccount", "CashAccountAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 39, "CashAccount", "CashAccountUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 40, "CashAccount", "CashAccountDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 41, "CashAccount", "CashAccountPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 42, "BankAccount", "BankAccountAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 43, "BankAccount", "BankAccountUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 44, "BankAccount", "BankAccountDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 45, "BankAccount", "BankAccountPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 46, "MobileAccount", "MobileAccountAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 47, "MobileAccount", "MobileAccountUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 48, "MobileAccount", "MobileAccountDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 49, "MobileAccount", "MobileAccountPage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 50, "UserType", "UserTypeAdd", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 51, "UserType", "UserTypeUpdate", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 52, "UserType", "UserTypeDelete", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 53, "UserType", "UserTypePage", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") },
                    { 54, "AccessSetup", "Allowed", new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM AspNetUserRoles 
                WHERE RoleId = '79149158-e28b-4ec3-b110-98cd62cb58bf'
            ");

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("79149158-e28b-4ec3-b110-98cd62cb58bf"));
        }
    }
}

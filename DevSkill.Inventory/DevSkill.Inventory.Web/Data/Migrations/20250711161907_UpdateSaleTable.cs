using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

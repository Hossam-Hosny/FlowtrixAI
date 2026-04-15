using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowtrixAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInventoryFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStockLevel",
                table: "Inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIncoming",
                table: "Inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalUsed",
                table: "Inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "MinimumStockLevel",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TotalIncoming",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TotalUsed",
                table: "Inventory");
        }
    }
}

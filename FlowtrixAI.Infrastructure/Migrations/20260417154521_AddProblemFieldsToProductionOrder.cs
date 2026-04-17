using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowtrixAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProblemFieldsToProductionOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProblemDescription",
                table: "ProductionOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportedByUserId",
                table: "ProductionOrders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProblemDescription",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "ReportedByUserId",
                table: "ProductionOrders");
        }
    }
}

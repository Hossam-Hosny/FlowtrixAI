using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowtrixAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompleteProductionOrderLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE ProductionOrders SET ReportedByUserId = NULL WHERE ReportedByUserId NOT IN (SELECT Id FROM AspNetUsers)");
            
            migrationBuilder.AddColumn<int>(
                name: "CurrentStepIndex",
                table: "ProductionOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ReportedByUserId",
                table: "ProductionOrders",
                column: "ReportedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_AspNetUsers_ReportedByUserId",
                table: "ProductionOrders",
                column: "ReportedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_AspNetUsers_ReportedByUserId",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_ReportedByUserId",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "CurrentStepIndex",
                table: "ProductionOrders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowtrixAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatIdToAiHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "AiChatHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AiChatHistories");
        }
    }
}

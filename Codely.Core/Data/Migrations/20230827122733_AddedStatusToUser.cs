using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codely.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_status",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_status",
                table: "users");
        }
    }
}

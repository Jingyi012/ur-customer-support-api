using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderForImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "ProjectImages");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "NewsImages");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ProjectImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "NewsImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ProjectImages");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "NewsImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "ProjectImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "NewsImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

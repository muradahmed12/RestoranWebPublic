using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestoranWeb.Migrations
{
    /// <inheritdoc />
    public partial class OrderNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Order");
        }
    }
}

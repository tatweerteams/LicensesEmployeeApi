using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChCount",
                table: "OrderItems");

            migrationBuilder.AddColumn<long>(
                name: "ChCount",
                table: "OrderRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChCount",
                table: "OrderRequests");

            migrationBuilder.AddColumn<long>(
                name: "ChCount",
                table: "OrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

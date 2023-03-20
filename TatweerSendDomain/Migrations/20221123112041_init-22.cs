using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OrderRequestAuthorization",
                table: "BranchSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderRequestAuthorization",
                table: "BranchSettings");
        }
    }
}

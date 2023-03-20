using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "OrderRequests",
                newName: "IdentityNumberBank");

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "OrderRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "OrderRequests");

            migrationBuilder.RenameColumn(
                name: "IdentityNumberBank",
                table: "OrderRequests",
                newName: "ReferenceNumber");
        }
    }
}

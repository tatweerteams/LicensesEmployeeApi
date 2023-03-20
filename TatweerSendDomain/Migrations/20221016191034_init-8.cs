using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Bank",
                table: "BankRegions");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Bank",
                table: "BankRegions",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Bank",
                table: "BankRegions");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Bank",
                table: "BankRegions",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }
    }
}

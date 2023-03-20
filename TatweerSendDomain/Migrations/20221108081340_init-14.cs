using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Work_Times",
                table: "BranchWorkTimes");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Work_Times",
                table: "BranchWorkTimes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Work_Times",
                table: "BranchWorkTimes");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Work_Times",
                table: "BranchWorkTimes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }
    }
}

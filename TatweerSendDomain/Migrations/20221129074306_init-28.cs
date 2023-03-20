using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSends",
                table: "OrderSends");

            migrationBuilder.RenameTable(
                name: "OrderSends",
                newName: "OrderEvents");

            migrationBuilder.RenameIndex(
                name: "IX_OrderSends_OrderRequestId",
                table: "OrderEvents",
                newName: "IX_OrderEvents_OrderRequestId");

            migrationBuilder.AddColumn<int>(
                name: "OrderRequestState",
                table: "OrderEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderEvents",
                table: "OrderEvents",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderEvents",
                table: "OrderEvents");

            migrationBuilder.DropColumn(
                name: "OrderRequestState",
                table: "OrderEvents");

            migrationBuilder.RenameTable(
                name: "OrderEvents",
                newName: "OrderSends");

            migrationBuilder.RenameIndex(
                name: "IX_OrderEvents_OrderRequestId",
                table: "OrderSends",
                newName: "IX_OrderSends_OrderRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSends",
                table: "OrderSends",
                column: "Id");
        }
    }
}

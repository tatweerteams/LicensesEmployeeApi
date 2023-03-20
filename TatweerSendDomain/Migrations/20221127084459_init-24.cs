using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    public partial class init24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderSends_OrderRequestId",
                table: "OrderSends");

            migrationBuilder.RenameColumn(
                name: "OrderSendDate",
                table: "OrderSends",
                newName: "OrderCreationDate");

            migrationBuilder.AddColumn<string>(
                name: "RejectNote",
                table: "OrderSends",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderSends_OrderRequestId",
                table: "OrderSends",
                column: "OrderRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderSends_OrderRequestId",
                table: "OrderSends");

            migrationBuilder.DropColumn(
                name: "RejectNote",
                table: "OrderSends");

            migrationBuilder.RenameColumn(
                name: "OrderCreationDate",
                table: "OrderSends",
                newName: "OrderSendDate");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSends_OrderRequestId",
                table: "OrderSends",
                column: "OrderRequestId",
                unique: true);
        }
    }
}

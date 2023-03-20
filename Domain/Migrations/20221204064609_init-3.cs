using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 4, 8, 46, 9, 146, DateTimeKind.Local).AddTicks(8061),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2022, 12, 4, 8, 42, 24, 143, DateTimeKind.Local).AddTicks(2587));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Roles",
                type: "nvarchar(128)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreateUserId",
                table: "Roles",
                column: "CreateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreateUserId",
                table: "Roles",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreateUserId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreateUserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Roles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 4, 8, 42, 24, 143, DateTimeKind.Local).AddTicks(2587),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2022, 12, 4, 8, 46, 9, 146, DateTimeKind.Local).AddTicks(8061));

            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                table: "Users",
                type: "nvarchar(128)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId1",
                table: "Users",
                column: "RoleId1",
                unique: true,
                filter: "[RoleId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId1",
                table: "Users",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}

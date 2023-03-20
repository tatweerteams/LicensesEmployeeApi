using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 4, 8, 42, 24, 143, DateTimeKind.Local).AddTicks(2587),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2022, 12, 1, 20, 52, 58, 912, DateTimeKind.Local).AddTicks(7833));

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 1, 20, 52, 58, 912, DateTimeKind.Local).AddTicks(7833),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2022, 12, 4, 8, 42, 24, 143, DateTimeKind.Local).AddTicks(2587));
        }
    }
}

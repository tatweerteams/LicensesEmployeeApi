using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 4, 11, 38, 48, 646, DateTimeKind.Local).AddTicks(3650),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2022, 12, 5, 9, 26, 59, 210, DateTimeKind.Local).AddTicks(7069));

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 5, 9, 26, 59, 210, DateTimeKind.Local).AddTicks(7069),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 3, 4, 11, 38, 48, 646, DateTimeKind.Local).AddTicks(3650));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThresholdCondition",
                table: "Sys_ReportPropertyLog",
                newName: "ThresholdType");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCode",
                table: "Sys_ReportPropertyLog",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "Sys_ReportPropertyLog",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceCode",
                table: "Physical_PropertyThreshold",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "Physical_PropertyThreshold",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyCode",
                table: "Physical_PropertyThreshold",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "Sys_ReportPropertyLog");

            migrationBuilder.DropColumn(
                name: "DeviceCode",
                table: "Physical_PropertyThreshold");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "Physical_PropertyThreshold");

            migrationBuilder.DropColumn(
                name: "PropertyCode",
                table: "Physical_PropertyThreshold");

            migrationBuilder.RenameColumn(
                name: "ThresholdType",
                table: "Sys_ReportPropertyLog",
                newName: "ThresholdCondition");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCode",
                table: "Sys_ReportPropertyLog",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}

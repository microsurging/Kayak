using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThresholdValue",
                table: "Sys_ReportPropertyLog",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThresholdValue",
                table: "Sys_ReportPropertyLog");
        }
    }
}

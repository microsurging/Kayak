using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyName",
                table: "Sys_ReportPropertyLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyName",
                table: "Sys_ReportPropertyLog",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

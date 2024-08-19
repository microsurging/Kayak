using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sys_NetworkPart",
                table: "Sys_NetworkPart");

            migrationBuilder.RenameTable(
                name: "Sys_NetworkPart",
                newName: "Sys_OperateLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sys_OperateLog",
                table: "Sys_OperateLog",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sys_OperateLog",
                table: "Sys_OperateLog");

            migrationBuilder.RenameTable(
                name: "Sys_OperateLog",
                newName: "Sys_NetworkPart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sys_NetworkPart",
                table: "Sys_NetworkPart",
                column: "Id");
        }
    }
}

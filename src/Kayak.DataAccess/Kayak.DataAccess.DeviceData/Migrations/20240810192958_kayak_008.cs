using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactUserId",
                table: "SysOrganization");

            migrationBuilder.AddColumn<string>(
                name: "Contacter",
                table: "SysOrganization",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contacter",
                table: "SysOrganization");

            migrationBuilder.AddColumn<int>(
                name: "ContactUserId",
                table: "SysOrganization",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

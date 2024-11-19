using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Component_DeviceAccess");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "Component_DeviceAccess",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "Component_DeviceAccess");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Component_DeviceAccess",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

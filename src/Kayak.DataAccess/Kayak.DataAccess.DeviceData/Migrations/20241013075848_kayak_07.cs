using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentTypeId",
                table: "Component_NetworkPart");

            migrationBuilder.AddColumn<string>(
                name: "ComponentTypeCode",
                table: "Component_NetworkPart",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentTypeCode",
                table: "Component_NetworkPart");

            migrationBuilder.AddColumn<int>(
                name: "ComponentTypeId",
                table: "Component_NetworkPart",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

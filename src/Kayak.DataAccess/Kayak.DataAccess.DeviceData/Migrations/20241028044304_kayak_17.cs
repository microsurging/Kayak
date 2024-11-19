using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Constraint",
                table: "Physical_EventParameter");

            migrationBuilder.AlterColumn<string>(
                name: "Constraint",
                table: "Physical_FunctionParameter",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Constraint",
                table: "Physical_FunctionParameter",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Constraint",
                table: "Physical_EventParameter",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

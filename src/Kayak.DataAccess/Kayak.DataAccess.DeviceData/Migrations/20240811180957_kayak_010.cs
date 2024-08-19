using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SysOrganization",
                table: "SysOrganization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "SysOrganization",
                newName: "Sys_Organization");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "Component_ProductCategory");

            migrationBuilder.AddColumn<bool>(
                name: "IsChildren",
                table: "Component_ProductCategory",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sys_Organization",
                table: "Sys_Organization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Component_ProductCategory",
                table: "Component_ProductCategory",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sys_Organization",
                table: "Sys_Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Component_ProductCategory",
                table: "Component_ProductCategory");

            migrationBuilder.DropColumn(
                name: "IsChildren",
                table: "Component_ProductCategory");

            migrationBuilder.RenameTable(
                name: "Sys_Organization",
                newName: "SysOrganization");

            migrationBuilder.RenameTable(
                name: "Component_ProductCategory",
                newName: "ProductCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SysOrganization",
                table: "SysOrganization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");
        }
    }
}

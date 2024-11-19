using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Component_Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleCode = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleType = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleMode = table.Column<string>(type: "TEXT", nullable: false),
                    FileAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Creater = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Updater = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component_Module", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component_Module");
        }
    }
}

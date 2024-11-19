using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Component_BlackWhiteList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoutePathPattern = table.Column<string>(type: "TEXT", nullable: false),
                    BlackList = table.Column<string>(type: "TEXT", nullable: false),
                    WhiteList = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Creater = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Updater = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component_BlackWhiteList", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component_BlackWhiteList");
        }
    }
}

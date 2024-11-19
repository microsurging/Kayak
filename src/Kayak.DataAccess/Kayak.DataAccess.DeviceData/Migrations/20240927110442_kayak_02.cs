using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Component_DeviceGateway",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GatewayTypeValue = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    NetWorkId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProtocolCode = table.Column<string>(type: "TEXT", nullable: false),
                    Creater = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Updater = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component_DeviceGateway", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component_DeviceGateway");
        }
    }
}

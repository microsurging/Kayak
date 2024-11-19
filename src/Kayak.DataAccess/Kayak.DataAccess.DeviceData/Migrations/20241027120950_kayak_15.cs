using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kayak.DataAccess.DeviceData.Migrations
{
    public partial class kayak_15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Physical_FunctionParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FunctionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FunctionCode = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceCode = table.Column<string>(type: "TEXT", nullable: true),
                    ProductCode = table.Column<string>(type: "TEXT", nullable: false),
                    ParameterType = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Constraint = table.Column<string>(type: "TEXT", nullable: false),
                    DataTypeValue = table.Column<string>(type: "TEXT", nullable: false),
                    Creater = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Updater = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physical_FunctionParameter", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Physical_FunctionParameter");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddedTime = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    SavedTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    CreatorAccount = table.Column<string>(nullable: true),
                    CreatorName = table.Column<string>(nullable: true),
                    OperatorAccount = table.Column<string>(nullable: true),
                    OperatorName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PrivateKey = table.Column<string>(maxLength: 64, nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_User", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddedTime",
                table: "T_User",
                column: "AddedTime")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Number",
                table: "T_User",
                column: "Number",
                unique: true,
                filter: "[Number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateKey",
                table: "T_User",
                column: "PrivateKey",
                unique: true,
                filter: "[PrivateKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_User");
        }
    }
}

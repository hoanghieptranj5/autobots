using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    public partial class AddHanzi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hanzis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HanViet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pinyin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantonese = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stroke = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hanzis", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hanzis");
        }
    }
}

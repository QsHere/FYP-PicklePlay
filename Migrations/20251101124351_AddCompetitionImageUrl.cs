using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP_QS_CODE.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetitionImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "competitionImageUrl",
                table: "schedule",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "competitionImageUrl",
                table: "schedule");
        }
    }
}

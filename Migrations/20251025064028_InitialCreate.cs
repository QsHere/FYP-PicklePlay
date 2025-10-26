using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP_QS_CODE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    gameName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schedule_type = table.Column<int>(type: "int", nullable: true),
                    event_tag = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    num_player = table.Column<int>(type: "int", nullable: true),
                    minRankRestriction = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    maxRankRestriction = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    genderRestriction = table.Column<int>(type: "int", nullable: true),
                    ageGroupRestriction = table.Column<int>(type: "int", nullable: true),
                    feeType = table.Column<int>(type: "int", nullable: true),
                    feeAmount = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    privacy = table.Column<int>(type: "int", nullable: true),
                    gameFeature = table.Column<int>(type: "int", nullable: true),
                    cancellationfreeze = table.Column<int>(type: "int", nullable: true),
                    repeat = table.Column<int>(type: "int", nullable: true),
                    recurringWeek = table.Column<int>(type: "int", nullable: true),
                    autoCreateWhen = table.Column<int>(type: "int", nullable: true),
                    hostrole = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule", x => x.schedule_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "schedule");
        }
    }
}

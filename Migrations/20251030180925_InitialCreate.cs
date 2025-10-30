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
                    parentScheduleId = table.Column<int>(type: "int", nullable: true),
                    recurringEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    gameName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schedule_type = table.Column<int>(type: "int", nullable: true),
                    event_tag = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    endTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    num_player = table.Column<int>(type: "int", nullable: true),
                    num_team = table.Column<int>(type: "int", nullable: true),
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
                    status = table.Column<int>(type: "int", nullable: true),
                    approxStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    regOpen = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    regClose = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    earlyBirdClose = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule", x => x.schedule_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    profile_picture = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    age = table.Column<int>(type: "int", nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    emailVerify = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email_verification_token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email_verified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    verification_token_expiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "competition",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false),
                    format = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    numPool = table.Column<int>(type: "int", nullable: false),
                    winnersPerPool = table.Column<int>(type: "int", nullable: false),
                    thirdPlaceMatch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    doublePool = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    standingCalculation = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    standardWin = table.Column<int>(type: "int", nullable: false),
                    standardLoss = table.Column<int>(type: "int", nullable: false),
                    tieBreakWin = table.Column<int>(type: "int", nullable: false),
                    tieBreakLoss = table.Column<int>(type: "int", nullable: false),
                    draw = table.Column<int>(type: "int", nullable: false),
                    matchRule = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_competition_schedule_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedule",
                        principalColumn: "schedule_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "competition");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "schedule");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP_QS_CODE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRankDefaultsAndRemoveRepeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "repeat",
                table: "schedule");

            migrationBuilder.AlterColumn<decimal>(
                name: "minRankRestriction",
                table: "schedule",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "maxRankRestriction",
                table: "schedule",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "minRankRestriction",
                table: "schedule",
                type: "decimal(5,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "maxRankRestriction",
                table: "schedule",
                type: "decimal(5,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "repeat",
                table: "schedule",
                type: "int",
                nullable: true);
        }
    }
}

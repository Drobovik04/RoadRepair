using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_worktime_repairEventId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes");

            migrationBuilder.AlterColumn<long>(
                name: "RepairEventWorkerId",
                table: "WorkTimes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes",
                column: "RepairEventWorkerId",
                principalTable: "RepairEventWorkers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes");

            migrationBuilder.AlterColumn<long>(
                name: "RepairEventWorkerId",
                table: "WorkTimes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes",
                column: "RepairEventWorkerId",
                principalTable: "RepairEventWorkers",
                principalColumn: "Id");
        }
    }
}

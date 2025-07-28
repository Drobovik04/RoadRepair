using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class uniqueWorkTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_RepairEventWorkerId",
                table: "WorkTimes");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_RepairEventWorkerId_DayOfWork",
                table: "WorkTimes",
                columns: new[] { "RepairEventWorkerId", "DayOfWork" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_RepairEventWorkerId_DayOfWork",
                table: "WorkTimes");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_RepairEventWorkerId",
                table: "WorkTimes",
                column: "RepairEventWorkerId");
        }
    }
}

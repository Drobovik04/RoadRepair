using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moveWorkTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes");

            migrationBuilder.DropTable(
                name: "RepairEventWorkers");

            migrationBuilder.RenameColumn(
                name: "RepairEventWorkerId",
                table: "WorkTimes",
                newName: "WorkAreaWorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkTimes_RepairEventWorkerId_DayOfWork",
                table: "WorkTimes",
                newName: "IX_WorkTimes_WorkAreaWorkerId_DayOfWork");

            migrationBuilder.CreateTable(
                name: "WorkAreaWorkers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkAreaId = table.Column<long>(type: "bigint", nullable: false),
                    WorkerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAreaWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkAreaWorkers_WorkAreas_WorkAreaId",
                        column: x => x.WorkAreaId,
                        principalTable: "WorkAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkAreaWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkAreaWorkers_WorkAreaId",
                table: "WorkAreaWorkers",
                column: "WorkAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAreaWorkers_WorkerId",
                table: "WorkAreaWorkers",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_WorkAreaWorkers_WorkAreaWorkerId",
                table: "WorkTimes",
                column: "WorkAreaWorkerId",
                principalTable: "WorkAreaWorkers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_WorkAreaWorkers_WorkAreaWorkerId",
                table: "WorkTimes");

            migrationBuilder.DropTable(
                name: "WorkAreaWorkers");

            migrationBuilder.RenameColumn(
                name: "WorkAreaWorkerId",
                table: "WorkTimes",
                newName: "RepairEventWorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkTimes_WorkAreaWorkerId_DayOfWork",
                table: "WorkTimes",
                newName: "IX_WorkTimes_RepairEventWorkerId_DayOfWork");

            migrationBuilder.CreateTable(
                name: "RepairEventWorkers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairEventId = table.Column<long>(type: "bigint", nullable: false),
                    WorkerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairEventWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairEventWorkers_RepairEvents_RepairEventId",
                        column: x => x.RepairEventId,
                        principalTable: "RepairEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairEventWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairEventWorkers_RepairEventId",
                table: "RepairEventWorkers",
                column: "RepairEventId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairEventWorkers_WorkerId",
                table: "RepairEventWorkers",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_RepairEventWorkers_RepairEventWorkerId",
                table: "WorkTimes",
                column: "RepairEventWorkerId",
                principalTable: "RepairEventWorkers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

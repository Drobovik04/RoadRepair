using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_typesOfServiceAndOtherTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractorServices_TypeOfServices_TypeOfServiceId",
                table: "ContractorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MeasureTypes_MeasureTypeId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairEvents_TypeOfRepairs_TypeOfRepairId",
                table: "RepairEvents");

            migrationBuilder.DropTable(
                name: "MeasureTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeOfServices",
                table: "TypeOfServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeOfRepairs",
                table: "TypeOfRepairs");

            migrationBuilder.RenameTable(
                name: "TypeOfServices",
                newName: "TypesOfService");

            migrationBuilder.RenameTable(
                name: "TypeOfRepairs",
                newName: "TypesOfRepair");

            migrationBuilder.RenameColumn(
                name: "MeasureTypeId",
                table: "Materials",
                newName: "TypeOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_MeasureTypeId",
                table: "Materials",
                newName: "IX_Materials_TypeOfMeasureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesOfService",
                table: "TypesOfService",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesOfRepair",
                table: "TypesOfRepair",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TypesOfMeasure",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfMeasure", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractorServices_TypesOfService_TypeOfServiceId",
                table: "ContractorServices",
                column: "TypeOfServiceId",
                principalTable: "TypesOfService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_TypesOfMeasure_TypeOfMeasureId",
                table: "Materials",
                column: "TypeOfMeasureId",
                principalTable: "TypesOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairEvents_TypesOfRepair_TypeOfRepairId",
                table: "RepairEvents",
                column: "TypeOfRepairId",
                principalTable: "TypesOfRepair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractorServices_TypesOfService_TypeOfServiceId",
                table: "ContractorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_TypesOfMeasure_TypeOfMeasureId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairEvents_TypesOfRepair_TypeOfRepairId",
                table: "RepairEvents");

            migrationBuilder.DropTable(
                name: "TypesOfMeasure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesOfService",
                table: "TypesOfService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesOfRepair",
                table: "TypesOfRepair");

            migrationBuilder.RenameTable(
                name: "TypesOfService",
                newName: "TypeOfServices");

            migrationBuilder.RenameTable(
                name: "TypesOfRepair",
                newName: "TypeOfRepairs");

            migrationBuilder.RenameColumn(
                name: "TypeOfMeasureId",
                table: "Materials",
                newName: "MeasureTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_TypeOfMeasureId",
                table: "Materials",
                newName: "IX_Materials_MeasureTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeOfServices",
                table: "TypeOfServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeOfRepairs",
                table: "TypeOfRepairs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MeasureTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractorServices_TypeOfServices_TypeOfServiceId",
                table: "ContractorServices",
                column: "TypeOfServiceId",
                principalTable: "TypeOfServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MeasureTypes_MeasureTypeId",
                table: "Materials",
                column: "MeasureTypeId",
                principalTable: "MeasureTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairEvents_TypeOfRepairs_TypeOfRepairId",
                table: "RepairEvents",
                column: "TypeOfRepairId",
                principalTable: "TypeOfRepairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

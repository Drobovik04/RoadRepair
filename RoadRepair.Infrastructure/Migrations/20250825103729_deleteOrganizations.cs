using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteOrganizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Organizations_OrganizationId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAreas_Organizations_OrganizationId",
                table: "WorkAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Organizations_OrganizationId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Workers_OrganizationId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_WorkAreas_OrganizationId",
                table: "WorkAreas");

            migrationBuilder.DropIndex(
                name: "IX_OrgUsers_OrganizationId",
                table: "OrgUsers");

            migrationBuilder.DropIndex(
                name: "IX_Materials_OrganizationId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "WorkAreas");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "OrgUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Materials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "Workers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "WorkAreas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "OrgUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "Materials",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UNP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Organizations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_OrganizationId",
                table: "Workers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAreas_OrganizationId",
                table: "WorkAreas",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUsers_OrganizationId",
                table: "OrgUsers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_OrganizationId",
                table: "Materials",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ParentId",
                table: "Organizations",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Organizations_OrganizationId",
                table: "Materials",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAreas_Organizations_OrganizationId",
                table: "WorkAreas",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Organizations_OrganizationId",
                table: "Workers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

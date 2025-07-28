using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_refs_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUsers_Organizations_OrganizationId",
                table: "OrgUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}

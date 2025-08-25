using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadRepair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeNameOfOrgUsersToUsersInfoAndAddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgUsers",
                table: "OrgUsers");

            migrationBuilder.RenameTable(
                name: "OrgUsers",
                newName: "UsersInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersInfo",
                table: "UsersInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInfo_IdentityId",
                table: "UsersInfo",
                column: "IdentityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInfo_AspNetUsers_IdentityId",
                table: "UsersInfo",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInfo_AspNetUsers_IdentityId",
                table: "UsersInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersInfo",
                table: "UsersInfo");

            migrationBuilder.DropIndex(
                name: "IX_UsersInfo_IdentityId",
                table: "UsersInfo");

            migrationBuilder.RenameTable(
                name: "UsersInfo",
                newName: "OrgUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgUsers",
                table: "OrgUsers",
                column: "Id");
        }
    }
}

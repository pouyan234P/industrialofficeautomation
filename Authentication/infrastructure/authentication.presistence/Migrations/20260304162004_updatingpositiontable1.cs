using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication.presistence.Migrations
{
    /// <inheritdoc />
    public partial class updatingpositiontable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_positions_AspNetUsers_userIDId",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "FK_positions_departments_depIDId",
                table: "positions");

            migrationBuilder.RenameColumn(
                name: "userIDId",
                table: "positions",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "depIDId",
                table: "positions",
                newName: "departmentId");

            migrationBuilder.RenameIndex(
                name: "IX_positions_userIDId",
                table: "positions",
                newName: "IX_positions_userId");

            migrationBuilder.RenameIndex(
                name: "IX_positions_depIDId",
                table: "positions",
                newName: "IX_positions_departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_positions_AspNetUsers_userId",
                table: "positions",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_positions_departments_departmentId",
                table: "positions",
                column: "departmentId",
                principalTable: "departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_positions_AspNetUsers_userId",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "FK_positions_departments_departmentId",
                table: "positions");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "positions",
                newName: "userIDId");

            migrationBuilder.RenameColumn(
                name: "departmentId",
                table: "positions",
                newName: "depIDId");

            migrationBuilder.RenameIndex(
                name: "IX_positions_userId",
                table: "positions",
                newName: "IX_positions_userIDId");

            migrationBuilder.RenameIndex(
                name: "IX_positions_departmentId",
                table: "positions",
                newName: "IX_positions_depIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_positions_AspNetUsers_userIDId",
                table: "positions",
                column: "userIDId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_positions_departments_depIDId",
                table: "positions",
                column: "depIDId",
                principalTable: "departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

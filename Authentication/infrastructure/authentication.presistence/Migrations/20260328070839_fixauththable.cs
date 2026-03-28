using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication.presistence.Migrations
{
    /// <inheritdoc />
    public partial class fixauththable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_signitureimages_signitureimageidId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "signitureimageidId",
                table: "AspNetUsers",
                newName: "SignitureImageId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_signitureimageidId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_SignitureImageId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "signitureimages",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_signitureimages_SignitureImageId",
                table: "AspNetUsers",
                column: "SignitureImageId",
                principalTable: "signitureimages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_signitureimages_SignitureImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SignitureImageId",
                table: "AspNetUsers",
                newName: "signitureimageidId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_SignitureImageId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_signitureimageidId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageData",
                table: "signitureimages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_signitureimages_signitureimageidId",
                table: "AspNetUsers",
                column: "signitureimageidId",
                principalTable: "signitureimages",
                principalColumn: "Id");
        }
    }
}

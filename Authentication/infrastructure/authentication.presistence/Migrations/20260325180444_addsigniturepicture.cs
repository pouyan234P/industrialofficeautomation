using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication.presistence.Migrations
{
    /// <inheritdoc />
    public partial class addsigniturepicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "signitureimageidId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "signitureimages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_signitureimages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_signitureimageidId",
                table: "AspNetUsers",
                column: "signitureimageidId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_signitureimages_signitureimageidId",
                table: "AspNetUsers",
                column: "signitureimageidId",
                principalTable: "signitureimages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_signitureimages_signitureimageidId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "signitureimages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_signitureimageidId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "signitureimageidId",
                table: "AspNetUsers");
        }
    }
}

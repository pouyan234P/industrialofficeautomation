using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceCore.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class addedreplaytothetablecorrespondences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentLetterID",
                table: "letters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyToLetterID",
                table: "letters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_letters_ParentLetterID",
                table: "letters",
                column: "ParentLetterID");

            migrationBuilder.AddForeignKey(
                name: "FK_letters_letters_ParentLetterID",
                table: "letters",
                column: "ParentLetterID",
                principalTable: "letters",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_letters_letters_ParentLetterID",
                table: "letters");

            migrationBuilder.DropIndex(
                name: "IX_letters_ParentLetterID",
                table: "letters");

            migrationBuilder.DropColumn(
                name: "ParentLetterID",
                table: "letters");

            migrationBuilder.DropColumn(
                name: "ReplyToLetterID",
                table: "letters");
        }
    }
}

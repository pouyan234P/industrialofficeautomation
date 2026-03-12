using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceCore.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class fixrelationshipfortables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attachments_letters_LetterID",
                table: "attachments");

            migrationBuilder.DropIndex(
                name: "IX_attachments_LetterID",
                table: "attachments");

            migrationBuilder.DropColumn(
                name: "LetterID",
                table: "attachments");

            migrationBuilder.AddColumn<int>(
                name: "attachmentIDID",
                table: "letters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_letters_attachmentIDID",
                table: "letters",
                column: "attachmentIDID");

            migrationBuilder.AddForeignKey(
                name: "FK_letters_attachments_attachmentIDID",
                table: "letters",
                column: "attachmentIDID",
                principalTable: "attachments",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_letters_attachments_attachmentIDID",
                table: "letters");

            migrationBuilder.DropIndex(
                name: "IX_letters_attachmentIDID",
                table: "letters");

            migrationBuilder.DropColumn(
                name: "attachmentIDID",
                table: "letters");

            migrationBuilder.AddColumn<int>(
                name: "LetterID",
                table: "attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_attachments_LetterID",
                table: "attachments",
                column: "LetterID");

            migrationBuilder.AddForeignKey(
                name: "FK_attachments_letters_LetterID",
                table: "attachments",
                column: "LetterID",
                principalTable: "letters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

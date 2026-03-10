using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceCore.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class changeinttostringforletterNoforcorrespondencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LetterNo",
                table: "letters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LetterNo",
                table: "letters",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}

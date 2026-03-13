using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceCore.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatorpositonidandisdraftforlettertabble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorPositionID",
                table: "letters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "letters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorPositionID",
                table: "letters");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "letters");
        }
    }
}

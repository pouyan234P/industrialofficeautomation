using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceCore.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class intitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "indicatorBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    LastNumber = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Separator = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indicatorBook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "letters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetterNo = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    confidentiality = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    BodyHTMLID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_letters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LetterID = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_attachments_letters_LetterID",
                        column: x => x.LetterID,
                        principalTable: "letters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_attachments_LetterID",
                table: "attachments",
                column: "LetterID");

            migrationBuilder.CreateIndex(
                name: "IX_indicatorBook_FiscalYear_type_DepartmentID",
                table: "indicatorBook",
                columns: new[] { "FiscalYear", "type", "DepartmentID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "indicatorBook");

            migrationBuilder.DropTable(
                name: "letters");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPrintServerVueNet.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterJobsAddPrinter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrinterId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PrinterId",
                table: "Jobs",
                column: "PrinterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_PrinterId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PrinterId",
                table: "Jobs");
        }
    }
}

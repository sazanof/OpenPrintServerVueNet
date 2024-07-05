using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPrintServerVueNet.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeJobPortConsumable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumables_Printers_PrinterId",
                table: "Consumables");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_PrinterPorts_Printers_PrinterId",
                table: "PrinterPorts");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumables_Printers_PrinterId",
                table: "Consumables",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrinterPorts_Printers_PrinterId",
                table: "PrinterPorts",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumables_Printers_PrinterId",
                table: "Consumables");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_PrinterPorts_Printers_PrinterId",
                table: "PrinterPorts");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumables_Printers_PrinterId",
                table: "Consumables",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Printers_PrinterId",
                table: "Jobs",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrinterPorts_Printers_PrinterId",
                table: "PrinterPorts",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");
        }
    }
}

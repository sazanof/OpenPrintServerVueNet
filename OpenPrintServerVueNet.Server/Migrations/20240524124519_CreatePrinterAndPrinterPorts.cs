using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPrintServerVueNet.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreatePrinterAndPrinterPorts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    Direct = table.Column<bool>(type: "bit", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attributes = table.Column<long>(type: "bigint", nullable: false),
                    Capabilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperSizesSupported = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterPaperNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedPrinterStatus = table.Column<int>(type: "int", nullable: false),
                    Hidden = table.Column<bool>(type: "bit", nullable: false),
                    HorizontalResolution = table.Column<long>(type: "bigint", nullable: false),
                    InstallDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<bool>(type: "bit", nullable: false),
                    Network = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterStatus = table.Column<int>(type: "int", nullable: false),
                    PrintJobDataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintProcessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shared = table.Column<bool>(type: "bit", nullable: false),
                    ShareName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerticalResolution = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterPorts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterPorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrinterPorts_Printers_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "Printers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrinterPorts_PrinterId",
                table: "PrinterPorts",
                column: "PrinterId");

            migrationBuilder.CreateIndex(
                name: "IX_Printers_DeviceID",
                table: "Printers",
                column: "DeviceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrinterPorts");

            migrationBuilder.DropTable(
                name: "Printers");
        }
    }
}

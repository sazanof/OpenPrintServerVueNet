using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPrintServerVueNet.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateSnmpTableAndAlterPrintersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperatorMessage",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpContact",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SnmpCountTotal",
                table: "Printers",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SnmpCountUptime",
                table: "Printers",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpFQDN",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpLocation",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpManufacturerOID",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpName",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpSerialNumber",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpSystemName",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnmpUptime",
                table: "Printers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Consumables",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumables_Printers_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "Printers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consumables_PrinterId",
                table: "Consumables",
                column: "PrinterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consumables");

            migrationBuilder.DropColumn(
                name: "OperatorMessage",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpContact",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpCountTotal",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpCountUptime",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpFQDN",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpLocation",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpManufacturerOID",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpName",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpSerialNumber",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpSystemName",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "SnmpUptime",
                table: "Printers");
        }
    }
}

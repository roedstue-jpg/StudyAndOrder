using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyAndOrder.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProducedMaterialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpectedOutcome",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaterialNumber",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "OrderProducedMaterialLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MaterialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedOutcome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducedMaterialLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProducedMaterialLine_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentOrderProducedMaterialLine",
                columns: table => new
                {
                    EquipmentsId = table.Column<int>(type: "int", nullable: false),
                    ProducedMaterialLinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOrderProducedMaterialLine", x => new { x.EquipmentsId, x.ProducedMaterialLinesId });
                    table.ForeignKey(
                        name: "FK_EquipmentOrderProducedMaterialLine_Equipments_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentOrderProducedMaterialLine_OrderProducedMaterialLine_ProducedMaterialLinesId",
                        column: x => x.ProducedMaterialLinesId,
                        principalTable: "OrderProducedMaterialLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOrderProducedMaterialLine_ProducedMaterialLinesId",
                table: "EquipmentOrderProducedMaterialLine",
                column: "ProducedMaterialLinesId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducedMaterialLine_OrderId",
                table: "OrderProducedMaterialLine",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentOrderProducedMaterialLine");

            migrationBuilder.DropTable(
                name: "OrderProducedMaterialLine");

            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedOutcome",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

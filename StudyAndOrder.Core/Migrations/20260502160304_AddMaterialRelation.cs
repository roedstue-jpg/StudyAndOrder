using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyAndOrder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "OrderProducedMaterialLine",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducedMaterialLine_MaterialId",
                table: "OrderProducedMaterialLine",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducedMaterialLine_MaterialId",
                table: "OrderProducedMaterialLine");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "OrderProducedMaterialLine");
        }
    }
}

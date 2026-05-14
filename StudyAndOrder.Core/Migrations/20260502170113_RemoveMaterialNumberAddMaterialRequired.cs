using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyAndOrder.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaterialNumberAddMaterialRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine");

            migrationBuilder.DropColumn(
                name: "MaterialNumber",
                table: "OrderProducedMaterialLine");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "OrderProducedMaterialLine",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "OrderProducedMaterialLine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "MaterialNumber",
                table: "OrderProducedMaterialLine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducedMaterialLine_Materials_MaterialId",
                table: "OrderProducedMaterialLine",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

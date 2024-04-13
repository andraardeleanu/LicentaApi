using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitDb8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_WorkPoints_WorkPointId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WorkPointId",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkPointId",
                table: "Orders",
                column: "WorkPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_WorkPoints_WorkPointId",
                table: "Orders",
                column: "WorkPointId",
                principalTable: "WorkPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

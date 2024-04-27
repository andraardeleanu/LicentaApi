using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class initDb10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "WorkPointId",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Bills",
                newName: "WorkpointName");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "WorkpointName",
                table: "Bills",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkPointId",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

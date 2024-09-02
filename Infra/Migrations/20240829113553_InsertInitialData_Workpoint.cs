using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertInitialData_Workpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "WorkPoints", columns: new[] {
               "Id",
               "Name",
               "Address",
               "CreatedBy",
               "CompanyId",
               "Author",
               "DateCreated",
               "DateUpdated"
             }, values: new object[,] {
               {
                 1,
                 "Workpoint Demo",
                 "Address W Demo",
                 "45953c90-29d1-48db-a173-ea8ab1009f1d",
                 "1",
                 "custone",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 2,
                 "Update Workpoint Test-ABC",
                 "Address W ABC",
                 "4e7b6dc6-b6ef-4f6c-b9ca-c0a132fcd572",
                 "2",
                 "admin",
                 "2024-08-29 22:14:11.0744540",
                 "2024-08-29 22:14:11.0744548",
               }
             });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "WorkPoints", keyColumn: "Id", keyValues: new object[] {
             1,2
            });
        }
    }
}

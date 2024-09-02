using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertInitialData_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Companies", columns: new[] {
               "Id",
               "Name",
               "Cui",
               "Author",
               "DateCreated",
               "DateUpdated",
               "CreatedBy"
             }, values: new object[,] {
               {
                 1,
                 "Company Demo",
                 "1234567",
                 "admin",
                 "2024-08-28 22:14:11.0696742",
                 "2024-08-28 22:14:11.0744496",
                 "4e7b6dc6-b6ef-4f6c-b9ca-c0a132fcd572"
               },
               {
                 2,
                 "Update Company Test-ABC",
                 "3456789",
                 "admin",
                 "2024-08-28 22:14:11.0744540",
                 "2024-08-28 22:14:11.0744548",
                 "4e7b6dc6-b6ef-4f6c-b9ca-c0a132fcd572"
               },
               {
                 3,
                 "No Workpoints Company",
                 "0987654",
                 "admin",
                 "2024-08-28 22:14:11.0744540",
                 "2024-08-28 22:14:11.0744548",
                 "4e7b6dc6-b6ef-4f6c-b9ca-c0a132fcd572"
               }
             });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Companies", keyColumn: "Id", keyValues: new object[] {
             1,
             2,
             3,
            });
        }
    }
}

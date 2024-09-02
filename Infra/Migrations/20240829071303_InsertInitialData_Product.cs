using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertInitialData_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Products", columns: new[] {
               "Id",
               "Name",
               "Price",
               "Author",
               "DateCreated",
               "DateUpdated"
             }, values: new object[,] {
               {
                 1,
                 "Product Demo 1",
                 "100.00",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 2,
                 "Product Demo 2",
                 "50.00",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 3,
                 "Product Demo 3",
                 "25.99",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 4,
                 "Product Demo 4",
                 "55.25",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 5,
                 "Stock Test API",
                 "155.25",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 6,
                 "Update Product Test-ABC",
                 "299.99",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 7,
                 "Lowest Price",
                 "1",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 8,
                 "Highest Price",
                 "999.99",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 9,
                 "Stock Update",
                 "100.00",
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               }
             });

            migrationBuilder.InsertData(table: "Stocks", columns: new[] {
               "Id",
               "AvailableStock",
               "PendingStock",
               "ProductId",
               "Author",
               "DateCreated",
               "DateUpdated"
             }, values: new object[,] {
               {
                 1,
                 5000,
                 100,
                 1,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 2,
                 3000,
                 150,
                 2,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 3,
                 7000,
                 200,
                 3,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 4,
                 300,
                 10,
                 4,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 5,
                 600,
                 80,
                 5,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 6,
                 20,
                 3,
                 6,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 7,
                 578,
                 39,
                 7,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 8,
                 210,
                 55,
                 8,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               },
               {
                 9,
                 77,
                 12,
                 9,
                 "admin",
                 "2024-08-29 22:14:11.0696742",
                 "2024-08-29 22:14:11.0744496",
               }
             });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValues: new object[] {
             1,
             2,
             3,
             4,
             5,
             6,
             7,
             8,
             9
            });

            migrationBuilder.DeleteData(table: "Stocks", keyColumn: "Id", keyValues: new object[] {
             1,
             2,
             3,
             4,
             5,
             6,
             7,
             8,
             9
            });
        }
    }
}

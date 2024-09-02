using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertInitialData_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Orders", columns: new[] {
               "Id",
               "OrderNo",
               "CreatedBy",
               "Status",
               "OrderType",
               "WorkPointId",
               "Author",
               "DateCreated",
               "DateUpdated",
               "TotalPrice"
             }, values: new object[,] {
               {
                 1,
                 "2bf92285-b09b-4e04-a5b3-3dc5c27baadb",
                 "45953c90-29d1-48db-a173-ea8ab1009f1d",
                 "Initializata",
                 0,
                 1,
                 "custone",
                 "2024-08-29 22:15:11.0696742",
                 "2024-08-29 22:15:11.0744496",
                 1277.97
               },
             });

            migrationBuilder.InsertData(table: "OrderProduct", columns: new[] {
               "Id",
               "OrderId",
               "ProductId",
               "Author",
               "DateCreated",
               "DateUpdated",
               "Quantity"
             }, values: new object[,] {
               {
                 1,
                 1,
                 1,
                 "custone",
                 "2024-08-29 22:15:11.0696742",
                 "2024-08-29 22:15:11.0744496",
                 7
               },
             });

            migrationBuilder.InsertData(table: "OrderProduct", columns: new[] {
               "Id",
               "OrderId",
               "ProductId",
               "Author",
               "DateCreated",
               "DateUpdated",
               "Quantity"
             }, values: new object[,] {
               {
                 2,
                 1,
                 2,
                 "custone",
                 "2024-08-29 22:15:11.0696742",
                 "2024-08-29 22:15:11.0744496",
                 10
               },
             });

            migrationBuilder.InsertData(table: "OrderProduct", columns: new[] {
               "Id",
               "OrderId",
               "ProductId",
               "Author",
               "DateCreated",
               "DateUpdated",
               "Quantity"
             }, values: new object[,] {
               {
                 3,
                 1,
                 3,
                 "custone",
                 "2024-08-29 22:15:11.0696742",
                 "2024-08-29 22:15:11.0744496",
                 3
               },
             });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValues: new object[] {
             1
            });

            migrationBuilder.DeleteData(table: "OrderProduct", keyColumn: "Id", keyValues: new object[] {
             1,2,3
            });
        }
    }
}

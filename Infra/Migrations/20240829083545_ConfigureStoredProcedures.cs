using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_CompanyCleanUp]
				@Cui nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;

				SET @Id=(SELECT Id FROM Companies WHERE Cui=@Cui)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM Companies WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_BillCleanUp]
				@OrderNo nvarchar(100)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Bills WHERE OrderNo=@OrderNo)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM Bills WHERE OrderNo=@OrderNo;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_OrderCleanUp]
				@OrderNo nvarchar(100)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Orders WHERE OrderNo=@OrderNo)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM ORDERS WHERE OrderNo=@OrderNo;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_OrderProductsReset] 
				@ProductId nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Stocks WHERE ProductId=@ProductId)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE Stocks set AvailableStock=5000 WHERE ProductId=@ProductId;
					UPDATE Stocks set PendingStock=7 WHERE ProductId=@ProductId;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ProductCleanUp] 
				@Name nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Products WHERE Name=@Name)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM Products WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ResetCompanyName] 
				@Name nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Companies WHERE Name=@Name)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE Companies set Name='Update Company Test-ABC' WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ResetFirstname] 
				@Username nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id nvarchar (450);
			
				SET @Id=(SELECT Id FROM AspNetUsers WHERE UserName=@Username)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE AspNetUsers set FirstName='ToBeUpdated' WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ResetProductName] 
				@Name nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM Products WHERE Name=@Name)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE Products set Name='Update Product Test-ABC' WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ResetUsername] 
				@Username nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id nvarchar (450);
			
				SET @Id=(SELECT Id FROM AspNetUsers WHERE UserName=@Username)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE AspNetUsers set Username='userToBeUpdated' WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_ResetWorkpointData] 
				@Name nvarchar(100)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM WorkPoints WHERE Name=@Name)
				if(@Id IS NOT NULL)
				BEGIN
					UPDATE WorkPoints set Name='Update Workpoint Test-ABC' WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_StockReset] 
				@Name nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @ProductId int;
			
				SET @ProductId=(SELECT Id FROM Products WHERE Name=@Name)
				if(@ProductId IS NOT NULL)
				BEGIN
					UPDATE dbo.Stocks SET AvailableStock = 10 WHERE ProductId=@ProductId; 
				END
				ELSE
					SELECT -1;
				SELECT @ProductId;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_UserCleanUp] 
				@Username nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id nvarchar(450);
			
				SET @Id=(SELECT Id FROM AspNetUsers WHERE Username=@Username)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM AspNetUsers WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END;");

            migrationBuilder.Sql(@"
			CREATE PROCEDURE [dbo].[SP_WorkpointCleanUp] 
				@Name nvarchar(24)
			AS
			BEGIN
				SET NOCOUNT ON;
				DECLARE @Id int;
			
				SET @Id=(SELECT Id FROM WorkPoints WHERE Name=@Name)
				if(@Id IS NOT NULL)
				BEGIN
					DELETE FROM WorkPoints WHERE Id=@Id;
				END
				ELSE
					SELECT -1;
				SELECT @Id;
			END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_CompanyCleanUp];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_BillCleanUp];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_OrderCleanUp];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_OrderProductsReset];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ProductCleanUp];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ResetCompanyName];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ResetFirstname];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ResetProductName];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ResetUsername];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_ResetWorkpointData];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_StockReset];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_UserCleanUp];
			");

            migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS [dbo].[SP_WorkpointCleanUp];
			");
        }

    }
}

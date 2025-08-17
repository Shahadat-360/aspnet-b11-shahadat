using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetSalesStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetSales]
                    @PageIndex INT,
                    @PageSize INT,
                    @OrderBy NVARCHAR(50)='Id asc',
                    @SalesDateFrom Datetime = NULL,
                    @SalesDateTo Datetime = NULL,
                    @Id NVARCHAR(MAX) = '%',
                    @CustomerName NVARCHAR(MAX) = '%',
                    @CustomerPhone NVARCHAR(MAX) = '%',
                    @MinTotal DECIMAL(18, 2) = NULL,
                    @MaxTotal DECIMAL(18, 2) = NULL,
                    @MinPaid DECIMAL(18, 2) = NULL,
                    @MaxPaid DECIMAL(18, 2) = NULL,
                    @MinDue DECIMAL(18, 2) = NULL,
                    @MaxDue DECIMAL(18, 2) = NULL,
                    @PaymentStatus INT = NULL,
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @sql NVARCHAR(MAX);
                    DECLARE @countsql NVARCHAR(MAX);
                    DECLARE @paramList NVARCHAR(MAX);
                    DECLARE @countParamList NVARCHAR(MAX);

                    -- Total number of rows in Sales
                    SELECT @Total = COUNT(*) FROM Sales;

                    -- COUNT query for filtered results
                    SET @countsql = '
                        SELECT @TotalDisplay = COUNT(*)
                        FROM Sales s
                        INNER JOIN Customers c ON s.CustomerId = c.Id
                        WHERE 1 = 1';

                    SET @countsql += ' AND s.Id LIKE ''%'' + @xId + ''%'' ';
                    SET @countsql += ' AND c.CustomerName LIKE ''%'' + @xCustomerName + ''%'' ';
                    SET @countsql += ' AND c.Mobile LIKE ''%'' + @xCustomerPhone + ''%'' ';

                    IF @SalesDateFrom IS NOT NULL
                        SET @countsql += ' AND s.SaleDate >= @xSalesDateFrom ';
                    IF @SalesDateTo IS NOT NULL
                        SET @countsql += ' AND s.SaleDate <= @xSalesDateTo ';
                    IF @MinTotal IS NOT NULL
                        SET @countsql += ' AND s.Total >= @xMinTotal ';
                    IF @MaxTotal IS NOT NULL
                        SET @countsql += ' AND s.Total <= @xMaxTotal ';
                    IF @MinPaid IS NOT NULL
                        SET @countsql += ' AND s.Paid >= @xMinPaid ';
                    IF @MaxPaid IS NOT NULL
                        SET @countsql += ' AND s.Paid <= @xMaxPaid ';
                    IF @MinDue IS NOT NULL
                        SET @countsql += ' AND s.Due >= @xMinDue ';
                    IF @MaxDue IS NOT NULL
                        SET @countsql += ' AND s.Due <= @xMaxDue ';
                    IF @PaymentStatus IS NOT NULL
                        SET @countsql += ' AND s.PaymentStatus = @xPaymentStatus ';

                    SET @countParamList = '
                        @xId NVARCHAR(MAX),
                        @xCustomerName NVARCHAR(MAX),
                        @xCustomerPhone NVARCHAR(MAX),
                        @xSalesDateFrom Datetime,
                        @xSalesDateTo Datetime,
                        @xMinTotal DECIMAL(18,2),
                        @xMaxTotal DECIMAL(18,2),
                        @xMinPaid DECIMAL(18,2),
                        @xMaxPaid DECIMAL(18,2),
                        @xMinDue DECIMAL(18,2),
                        @xMaxDue DECIMAL(18,2),
                        @xPaymentStatus INT,
                        @TotalDisplay INT OUTPUT';

                    EXEC sp_executesql @countsql, @countParamList,
                        @Id, @CustomerName, @CustomerPhone,
                        @SalesDateFrom, @SalesDateTo,
                        @MinTotal, @MaxTotal,
                        @MinPaid, @MaxPaid,
                        @MinDue, @MaxDue,
                        @PaymentStatus,
                        @TotalDisplay OUTPUT;

                    -- DATA query for actual page
                    SET @sql = '
                        SELECT 
                            s.Id,
                            s.SaleDate,
                            c.CustomerName,
                            c.Mobile AS CustomerPhone,
                            s.Total,
                            s.Paid,
                            s.Due,
                            s.PaymentStatus
                        FROM Sales s
                        INNER JOIN Customers c ON s.CustomerId = c.Id
                        WHERE 1 = 1';

                    SET @sql += ' AND s.Id LIKE ''%'' + @xId + ''%'' ';
                    SET @sql += ' AND c.CustomerName LIKE ''%'' + @xCustomerName + ''%'' ';
                    SET @sql += ' AND c.Mobile LIKE ''%'' + @xCustomerPhone + ''%'' ';

                    IF @SalesDateFrom IS NOT NULL
                        SET @sql += ' AND s.SaleDate >= @xSalesDateFrom ';
                    IF @SalesDateTo IS NOT NULL
                        SET @sql += ' AND s.SaleDate <= @xSalesDateTo ';
                    IF @MinTotal IS NOT NULL
                        SET @sql += ' AND s.Total >= @xMinTotal ';
                    IF @MaxTotal IS NOT NULL
                        SET @sql += ' AND s.Total <= @xMaxTotal ';
                    IF @MinPaid IS NOT NULL
                        SET @sql += ' AND s.Paid >= @xMinPaid ';
                    IF @MaxPaid IS NOT NULL
                        SET @sql += ' AND s.Paid <= @xMaxPaid ';
                    IF @MinDue IS NOT NULL
                        SET @sql += ' AND s.Due >= @xMinDue ';
                    IF @MaxDue IS NOT NULL
                        SET @sql += ' AND s.Due <= @xMaxDue ';
                    IF @PaymentStatus IS NOT NULL
                        SET @sql += ' AND s.PaymentStatus = @xPaymentStatus ';

                    SET @sql = @sql + ' ORDER BY '+@OrderBy+ ' OFFSET @PageSize * (@PageIndex - 1) ROWS 
                          FETCH NEXT @PageSize ROWS ONLY';

                    SET @paramList = '
                        @xId NVARCHAR(MAX),
                        @xCustomerName NVARCHAR(MAX),
                        @xCustomerPhone NVARCHAR(MAX),
                        @xSalesDateFrom Datetime,
                        @xSalesDateTo Datetime,
                        @xMinTotal DECIMAL(18,2),
                        @xMaxTotal DECIMAL(18,2),
                        @xMinPaid DECIMAL(18,2),
                        @xMaxPaid DECIMAL(18,2),
                        @xMinDue DECIMAL(18,2),
                        @xMaxDue DECIMAL(18,2),
                        @xPaymentStatus INT,
                        @PageIndex INT,
                        @PageSize INT';

                    EXEC sp_executesql @sql, @paramList,
                        @Id, @CustomerName, @CustomerPhone,
                        @SalesDateFrom, @SalesDateTo,
                        @MinTotal, @MaxTotal,
                        @MinPaid, @MaxPaid,
                        @MinDue, @MaxDue,
                        @PaymentStatus,
                        @PageIndex, @PageSize;

                    -- Debugging purposes
                    PRINT @sql;
                    PRINT @countsql;
                END;
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetSales]");
        }
    }
}

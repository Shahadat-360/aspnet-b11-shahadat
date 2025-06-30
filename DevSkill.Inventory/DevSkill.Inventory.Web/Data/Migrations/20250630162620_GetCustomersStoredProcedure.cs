using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetCustomersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE or ALTER procedure [dbo].[GetCustomers]
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@CustomerName nvarchar(max)='%',
                	@CompanyName nvarchar(max)='%',
                	@Email nvarchar(max)='%',
                	@Mobile nvarchar(max)='%',
                	@Address nvarchar(max)='%',
                	@MinCurrentBalance decimal(18,2) = NULL,
                	@MaxCurrentBalance decimal(18,2) = NULL,
                	@Status int = null,
                	@Total int output,
                	@TotalDisplay int output
                as 
                begin
                	SET NOCOUNT ON;

                	Declare @sql nvarchar(MAX);
                	Declare @countsql nvarchar(MAX);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	select @Total = count(*) from Customers;

                	--count query
                	SET @countsql = 'select @TotalDisplay = count(*) from Customers c where 1=1'
                	SET @countsql = @countsql + ' AND c.CustomerName like ''%'' + @xCustomerName + ''%'''
                	SET @countsql = @countsql + ' AND c.CompanyName like ''%'' + @xCompanyName + ''%'''
                	SET @countsql = @countsql + ' AND c.Email like ''%'' + @xEmail + ''%'''
                	SET @countsql = @countsql + ' AND c.Mobile like ''%'' + @xMobile + ''%'''
                	SET @countsql = @countsql + ' AND c.Address like ''%'' + @xAddress + ''%'''

                	IF @MinCurrentBalance is not null
                	SET @countsql = @countsql + ' AND c.CurrentBalance >= @xMinCurrentBalance'

                	IF @MaxCurrentBalance is not null
                	SET @countsql = @countsql + ' AND c.CurrentBalance <= @xMaxCurrentBalance'

                	IF @Status is not null
                	SET @countsql = @countsql + ' AND c.Status = @xStatus'

                	select @countparamList = '@xCustomerName nvarchar(max),
                	@xCompanyName nvarchar(max),
                	 @xEmail nvarchar(max),
                	 @xMobile nvarchar(max),
                	 @xAddress nvarchar(max),
                	 @xMinCurrentBalance int,
                	 @xMaxCurrentBalance int,
                	 @xStatus int,
                	 @TotalDisplay int output'

                	 exec sp_executesql @countsql, @countparamList,
                	 @CustomerName,
                	 @CompanyName,
                	 @Email,
                	 @Mobile,
                	 @Address,
                	 @MinCurrentBalance,
                	 @MaxCurrentBalance,
                	 @Status,
                	 @TotalDisplay = @TotalDisplay output

                	 --collecting data
                	SET @sql = 'select * from Customers c where 1=1'
                	SET @sql = @sql + ' AND c.CustomerName like ''%'' + @xCustomerName + ''%'''
                	SET @sql = @sql + ' AND c.CompanyName like ''%'' + @xCompanyName + ''%'''
                	SET @sql = @sql + ' AND c.Email like ''%'' + @xEmail + ''%'''
                	SET @sql = @sql + ' AND c.Mobile like ''%'' + @xMobile + ''%'''
                	SET @sql = @sql + ' AND c.Address like ''%'' + @xAddress + ''%'''

                	IF @MinCurrentBalance is not null
                	SET @sql = @sql + ' AND c.CurrentBalance >= @xMinCurrentBalance'

                	IF @MaxCurrentBalance is not null
                	SET @sql = @sql + ' AND c.CurrentBalance <= @xMaxCurrentBalance'

                	IF @Status is not null
                	SET @sql = @sql + ' AND c.Status = @xStatus'

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY'

                	select @paramList = '@xCustomerName nvarchar(max),
                	@xCompanyName nvarchar(max),
                	 @xEmail nvarchar(max),
                	 @xMobile nvarchar(max),
                	 @xAddress nvarchar(max),
                	 @xMinCurrentBalance int,
                	 @xMaxCurrentBalance int,
                	 @xStatus int,
                	 @PageIndex int,
                	 @PageSize int'

                	 exec sp_executesql @sql,@paramList,
                	 @CustomerName,
                	 @CompanyName,
                	 @Email,
                	 @Mobile,
                	 @Address,
                	 @MinCurrentBalance,
                	 @MaxCurrentBalance,
                	 @Status,
                	 @PageIndex,
                	 @PageSize

                	 print @sql
                	 print @countsql

                	 end
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """
                DROP PROCEDURE IF EXISTS [dbo].[GetCustomers]
                """;
            migrationBuilder.Sql(sql);
        }
    }
}

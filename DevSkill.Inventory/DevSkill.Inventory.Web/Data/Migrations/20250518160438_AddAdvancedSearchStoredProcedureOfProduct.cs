using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvancedSearchStoredProcedureOfProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                create or ALTER PROCEDURE [dbo].[GetProducts] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Name nvarchar(max) = '%',
                	@CategoryId uniqueidentifier = NULL,
                	@MinPrice int = NULL,
                	@MaxPrice int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                	SET NOCOUNT ON;

                	Declare @sql nvarchar(MAX);
                	Declare @countsql nvarchar(MAX);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	Select @Total = count(*) from Products;

                	--COUNT QUERY
                	SET @countsql = 'select @TotalDisplay = count(*) from Products p where 1 = 1 ';

                	SET @countsql = @countsql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	if @CategoryId IS NOT NULL
                	SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId' 

                	IF @MinPrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.Price >= @xMinPrice'

                	IF @MaxPrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.Price <= @xMaxPrice' 

                	SELECT @countparamlist = '@xName nvarchar(max),
                		@xCategoryId uniqueidentifier,
                		@xMinPrice int,
                		@xMaxPrice int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Name,
                		@CategoryId,
                		@MinPrice,
                		@MaxPrice,
                		@TotalDisplay = @TotalDisplay output;

                	-- COLLECTING DATA
                	SET @sql = 'select * from Products p where 1 = 1 ';

                	SET @sql = @sql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	if @CategoryId IS NOT NULL
                	SET @sql = @sql + ' AND p.CategoryId = @xCategoryId' 

                	IF @MinPrice IS NOT NULL
                	SET @sql = @sql + ' AND p.Price >= @xMinPrice'

                	IF @MaxPrice IS NOT NULL
                	SET @sql = @sql + ' AND p.Price <= @xMaxPrice'

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xName nvarchar(max),
                		@xCategoryId uniqueidentifier,
                		@xMinPrice int,
                		@xMaxPrice int,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Name,
                		@CategoryId,
                		@MinPrice,
                		@MaxPrice,
                		@PageIndex,
                		@PageSize;

                	print @sql;
                	print @countsql;

                END
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """
                DROP PROCEDURE [dbo].[GetProducts]
                """;
            migrationBuilder.Sql(sql);
        }
    }
}

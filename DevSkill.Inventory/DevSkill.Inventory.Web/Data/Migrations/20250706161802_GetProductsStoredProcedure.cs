using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetProductsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = 
                """
                CREATE or ALTER PROCEDURE [dbo].[GetProducts] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Id nvarchar(max)='%',
                	@Name nvarchar(max) = '%',
                	@Category nvarchar(max) = '%',
                	@MaxPurchasePrice decimal(18,2) = NULL,
                	@MinPurchasePrice decimal(18,2) = NULL,
                	@MaxMRP decimal(18,2) = NULL,
                	@MinMRP decimal(18,2) = NULL,
                	@MaxWholesalePrice decimal(18,2) = NULL,
                	@MinWholesalePrice decimal(18,2) = NULL,
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
                	SET @countsql = 'select @TotalDisplay = count(*) from Products p 
                					 LEFT JOIN Categories c ON p.CategoryId = c.Id 
                					 where 1 = 1';

                	SET @countsql = @countsql + ' AND p.Id like ''%'' + @xId + ''%''' 

                	SET @countsql = @countsql + ' AND p.Name like ''%'' + @xName + ''%'''

                	SET @countsql = @countsql + ' AND c.Name like ''%'' + @xCategory + ''%'''

                	if @MaxPurchasePrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.PurchasePrice <= @xMaxPurchasePrice' 

                	if @MinPurchasePrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.PurchasePrice >= @xMinPurchasePrice' 

                	if @MaxMRP IS NOT NULL
                	SET @countsql = @countsql + ' AND p.MRP <= @xMaxMRP' 

                	if @MinMRP IS NOT NULL
                	SET @countsql = @countsql + ' AND p.MRP >= @xMinMRP' 

                	if @MaxWholesalePrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.WholesalePrice <= @xMaxWholesalePrice' 

                	if @MinWholesalePrice IS NOT NULL
                	SET @countsql = @countsql + ' AND p.WholesalePrice >= @xMinWholesalePrice' 

                	SELECT @countparamlist = '@xId nvarchar(max),
                		@xName nvarchar(max),
                		@xCategory nvarchar(max),
                		@xMaxPurchasePrice decimal(18,2),
                		@xMinPurchasePrice decimal(18,2),
                		@xMaxMRP decimal(18,2),
                		@xMinMRP decimal(18,2),
                		@xMaxWholesalePrice decimal(18,2),
                		@xMinWholesalePrice decimal(18,2),
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Id,
                		@Name,
                		@Category,
                		@MaxPurchasePrice,
                		@MinPurchasePrice,
                		@MaxMRP,
                		@MinMRP,
                		@MaxWholesalePrice,
                		@MinWholesalePrice,
                		@TotalDisplay = @TotalDisplay output;

                	-- COLLECTING DATA
                	SET @sql = 'select p.Id,p.ImageUrl,p.Name,c.Name as CategoryName,p.PurchasePrice,p.MRP,p.WholesalePrice,
                					   p.Stock,p.LowStock,p.DamageStock from Products p 
                					   LEFT JOIN Categories c ON p.CategoryId = c.Id 
                					   where 1 = 1';

                	SET @sql = @sql + ' AND p.Id like ''%'' + @xId + ''%''' 

                	SET @sql = @sql + ' AND p.Name like ''%'' + @xName + ''%'''

                	SET @sql = @sql + ' AND c.Name like ''%'' + @xCategory + ''%'''

                	if @MaxPurchasePrice IS NOT NULL
                	SET @sql = @sql + ' AND p.PurchasePrice <= @xMaxPurchasePrice' 

                	if @MinPurchasePrice IS NOT NULL
                	SET @sql = @sql + ' AND p.PurchasePrice >= @xMinPurchasePrice' 

                	if @MaxMRP IS NOT NULL
                	SET @sql = @sql + ' AND p.MRP <= @xMaxMRP' 

                	if @MinMRP IS NOT NULL
                	SET @sql = @sql + ' AND p.MRP >= @xMinMRP' 

                	if @MaxWholesalePrice IS NOT NULL
                	SET @sql = @sql + ' AND p.WholesalePrice <= @xMaxWholesalePrice' 

                	if @MinWholesalePrice IS NOT NULL
                	SET @sql = @sql + ' AND p.WholesalePrice >= @xMinWholesalePrice' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xId nvarchar(max),
                		@xName nvarchar(max),
                		@xCategory nvarchar(max),
                		@xMaxPurchasePrice decimal(18,2),
                		@xMinPurchasePrice decimal(18,2),
                		@xMaxMRP decimal(18,2),
                		@xMinMRP decimal(18,2),
                		@xMaxWholesalePrice decimal(18,2),
                		@xMinWholesalePrice decimal(18,2),
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Id,
                		@Name,
                		@Category,
                		@MaxPurchasePrice,
                		@MinPurchasePrice,
                		@MaxMRP,
                		@MinMRP,
                		@MaxWholesalePrice,
                		@MinWholesalePrice,
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
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetProducts]");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetEmployeeStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER   PROCEDURE [dbo].[GetEmployees] 
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(50),
                	@Id nvarchar(max) = '%',
                	@Name nvarchar(max) = '%',
                	@Mobile nvarchar(max) = '%',
                	@Email nvarchar(max) = '%',
                	@Address nvarchar(max) = '%',
                	@JoiningDateFrom datetime = NULL,
                	@JoiningDateTo datetime = NULL,
                	@SalaryFrom decimal(18,2) = NULL,
                	@SalaryTo decimal(18,2) = NULL,
                	@Status int = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                	SET NOCOUNT ON;

                	Declare @sql nvarchar(MAX);
                	Declare @countsql nvarchar(MAX);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	Select @Total = count(*) from Employees;

                	--COUNT QUERY
                	SET @countsql = 'select @TotalDisplay = count(*) from Employees e 
                					 where 1 = 1';

                	SET @countsql = @countsql + ' AND e.Id like ''%'' + @xId + ''%''' 

                	SET @countsql = @countsql + ' AND e.Name like ''%'' + @xName + ''%'''

                	SET @countsql = @countsql + ' AND e.Mobile like ''%'' + @xMobile + ''%'''

                	SET @countsql = @countsql + ' AND e.Email like ''%'' + @xEmail + ''%'''

                	SET @countsql = @countsql + ' AND e.Address like ''%'' + @xAddress + ''%'''

                	if @JoiningDateFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND e.JoiningDate >= @xJoiningDateFrom' 

                	if @JoiningDateTo IS NOT NULL
                	SET @countsql = @countsql + ' AND e.JoiningDate <= @xJoiningDateTo' 

                	if @SalaryFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND e.Salary >= @xSalaryFrom' 

                	if @SalaryTo IS NOT NULL
                	SET @countsql = @countsql + ' AND e.Salary <= @xSalaryTo' 

                	if @Status IS NOT NULL
                	SET @countsql = @countsql + ' AND e.Status = @xStatus' 

                	SELECT @countparamlist = '@xId nvarchar(max),
                		@xName nvarchar(max),
                		@xMobile nvarchar(max),
                		@xEmail nvarchar(max),
                		@xAddress nvarchar(max),
                		@xJoiningDateFrom datetime,
                		@xJoiningDateTo datetime,
                		@xSalaryFrom decimal(18,2),
                		@xSalaryTo decimal(18,2),
                		@xStatus int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Id,
                		@Name,
                		@Mobile,
                		@Email,
                		@Address,
                		@JoiningDateFrom,
                		@JoiningDateTo,
                		@SalaryFrom,
                		@SalaryTo,
                		@Status,
                		@TotalDisplay = @TotalDisplay output;

                	-- COLLECTING DATA
                	SET @sql = 'select e.Id, e.Name, e.Mobile, e.Email, e.Address, e.JoiningDate, 
                					   e.Salary, e.Status from Employees e 
                					   where 1 = 1';

                	SET @sql = @sql + ' AND e.Id like ''%'' + @xId + ''%''' 

                	SET @sql = @sql + ' AND e.Name like ''%'' + @xName + ''%'''

                	SET @sql = @sql + ' AND e.Mobile like ''%'' + @xMobile + ''%'''

                	SET @sql = @sql + ' AND e.Email like ''%'' + @xEmail + ''%'''

                	SET @sql = @sql + ' AND e.Address like ''%'' + @xAddress + ''%'''

                	if @JoiningDateFrom IS NOT NULL
                	SET @sql = @sql + ' AND e.JoiningDate >= @xJoiningDateFrom' 

                	if @JoiningDateTo IS NOT NULL
                	SET @sql = @sql + ' AND e.JoiningDate <= @xJoiningDateTo' 

                	if @SalaryFrom IS NOT NULL
                	SET @sql = @sql + ' AND e.Salary >= @xSalaryFrom' 

                	if @SalaryTo IS NOT NULL
                	SET @sql = @sql + ' AND e.Salary <= @xSalaryTo' 

                	if @Status IS NOT NULL
                	SET @sql = @sql + ' AND e.Status = @xStatus' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xId nvarchar(max),
                		@xName nvarchar(max),
                		@xMobile nvarchar(max),
                		@xEmail nvarchar(max),
                		@xAddress nvarchar(max),
                		@xJoiningDateFrom datetime,
                		@xJoiningDateTo datetime,
                		@xSalaryFrom decimal(18,2),
                		@xSalaryTo decimal(18,2),
                		@xStatus int,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Id,
                		@Name,
                		@Mobile,
                		@Email,
                		@Address,
                		@JoiningDateFrom,
                		@JoiningDateTo,
                		@SalaryFrom,
                		@SalaryTo,
                		@Status,
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
            var sql = "DROP PROCEDURE IF EXISTS [dbo].[GetEmployees]";
            migrationBuilder.Sql(sql);
        }
    }
}

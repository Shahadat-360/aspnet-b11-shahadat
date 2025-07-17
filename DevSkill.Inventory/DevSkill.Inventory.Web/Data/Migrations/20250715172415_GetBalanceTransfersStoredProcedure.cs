using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetBalanceTransfersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetBalanceTransfers] 
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(50),
                	@TransferDateFrom datetime = NULL,
                	@TransferDateTo datetime = NULL,
                	@SendingAccountType int = NULL,
                	@SendingAccount nvarchar(max) = NULL,
                	@ReceivingAccountType int = NULL,
                	@ReceivingAccount nvarchar(max) = NULL,
                	@MinTransferAmount decimal(18,2) = NULL,
                	@MaxTransferAmount decimal(18,2) = NULL,
                	@Note nvarchar(max) = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                	SET NOCOUNT ON;

                	Declare @sql nvarchar(MAX);
                	Declare @countsql nvarchar(MAX);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	-- Get total count of all balance transfers
                	Select @Total = count(*) from BalanceTransfers;

                	--COUNT QUERY WITH JOINS
                	SET @countsql = 'select @TotalDisplay = count(*) from BalanceTransfers bt 
                					 LEFT JOIN CashAccounts sca ON bt.SendingAccountType = 0 AND bt.SendingAccountId = sca.Id
                					 LEFT JOIN BankAccounts sba ON bt.SendingAccountType = 1 AND bt.SendingAccountId = sba.Id
                					 LEFT JOIN MobileAccounts sma ON bt.SendingAccountType = 2 AND bt.SendingAccountId = sma.Id
                					 LEFT JOIN CashAccounts rca ON bt.ReceivingAccountType = 0 AND bt.ReceivingAccountId = rca.Id
                					 LEFT JOIN BankAccounts rba ON bt.ReceivingAccountType = 1 AND bt.ReceivingAccountId = rba.Id
                					 LEFT JOIN MobileAccounts rma ON bt.ReceivingAccountType = 2 AND bt.ReceivingAccountId = rma.Id
                					 where 1 = 1';

                	-- Add date range filter
                	if @TransferDateFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND CAST(bt.TransferDate AS DATE) >= CAST(@xTransferDateFrom AS DATE)';

                	if @TransferDateTo IS NOT NULL
                	SET @countsql = @countsql + ' AND CAST(bt.TransferDate AS DATE) <= CAST(@xTransferDateTo AS DATE)';

                	-- Add sending account type filter
                	if @SendingAccountType IS NOT NULL
                	SET @countsql = @countsql + ' AND bt.SendingAccountType = @xSendingAccountType';

                	-- Add sending account filter
                	if @SendingAccount IS NOT NULL
                	SET @countsql = @countsql + ' AND (
                		(bt.SendingAccountType = 0 AND sca.AccountName like ''%'' + @xSendingAccount + ''%'') OR
                		(bt.SendingAccountType = 1 AND sba.AccountName like ''%'' + @xSendingAccount + ''%'') OR
                		(bt.SendingAccountType = 2 AND sma.AccountName like ''%'' + @xSendingAccount + ''%'')
                	)';

                	-- Add receiving account type filter
                	if @ReceivingAccountType IS NOT NULL
                	SET @countsql = @countsql + ' AND bt.ReceivingAccountType = @xReceivingAccountType';

                	-- Add receiving account filter
                	if @ReceivingAccount IS NOT NULL
                	SET @countsql = @countsql + ' AND (
                		(bt.ReceivingAccountType = 0 AND rca.AccountName like ''%'' + @xReceivingAccount + ''%'') OR
                		(bt.ReceivingAccountType = 1 AND rba.AccountName like ''%'' + @xReceivingAccount + ''%'') OR
                		(bt.ReceivingAccountType = 2 AND rma.AccountName like ''%'' + @xReceivingAccount + ''%'')
                	)';

                	-- Add minimum transfer amount filter
                	if @MinTransferAmount IS NOT NULL
                	SET @countsql = @countsql + ' AND bt.TransferAmount >= @xMinTransferAmount';

                	-- Add maximum transfer amount filter
                	if @MaxTransferAmount IS NOT NULL
                	SET @countsql = @countsql + ' AND bt.TransferAmount <= @xMaxTransferAmount';

                	-- Add note filter
                	if @Note IS NOT NULL
                	SET @countsql = @countsql + ' AND bt.Note like ''%'' + @xNote + ''%''';

                	SELECT @countparamlist = '@xTransferDateFrom datetime,
                		@xTransferDateTo datetime,
                		@xSendingAccountType int,
                		@xSendingAccount nvarchar(max),
                		@xReceivingAccountType int,
                		@xReceivingAccount nvarchar(max),
                		@xMinTransferAmount decimal(18,2),
                		@xMaxTransferAmount decimal(18,2),
                		@xNote nvarchar(max),
                		@TotalDisplay int output';

                	exec sp_executesql @countsql, @countparamlist,
                		@TransferDateFrom,
                		@TransferDateTo,
                		@SendingAccountType,
                		@SendingAccount,
                		@ReceivingAccountType,
                		@ReceivingAccount,
                		@MinTransferAmount,
                		@MaxTransferAmount,
                		@Note,
                		@TotalDisplay = @TotalDisplay output;

                	-- COLLECTING DATA WITH JOINS
                	SET @sql = 'select bt.Id as Id, bt.TransferDate as TransferDate,
                				       bt.SendingAccountType,
                				       CASE 
                				           WHEN bt.SendingAccountType = 0 THEN sca.AccountName
                				           WHEN bt.SendingAccountType = 1 THEN sba.AccountName
                				           WHEN bt.SendingAccountType = 2 THEN sma.AccountName
                				       END as SendingAccount,
                				       bt.ReceivingAccountType,
                				       CASE 
                				           WHEN bt.ReceivingAccountType = 0 THEN rca.AccountName
                				           WHEN bt.ReceivingAccountType = 1 THEN rba.AccountName
                				           WHEN bt.ReceivingAccountType = 2 THEN rma.AccountName
                				       END as ReceivingAccount,
                				       bt.TransferAmount,
                				       bt.Note
                				from BalanceTransfers bt 
                				LEFT JOIN CashAccounts sca ON bt.SendingAccountType = 0 AND bt.SendingAccountId = sca.Id
                				LEFT JOIN BankAccounts sba ON bt.SendingAccountType = 1 AND bt.SendingAccountId = sba.Id
                				LEFT JOIN MobileAccounts sma ON bt.SendingAccountType = 2 AND bt.SendingAccountId = sma.Id
                				LEFT JOIN CashAccounts rca ON bt.ReceivingAccountType = 0 AND bt.ReceivingAccountId = rca.Id
                				LEFT JOIN BankAccounts rba ON bt.ReceivingAccountType = 1 AND bt.ReceivingAccountId = rba.Id
                				LEFT JOIN MobileAccounts rma ON bt.ReceivingAccountType = 2 AND bt.ReceivingAccountId = rma.Id
                				where 1 = 1';

                	-- Add date range filter
                	if @TransferDateFrom IS NOT NULL
                	SET @sql = @sql + ' AND CAST(bt.TransferDate AS DATE) >= CAST(@xTransferDateFrom AS DATE)';

                	if @TransferDateTo IS NOT NULL
                	SET @sql = @sql + ' AND CAST(bt.TransferDate AS DATE) <= CAST(@xTransferDateTo AS DATE)';

                	-- Add sending account type filter
                	if @SendingAccountType IS NOT NULL
                	SET @sql = @sql + ' AND bt.SendingAccountType = @xSendingAccountType';

                	-- Add sending account filter
                	if @SendingAccount IS NOT NULL
                	SET @sql = @sql + ' AND (
                		(bt.SendingAccountType = 0 AND sca.AccountName like ''%'' + @xSendingAccount + ''%'') OR
                		(bt.SendingAccountType = 1 AND sba.AccountName like ''%'' + @xSendingAccount + ''%'') OR
                		(bt.SendingAccountType = 2 AND sma.AccountName like ''%'' + @xSendingAccount + ''%'')
                	)';

                	-- Add receiving account type filter
                	if @ReceivingAccountType IS NOT NULL
                	SET @sql = @sql + ' AND bt.ReceivingAccountType = @xReceivingAccountType';

                	-- Add receiving account filter
                	if @ReceivingAccount IS NOT NULL
                	SET @sql = @sql + ' AND (
                		(bt.ReceivingAccountType = 0 AND rca.AccountName like ''%'' + @xReceivingAccount + ''%'') OR
                		(bt.ReceivingAccountType = 1 AND rba.AccountName like ''%'' + @xReceivingAccount + ''%'') OR
                		(bt.ReceivingAccountType = 2 AND rma.AccountName like ''%'' + @xReceivingAccount + ''%'')
                	)';

                	-- Add minimum transfer amount filter
                	if @MinTransferAmount IS NOT NULL
                	SET @sql = @sql + ' AND bt.TransferAmount >= @xMinTransferAmount';

                	-- Add maximum transfer amount filter
                	if @MaxTransferAmount IS NOT NULL
                	SET @sql = @sql + ' AND bt.TransferAmount <= @xMaxTransferAmount';

                	-- Add note filter
                	if @Note IS NOT NULL
                	SET @sql = @sql + ' AND bt.Note like ''%'' + @xNote + ''%''';

                	-- Add ordering and pagination
                	SET @sql = @sql + ' Order by ' + @OrderBy + ' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xTransferDateFrom datetime,
                		@xTransferDateTo datetime,
                		@xSendingAccountType int,
                		@xSendingAccount nvarchar(max),
                		@xReceivingAccountType int,
                		@xReceivingAccount nvarchar(max),
                		@xMinTransferAmount decimal(18,2),
                		@xMaxTransferAmount decimal(18,2),
                		@xNote nvarchar(max),
                		@PageIndex int,
                		@PageSize int';

                	exec sp_executesql @sql, @paramlist,
                		@TransferDateFrom,
                		@TransferDateTo,
                		@SendingAccountType,
                		@SendingAccount,
                		@ReceivingAccountType,
                		@ReceivingAccount,
                		@MinTransferAmount,
                		@MaxTransferAmount,
                		@Note,
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
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBalanceTransfers]");
        }
    }
}

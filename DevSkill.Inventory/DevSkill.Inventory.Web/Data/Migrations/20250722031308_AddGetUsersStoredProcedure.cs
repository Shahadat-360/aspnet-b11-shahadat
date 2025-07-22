using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUsersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetUsers]
                    @PageIndex      INT,
                    @PageSize       INT,
                    @OrderBy        NVARCHAR(50),
                    @EmployeeName   NVARCHAR(MAX) = '%',
                    @Email          NVARCHAR(MAX) = '%',
                    @Mobile         NVARCHAR(MAX) = '%',
                    @RoleName       NVARCHAR(MAX) = '%',
                    @Status         INT           = NULL,
                    @Total          INT           OUTPUT,
                    @TotalDisplay   INT           OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    ----------------------------------------------------------------
                    -- 1) UNFILTERED TOTAL
                    ----------------------------------------------------------------
                    SELECT @Total = COUNT(*) 
                      FROM AspNetUsers;

                    ----------------------------------------------------------------
                    -- 2) FILTERED COUNT
                    ----------------------------------------------------------------
                    DECLARE @countSql NVARCHAR(MAX) = 
                    N'
                    SELECT @TotalDisplay = COUNT(*) 
                      FROM AspNetUsers u
                      LEFT JOIN Employees       e  ON u.EmployeeId  = e.Id
                      LEFT JOIN AspNetUserRoles ur ON u.Id          = ur.UserId
                      LEFT JOIN AspNetRoles     r  ON ur.RoleId     = r.Id
                    WHERE 1=1
                      AND e.Name        LIKE ''%'' + @xEmployeeName + ''%''
                      AND u.Email       LIKE ''%'' + @xEmail        + ''%''
                      AND u.PhoneNumber LIKE ''%'' + @xMobile       + ''%''
                      AND r.Name        LIKE ''%'' + @xRoleName     + ''%''
                    ';

                    IF @Status IS NOT NULL
                        SET @countSql += N' AND u.Status = @xStatus';

                    EXEC sp_executesql
                        @countSql,
                        N'@xEmployeeName NVARCHAR(MAX),
                          @xEmail        NVARCHAR(MAX),
                          @xMobile       NVARCHAR(MAX),
                          @xRoleName     NVARCHAR(MAX),
                          @xStatus       INT,
                          @TotalDisplay  INT OUTPUT',
                        @xEmployeeName = @EmployeeName,
                        @xEmail        = @Email,
                        @xMobile       = @Mobile,
                        @xRoleName     = @RoleName,
                        @xStatus       = @Status,
                        @TotalDisplay  = @TotalDisplay OUTPUT;

                    ----------------------------------------------------------------
                    -- 3) PAGED DATA
                    ----------------------------------------------------------------
                    DECLARE @sql NVARCHAR(MAX) = 
                    N'
                    SELECT
                        u.Id                 AS Id,
                        e.Name               AS EmployeeName,
                        r.Company            AS Company,
                        u.Email              AS Email,
                        u.PhoneNumber        AS Mobile,
                        r.Name               AS RoleName,
                        u.Status             AS Status
                      FROM AspNetUsers u
                      LEFT JOIN Employees       e  ON u.EmployeeId  = e.Id
                      LEFT JOIN AspNetUserRoles ur ON u.Id          = ur.UserId
                      LEFT JOIN AspNetRoles     r  ON ur.RoleId     = r.Id
                    WHERE 1=1
                      AND e.Name        LIKE ''%'' + @xEmployeeName + ''%''
                      AND u.Email       LIKE ''%'' + @xEmail        + ''%''
                      AND u.PhoneNumber LIKE ''%'' + @xMobile       + ''%''
                      AND r.Name        LIKE ''%'' + @xRoleName     + ''%''
                    ';

                    IF @Status IS NOT NULL
                        SET @sql += N' AND u.Status = @xStatus';

                    SET @sql += N'
                    ORDER BY ' + @OrderBy + N'
                    OFFSET @PageSize * (@PageIndex - 1) ROWS
                    FETCH NEXT @PageSize ROWS ONLY;';

                    EXEC sp_executesql
                        @sql,
                        N'@xEmployeeName NVARCHAR(MAX),
                          @xEmail        NVARCHAR(MAX),
                          @xMobile       NVARCHAR(MAX),
                          @xRoleName     NVARCHAR(MAX),
                          @xStatus       INT,
                          @PageIndex     INT,
                          @PageSize      INT',
                        @xEmployeeName = @EmployeeName,
                        @xEmail        = @Email,
                        @xMobile       = @Mobile,
                        @xRoleName     = @RoleName,
                        @xStatus       = @Status,
                        @PageIndex     = @PageIndex,
                        @PageSize      = @PageSize;

                END
                GO
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """
                DROP PROCEDURE IF EXISTS [dbo].[GetUsers];
                """;
            migrationBuilder.Sql(sql);
        }
    }
}

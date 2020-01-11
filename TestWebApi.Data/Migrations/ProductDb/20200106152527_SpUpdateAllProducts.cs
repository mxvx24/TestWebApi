namespace TestWebApi.Data.Migrations.ProductDb
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// The stored proc to update all products.
    /// </summary>
    public partial class SpUpdateAllProducts : Migration
    {
        /// <summary>
        /// The up.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*************************************************************************************************************
             * WAITFOR (Transact-SQL):
             * https://docs.microsoft.com/en-us/sql/t-sql/language-elements/waitfor-transact-sql?view=sql-server-ver15
             *************************************************************************************************************/
            var sp = @"

                        /*An example to mimic a long running stored proc*/

                        IF OBJECT_ID('dbo.SpUpdateAllProducts','P') IS NOT NULL
                        BEGIN
                            DROP PROCEDURE dbo.SpUpdateAllProducts;  
                        END
                        GO

                        CREATE PROCEDURE [dbo].[SpUpdateAllProducts]
                        (
                            @ProductIds VARCHAR(100), @UpdatedBy VARCHAR(100) = NULL
                        )
                        AS
                        BEGIN
                            SET NOCOUNT ON;

                            DECLARE @PrintInfo VARCHAR(255);
                            DECLARE @DelayLength CHAR(8) = '00:00:10';

                            IF @UpdatedBy IS NULL
	                        BEGIN
		                        SET @UpdatedBy = SYSTEM_USER;
	                        END

                            /*IF ISDATE('2000-01-01 ' + @DelayLength + '.000') = 0  
                            BEGIN  
                                SELECT @ReturnInfo = 'Invalid time ' + @DelayLength   
                                + ',hh:mm:ss, submitted.';  
                                -- This PRINT statement is for testing, not use in production.  
                                PRINT @ReturnInfo   
                                RETURN(1)  
                            END */

                            WAITFOR DELAY @DelayLength  
                                SET @PrintInfo = 'A total time of ' + @DelayLength + ', hh:mm:ss, has elapsed.'  
                                -- This PRINT statement is for testing, not use in production.  
                            PRINT @PrintInfo;

                            --SELECT * FROM dbo.Products WHERE Id IN (STRING_SPLIT(@ProductIds, ','));
                            UPDATE dbo.Products SET UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy;
                        END
                ";

            migrationBuilder.Sql(sp);
        }

        /// <summary>
        /// The down.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

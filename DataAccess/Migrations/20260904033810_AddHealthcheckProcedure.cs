using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthcheckProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            CREATE OR ALTER PROCEDURE HealthCheck
            -- No parameters for HealthCheck
            AS
            BEGIN
            SET NOCOUNT ON;
            SELECT
                GETUTCDATE() AS UtcTime,
                CAST(
                    GETUTCDATE()
                    AT TIME ZONE 'UTC'
                    AT TIME ZONE 'China Standard Time'
                    AS DATETIME2
                ) AS MacauTime;
            END
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            DROP PROCEDURE IF EXISTS HealthCheck;
            """);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthCheckStoredProcedure : Migration
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
                1 AS HealthCheckStatus; -- 1 indicates healthy
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

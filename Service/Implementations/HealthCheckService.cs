using BusinessModel.DTOs;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class HealthCheckService : IHealthCheckService
    {
        public async Task<HealthCheckDto?> HealthCheck(DbContext context)
        {
            try
            {
                var result = (await context
                        .Database.SqlQuery<HealthCheckDto>($"EXEC HEALTHCHECK")
                        .ToListAsync())
                        .AsEnumerable().FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                // Implement logger??
                return new HealthCheckDto();
            }
        }
    }
}

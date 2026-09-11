using BusinessModel.DTOs;
using DataAccess.Dbcontexts;
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
        private readonly AppDbContext _context;
        public HealthCheckService(AppDbContext context) {
            _context = context;
        }
        public async Task<HealthCheckDto?> HealthCheck()
        {
            try
            {
                var result = await _context.Database
                    .SqlQuery<HealthCheckDto>($"EXEC HEALTHCHECK")
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                // Implement logger??
                return new HealthCheckDto()
                {
                    HealthCheckStatus = BusinessModel.Enums.HealthCheckStatus.Unhealthy
                };
            }
        }
    }
}

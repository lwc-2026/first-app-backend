using BusinessModel.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Interfaces
{
    public interface IHealthCheckService
    {
        public Task<HealthCheckDto?> HealthCheck(DbContext context);
    }
}

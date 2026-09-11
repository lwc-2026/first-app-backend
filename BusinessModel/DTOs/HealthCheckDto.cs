using System;
using System.Collections.Generic;
using System.Text;
using BusinessModel.Enums;

namespace BusinessModel.DTOs
{ 
    public class HealthCheckDto
    {
        public HealthCheckStatus HealthCheckStatus { get; set; } = HealthCheckStatus.Unhealthy;

        public void UpdateHealthCheckStatusToHealthy()
        {
            HealthCheckStatus = HealthCheckStatus.Healthy;
        }

        public bool IsHealthy()
        {
            return HealthCheckStatus == HealthCheckStatus.Healthy;
        }
    }
}
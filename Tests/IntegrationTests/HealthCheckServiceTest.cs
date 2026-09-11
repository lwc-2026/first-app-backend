using BusinessModel.DTOs;
using BusinessModel.Enums;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Fixtures;

namespace Tests.IntegrationTests
{
    [Collection("Integration")]
    public class HealthCheckServiceTest(DatabaseFixture fixture)
    {
        private DatabaseFixture _fixture = fixture;

        [Fact]
        public async Task Can_return_health_check_dto()
        {
            using var scope = _fixture.CreateScope();

            var healthCheckService = scope.ServiceProvider.GetRequiredService<IHealthCheckService>();

            var result = await healthCheckService.HealthCheck();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                new HealthCheckDto
                {
                    HealthCheckStatus = HealthCheckStatus.Healthy
                });        
        }
    }
}

using BusinessModel.DTOs;
using BusinessModel.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Controllers;

namespace Tests.UnitTests
{
    public class HealthCheckControllerTest
    {
        [Fact]
        public async Task Test_controller_can_return_ok()
        {
            Mock<IHealthCheckService> healthCheckService = new Mock<IHealthCheckService>();
            HealthCheckDto healthCheckResult = new HealthCheckDto()
            {
                HealthCheckStatus = HealthCheckStatus.Healthy
            };
            healthCheckService.Setup(x => x.HealthCheck())
                .ReturnsAsync(healthCheckResult);

            HealthCheckController controller = new HealthCheckController(healthCheckService.Object);

            var result = await controller.HealthCheck();

            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(healthCheckResult);
            healthCheckService.Verify(x => x.HealthCheck(), Times.Once, "HealthCheck method was not called exactly once on the healthCheckService.");
        }
    }
}

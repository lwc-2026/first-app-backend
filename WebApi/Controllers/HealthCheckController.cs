using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/healthcheck")]
    public class HealthCheckController(AppDbContext context, IHealthCheckService healthCheckService) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> HealthCheck()
        {
            var result = await healthCheckService.HealthCheck(context);
            if (result != null && result.IsHealthy())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, result);
            }
        }
    }
}

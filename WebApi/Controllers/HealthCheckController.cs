using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/healthcheck")]
    public class HealthCheckController(AppDbContext context) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> HealthCheck()
        {
            var result = context.HealthChecks
                .FromSql($"EXEC HEALTHCHECK")
                .AsEnumerable()
                .FirstOrDefault();

            return await Task.FromResult(Ok(result));
        }
    }
}

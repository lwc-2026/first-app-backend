using BusinessModel.DTOs;
using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        
        [HttpGet("asset-summary")]
        public IActionResult GetAssetSummary()
        {
            var result = _context.Database.SqlQuery<AssetSummaryDto>($"EXEC GetAssetSummary").ToList();
            return Ok(result);
        }
    }
}

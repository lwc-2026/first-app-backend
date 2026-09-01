using DataAccess.Dbcontexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IReadOnlyCollection<AppUser>> GetUsers()
        {
            var users = context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser>? GetUser(string id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFound();
            return user;
        }
    }
}

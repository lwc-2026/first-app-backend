using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Requests.Builders;
using WebApi.Requests;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<AppUser>>> GetUsers()
        {
            return Ok(await usersService.GetUsersList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>?> GetUser(string id)
        {
            return Ok(await usersService.GetUserById(id));
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>?> CreateUser(CreateUserServiceRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, AppUser user)
        {
            //if (id != user.Id)
            //{
            //    return BadRequest();
            //}

            //context.Entry(user).State = EntityState.Modified;

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{

            //}

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            //AppUser? user = await context.Users.FindAsync(id);
            //if(user == null) return NotFound();
            //context.Users.Remove(user);
            //await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

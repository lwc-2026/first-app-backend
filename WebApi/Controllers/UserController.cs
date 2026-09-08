using System.Text.Json;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Implementations;
using Service.Interfaces;

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
        public async Task<ActionResult<AppUser>?> CreateUser(AppUser user)
        {
            //context.Users.Add(user);
            //await context.SaveChangesAsync();
            //return CreatedAtAction("GetUser", new { id = user.Id }, user);
            return NoContent();
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

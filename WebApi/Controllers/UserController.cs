using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Requests;
using WebApi.Requests;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<AppUser>>> GetUsers()
        {
            return Ok(await usersService.GetUsersList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>?> GetUser([FromRoute] string id)
        {
            return Ok(await usersService.GetUserById(id));
        }   

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserHttpRequest request)
        {
            CreateUserServiceRequest serviceRequest = new(username: request.Username, email: request.Email,password: request.Password);
            AppUser user = await usersService.CreateUserAsync(serviceRequest);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserHttpRequest request, [FromRoute] string id)
        {
            UpdateUserServiceRequest serviceRequest = new UpdateUserServiceRequest()
            {
                Id = id,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };
            await usersService.UpdateUserAsync(serviceRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            DeleteUserServiceRequest serviceRequest = new(id);
            await usersService.DeleteUserAsync(serviceRequest);
            return NoContent();
        }
    }
}

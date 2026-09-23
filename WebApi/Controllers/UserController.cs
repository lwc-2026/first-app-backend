using BusinessModel.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service.Interfaces;
using Service.Requests;
using WebApi.Requests;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet(Name = "Users.Index")]
        public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetUsers()
        {
            return Ok(await usersService.GetUsersList());
        }

        [HttpGet("{id}", Name = "Users.Show")]
        public async Task<ActionResult<UserDto>?> GetUser([FromRoute] string id)
        {
            return Ok(await usersService.GetUserById(id));
        }   

        [HttpPost(Name = "Users.Store")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserHttpRequest request)
        {
            CreateUserServiceRequest serviceRequest = new(username: request.Username, email: request.Email,password: request.Password);
            UserDto user = await usersService.CreateUserAsync(serviceRequest);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}", Name = "Users.Update")]
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

        [HttpDelete("{id}", Name = "Users.Delete")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            DeleteUserServiceRequest serviceRequest = new(id);
            await usersService.DeleteUserAsync(serviceRequest);
            return NoContent();
        }
    }
}

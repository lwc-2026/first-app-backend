using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Service.Interfaces;
using WebApi.Requests;
using BusinessModel.Http.Requests;
using BusinessModel.DTOs;

namespace WebApi.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginHttpRequest request)
        {
            _logger.LogInformation("Attempting to authenticate user {Username}", request.Username);
            AuthDto? authDto = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (authDto == null || authDto.Token == null)
            {
                return Unauthorized();
            }
            _logger.LogInformation("User {Username} authenticated successfully", request.Username);
            _logger.LogInformation("The valid time for the access token is until {ExpiresAt}", authDto.ExpiresAt);
            return Ok(authDto);
        }

        // check refresh token valid and return new access token and refresh token
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthDto>> Refresh([FromBody] RefreshHttpRequest request)
        {
            _logger.LogInformation("Attempting to refresh token {RefreshToken}", request.RefreshToken);
            AuthDto? authDto = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (authDto == null || authDto.Token == null)
            {
                return Unauthorized();
            }
            _logger.LogInformation("Token refreshed successfully");
            _logger.LogInformation("The valid time for the new access token is until {ExpiresAt}", authDto.ExpiresAt);
            return Ok(authDto);
        }
    }
}

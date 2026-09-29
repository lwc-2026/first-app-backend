using System;
using System.Threading.Tasks;
using BusinessModel.DTOs;

namespace Service.Interfaces;

public interface IAuthService
{
    public Task<AuthDto?> AuthenticateAsync(string username, string password);
    public Task<AuthDto?> RefreshTokenAsync(string refreshToken);
    public Task LogoutAsync();
}

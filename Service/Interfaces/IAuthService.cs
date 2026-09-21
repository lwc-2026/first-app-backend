using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IAuthService
{
    public Task<string> AuthenticateAsync(string username, string password);
    public Task LogoutAsync();
}

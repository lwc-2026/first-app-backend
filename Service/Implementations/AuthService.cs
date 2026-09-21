using System;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Implementations;

public class AuthService: IAuthService
{
    public Task<string> AuthenticateAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }
}

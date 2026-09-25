using System;

namespace Service.Interfaces;

public interface IUserContext
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}

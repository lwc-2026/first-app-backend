using System;
using Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service.Implementations;

public class UserContext(IHttpContextAccessor httpContextAccessor): IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}

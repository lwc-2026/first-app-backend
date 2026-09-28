using System;
using System.Linq.Expressions;
using BusinessModel.DTOs;
using DataAccess.Entities;

namespace BusinessLogic.Projections;

public static class UserProjection
{
    public readonly static Func<AppUser, UserDto> ToSql = (user) =>
        new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
}

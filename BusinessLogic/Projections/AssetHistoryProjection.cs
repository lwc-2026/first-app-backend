using System;
using System.Linq.Expressions;
using BusinessModel.DTOs;
using DataAccess.Entities;

namespace BusinessLogic.Projections;

public class AssetHistoryProjection
{
    public readonly static Expression<Func<AssetHistory, AssetHistoryDto>> ToSql = (ah) =>
        new AssetHistoryDto
        {
            Id = ah.Id,
            AssetId = ah.AssetId,
            Action = ah.Action,
            Description = ah.Description,
            CreatedAt = ah.CreatedAt,
            UpdatedAt = ah.UpdatedAt,
            AssignedUser = ah.AssignedUser != null && ah.AssignedUserId != null ? new UserDto
            {
                Id = ah.AssignedUserId,
                Username = ah.AssignedUser.Username,
                Email = ah.AssignedUser.Email
            } : null,
            CreatedByUser = ah.CreatedByUser != null ? new UserDto
            {
                Id = ah.CreatedByUserId,
                Username = ah.CreatedByUser.Username,
                Email = ah.CreatedByUser.Email
            } : null            
        };
}

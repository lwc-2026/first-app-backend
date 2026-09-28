using System;
using System.Linq.Expressions;
using BusinessModel.DTOs;
using BusinessModel.Enums;
using DataAccess.Entities;

namespace BusinessLogic.Projections;

public static class AssetProjection
{
    public readonly static Expression<Func<Asset, AssetDto>> ToSql = (asset) =>
        new AssetDto
        {
            Id = asset.Id,
            SerialNo = asset.SerialNo,
            AssetNo = asset.AssetNo,
            Model = asset.Model,
            Status = asset.Status,
            User = asset.User != null ? new UserDto
            {
                Id = asset.User.Id,
                Username = asset.User.Username,
                Email = asset.User.Email
            }: null,
            CreatedAt = asset.CreatedAt,
            UpdatedAt = asset.UpdatedAt,
            DeletedAt = asset.DeletedAt,
            IsDeleted = asset.IsDeleted,
            AssetHistories = asset.AssetHistories != null ? asset.AssetHistories.Select(ah => new AssetHistoryDto
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
            }).ToList() : null,
        };

}

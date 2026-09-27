using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModel.Enums;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using DataAccess.Dbcontexts;
using Service.Requests;
using BusinessModel.DTOs;

namespace Service.Implementations;

public class AssetService(AppDbContext context) : IAssetService
{
    private readonly AppDbContext _context = context;
    public async Task<IEnumerable<AssetDto>> GetAssetsAsync()
    {
        return await _context.Assets
            .AsNoTracking()
            .Include(a => a.User)
            .Select(a => new AssetDto
            {
                Id = a.Id,
                SerialNo = a.SerialNo,
                AssetNo = a.AssetNo,
                Model = a.Model,
                Status = a.Status,
                User = a.User != null ? new UserDto
                {
                    Id = a.User.Id,
                    Username = a.User.Username,
                    Email = a.User.Email
                } : null,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                DeletedAt = a.DeletedAt,
                IsDeleted = a.IsDeleted
            })
            .ToListAsync();
    }

    public async Task<AssetDto> GetAssetByIdAsync(int id)
    {
        return await _context.Assets
            .Include(a => a.User)
            .Select(a => new AssetDto
            {
                Id = a.Id,
                SerialNo = a.SerialNo,
                AssetNo = a.AssetNo,
                Model = a.Model,
                Status = a.Status,
                User = a.User != null ? new UserDto
                {
                    Id = a.User.Id,
                    Username = a.User.Username,
                    Email = a.User.Email
                } : null,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                DeletedAt = a.DeletedAt,
                IsDeleted = a.IsDeleted
            })
            .FirstOrDefaultAsync(a => a.Id == id)
             ?? throw new KeyNotFoundException($"Asset with ID {id} not found.");
    }

    public async Task<AssetDto> CreateAssetAsync(CreateAssetServiceRequest request)
    {
        Asset asset = request.ToEntity();
        if (asset.UserId != null && !await this.UserExistsAsync(asset.UserId))
        {
            throw new KeyNotFoundException($"User with ID {asset.UserId} not found.");
        }
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return new AssetDto
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
            } : null,
            CreatedAt = asset.CreatedAt,
            UpdatedAt = asset.UpdatedAt,
            DeletedAt = asset.DeletedAt,
            IsDeleted = asset.IsDeleted
        };
    }

    public async Task UpdateAssetAsync(UpdateAssetServiceRequest request)
    {
        Asset asset = await GetAssetById(request.Id);
        // compare and update only the fields that are not null
        if (request.SerialNo != null) asset.SerialNo = request.SerialNo;
        if (request.AssetNo != null) asset.AssetNo = request.AssetNo;
        if (request.Model != null) asset.Model = request.Model;
        if (request.Status != null) asset.Status = (AssetStatus)request.Status;
        if (request.UserId != null)
        {
            if(!await this.UserExistsAsync(request.UserId))
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");    
            }
            asset.UserId = request.UserId;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAssetAsync(int id)
    {
        Asset asset = await GetAssetById(id);
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> UserExistsAsync(string userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task AssignAssetAsync(AssignAssetServiceRequest request, string requestUserId, bool forceFailure = false)
    {
        // create database transaction

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            Asset asset = await GetAssetById(request.AssetId);
            if(!await this.UserExistsAsync(request.UserId))
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");    
            }
            asset.UserId = request.UserId;
            AssetHistory history = new AssetHistory
            {
                AssetId = asset.Id,
                Description = request.Description,
                Action = AssetHistoryAction.Assigned,
                CreatedByUserId = requestUserId
            };
            _context.AssetHistories.Add(history);
            await _context.SaveChangesAsync();

            if (forceFailure)
            {
                throw new InvalidOperationException("Forced failure after assigning asset.");
            }

            await transaction.CommitAsync();
        }catch(Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task ReturnAssetAsync(ReturnAssetServiceRequest request, string requestUserId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            Asset asset = await GetAssetById(request.AssetId);
            asset.UserId = null;
            AssetHistory history = new AssetHistory
            {
                AssetId = asset.Id,
                Description = request.Description,
                Action = AssetHistoryAction.Returned,
                CreatedByUserId = requestUserId
            };
            _context.AssetHistories.Add(history);
            await _context.SaveChangesAsync();            
        }catch(Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }

        await transaction.CommitAsync();
    }

    private async Task<Asset> GetAssetById(int id)
    {
        return await _context.Assets.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Asset with ID {id} not found.");
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModel.Enums;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using DataAccess.Dbcontexts;
using Service.Requests;

namespace Service.Implementations;

public class AssetService(AppDbContext context) : IAssetService
{
    private readonly AppDbContext _context = context;
    public async Task<IEnumerable<Asset>> GetAssetsAsync()
    {
        return await _context.Assets
            .AsNoTracking()
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<Asset> GetAssetByIdAsync(int id)
    {
        return await _context.Assets
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Asset with ID {id} not found.");
    }

    public async Task<Asset> CreateAssetAsync(CreateAssetServiceRequest request)
    {
        Asset asset = request.ToEntity();
        if (asset.UserId != null && !await this.UserExistsAsync(asset.UserId))
        {
            throw new KeyNotFoundException($"User with ID {asset.UserId} not found.");
        }
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset;
    }

    public async Task UpdateAssetAsync(UpdateAssetServiceRequest request)
    {
        Asset asset = await GetAssetByIdAsync(request.Id);
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
        Asset asset = await GetAssetByIdAsync(id);
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> UserExistsAsync(string userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task AssignAssetAsync(AssignAssetServiceRequest request)
    {
        Asset asset = await GetAssetByIdAsync(request.AssetId);
        if(!await this.UserExistsAsync(request.UserId))
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");    
        }
        asset.UserId = request.UserId;
        await _context.SaveChangesAsync();
    }

    public async Task ReturnAssetAsync(ReturnAssetServiceRequest request)
    {
        Asset asset = await GetAssetByIdAsync(request.AssetId);
        if(asset.UserId != request.UserId)
        {
            throw new InvalidOperationException($"Asset with ID {request.AssetId} is not assigned to user with ID {request.UserId}.");
        }
        asset.UserId = null;
        await _context.SaveChangesAsync();
    }
}

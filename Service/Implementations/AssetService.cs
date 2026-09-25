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
        return await _context.Assets.Include(a => a.User).ToListAsync();
    }

    public async Task<Asset> GetAssetByIdAsync(int id)
    {
        return await _context.Assets.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Asset with ID {id} not found.");
    }

    public async Task<Asset> CreateAssetAsync(CreateAssetServiceRequest request)
    {
        Asset asset = request.ToEntity();
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
        if (request.UserId != null) asset.UserId = request.UserId;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAssetAsync(int id)
    {
        Asset asset = await GetAssetByIdAsync(id);
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }
}

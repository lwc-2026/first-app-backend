using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using BusinessModel.DTOs;
using Service.Requests;

namespace Service.Interfaces;

public interface IAssetService
{
    public Task<IEnumerable<AssetDto>> GetAssetsAsync();
    public Task<AssetDto> GetAssetByIdAsync(int id);
    public Task<AssetDto> CreateAssetAsync(CreateAssetServiceRequest request);
    public Task UpdateAssetAsync(UpdateAssetServiceRequest request);
    public Task DeleteAssetAsync(int id);
    public Task AssignAssetAsync(AssignAssetServiceRequest request, string userId, bool forceFailure = false);
    public Task ReturnAssetAsync(ReturnAssetServiceRequest request, string userId);
}

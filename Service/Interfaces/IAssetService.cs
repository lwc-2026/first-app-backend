using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using Service.Requests;

namespace Service.Interfaces;

public interface IAssetService
{
    public Task<IEnumerable<Asset>> GetAssetsAsync();
    public Task<Asset> GetAssetByIdAsync(int id);
    public Task<Asset> CreateAssetAsync(CreateAssetServiceRequest request);
    public Task UpdateAssetAsync(UpdateAssetServiceRequest request);
    public Task DeleteAssetAsync(int id);
}

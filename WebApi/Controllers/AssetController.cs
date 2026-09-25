using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Requests;
using Service.Interfaces;
using Service.Requests;

namespace WebApi.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetController(IAssetService assetService) : ControllerBase
    {
        private IAssetService _assetService = assetService;

        [HttpGet]
        public async Task<IActionResult> GetAssets()
        {
            var assets = await _assetService.GetAssetsAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsset([FromRoute] int id)
        {
            // Placeholder for getting a single asset by id
            var asset = await _assetService.GetAssetByIdAsync(id);
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetHttpRequest request)
        {
            // Placeholder for creating an asset
            CreateAssetServiceRequest serviceRequest = new CreateAssetServiceRequest(
                request.SerialNo,
                request.AssetNo,
                request.Model,
                request.Status,
                request.UserId
            );
            var asset = await _assetService.CreateAssetAsync(serviceRequest);
            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset([FromBody] UpdateAssetHttpRequest request, [FromRoute] int id)
        {
            UpdateAssetServiceRequest serviceRequest = new UpdateAssetServiceRequest(
                id,
                request.SerialNo,
                request.AssetNo,
                request.Model,
                request.Status,
                request.UserId
            );
            await _assetService.UpdateAssetAsync(serviceRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset([FromRoute] int id)
        {
            await _assetService.DeleteAssetAsync(id);
            return NoContent();
        }
    }
}

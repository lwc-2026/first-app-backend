using System;

namespace BusinessModel.DTOs;

public class AssetSummaryDto(string Model, int AssetCount)
{
    public string Model { get; set; } = Model;
    public int AssetCount { get; set; } = AssetCount;
}

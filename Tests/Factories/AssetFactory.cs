using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tests.Builders;
using DataAccess.Entities;

namespace Tests.Factories;

public class AssetFactory(TimeProvider timeProvider) : Factory
{
    public Asset Create([Optional] object[] parameters)
    {
        AssetBuilder builder = new AssetBuilder(timeProvider);
        return builder.Build();
    }

    public List<Asset> CreateMany(int count)
    {
        var assets = new List<Asset>();
        for (int i = 0; i < count; i++)
        {
            assets.Add(Create());
        }
        return assets;
    }
}

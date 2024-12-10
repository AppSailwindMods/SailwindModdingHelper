using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper.Shops
{
    public static class ShopItemSpawnerHandler
    {
        internal static List<ShopItemIslandData> spawnData = new List<ShopItemIslandData>();

        public static ShopItemIslandData AddShopItemSpawner(int islandIndex, ShopItemSpawnerData data, Vector3 localPosition, Vector3 rotation)
        {
            var p = new ShopItemIslandData(data, islandIndex, localPosition, rotation);
            spawnData.Add(p);
            return p;
        }

        public class ShopItemIslandData
        {
            public readonly ShopItemSpawnerData data;
            public readonly int islandIndex;
            public SMShopItemSpawner shopItemSpawner;
            internal Vector3 localPosition, rotation;

            public ShopItemIslandData(ShopItemSpawnerData data, int islandIndex, Vector3 localPosition, Vector3 rotation)
            {
                this.data = data;
                this.islandIndex = islandIndex;
                this.shopItemSpawner = null;
                this.localPosition = localPosition;
                this.rotation = rotation;
            }
        }
    }
}

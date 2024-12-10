using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindModdingHelper.Shops
{
    public class ShopItemSpawnerData
    {
        public float respawnTime = 120;
        public bool availableAtNight;

        public virtual ShipItem OnItemSpawn()
        {
            return null;
        }
    }

    public class GenericShopItemSpawnerData : ShopItemSpawnerData
    {
        private System.Func<ShipItem> itemSpawn;

        public GenericShopItemSpawnerData(Func<ShipItem> itemSpawn)
        {
            this.itemSpawn = itemSpawn;
        }

        public override ShipItem OnItemSpawn()
        {
            return itemSpawn?.Invoke();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper.Shops
{
    public class SMShopItemSpawner : ShopItemSpawner
    {
        private static int spawnsThisFrame { get { return Extensions.GetPrivateField<int, ShopItemSpawner>("spawnsThisFrame"); } set { Extensions.SetPrivateField<ShopItemSpawner>("spawnsThisFrame", value); } }
        public ShipItem item { get { return this.GetPrivateField<ShipItem>("item"); } set { this.SetPrivateField("item", value); } }
        public bool itemAvailableAtShop { get { return this.GetPrivateField<bool>("itemAvailableAtShop"); } set { this.SetPrivateField("itemAvailableAtShop", value); } }
        // Cooldown until shop is restocked after item is bought
        public float respawnTimer { get { return this.GetPrivateField<float>("respawnTimer"); } set { this.SetPrivateField("respawnTimer", value); } }
        public bool spawnedLastFrame { get { return this.GetPrivateField<bool>("spawnedLastFrame"); } set { this.SetPrivateField("spawnedLastFrame", value); } }
        public float timer { get { return this.GetPrivateField<float>("timer"); } set { this.SetPrivateField("timer", value); } }
        public new bool availableAtNight => spawnerData.availableAtNight;
        public float respawnTime => spawnerData.respawnTime;

        internal ShopItemSpawnerData spawnerData;

        protected virtual void SpawnItem()
        {
            spawnsThisFrame++;
            /*GameObject prefab = Instantiate(GameAssets.GetPrefabGameObject("crate salmon (E)"));
            Destroy(prefab.GetComponent<Good>());
            Destroy(prefab.GetComponent<ShipItem>());
            prefab.transform.position = transform.position;
            prefab.transform.rotation = transform.rotation;
            prefab.transform.localScale = Vector3.one * 0.5f;
            //prefab.AddComponent<Rigidbody>();
            prefab.name = "Shop prefab cube";
            //prefab.AddComponent<SaveablePrefab>().prefabIndex = 500;
            Destroy(prefab.GetComponent<ShipItem>());
            item = prefab.AddComponent<ShipItemLuckyCrate>();
            item.name = "Lucky Crate";*/
            OnSpawnItem();
            if (!item) return;
            this.item.transform.parent = base.transform;
            item.transform.localPosition = Vector3.zero;
            this.itemAvailableAtShop = true;
            Good component = this.item.GetComponent<Good>();
            if (component)
            {
                component.RegisterAsMissionless();
            }
            ShipItemScroll component2 = this.item.GetComponent<ShipItemScroll>();
            if (component2)
            {
                component2.amount = 3f;
            }
        }

        protected virtual void OnSpawnItem()
        {
            item = spawnerData.OnItemSpawn();
        }

        protected virtual void Update()
        {
            if (!GameState.playing)
            {
                return;
            }
            if (this.respawnTimer > 0f && !this.itemAvailableAtShop)
            {
                this.respawnTimer -= Time.deltaTime;
            }
            if (!this.itemAvailableAtShop)
            {
                if (this.timer <= 0f && this.respawnTimer <= 0f)
                {
                    this.timer = Random.Range(0.25f, 1.2f);
                    if (GameState.justStarted || Vector3.Distance(Camera.main.transform.position, base.transform.position) < 500f)
                    {
                        this.SpawnItem();
                    }
                }
                this.timer -= Time.deltaTime;
                return;
            }
            if (this.item)
            {
                if (this.item.sold)
                {
                    ResetItemSpawn();
                    return;
                }
                if (!this.availableAtNight)
                {
                    if (Sun.sun.localTime > 18f || Sun.sun.localTime < 7f)
                    {
                        if (this.item.gameObject.activeInHierarchy)
                        {
                            this.item.gameObject.SetActive(false);
                            return;
                        }
                    }
                    else if (!this.item.gameObject.activeInHierarchy)
                    {
                        this.item.gameObject.SetActive(true);
                    }
                }
            }
        }

        public virtual void ResetItemSpawn()
        {
            this.itemAvailableAtShop = false;
            this.item = null;
            this.respawnTimer = respawnTime;
        }
    }
}

using HarmonyLib;
using SailwindModdingHelper.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SailwindModdingHelper.Patches
{
    internal static class ShopItemSpawnPatches
    {
        [HarmonyPatch(typeof(IslandHorizon), "LoadIslandScene")]
        public static class IslandHorizonLoadIslandScene
        {
            [HarmonyPrefix]
            private static bool Prefix(IslandHorizon __instance)
            {
                __instance.SetPrivateField("loadUnloadCoolown", 12);
                Debug.Log(string.Concat(new object[]
                {
        "Loading scene ",
        __instance.islandIndex,
        ". sceneLoaded is ",
        __instance.GetPrivateField<bool>("sceneLoaded").ToString()
                }));
                __instance.SetPrivateField("sceneLoaded", true);
                SceneManager.LoadSceneAsync(__instance.islandIndex, LoadSceneMode.Additive);
                GameState.loadingScenes++;
                __instance.StartCoroutine(RegisterLoadingFinished(__instance));
                return false;
            }

            private static IEnumerator RegisterLoadingFinished(IslandHorizon __instance)
            {
                while (!SceneManager.GetSceneByBuildIndex(__instance.islandIndex).isLoaded)
                {
                    yield return new WaitForEndOfFrame();
                }
                foreach (var data in ShopItemSpawnerHandler.spawnData)
                {
                    if(data.islandIndex == __instance.islandIndex)
                    {
                        if(data.shopItemSpawner)
                            GameObject.Destroy(data.shopItemSpawner);
                        var root = SceneManager.GetSceneByBuildIndex(data.islandIndex).GetRootGameObjects()[0];
                        var itemShopSpawner = new GameObject();
                        itemShopSpawner.name = "Item Shop Spawner";
                        itemShopSpawner.transform.parent = root.transform;
                        itemShopSpawner.transform.localPosition = data.localPosition;
                        itemShopSpawner.transform.rotation = Quaternion.Euler(data.rotation);
                        data.shopItemSpawner = itemShopSpawner.AddComponent<SMShopItemSpawner>();
                        data.shopItemSpawner.spawnerData = data.data;
                    }
                }
                GameState.loadingScenes--;
                yield break;
            }
        }

        [HarmonyPatch(typeof(IslandHorizon), "UnloadIslandScene")]
        public static class IslandHorizonUnloadIslandScene
        {
            [HarmonyPrefix]
            private static void Postfix(IslandHorizon __instance)
            {
                if (!__instance.GetPrivateField<bool>("unloading")) return;
                foreach (var data in ShopItemSpawnerHandler.spawnData)
                {
                    if (data.islandIndex == __instance.islandIndex)
                    {
                        if (data.shopItemSpawner)
                            GameObject.Destroy(data.shopItemSpawner);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "AddLODGroup")]
        public static class AddLODGroup
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItem __instance)
            {
                LODGroup lodgroup = __instance.gameObject.GetComponent<LODGroup>();
                if (!lodgroup)
                    lodgroup = __instance.gameObject.AddComponent<LODGroup>();
                LOD[] array = (LOD[])RefsDirectory.instance.LODtemplateItems.GetLODs().Clone();
                array[0].renderers = new Renderer[1];
                array[0].renderers[0] = __instance.GetComponent<Renderer>();
                lodgroup.SetLODs(array);
                return false;
            }
        }
    }
}

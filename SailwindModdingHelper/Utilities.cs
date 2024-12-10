using BepInEx;
using BepInEx.Bootstrap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper
{
    public static class Utilities
    {
        private static CharacterController characterController;
        internal static Transform playerTransform;
        public static CharacterController CharacterController => characterController;
        public static Transform PlayerTransform => playerTransform;

        public static bool GamePaused { get; internal set; }

        internal static void SetPlayerController(CharacterController characterController)
        {
            Utilities.characterController = characterController;
        }

        public static IslandSceneryScene GetNearestIslandSceneryScene(Vector3 position)
        {
            float closestDistance = float.MaxValue;
            IslandSceneryScene closestIsland = null;
            foreach (var island in GameObject.FindObjectsOfType<IslandSceneryScene>())
            {
                float distance = Vector3.Distance(island.transform.position, position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIsland = island;
                }
            }
            return closestIsland;
        }

        public static Vector3 GetPlayerGlobeCoords()
        {
            return FloatingOriginManager.instance.GetGlobeCoords(playerTransform);
        }

        public static Vector3 GetGlobeCoords(Vector3 position)
        {
            return (position - FloatingOriginManager.instance.outCurrentOffset - FloatingOriginManager.instance.GetPrivateField<Vector3>("globeOffset")) / 9000f;
        }

        public static T Next<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public struct PluginFile
        {
            public readonly PluginInfo pluginInfo;
            public readonly string filePath;

            public PluginFile(PluginInfo pluginInfo, string filePath)
            {
                this.pluginInfo = pluginInfo;
                this.filePath = filePath;
            }
        }

        public static List<PluginFile> GetFileInAllPlugins(string fileName, string filePath, params string[] extraFilePaths)
        {
            var files = new List<PluginFile>();
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var directory = plugin.Value.GetFolderLocation();
                {
                    var fullPath = Path.Combine(directory, fileName + "." + filePath);
                    if (File.Exists(fullPath))
                    {
                        files.Add(new PluginFile(plugin.Value, fullPath));
                    }
                }
                foreach (var path in extraFilePaths)
                {
                    var fullPath = Path.Combine(directory, fileName+"."+path);
                    if (File.Exists(fullPath))
                    {
                        files.Add(new PluginFile(plugin.Value, fullPath));
                    }
                }
            }
            return files;
        }

        public static RegionData LoadRegionData(string regionName)
        {
            var regionFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "regions", regionName+".json");
            if (!File.Exists(regionFilePath))
            {
                SailwindModdingHelperMain.instance.Info.LogError($"Could not load region data for region '{regionName}', could not locate file '{regionFilePath}'");
                return null;
            }

            var data = JsonConvert.DeserializeObject<float[]>(File.ReadAllText(regionFilePath));

            if(data.Length < 4)
            {
                SailwindModdingHelperMain.instance.Info.LogError($"Could not load region data for region '{regionName}', missing parameters");
                return null;
            }

            SailwindModdingHelperMain.instance.Info.LogInfo($"Loaded region '{regionName}'");
            return new RegionData(new Vector3(data[0], 0, data[1]), new Vector3(data[2], 0, data[3]));
        }

        public static AssetBundle LoadAssetBundle(string filePath)
        {
            var bundle = AssetBundle.LoadFromFile(filePath);
            if (!bundle)
            {
                SailwindModdingHelperMain.instance.Info.LogError($"Could not load bundle at {filePath}");
                return null;
            }
            return bundle;
        }
    }
}

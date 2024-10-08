﻿using BepInEx.Bootstrap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace SailwindModdingHelper
{
    public sealed class RegionData
    {
        public Vector3 globePosition;
        public Vector3 globeSize;
        public Bounds globeBounds => new Bounds(globePosition, globeSize);

        public RegionData(Vector3 globePosition, Vector3 globeSize)
        {
            this.globePosition = globePosition;
            this.globeSize = globeSize;
        }
    }

    public static class GameAssets
    {
        public static Font ArealFont { get; internal set; }

        public static Region AlAnkhRegion { get; internal set; }
        public static Region EmeraldRegion { get; internal set; }
        /// <summary>
        /// Region for Aestrin
        /// </summary>
        public static Region MediRegion { get; internal set; }
        public static Region MediEastRegion { get; internal set; }
        /// <summary>
        /// Region for Fire Fish Lagoon
        /// </summary>
        public static Region LagoonRegion { get; internal set; }

        public static RegionData AlAnkhRegionData { get; internal set; }
        public static RegionData EmeraldRegionData { get; internal set; }
        /// <summary>
        /// Region Data for Aestrin
        /// </summary>
        public static RegionData MediRegionData { get; internal set; }
        public static RegionData OceanRegionData { get; internal set; }

        public static StartMenu StartMenu { get; internal set; }

        public static Font ImmortalFont { get; internal set; }

        public static IReadOnlyList<string> AlcoholNames { get; private set; }

        public static IReadOnlyList<SailwindPrefab> Prefabs { get; private set; }

        public static bool IsAlcohol(string liquidName)
        {
            return AlcoholNames.Contains(liquidName.ToLower());
        }

        public static bool IsAlcohol(float liquidIndexAsAmount)
        {
            return IsAlcohol(Liquids.GetLiquidName(liquidIndexAsAmount));
        }

        internal static void LoadPrefabs()
        {
            List<SailwindPrefab> prefabs = new List<SailwindPrefab>();
            foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                List<string> parts = new List<string>(obj.name.Split(' '));
                if (int.TryParse(parts[0], out _))
                {
                    parts.RemoveAt(0);
                }
                string name = string.Join(" ", parts);
                prefabs.Add(new SailwindPrefab(obj.name, name, obj));
            }
            Prefabs = prefabs;
        }

        internal static void LoadAlcohols()
        {
            var alcoholNames = new List<string>();
            var alcoholFiles = Utilities.GetFileInAllPlugins("alcohols", "json");
            foreach(var alcoholFile in alcoholFiles)
            {
                try
                {
                    var fileAlcoholNames = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(alcoholFile.filePath));

                    foreach (var name in fileAlcoholNames)
                    {
                        if (!string.IsNullOrEmpty(name.ToLower()))
                        {
                            if (!alcoholNames.Contains(name.ToLower()))
                                alcoholNames.Add(name.ToLower());
                        }
                    }
                }
                catch(Exception ex)
                {
                    SailwindModdingHelperMain.instance.Info.LogFatal($"Could not load file '{Path.GetFileName(alcoholFile.filePath)}' for mod '{alcoholFile.pluginInfo.Metadata.GUID}'");
                    SailwindModdingHelperMain.instance.Info.LogFatal(ex);
                }
            }

            AlcoholNames = alcoholNames;
            SailwindModdingHelperMain.instance.Info.LogInfo("Loaded all alcohols");
        }

        /// <summary>
        /// Returns a prefab <see cref="GameObject"/> that matches the <paramref name="prefabName"/>
        /// </summary>
        /// <param name="prefabName">Can either be the full prefab name, or the shorten version</param>
        /// <returns>The prefab <see cref="GameObject"/></returns>
        public static GameObject GetPrefabGameObject(string prefabName)
        {
            var prefab = GetPrefab(prefabName);
            return prefab.PrefabGameObject;
        }

        /// <summary>
        /// Returns a <see cref="SailwindPrefab"/> that matches the <paramref name="prefabName"/>
        /// </summary>
        /// <param name="prefabName">Can either be the full prefab name, or the shorten version</param>
        /// <returns>A <see cref="SailwindPrefab"/></returns>
        public static SailwindPrefab GetPrefab(string prefabName)
        {
            string lower = prefabName.ToLower();
            foreach (var prefab in Prefabs)
            {
                if (prefab.PrefabName.ToLower() == lower || prefab.FullPrefabName.ToLower() == lower) return prefab;
            }
            return new SailwindPrefab("", "", null);
        }
    }

    public struct SailwindPrefab
    {
        internal SailwindPrefab(string fullPrefabName, string prefabName, GameObject prefabGameObject)
        {
            FullPrefabName = fullPrefabName;
            PrefabName = prefabName;
            PrefabGameObject = prefabGameObject;
        }

        /// <summary>
        /// Some prefabs have an index prefix in their name, <see cref="PrefabName">PrefabName</see> removes this prefix, whilst <see cref="FullPrefabName">FullPrefabName</see> keeps it
        /// </summary>
        public string FullPrefabName { get; }
        /// <summary>
        /// <inheritdoc cref="FullPrefabName"/>
        /// </summary>
        public string PrefabName { get; }
        /// <summary>
        /// The <see cref="GameObject"/> of the prefab
        /// </summary>
        public GameObject PrefabGameObject { get; }
    }
}

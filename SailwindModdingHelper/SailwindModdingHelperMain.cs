using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace SailwindModdingHelper
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class SailwindModdingHelperMain : BaseUnityPlugin
    {
        public const string GUID = "com.app24.sailwindmoddinghelper";
        public const string NAME = "Sailwind Modding Helper";
        public const string VERSION = "2.2.0";

        internal static ManualLogSource logSource;

        internal static SailwindModdingHelperMain instance;

        private void Awake()
        {
            instance = this;
            logSource = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            ModLoading.LoadMod();
        }
    }
}
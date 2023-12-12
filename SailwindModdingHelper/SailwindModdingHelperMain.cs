using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace SailwindModdingHelper
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class SailwindModdingHelperMain : BaseUnityPlugin
    {
        public const string GUID = "com.app24.sailwindmoddinghelper";
        public const string NAME = "Sailwind Modding Helper";
        public const string VERSION = "2.0.1";

        internal static ManualLogSource logSource;

        private void Awake()
        {
            logSource = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            ModLoading.LoadMod();
        }
    }
}
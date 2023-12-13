#if BepInEx
using BepInEx.Logging;
#elif UMM
using UnityModManagerNet;
#endif
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper
{
    internal static class ModLoading
    {
        public static void LoadMod()
        {
            ModLogger.OnLog += (logLevel, mod, message) =>
            {
                var manualLogSource = BepInEx.Logging.Logger.CreateLogSource(mod.Metadata.Name);

                manualLogSource.Log(logLevel, message);

                BepInEx.Logging.Logger.Sources.Remove(manualLogSource);
            };

            GameEvents.OnGameStart += (_, __) =>
            {
                Utilities.SetPlayerController(Refs.charController);
            };

            GameEvents.OnGameStart += (_, __) =>
            {
                GameAssets.ArealFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

                GameAssets.AlAnkhRegion = GameObject.Find("Region Al'ankh").GetComponent<Region>();
                GameAssets.EmeraldRegion = GameObject.Find("Region Emerald (new smaller)").GetComponent<Region>();
                GameAssets.LagoonRegion = GameObject.Find("Region Emerald Lagoon").GetComponent<Region>();
                GameAssets.MediRegion = GameObject.Find("Region Medi").GetComponent<Region>();
                GameAssets.MediEastRegion = GameObject.Find("Region Medi East").GetComponent<Region>();

                GameAssets.StartMenu = GameObject.FindObjectOfType<StartMenu>();

                GameAssets.LoadPrefabs();
            };

            GameEvents.OnGameStart += (_, __) =>
            {
                GameObject settingsUi = GameAssets.StartMenu.GetPrivateField<GameObject>("settingsUI");
                //UIManager.settingsUI = settingsUi;
                if (settingsUi)
                {
                    foreach (Transform child in settingsUi.transform)
                    {
                        if (child.name == "button recover")
                        {
                            Transform bg_trigger = child.GetChildByName("bg+trigger");
                            UIManager.blueButtonMaterial = bg_trigger.GetComponent<MeshRenderer>().sharedMaterial;
                            UIManager.buttonMesh = bg_trigger.GetComponent<MeshFilter>().sharedMesh;
                            GameAssets.ImmortalFont = child.GetChildByName("text").GetComponent<TextMesh>().font;
                            UIManager.uiLayer = child.gameObject.layer;
                        }
                        else if (child.name == "button back")
                        {
                            UIManager.whiteButtonMaterial = child.GetChildByName("bg+trigger").GetComponent<MeshRenderer>().sharedMaterial;
                        }
                    }
                }
            };

            GameEvents.OnGamePause += (_, __) =>
            {
                Utilities.GamePaused = true;
            };

            GameEvents.OnGameUnpause += (_, __) =>
            {
                Utilities.GamePaused = false;
            };

            GameEvents.OnNewGame += (_, e) =>
            {
                if (Directory.Exists(ModSave.GetSaveDirectory(e.SaveSlot)))
                {
                    Directory.Delete(ModSave.GetSaveDirectory(e.SaveSlot), true);
                }
            };

            GameEvents.OnGameStart += (_, __) =>
            {
                GameCanvas.InitializeCanvas();
                //ModButton.InitConsole();
            };
        }
    }
}

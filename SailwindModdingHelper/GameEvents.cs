using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindModdingHelper
{
    public static class GameEvents
    {
        public static event EventHandler OnGameStart;
        public static event GameQuitEventHandler OnGameQuit;

        public static event EventHandler OnGamePause;
        public static event EventHandler OnGameUnpause;

        public static event SaveSlotEventHandler OnSaveLoad;
        public static event SaveSlotEventHandler OnSaveLoadPost;
        public static event SaveSlotEventHandler OnNewGame;
        public static event GameSaveEventHandler OnGameSave;

        public static event EventHandler OnSleep;
        public static event EventHandler OnWakeUp;
        public static event EventHandler OnBedEnter;
        public static event EventHandler OnBedLeave;

        public static event EventHandler OnPlayerInput;

        public static event Sun.NewDay OnNewDay
        {
            add
            {
                Sun.OnNewDay += value;
            }
            remove
            {
                Sun.OnNewDay -= value;
            }
        }

        public delegate void SaveSlotEventHandler(object sender, SaveSlotEventArgs e);
        public delegate void GameSaveEventHandler(object sender, SaveSlotEventArgs e);
        public delegate void GameQuitEventHandler(object sender, GameQuitEventArgs e);

        [HarmonyPatch(typeof(Sun), "Start")]
        private static class GameStartPatch
        {
            [HarmonyPostfix]
            public static void Postfix(Sun __instance)
            {
                OnGameStart?.Invoke(__instance, new EventArgs());
            }
        }


        [HarmonyPatch(typeof(StartMenu), "ButtonClick", typeof(StartMenuButtonType))]
        private static class GameQuit
        {
            [HarmonyPostfix]
            public static void Postfix(StartMenu __instance, StartMenuButtonType button)
            {
                if (button == StartMenuButtonType.Quit)
                {
                    OnGameQuit?.Invoke(__instance, new GameQuitEventArgs(GameState.playing));
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "GameToSettings")]
        private static class GamePausePatch
        {
            [HarmonyPrefix]
            public static void Prefix(StartMenu __instance)
            {

                OnGamePause?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(StartMenu), "SettingsToGame")]
        private static class GameUnpausePatch
        {
            [HarmonyPrefix]
            public static void Prefix(StartMenu __instance)
            {
                OnGameUnpause?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(StartMenu), "StartNewGame")]
        private static class SaveLoadNew
        {
            [HarmonyPostfix]
            public static void Postfix(StartMenu __instance)
            {
                OnNewGame?.Invoke(__instance, new SaveSlotEventArgs(SaveSlots.currentSlot));
            }
        }

        [HarmonyPatch(typeof(SaveLoadManager), "LoadGame")]
        private static class SaveLoad
        {
            [HarmonyPostfix]
            public static void Postfix(StartMenu __instance)
            {
                OnSaveLoad?.Invoke(__instance, new SaveSlotEventArgs(SaveSlots.currentSlot));
                OnSaveLoadPost?.Invoke(__instance, new SaveSlotEventArgs(SaveSlots.currentSlot));
            }
        }

        [HarmonyPatch(typeof(SaveLoadManager), "SaveModData")]
        private static class SaveSave
        {
            [HarmonyPostfix]
            public static void Postfix(SaveLoadManager __instance)
            {
                OnGameSave?.Invoke(__instance, new SaveSlotEventArgs(SaveSlots.currentSlot));
            }
        }

        [HarmonyPatch(typeof(Sleep), "FallAsleep")]
        private static class FallAsleep
        {
            [HarmonyPostfix]
            public static void Postfix(Sleep __instance)
            {
                OnSleep?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(Sleep), "WakeUp")]
        private static class WakeUp
        {
            [HarmonyPostfix]
            public static void Postfix(Sleep __instance)
            {
                OnWakeUp?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(Sleep), "EnterBed")]
        private static class EnterBed
        {
            [HarmonyPostfix]
            public static void Postfix(Sleep __instance)
            {
                OnBedEnter?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(Sleep), "LeaveBed")]
        private static class LeaveBed
        {
            [HarmonyPostfix]
            public static void Postfix(Sleep __instance)
            {
                OnBedLeave?.Invoke(__instance, new EventArgs());
            }
        }

        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        private static class PlayerInput
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerCrouching __instance)
            {
                if (!GameState.playing || Utilities.GamePaused) return;

                OnPlayerInput?.Invoke(__instance, new EventArgs());
            }
        }
    }

    public sealed class SaveSlotEventArgs
    {
        public int SaveSlot { get; }

        public SaveSlotEventArgs(int saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }

    public sealed class GameQuitEventArgs
    {
        public bool QuitFromSave { get; }

        public GameQuitEventArgs(bool quitFromSave)
        {
            QuitFromSave = quitFromSave;
        }
    }
}

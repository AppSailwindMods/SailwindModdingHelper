using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper
{
    public static class ModSave
    {
        public static void Save(PluginInfo pluginInfo, object data)
        {
            if (!GameState.playing) return;
            Directory.CreateDirectory(GetSaveDirectory(SaveSlots.currentSlot));

            FileStream stream = File.Create(GetSaveModFile(SaveSlots.currentSlot, pluginInfo));
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, data);
            }
            catch (Exception ex)
            {
                ModLogger.LogError(pluginInfo, $"Could not serialize data '{pluginInfo.Metadata.GUID}'");
                ModLogger.LogError(pluginInfo, ex.Message);
            }
            stream.Close();
        }

        public static bool Load(PluginInfo pluginInfo, out object loadedObject)
        {
            loadedObject = null;
            if (!GameState.playing && !GameState.currentlyLoading)
            {
                ModLogger.LogError(pluginInfo, $"Trying to load mod save while in main menu '{pluginInfo.Metadata.GUID}'");
                return false;
            }
            if (!File.Exists(GetSaveModFile(SaveSlots.currentSlot, pluginInfo)))
            {
                ModLogger.LogError(pluginInfo, $"Could not find mod save file for '{pluginInfo.Metadata.GUID}'");
                return false;
            }

            FileStream stream = File.OpenRead(GetSaveModFile(SaveSlots.currentSlot, pluginInfo));

            if (stream.Length <= 0)
            {
                stream.Close();
                ModLogger.LogError(pluginInfo, $"File stream length is 0 '{pluginInfo.Metadata.GUID}'");
                return false;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object value = formatter.Deserialize(stream);
                stream.Close();
                loadedObject = value;
                return true;
            }
            catch (Exception ex)
            {
                stream.Close();
                ModLogger.LogError(pluginInfo, $"Could not deserialize mod save for '{pluginInfo.Metadata.GUID}'");
                ModLogger.LogError(pluginInfo, ex.Message);
                return false;
            }
        }

        public static bool Load<T>(PluginInfo pluginInfo, out T loadedObject)
        {
            loadedObject = default;
            var result = Load(pluginInfo, out var obj);
            if(result)
                loadedObject = (T)obj;
            return result;
        }

        internal static string GetSaveModFile(int slot, PluginInfo pluginInfo)
        {
            return Path.Combine(GetSaveDirectory(slot), $"{pluginInfo.Metadata.GUID}.save");
        }

        public static string GetSaveDirectory(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"slot{slot}");
        }
    }
}

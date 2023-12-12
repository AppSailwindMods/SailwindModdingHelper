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
        public static void Save(string modId, object data)
        {
            if (!GameState.playing) return;
            Directory.CreateDirectory(GetSaveDirectory(SaveSlots.currentSlot));

            FileStream stream = File.Create(GetSaveModFile(SaveSlots.currentSlot, modId));
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, data);
            }
            catch (Exception ex)
            {
                ModLogger.Error(modId, $"Could not serialize data '{modId}'");
                ModLogger.Error(modId, ex.Message);
            }
            stream.Close();
        }

        public static object Load(string modId)
        {
            if (!GameState.playing)
            {
                ModLogger.Error(modId, $"Trying to load mod save while in main menu '{modId}'");
                return null;
            }
            if (!File.Exists(GetSaveModFile(SaveSlots.currentSlot, modId)))
            {
                ModLogger.Error(modId, $"Could not find mod save file for '{modId}'");
                return null;
            }

            FileStream stream = File.OpenRead(GetSaveModFile(SaveSlots.currentSlot, modId));

            if (stream.Length <= 0)
            {
                stream.Close();
                ModLogger.Error(modId, $"File stream length is 0 '{modId}'");
                return null;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object value = formatter.Deserialize(stream);
                stream.Close();
                return value;
            }
            catch (Exception ex)
            {
                stream.Close();
                ModLogger.Error(modId, $"Could not deserialize mod save for '{modId}'");
                ModLogger.Error(modId, ex.Message);
                return null;
            }
        }

        internal static string GetSaveModFile(int slot, string modId)
        {
            return Path.Combine(GetSaveDirectory(slot), $"{modId}.save");
        }

        internal static string GetSaveDirectory(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"slot{slot}");
        }
    }
}

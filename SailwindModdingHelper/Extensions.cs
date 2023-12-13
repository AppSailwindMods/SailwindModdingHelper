using BepInEx;
using HarmonyLib;
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
    public static class Extensions
    {
        public static object GetPrivateField(this object obj, string field)
        {
            return Traverse.Create(obj).Field(field).GetValue();
        }

        public static object GetPrivateField(Type type, string field)
        {
            return Traverse.Create(type).Field(field).GetValue();
        }

        public static T GetPrivateField<T>(Type type, string field)
        {
            return (T)GetPrivateField(type, field);
        }

        public static T GetPrivateField<T>(this object obj, string field)
        {
            return (T)obj.GetPrivateField(field);
        }
        public static object GetPrivateField<T>(string field)
        {
            return Traverse.Create(typeof(T)).Field(field).GetValue();
        }

        public static T GetPrivateField<T, E>(string field)
        {
            return (T)GetPrivateField<E>(field);
        }

        public static void SetPrivateField(this object obj, string field, object value)
        {
            Traverse.Create(obj).Field(field).SetValue(value);
        }

        public static void SetPrivateField<T>(string field, object value)
        {
            Traverse.Create(typeof(T)).Field(field).SetValue(value);
        }

        public static object InvokePrivateMethod(this object obj, string method, params object[] parameters)
        {
            return AccessTools.Method(obj.GetType(), method).Invoke(obj, parameters);
        }

        public static T InvokePrivateMethod<T>(this object obj, string method, params object[] parameters)
        {
            return (T)obj.InvokePrivateMethod(method, parameters);
        }

        public static object InvokePrivateMethod<T>(string method, params object[] parameters)
        {
            return AccessTools.Method(typeof(T), method).Invoke(null, parameters);
        }

        public static T InvokePrivateMethod<T, E>(string method, params object[] parameters)
        {
            return (T)InvokePrivateMethod<E>(method, parameters);
        }

        // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        public static Transform GetChildByName(this Transform transform, string childName)
        {
            foreach (Transform child in transform)
            {
                if (child.name == childName)
                    return child;
            }
            return null;
        }

        public static string GetFolderLocation(this PluginInfo pluginInfo)
        {
            return Directory.GetParent(pluginInfo.Location).FullName;
        }
    }
}

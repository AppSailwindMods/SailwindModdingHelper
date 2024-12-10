using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper
{
    public static class SailwindIslands
    {
        internal static void LoadAllIslands()
        {
            /*IslandHorizon[] d = GameObject.FindObjectsOfType<IslandHorizon>();
            foreach (IslandHorizon l in d)
            {
                SailwindModdingHelperMain.instance.Info.LogError(l.name.Replace(" ", "") + " = " + l.islandIndex);
            }*/
        }
    }

    public enum Islands
    {
        SirenSong = 21,
        LagoonOnna = 29,
        Hideout = 32,
        ClearMind = 5,
        HappyBay = 18,
        AlchemistsIsland = 7,
        Chronos = 25,
        Sanctuary = 10,
        Fort = 15,
        SerpentIsle = 22,
        LagoonShipyard = 27,
        NewPort = 12,
        DragonCliffs = 9,
        AlNilem = 2,
        Sunspire = 16,
        Oasis = 20,
        MountMalefic = 17,
        LagoonTemple = 26,
        LionFang = 6,
        CrabBeach = 11,
        Eastwind = 19,
        LagoonSenna = 28,
        FishIsland = 4,
        SageHills = 13,
        GoldRock = 1,
        Neverdin = 3,
        Moracle = 23,
        LagoonFisherman = 31,
        Academy = 8,
        RockOfDespair = 30,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

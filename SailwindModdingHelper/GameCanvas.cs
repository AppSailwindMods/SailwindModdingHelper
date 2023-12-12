using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SailwindModdingHelper
{
    public static class GameCanvas
    {
        public static GameObject GameObject { get; private set; }
        public static Transform Transform { get; private set; }

        internal static void InitializeCanvas()
        {
            GameObject = GameObject.FindObjectOfType<NotificationUi>().transform.parent.gameObject;
            Transform = GameObject.transform;
        }

        public static void AddToCanvas(GameObject gameObject)
        {
            gameObject.transform.SetParent(Transform, false);
        }
    }
}

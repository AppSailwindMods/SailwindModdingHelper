using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SailwindModdingHelper
{
    internal static class ModButton
    {
        private static bool initialised;

        internal static void InitConsole()
        {
            if (initialised)
                return;

            GameObject gameObject = new GameObject();

            GameCanvas.AddToCanvas(gameObject);

            {
                var rectTransform = gameObject.AddComponent<RectTransform>();
                var image = gameObject.AddComponent<Image>();

                rectTransform.localPosition = Vector3.zero;
                rectTransform.sizeDelta = new Vector2(120, 20);
            }

            var textGameObject = new GameObject();
            textGameObject.transform.SetParent(gameObject.transform, false);

            {
                //var rectTransform = textGameObject.AddComponent<RectTransform>();
                var text = textGameObject.AddComponent<Text>();
                text.text = "Test";
                text.fontSize = 32;
                text.color = Color.black;
            }

            initialised = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindModdingHelper
{
    public enum ButtonSize
    {
        Normal,
        Small
    }

    public enum ButtonColor
    {
        White,
        Blue
    }

    public static class UIManager
    {
        internal static Material blueButtonMaterial;
        internal static Material whiteButtonMaterial;
        internal static Mesh buttonMesh;
        internal static int uiLayer;

        /*public static GameObject CreateSettingsButton<T>(string name, string text, int fontSize, Vector2 position, ButtonSize buttonSize, ButtonColor buttonColor) where T : GoPointerButton
        {
            return CreateButton<T>(settingsUI.transform, name, text, fontSize, position, buttonSize, buttonColor);
        }

        public static GameObject CreateSettingsButton<T>(string name, string text, int fontSize, Vector2 position, ButtonSize buttonSize, Material buttonMaterial) where T : GoPointerButton
        {
            return CreateButton<T>(settingsUI.transform, name, text, fontSize, position, buttonSize, buttonMaterial);
        }

        public static GameObject CreateSettingsButton<T>(string name, string text, int fontSize, Vector2 position, Vector3 buttonSize, ButtonColor buttonColor) where T : GoPointerButton
        {
            return CreateButton<T>(settingsUI.transform, name, text, fontSize, position, buttonSize, buttonColor);
        }

        public static GameObject CreateSettingsButton<T>(string name, string text, int fontSize, Vector2 position, Vector3 buttonSize, Material buttonMaterial) where T : GoPointerButton
        {
            return CreateButton<T>(settingsUI.transform, name, text, fontSize, position, buttonSize, buttonMaterial);
        }*/

        public static GameObject CreateButton<T>(Transform parent, string name, string text, int fontSize, Vector2 position, ButtonSize buttonSize, ButtonColor buttonColor) where T : GoPointerButton
        {
            Vector3 scale = Vector3.one;
            switch (buttonSize)
            {
                case ButtonSize.Normal:
                    scale = new Vector3(0.8106962f, 0.3124738f, 0.8656517f);
                    break;
                case ButtonSize.Small:
                    scale = new Vector3(0.4137929f, 0.2343553f, 0.6492388f);
                    break;
            }

            Material buttonMaterial = whiteButtonMaterial;
            switch (buttonColor)
            {
                case ButtonColor.White:
                    buttonMaterial = whiteButtonMaterial;
                    break;
                case ButtonColor.Blue:
                    buttonMaterial = blueButtonMaterial;
                    break;
            }

            return CreateButton<T>(parent, name, text, fontSize, position, scale, buttonMaterial);
        }

        public static GameObject CreateButton<T>(Transform parent, string name, string text, int fontSize, Vector2 position, Vector3 buttonSize, ButtonColor buttonColor) where T : GoPointerButton
        {
            Material buttonMaterial = whiteButtonMaterial;
            switch (buttonColor)
            {
                case ButtonColor.White:
                    buttonMaterial = whiteButtonMaterial;
                    break;
                case ButtonColor.Blue:
                    buttonMaterial = blueButtonMaterial;
                    break;
            }

            return CreateButton<T>(parent, name, text, fontSize, position, buttonSize, buttonMaterial);
        }

        public static GameObject CreateButton<T>(Transform parent, string name, string text, int fontSize, Vector2 position, ButtonSize buttonSize, Material buttonMaterial) where T : GoPointerButton
        {
            Vector3 scale = Vector3.one;
            switch (buttonSize)
            {
                case ButtonSize.Normal:
                    scale = new Vector3(0.8106962f, 0.3124738f, 0.8656517f);
                    break;
                case ButtonSize.Small:
                    scale = new Vector3(0.4137929f, 0.2343553f, 0.6492388f);
                    break;
            }

            return CreateButton<T>(parent, name, text, fontSize, position, scale, buttonMaterial);
        }

        public static GameObject CreateButton<T>(Transform parent, string name, string text, int fontSize, Vector2 position, Vector3 buttonSize, Material buttonMaterial) where T : GoPointerButton
        {
            GameObject buttonGameObject = new GameObject();
            buttonGameObject.name = name;
            buttonGameObject.layer = uiLayer;
            buttonGameObject.transform.parent = parent;
            buttonGameObject.transform.localPosition = new Vector3(position.x, position.y, 0.038f);
            buttonGameObject.transform.localRotation = Quaternion.identity;
            buttonGameObject.transform.localScale = Vector3.one;

            GameObject bg_trigger = new GameObject();
            bg_trigger.name = "bg+trigger";
            bg_trigger.layer = uiLayer;
            bg_trigger.transform.parent = buttonGameObject.transform;
            bg_trigger.transform.localPosition = new Vector3(0, 0.02250695f, 0);
            bg_trigger.transform.localRotation = Quaternion.Euler(-180, 0, 180);
            bg_trigger.transform.localScale = buttonSize;
            bg_trigger.AddComponent<MeshFilter>().sharedMesh = buttonMesh;
            bg_trigger.AddComponent<MeshRenderer>().sharedMaterial = buttonMaterial;

            BoxCollider bg_trigger_collider = bg_trigger.AddComponent<BoxCollider>();
            bg_trigger_collider.isTrigger = true;
            bg_trigger_collider.center = new Vector3(0, 0, -0.00390625f);
            bg_trigger_collider.size = new Vector3(1, 1, 5.960464e-08f);

            bg_trigger.AddComponent<T>();

            GameObject textGameObject = new GameObject();
            textGameObject.name = "text";
            textGameObject.layer = uiLayer;
            textGameObject.transform.parent = buttonGameObject.transform;
            textGameObject.transform.localPosition = new Vector3(0, 0.02250695f, 0.004f);
            textGameObject.transform.localRotation = Quaternion.Euler(-180, 0, 180);
            textGameObject.transform.localScale = new Vector3(0.01731304f, 0.01731303f, 0.01731304f);

            TextMesh textMesh = textGameObject.AddComponent<TextMesh>();

            textMesh.font = GameAssets.ImmortalFont;
            textMesh.text = text;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.fontSize = fontSize;
            textMesh.richText = true;
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.color = Color.black;

            return buttonGameObject;
        }
    }
}

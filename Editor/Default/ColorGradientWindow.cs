using ch.sycoforge.Types;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public class ColorGradientWindow : EditorWindowExtension
    {
        public Gradient UnityGradient;
        public ColorGradient ColorGradientWrapper;

        public static Texture2D gradientPreview;

        public void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            SerializedObject serializedGradient = new SerializedObject(this);
            SerializedProperty colorGradient = serializedGradient.FindProperty("UnityGradient");
            EditorGUILayout.PropertyField(colorGradient, true, null);

            if (EditorGUI.EndChangeCheck())
            {
                serializedGradient.ApplyModifiedProperties();
            }

            if(GUILayout.Button("OK"))
            {
                ColorGradientWrapper = Convert(UnityGradient);
                Close();
            }
        }

        public static ColorGradientWindow ShowEditor(ColorGradient gradient)
        {
            Gradient g = Convert(gradient);

            ColorGradientWindow window = (ColorGradientWindow)EditorWindow.GetWindow(typeof(ColorGradientWindow), true, "Gradient Editor", false);
            window.UnityGradient = g;

            return window;
        }

        public static Gradient Convert(ColorGradient gradient)
        {
            if (gradient.ColorKeys == null || gradient.AlphaKeys == null)
            {
                gradient = ColorGradient.GetDefault();
            }

            Gradient g = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[gradient.colorKeyCount];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[gradient.alphaKeyCount];

            for (int i = 0; i < gradient.ColorKeys.Length; i++)
            {
                GradientKey key = gradient.ColorKeys[i];
                Color color = new Color(key.Color.x, key.Color.y, key.Color.z, 1);
                colorKeys[i] = new GradientColorKey(color, key.Position);
            }

            for (int i = 0; i < gradient.AlphaKeys.Length; i++)
            {
                GradientKey key = gradient.AlphaKeys[i];
                float alpha = key.Color.w;
                alphaKeys[i] = new GradientAlphaKey(alpha, key.Position);
            }

            g.SetKeys(colorKeys, alphaKeys);

            return g;
        }

        public static ColorGradient Convert(Gradient gradient)
        {
            ColorGradient g = new ColorGradient();
            g.ColorKeys = new GradientKey[gradient.colorKeys.Length];
            g.AlphaKeys = new GradientKey[gradient.alphaKeys.Length];
            g.colorKeyCount = gradient.colorKeys.Length;
            g.alphaKeyCount = gradient.alphaKeys.Length;

            for (int i = 0; i < gradient.colorKeys.Length; i++)
            {
                GradientColorKey key = gradient.colorKeys[i];
                Float4 color = new Float4(key.color.r, key.color.g, key.color.b, 1);
                g.ColorKeys[i] = new GradientKey(color, key.time);
            }

            for (int i = 0; i < gradient.alphaKeys.Length; i++)
            {
                GradientAlphaKey key = gradient.alphaKeys[i];
                Float4 color = new Float4(0, 0, 0, key.alpha);
                g.AlphaKeys[i] = new GradientKey(color, key.time);
            }

            return g;
        }

        private static void RefreshPreview(Gradient gradient, Texture2D preview)
        {
            Color[] colorArray = new Color[512];
            for (int i = 0; i < 256; i++)
            {
                Color color = gradient.Evaluate((float)i / 256f);
                colorArray[i + 256] = color;
                colorArray[i] = color;
            }
            preview.SetPixels(colorArray);
            preview.Apply();
        }

        private static void RefreshPreview(ColorGradient gradient, Texture2D preview)
        {
            if (gradient.ColorKeys == null || gradient.AlphaKeys == null) { return; }

            Color[] colorArray = new Color[512];
            for (int i = 0; i < 256; i++)
            {
                Float4 c = gradient.SampleColor((float)i / 256f);
                Color color = new Color(c.x, c.y, c.z, c.w);
                colorArray[i + 256] = color;
                colorArray[i] = color;
            }

            preview.SetPixels(colorArray);
            preview.Apply();
        }

        public static Texture2D CreateGradientPreview(Gradient gradient)
        {
            CreateTexture();

            RefreshPreview(gradient, gradientPreview);

            return gradientPreview;
        }

        public static Texture2D CreateGradientPreview(ColorGradient gradient)
        {
            CreateTexture();

            RefreshPreview(gradient, gradientPreview);

            return gradientPreview;
            //return CreateGradientPreview(Convert(gradient));
        }

        private static void CreateTexture()
        {
            if (gradientPreview == null)
            {
                gradientPreview = new Texture2D(256, 2, TextureFormat.ARGB32, false)
                {
                    wrapMode = TextureWrapMode.Clamp,
                    hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideAndDontSave
                };
            }
        }

    }
}

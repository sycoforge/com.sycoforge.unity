using ch.sycoforge.Unity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public class EditorPreview
    {
        //------------------------------
        // Properties
        //------------------------------
        public Camera RenderCamera
        {
            get
            {
                return previewRenderUtility.camera;
            }
        }

        //------------------------------
        // Fields
        //------------------------------
        private PreviewRenderUtility previewRenderUtility = new PreviewRenderUtility();

        //------------------------------
        // Methods
        //------------------------------
        public Texture2D RenderStaticPreview(int width, int height, Action<Camera> render)//, RenderTexture texture)
        {
            Texture2D renderTexture = null;

            if (previewRenderUtility != null)
            {
                BeginStaticPreview(new Rect(0, 0, width, height));

                if(render != null)
                {
                    render(RenderCamera);                    
                }

                RenderCamera.Render();

                renderTexture = EndStaticPreview();
            }

            return renderTexture;
        }

        private void BeginStaticPreview(Rect rect)
        {
            Caller.Call(previewRenderUtility, "InitPreview", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, rect);

            return;

            //RenderTexture texture = Caller.GetField<RenderTexture>(previewRenderUtility, "m_RenderTexture", BindingFlags.NonPublic);

            ////Color color = new Color(0.321568638f, 0.321568638f, 0.321568638f, 0f);
            //Color color = new Color(1, 1, 1, 0f);

            //Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, true, true);
            //texture2D.SetPixel(0, 0, color);
            //texture2D.Apply();

            ////Graphics.DrawTexture(new Rect(0f, 0f, texture.width, texture.height), texture2D);

            //UnityEngine.Object.DestroyImmediate(texture2D);
        }

        private Texture2D EndStaticPreview()
        {
            RenderTexture texture = Caller.GetField<RenderTexture>(previewRenderUtility, "m_RenderTexture", BindingFlags.NonPublic);

            RenderTexture temporary = RenderTexture.GetTemporary((int)texture.width, (int)texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            GL.sRGBWrite = false;//QualitySettings.activeColorSpace == ColorSpace.Linear;
            Graphics.Blit(texture, temporary);
            GL.sRGBWrite = false;
            RenderTexture.active = temporary;
            //Texture2D texture2D = new Texture2D((int)texture.width, (int)texture.height, TextureFormat.RGB24, false, true);
            Texture2D texture2D = new Texture2D((int)texture.width, (int)texture.height, TextureFormat.ARGB32, false, true);
            texture2D.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
            texture2D.Apply();
            RenderTexture.ReleaseTemporary(temporary);
            //this.m_SavedState.Restore();

            var state = Caller.GetField(previewRenderUtility, "m_SavedState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            Caller.Call(state, "Restore", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            return texture2D;
        }
    }
}

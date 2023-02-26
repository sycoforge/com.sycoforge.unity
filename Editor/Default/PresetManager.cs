using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public class PresetManager//<P> where P: BasePreset
    {
        //------------------------------
        // Properties
        //------------------------------

        //------------------------------
        // Fields
        //------------------------------
        private WebPresetProvider provider;

        //------------------------------
        // Constructor
        //------------------------------
        public PresetManager(WebPresetProvider provider)
        {
            this.provider = provider;
        }

        //------------------------------
        // Methods
        //------------------------------

        /// <summary>
        /// Creates an asset at the specified path.
        /// </summary>
        /// <param name="preset">The prset to save.</param>
        /// <param name="path">the save path.</param>
        /// <returns>Returns the saved asset preset file.</returns>
        public BasePreset Save(BasePreset preset, string path)
        {
            string directory = Path.GetDirectoryName(path);
            string nameWithExtension = Path.GetFileName(path);
            string name = nameWithExtension.Split('.')[0];

            preset.DsiplayName = name;

            path = AssetDatabase.GenerateUniqueAssetPath(path);

            

            AssetDatabase.CreateAsset(preset, path);

            AssetDatabase.ImportAsset(path);
            BasePreset asset = AssetDatabase.LoadAssetAtPath(path, preset.GetType()) as BasePreset;
            //BasePreset asset = AssetDatabase.LoadAssetAtPath<BasePreset>(path);

            //SubAssets(path);

            
            //ExportWithSubAssets(asset);

            return asset;
        }



        //private void SubAssets(string mainAssetPath)
        //{
        //    UnityEngine.Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(mainAssetPath);

        //    for (int i = 0; i < assets.Length; i++)
        //    {
        //        UnityEngine.Object asset = assets[i];

        //        string path = string.Empty;

        //        if (AssetDatabase.IsMainAsset(asset))
        //        {
        //            path += "Main Asset :: ";
        //        }
        //        else if(AssetDatabase.IsSubAsset(asset))
        //        {
        //            path += "Sub Asset :: ";
        //        }

        //        string p = AssetDatabase.GetAssetPath(asset);

        //        path += p;

        //        Debug.Log(path);
        //    }
        //}

        public void RerenderThumbnail(BasePreset preset)
        {
            Texture2D newThumb = RenderThumbnail(preset);

            ReplaceThumbnail(preset, newThumb);
        }

        public void ReplaceThumbnail(BasePreset preset, Texture2D newThumbnail)
        {
            string path = AssetDatabase.GetAssetPath(preset);
            BasePreset asset = AssetDatabase.LoadAssetAtPath(path, preset.GetType()) as BasePreset;

            string thumbPath = AssetDatabase.GetAssetPath(asset.Thumbnail);

            Debug.Log("ReplaceThumbnail() :: Current Preset Path: " + path + " Current Thumb Path: " + thumbPath + " Current Thumbnail: " + asset.Thumbnail);

            if (!string.IsNullOrEmpty(thumbPath))
            {
                Texture2D thumb = AssetDatabase.LoadAssetAtPath<Texture2D>(thumbPath);

                GameObject.DestroyImmediate(thumb, true);
            }
            else
            {
                string filename = Path.GetFileName(path).Split('.')[0];
                thumbPath = "Assets/" + string.Format(EditorExtension.PRESET_THUMBS_PATH, provider.App) + "/" + filename + ".png";

                //thumbPath = path.Replace(".asset", ".png");
                //thumbPath = path.Replace(".asset", "");
                thumbPath = AssetDatabase.GenerateUniqueAssetPath(thumbPath);
            }

            string absolutPath = EditorExtension.MakePathAbsolute(thumbPath);
            EditorExtension.SaveTexture(newThumbnail, absolutPath);

            Debug.Log("Saved texture thumb: " + absolutPath + " :: " + thumbPath);

            AssetDatabase.ImportAsset(thumbPath);

            newThumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(thumbPath);           


            //AssetDatabase.CreateAsset(newThumbnail, thumbPath);


            asset.Thumbnail = newThumbnail;

            EditorUtility.SetDirty(asset);
            AssetDatabase.ImportAsset(path);
        }

        public Texture2D RenderThumbnail(BasePreset preset)
        {
            Texture2D thumb = null;

            Type type = EditorExtension.FindType(provider.RendererTypeName);

            if (type != null)
            {
                //Debug.Log("type not null: " + type);

                if (typeof(IPresetThumbRenderer).IsAssignableFrom(type))
                {
                    //Debug.Log("IsAssignableFrom: " + type);

                    IPresetThumbRenderer instance = EditorExtension.CreateCtorInstance(type) as IPresetThumbRenderer;

                    return instance.RenderThumbnail(preset);
                }
            }

            return thumb;
        }

        //------------------------------
        // Static Methods
        //------------------------------
        //public BasePreset Save(BasePreset preset, string path)
        //{
        //    path = AssetDatabase.GenerateUniqueAssetPath(path);

        //    AssetDatabase.CreateAsset(preset, path);

        //    AssetDatabase.ImportAsset(path);
        //    BasePreset asset = AssetDatabase.LoadAssetAtPath<BasePreset>(path);

        //    return asset;
        //}

        //public void RerenderThumbnail(BasePreset preset)
        //{
        //    Texture2D newThumb = RenderThumbnail(preset);

        //    ReplaceThumbnail(preset, newThumb);
        //}

        //public void ReplaceThumbnail(BasePreset preset, Texture2D newThumbnail)
        //{
        //    string path = AssetDatabase.GetAssetPath(preset);
        //    BasePreset asset = AssetDatabase.LoadAssetAtPath<BasePreset>(path);

        //    string thumbPath = AssetDatabase.GetAssetPath(asset.Thumbnail);

        //    if (!string.IsNullOrEmpty(thumbPath))
        //    {
        //        Texture2D thumb = AssetDatabase.LoadAssetAtPath<Texture2D>(thumbPath);

        //        GameObject.DestroyImmediate(thumb, true);
        //    }
        //    else
        //    {
        //        string filename = Path.GetFileName(path).Split('.')[0];
        //        thumbPath = "Assets/" + string.Format(EditorExtension.PRESET_THUMBS_PATH, provider.App) + "/" + filename + ".png";

        //        //thumbPath = path.Replace(".asset", ".png");
        //        //thumbPath = path.Replace(".asset", "");
        //        thumbPath = AssetDatabase.GenerateUniqueAssetPath(thumbPath);
        //    }

        //    string absolutPath = EditorExtension.MakePathAbsolute(thumbPath);
        //    EditorExtension.SaveTexture(newThumbnail, absolutPath);

        //    Debug.Log("Saved texture thumb: " + absolutPath + " :: " + thumbPath);

        //    AssetDatabase.ImportAsset(thumbPath);

        //    newThumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(thumbPath);


        //    //AssetDatabase.CreateAsset(newThumbnail, thumbPath);


        //    asset.Thumbnail = newThumbnail;

        //    EditorUtility.SetDirty(asset);
        //    AssetDatabase.ImportAsset(path);
        //}
    }
}

using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    public class PresetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {

            UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);

            if(obj != null)
            {
                if(obj is BasePreset)
                {

                    BasePreset preset = obj as BasePreset;

                    if(preset.Thumbnail != null)
                    {
                        //Debug.Log("4) preset.Thumbnail :: " + preset.Thumbnail);

                        ScriptableObject.DestroyImmediate(preset.Thumbnail, true);
                    }

                    //ScriptableObject.DestroyImmediate(preset, true);

                    //return AssetDeleteResult.DidDelete;
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}

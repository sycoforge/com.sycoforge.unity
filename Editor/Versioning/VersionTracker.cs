//using ch.sycoforge.Unity.Config;
using ch.sycoforge.Unity.Runtime.Config;
//using ch.sycoforge.Unity.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Versioning
{
    public class VersionTracker : ScriptableObject
    {
        //-------------------------------------
        // Property
        //-------------------------------------


        //-------------------------------------
        // Fields
        //-------------------------------------
        public int Major;
        public int Minor;
        public int Build;

        //-------------------------------------
        // Methods
        //-------------------------------------

        public static void IncrementBuild()
        {
            ApplicationVersion version = ApplicationVersion.Instance;

            if (version != null)
            {
                version.Build++;
                //if (version.Build > 99)
                //{
                //    version.Build = 0;
                //    version.Minor++;
                //    if (version.Minor > 9)
                //    {
                //        version.Minor = 0;
                //        version.Major++;
                //    }
                //}

                EditorUtility.SetDirty(version);
            }
        }

        //-------------------------------------
        // Menu Methods
        //-------------------------------------

        //[MenuItem("Tools/Version/Create")]
        static void Create()
        {
            string p = Application.dataPath + "/Resources/";

            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }

            EditorExtension.CreateAsset<ApplicationVersion>("Assets/Resources/Version.asset");
        }

        //[MenuItem("Version/Increment Build")]
        static void Init()
        {
            IncrementBuild();
        }

        //[PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string buildPath)
        {
            IncrementBuild();
        }
    }
}

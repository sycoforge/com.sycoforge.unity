using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Runtime.Config
{

    public class ApplicationVersion : ScriptableObject
    {
        //-------------------------------------
        // Property
        //-------------------------------------
        public static ApplicationVersion Instance
        {
            get
            {
                if(instance == null)
                {
                    instance= Resources.Load<ApplicationVersion>("Version");
                }
    
                return instance;
            }
        }

        public string VersionString
        {
            get
            {
                string s = string.Format("{0}.{1}.{2}", Major, Minor, Build);

                return s;
            }
        }

        //-------------------------------------
        // Fields
        //-------------------------------------
        public int Major;
        public int Minor;
        public int Build;

        private static ApplicationVersion instance;

        //-------------------------------------
        // Methods
        //-------------------------------------
        public override string ToString()
        {
            return VersionString;
        }

    }
}

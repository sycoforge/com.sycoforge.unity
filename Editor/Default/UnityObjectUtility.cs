using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Editor.Default
{
    using UnityEditor;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;


    public static class UnityObjectUtility
    {
        public static UnityObject GetPrefabDefinition(this UnityObject uo)
        {
            return PrefabUtility.GetPrefabParent(uo);
        }

        public static bool IsPrefabInstance(this UnityObject uo)
        {
            return GetPrefabDefinition(uo) != null;
        }

        public static bool IsPrefabDefinition(this UnityObject uo)
        {
            return GetPrefabDefinition(uo) == null && PrefabUtility.GetPrefabObject(uo) != null;
        }

        public static bool IsConnectedPrefabInstance(this UnityObject go)
        {
            return IsPrefabInstance(go) && PrefabUtility.GetPrefabObject(go) != null;
        }

        public static bool IsDisconnectedPrefabInstance(this UnityObject go)
        {
            return IsPrefabInstance(go) && PrefabUtility.GetPrefabObject(go) == null;
        }

        public static bool IsSceneBound(this UnityObject uo)
        {
            return
                (uo is GameObject && !IsPrefabDefinition((UnityObject)uo)) ||
                (uo is Component && !IsPrefabDefinition(((Component)uo).gameObject));
        }
    }
}
 


using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public static class CloneUtil
    {
        public static void CopyFromTo(object from, object to, PropertyInfo propertyInfo)
        {
            try
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)// && Application.isEditor)
                {
                    MethodInfo methodGet = propertyInfo.GetGetMethod();
                    MethodInfo methodSet = propertyInfo.GetSetMethod();

                    // Get value
                    object value = methodGet.Invoke(from, null);

                    // Set value
                    methodSet.Invoke(to, new object[] { value });
                }
            }
            catch (Exception)
            {
                //Debug.LogError("Could not copy: " + propertyInfo.Name + " " + ex);
            }
        }

        public static void CopySettingsFrom(object source, object target)
        {
            if (Application.isEditor)
            {
                PropertyInfo[] propertyInfos = source.GetType().GetProperties();

                foreach (PropertyInfo info in propertyInfos)
                {
                    CloneUtil.CopyFromTo(source, target, info);
                }
            }
        }
    }
}

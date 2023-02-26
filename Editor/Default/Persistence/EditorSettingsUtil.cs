using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;


namespace ch.sycoforge.Unity.Editor
{

    public class EditorSettingsUtil
    {
        //------------------------------
        // Static Properties
        //------------------------------


        //-----------------------------
        // Static Fields
        //-----------------------------


        //-----------------------------
        // Static Methods
        //-----------------------------
        public static T LoadSetting<T>(string name, T defaultValue)
        {
            T value = default(T);
            Type type = typeof(T);

            if (type == typeof(float))
            {
                value = (T)((object)EditorPrefs.GetFloat(name, (float)(object)defaultValue));
            }
            else if (type == typeof(int))
            {
                value = (T)((object)EditorPrefs.GetInt(name, (int)(object)defaultValue));
            }
            else if (type == typeof(string))
            {
                value = (T)((object)EditorPrefs.GetString(name, (string)(object)defaultValue));
            }
            else if (type == typeof(bool))
            {
                value = (T)((object)EditorPrefs.GetBool(name, (bool)(object)defaultValue));
            }


            return value;
        }

        public static T LoadAssetSetting<T>(string name, T defaultValue) where T : UnityEngine.Object
        {
            string assetPath = EditorPrefs.GetString(name, (string)(object)defaultValue);

            //return AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
        }

        public static string SaveToString<T>(T data)
        {
            string result;

            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, data);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }

        public static T LoadFromString<T>(string data)
        {
            T result = default(T);

            try
            {
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(data)))
                {
                    var serializer = new DataContractSerializer(typeof(T));

                    result = (T)serializer.ReadObject(xmlReader);
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static void SaveSetting(string name, object value)
        {
            Type type = value.GetType();

            if (type == typeof(float))
            {
                EditorPrefs.SetFloat(name, (float)value);
            }
            else if (type == typeof(int))
            {
                EditorPrefs.SetInt(name, (int)value);
            }
            else if (type == typeof(string))
            {
                EditorPrefs.SetString(name, (string)value);
            }
            else if (type == typeof(bool))
            {
                EditorPrefs.SetBool(name, (bool)value);
            }
            else if (value is UnityEngine.Object)
            {
                if (value != null)
                {
                    string path = AssetDatabase.GetAssetPath(value as UnityEngine.Object);
                    EditorPrefs.SetString(name, path);
                }
            }
        }
    }
}

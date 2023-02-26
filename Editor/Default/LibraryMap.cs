using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public static class LibraryMap
    {
        public const string OSX = "osx";
        public const string WINOWS = "windos";
        public const string LINUX = "linux";

        public static bool MapOpenCL()
        {
            bool success = false;

            string os = string.Empty;
            string target = string.Empty;

            if(Application.platform == RuntimePlatform.OSXEditor)
            {
                os = OSX;
                target = "/System/Library/Frameworks/OpenCL.framework/OpenCL";

            }
            else if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                //LINUX
                //os = LINUX;
                //target = "libOpenCL.so";
            }

            success = MappingExist(os, "OpenCL.dll", target);

            if (!success && Application.platform != RuntimePlatform.WindowsEditor)
            {
                success = AppendMapping(os, "OpenCL.dll", target);
            }

            return success;
        }

        public static bool AppendMappingOSX(string dll, string target)
        {
            bool success = false;

            if (Application.platform == RuntimePlatform.OSXEditor)
            {

                success = MappingExist(OSX, dll, target);

                if (!success)
                {
                    success = AppendMapping(OSX, dll, target);
                }
            }

            return success;
        }

        public static bool AppendMapping(string os, string dll, string target)
        {
            bool success = false;

            try
            {
                if (ConfigFileFound() && !MappingExist(os, dll, target)) 
                {
                    string path = GetPlatformConfigPath();
                    string content = File.ReadAllText(path);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);

                    XmlNode root = doc.DocumentElement;

                    XmlAttribute attrDll = doc.CreateAttribute("dll");
                    attrDll.Value = dll;

                    XmlAttribute attrTarget = doc.CreateAttribute("target");
                    attrTarget.Value = target;

                    XmlAttribute attrOS = doc.CreateAttribute("os");
                    attrOS.Value = os;

                    //Create a new node.
                    XmlElement elem = doc.CreateElement("dllmap");
                    elem.Attributes.Append(attrOS);
                    elem.Attributes.Append(attrDll);
                    elem.Attributes.Append(attrTarget);

                    //Add the node v2 the document.
                    root.InsertAfter(elem, root.FirstChild);

                    using (StreamWriter outStream = System.IO.File.CreateText(path))
                    {
                        doc.Save(outStream);
                    }
                }

                success = true;
            }
            catch(Exception)
            {
                success = false;
            }

            return success;
        }

        public static bool MappingExist(string os, string dll, string target)
        {
            bool success = false;

            try
            {
                string path = GetPlatformConfigPath();
                if (File.Exists(path))
                {

                    string content = File.ReadAllText(path);

                    bool cOs = content.Contains(string.Format("os=\"{0}\"", os));
                    bool cDll = content.Contains(string.Format("dll=\"{0}\"", dll));
                    bool cTarget = content.Contains(string.Format("target=\"{0}\"", target));

                    success = cOs && cDll && cTarget;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public static bool ConfigFileFound()
        {
            bool success = false;                       
            
            try
            {
                string configPath = GetPlatformConfigPath();
                success = File.Exists(configPath);
            }
            catch(Exception)
            {
                success = false;
            }

            return success;
        }

        public static string GetPlatformConfigPath()
        {
            string configPath = string.Empty;


            if(Application.platform == RuntimePlatform.OSXEditor)
            {
                configPath = "/Applications/Unity/Unity.app/Contents/Frameworks/Mono/etc/mono/config";
            }
            else if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                string programFiles = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
                configPath = programFiles + @"\Unity\Editor\Data\Mono\etc\mono\config";
            }

            return configPath;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Editor
{
    public static class EditorVersion
    {
        public static string GetVersionString(Type type)
        {
            string version = "-.-.-";

            try
            {
                System.Reflection.Assembly assembly = type.Assembly;
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

                version = fvi.FileVersion;
            }
            catch (Exception) { }

            return version;
        }

        public static Version GetVersion(Type type)
        {
            return new Version(GetVersionString(type));
        }
    }
}

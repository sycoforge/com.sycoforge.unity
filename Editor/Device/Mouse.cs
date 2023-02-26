using ch.sycoforge.Interop;
using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Device
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        Other
    }

    public static class Mouse
    {
        public static void SetPosition(int x, int y)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                SetCursorPos(x, y);
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                CGSetLocalEventsSuppressionInterval(0.0);
                CGWarpMouseCursorPosition(new Double2(x, y));
                CGSetLocalEventsSuppressionInterval(0.25);
            }
        }

        #region --- WINDOWS ---
        [DllImport("user32.dll")]
        [return: MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetCursorPos(Int2 point);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        private static extern IntPtr SetCursor(IntPtr hCursor);


        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);


        [DllImport("user32.dll")]
        private static extern IntPtr LoadImage(
            IntPtr hInstance, 
            string lpImageName,
            uint uType,
            int cxDesired,
            int cyDesired,
            uint fuLoad
        );

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// (Suggestion : Make another class for mouse extensions)
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(out Int2 lpPoint);

        #endregion

        #region --- OSX ---

        [DllImport(PathsOSX.CoreGraphicsLibrary)]
        static extern void CGWarpMouseCursorPosition(Double2 position);

        [DllImport(PathsOSX.CoreGraphicsLibrary)]
        static extern void CGSetLocalEventsSuppressionInterval(double seconds);

        #endregion

    }
}

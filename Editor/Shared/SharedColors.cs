using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Shared
{
    public static class SharedColors
    {        
        //------------------------------
        // Public Static
        //------------------------------
        public static Color SkyBlue;
        public static Color UIBlue;
        public static Color HyperlinkBlue;
        public static Color DimGray;
        public static Color Gray;
        public static Color LightGray;
        public static Color TangentNormalUp;

        /// <summary>
        /// The default text color depends on the Unity version that used.
        /// It's a lighter gray when on Pro and a darker gray when in Personal.
        /// </summary>
        public static Color DefaultTextColor;



        //------------------------------
        // Private Static
        //------------------------------


        internal static void Initialize()
        {
            bool pro = Application.HasProLicense();


            SkyBlue = new Color(0, 0.475f, 0.698f);
            UIBlue = new Color(0.0470589f, 0.4117647059f, 0.580392157f);
            HyperlinkBlue = new Color(0.0235f, 0.2705f, 0.6784f);
            DimGray = new Color(0.2f, 0.2f, 0.2f);
            Gray = new Color(0.5f, 0.5f, 0.5f);
            LightGray = new Color(0.7f, 0.7f, 0.7f);
            TangentNormalUp = new Color(0.5f, 0.5f, 1f);

            DefaultTextColor = pro ? SharedColors.LightGray : SharedColors.DimGray;
        }
    }
}

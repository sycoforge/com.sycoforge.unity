using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Defines a color in the HSB space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HSBColor
    {
        //-----------------------------------
        // Fields
        //-----------------------------------
        /// <summary>
        ///   <para>Hue component of the color.</para>
        /// </summary>
        public float h;

        /// <summary>
        ///   <para>Saturation component of the color.</para>
        /// </summary>
        public float s;

        /// <summary>
        ///   <para>Brightness component of the color.</para>
        /// </summary>
        public float b;

        /// <summary>
        ///   <para>Alpha component of the color.</para>
        /// </summary>
        public float a;

        //-----------------------------------
        // Extern
        //-----------------------------------
        [DllImport("sycoforge_imaging", CallingConvention = CallingConvention.Cdecl)]
        private static extern HSBColor RGBToHSB(FloatColor rgb);

        [DllImport("sycoforge_imaging", CallingConvention = CallingConvention.Cdecl)]
        private static extern FloatColor HSBToRGB(HSBColor hsb);

        //-----------------------------------
        // Conversions
        //-----------------------------------

        public static implicit operator FloatColor(HSBColor hsb)
        {
            return HSBToRGB(hsb);

            //hsb.h = FloatMath.Clamp01(hsb.h);
            //hsb.s = FloatMath.Clamp01(hsb.s);
            //hsb.b = FloatMath.Clamp01(hsb.b);

            //FloatColor result = new FloatColor();

            //float r = 0, g = 0, b = 0;

            //if (FloatMath.Approximately(hsb.s, 0))
            //{
            //    r = g = b = hsb.b;
            //}
            //else
            //{
            //    float h = (hsb.h - FloatMath.Floor(hsb.h)) * 6.0f;
            //    float f = h - FloatMath.Floor(h);
            //    float p = hsb.b * (1.0f - hsb.s);
            //    float q = hsb.b * (1.0f - hsb.s * f);
            //    float t = hsb.b * (1.0f - (hsb.s * (1.0f - f)));

            //    switch ((int)h)
            //    {
            //        case 0:
            //            r = hsb.b;
            //            g = t;
            //            b = p;
            //            break;
            //        case 1:
            //            r = q;
            //            g = hsb.b;
            //            b = p;
            //            break;
            //        case 2:
            //            r = p;
            //            g = hsb.b;
            //            b = t;
            //            break;
            //        case 3:
            //            r = p;
            //            g = q;
            //            b = hsb.b;
            //            break;
            //        case 4:
            //            r = t;
            //            g = p;
            //            b = hsb.b;
            //            break;
            //        case 5:
            //            r = hsb.b;
            //            g = p;
            //            b = q;
            //            break;
            //    }
            //}

            //result.r = r;
            //result.g = g;
            //result.b = b;

            //return result;
        }

        public static implicit operator HSBColor(FloatColor rgb)
        {
            return RGBToHSB(rgb);

            //HSBColor result = new HSBColor();

            //float hue, saturation, brightness;

            //float cmax = (rgb.r > rgb.g) ? rgb.r : rgb.g;
            //if (rgb.b > cmax) cmax = rgb.b;

            //float cmin = (rgb.r < rgb.g) ? rgb.r : rgb.g;
            //if (rgb.b < cmin) cmin = rgb.b;

            //brightness = ((float)cmax);
            //if (cmax != 0)
            //{
            //    saturation = ((float)(cmax - cmin)) / ((float)cmax);
            //}
            //else
            //{
            //    saturation = 0;
            //}
            //if (saturation == 0)
            //{
            //    hue = 0;
            //}
            //else
            //{
            //    float redc = ((float)(cmax - rgb.r)) / ((float)(cmax - cmin));
            //    float greenc = ((float)(cmax - rgb.g)) / ((float)(cmax - cmin));
            //    float bluec = ((float)(cmax - rgb.b)) / ((float)(cmax - cmin));
            //    if (rgb.r == cmax)
            //    {
            //        hue = bluec - greenc;
            //    }
            //    else if (rgb.g == cmax)
            //    {
            //        hue = 2.0f + redc - bluec;
            //    }
            //    else
            //    {
            //        hue = 4.0f + greenc - redc;
            //    }
            //    hue = hue / 6.0f;
            //    if (hue < 0)
            //    {
            //        hue = hue + 1.0f;
            //    }
            //}

            //result.h = hue;
            //result.s = saturation;
            //result.b = brightness;

            //return result;
        }
    }
}

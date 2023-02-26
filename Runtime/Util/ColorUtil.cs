using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.TextureUtil
{
    public static class ColorUtil
    {
        public static Color Parse(string hexstring)
        {
            if (hexstring.StartsWith("#"))
            {
                hexstring = hexstring.Substring(1);
            }

            if (hexstring.StartsWith("0x"))
            {
                hexstring = hexstring.Substring(2);
            }

            if (hexstring.Length != 6) 
            {
                throw new Exception(string.Format("{0} is not a valid color string.", hexstring));
            }

            byte r = byte.Parse(hexstring.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hexstring.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hexstring.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color32(r, g, b, 255);
        }

        public static Color NormalToColor(Vector3 normal)
        {
            float r = (normal.x + 1) * 0.5f;
            float g = (normal.y + 1) * 0.5f;
            float b = (normal.z + 1) * 0.5f;

            return new Color(r, g, b);
        }

        public static Color NormalToColorUnity(Vector3 normal)
        {
            float g = (normal.y + 1) * 0.5f;
            float a = (normal.x + 1) * 0.5f;

            return new Color(g, g, g, a);
        }

        public static Float3 ColorToVector3(FloatColor color)
        {
            float x = color.r;
            float y = color.g;
            float z = color.b;

            return new Float3(x, y, z);
        }

        public static Float3 ColorToNormal(FloatColor color)
        {
            float x = (color.r * 2f) - 1;
            float y = (color.g * 2f) - 1;
            float z = (color.b * 2f) - 1;

            //return new Float3(x, y, z).normalized;
            return new Float3(x, y, z).normalized;
        }

        public static FloatColor NormalToColor(Float3 normal)
        {
            float r = (normal.x + 1) * 0.5f;
            float g = (normal.y + 1) * 0.5f;
            float b = (normal.z + 1) * 0.5f;

            return new FloatColor(r, g, b);
        }

        public static FloatColor NormalToColorUnity(Float3 normal)
        {
            float g = (normal.y + 1) * 0.5f;
            float a = (normal.x + 1) * 0.5f;

            return new FloatColor(g, g, g, a);
        }

        /// <summary>
        /// Converts the specified color to the DXT5nm format.
        /// R = G = B = input.G and A = input.R.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToDXT5(Color color)
        {
            float g = color.g;
            float a = color.r;

            return new Color(g, g, g, a);
        }

        public static Vector3 ColorToNormal(Color color)
        {
            float x = (color.r * 2f) - 1;
            float y = (color.g * 2f) - 1;
            float z = (color.b * 2f) - 1;

            return new Vector3(x, y, z).normalized;
        }

        public static Vector3 ColorToVector3(Color color)
        {
            float x = color.r;
            float y = color.g;
            float z = color.b;

            return new Vector3(x, y, z);
        }

        public static int ToARGBInteger(Color color)
        {
            int result = ToARGBInteger((Color32)color);

            return result;
        }

        public static int ToARGBInteger(Color32 color)
        {
            int result = (color.a << 24) | (color.r << 16) | (color.g << 8) | (color.b);

            return result;
        }

        public static Color FromARGBInteger(int color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)((color >> 16) & 0x000F); 
            byte g = (byte)((color >> 8) & 0x000F); 
            byte b = (byte)(color & 0x000F); 

            return new Color(r, g, b, a);
        }

        public static int NextPowerOfTwo(int number)
        {
            int i = 2;
            while (i < Mathf.Pow(2, 14))
            {
                i *= 2;
                if (i >= number)
                {
                    return i;
                }
            }

            return i;
        }
    }
}

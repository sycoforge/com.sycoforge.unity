using ch.sycoforge.Types;
using System;
using System.Runtime.InteropServices;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a color gradient. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct ColorGradient
    {
        public GradientKey[] ColorKeys;
        public GradientKey[] AlphaKeys;
        public int colorKeyCount;
        public int alphaKeyCount;


        public ColorGradient(int colorKeyCount, int alphaKeyCount)
        {
            this.ColorKeys = new GradientKey[colorKeyCount];
            this.colorKeyCount = colorKeyCount;

            this.AlphaKeys = new GradientKey[alphaKeyCount];
            this.alphaKeyCount = alphaKeyCount;
        }

        /// <summary>
        /// Sample the color at the specified position.
        /// </summary>
        /// <param name="position">The position [0..1] v2 sample the color at.</param>
        public Float4 SampleColor(float position)
        { 
            Float4 result = Float4.zero;

            if(ColorKeys.Length >= 2)
            {
                if(position < ColorKeys[0].Position)
                {
                    result = ColorKeys[0].Color;
                }
                else if (position > ColorKeys[ColorKeys.Length - 1].Position)
                {
                    result = ColorKeys[ColorKeys.Length - 1].Color;
                }
                else
                {
                    result = Sample(position, ColorKeys);
                    result.w = Sample(position, AlphaKeys).w;
                }
            }

            return result;
        }

        private Float4 Sample(float position, GradientKey[] keys)
        {
            Float4 result = Float4.zero;

            if (keys != null)
            {
                Array.Sort(keys, (x, y) => x.Position.CompareTo(y.Position));

                GradientKey c1 = default(GradientKey), c2 = default(GradientKey);

                for (int i = 0; i < keys.Length - 1; i++)
                {
                    c1 = keys[i];
                    c2 = keys[i + 1];

                    if (position > c1.Position && position < c2.Position)
                    {
                        break;
                    }
                }

                float offset = c1.Position;
                float d = c2.Position - offset;
                float scale = 1.0f / d;

                float t = (position - offset) * scale;

                result = c2.Color * t + c1.Color * (1.0f - t);
            }

            return result;
        }

        public void Sort()
        {
            Array.Sort(ColorKeys, (x, y) => x.Position.CompareTo(y.Position));
            Array.Sort(AlphaKeys, (x, y) => x.Position.CompareTo(y.Position));

            colorKeyCount = ColorKeys.Length;
            alphaKeyCount = AlphaKeys.Length;
        }

        public static ColorGradient GetDefault()
        {
            ColorGradient g = new ColorGradient();
            g.colorKeyCount = 2;
            g.alphaKeyCount = 2;
            g.ColorKeys = new GradientKey[2];
            g.AlphaKeys = new GradientKey[2];

            g.ColorKeys[0] = new GradientKey(new Float4(1, 0.5f, 0), 0.0f);
            g.ColorKeys[1] = new GradientKey(new Float4(1, 1, 1), 0.51f);

            g.AlphaKeys[0] = new GradientKey(new Float4(1, 1, 1, 1), 0);
            g.AlphaKeys[1] = new GradientKey(new Float4(1, 1, 1, 1), 1);

            return g;
        }
      
    }
}


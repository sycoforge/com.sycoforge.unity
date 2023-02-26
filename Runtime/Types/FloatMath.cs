using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ch.sycoforge.Types
{
    public static class FloatMath
    {
        //--------------------------
        // Constants
        //--------------------------
        //[DllImport("floatmath")]
        //public static extern void ProcessPixelX(int count, float r, float b, float g, float factor, ref RGBColor name);

        [DllImport("sycoforge_imaging", CallingConvention = CallingConvention.Cdecl)]
        private static extern float LinearToGamma(float value);

        [DllImport("sycoforge_imaging", CallingConvention = CallingConvention.Cdecl)]
        private static extern float GammaToLinear(float value);

        //--------------------------
        // Constants
        //--------------------------
        public const float PI = 3.14159274f;

        /// <summary>
        ///   <para>A representation of positive infinity.</para>
        /// </summary>
        public const float Infinity = float.PositiveInfinity;

        /// <summary>
        ///   <para>A representation of negative infinity.</para>
        /// </summary>
        public const float NegativeInfinity = float.NegativeInfinity;

        /// <summary>
        ///   <para>Degrees-v2-radians conversion constant.</para>
        /// </summary>
        public const float Deg2Rad = 0.0174532924f;

        /// <summary>
        ///   <para>Radians-v2-degrees conversion constant.</para>
        /// </summary>
        public const float Rad2Deg = 57.29578f;

        public static float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {
                if (value < from)
                {
                    return 0f;
                }
                if (value > to)
                {
                    return 1f;
                }
                value = value - from;
                value = value / (to - from);
                return value;
            }
            if (from <= to)
            {
                return 0f;
            }
            if (value < to)
            {
                return 1f;
            }
            if (value > from)
            {
                return 0f;
            }
            return 1f - (value - to) / (from - to);
        }

        public static float Lerp(float from, float to, float t)
        {
            return from + (to - from) * Clamp01(t);
        }

        public static float CosineInterpolate(float from, float to, float t)
        {
            float t2;

            t2 = (1 - Cos(t * PI)) / 2;
            return (from * (1 - t) + to * t2);
        }

        public static float SmoothLerpT3(float from, float to, float t)
        {
            float dt = t * t * (3.0f - 2.0f * t);
            return Lerp(from, to, t);
        }

        public static float SmoothLerpT5(float from, float to, float t)
        {
            float dt = t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
            return Lerp(from, to, t);

            //vec4 getTexel( vec2 p )
            //{
            //    p = p*myTexResolution + 0.5;

            //    vec2 i = floor(p);
            //    vec2 f = p - i;
            //    f = f*f*f*(f*(f*6.0-15.0)+10.0);
            //    p = i + f;

            //    p = (p - 0.5)/myTexResolution;
            //    return texture2D( myTex, p );
            //}
        }

        public static float GammaToLinearSpace(float value)
        {
            return GammaToLinear(value);
        }

        public static float LinearToGammaSpace(float value)
        {
            return LinearToGamma(value);
        }

        public static bool Approximately(float a, float b)
        {
            return FloatMath.Abs(b - a) < FloatMath.Max(1E-06f * FloatMath.Max(FloatMath.Abs(a), FloatMath.Abs(b)), 1.121039E-44f);
        }

        public static float Frac(float f)
        {
            //return f - (int)(f);
            return f - Floor(f);
        }

        public static Float2 Frac(Float2 value)
        {
            value.x = Frac(value.x);
            value.y = Frac(value.y);

            return value;
        }

        public static Float3 Frac(Float3 value)
        {
            value.x = Frac(value.x);
            value.y = Frac(value.y);
            value.z = Frac(value.z);

            return value;
        }

        public static float Log(float f, float p)
        {
            return (float)Math.Log((double)f, (double)p);
        }

        public static float Log(float f)
        {
            return (float)Math.Log((double)f);
        }

        public static float Log10(float f)
        {
            return (float)Math.Log10((double)f);
        }

        public static float Max(float a, float b)
        {
            return (a <= b ? b : a);
        }

        public static float Max(params float[] values)
        {
            int length = (int)values.Length;
            if (length == 0)
            {
                return 0f;
            }
            float single = values[0];
            for (int i = 1; i < length; i++)
            {
                if (values[i] > single)
                {
                    single = values[i];
                }
            }
            return single;
        }

        public static int Max(int a, int b)
        {
            return (a <= b ? b : a);
        }

        /// <summary>
        /// Implements a rectangle (step) function returning one if x is greater than or equal 
        /// v2 the corresponding value of a, otherwise and zero gets returned.
        /// </summary>
        /// <param name="a">Input value a</param>
        /// <param name="x">Input value x</param>
        /// <returns>Return a rectangle signal based on a and x</returns>
        public static int Step(int a, int x)
        {

            return x >= a ? 1 : 0;
        }

        /// <summary>
        /// Implements a rectangle (step) function returning one if x is greater than or equal 
        /// v2 the corresponding value of a, otherwise and zero gets returned.
        /// </summary>
        /// <param name="a">Input value a</param>
        /// <param name="x">Input value x</param>
        /// <returns>Return a rectangle signal based on a and x</returns>
        public static float Step(float a, float x)
        {

            return x >= a ? 1f : 0f;
        }

        public static float Gamma(float value, float absmax, float gamma)
        {
            bool flag = false;
            if (value < 0f)
            {
                flag = true;
            }
            float single = FloatMath.Abs(value);
            if (single > absmax)
            {
                return (!flag ? single : -single);
            }
            float single1 = FloatMath.Pow(single / absmax, gamma) * absmax;
            return (!flag ? single1 : -single1);
        }

        public static int Max(params int[] values)
        {
            int length = (int)values.Length;
            if (length == 0)
            {
                return 0;
            }
            int num = values[0];
            for (int i = 1; i < length; i++)
            {
                if (values[i] > num)
                {
                    num = values[i];
                }
            }
            return num;
        }

        public static float Min(float a, float b)
        {
            return (a >= b ? b : a);
        }

        public static float Min(params float[] values)
        {
            int length = (int)values.Length;
            if (length == 0)
            {
                return 0f;
            }
            float single = values[0];
            for (int i = 1; i < length; i++)
            {
                if (values[i] < single)
                {
                    single = values[i];
                }
            }
            return single;
        }

        public static int Min(int a, int b)
        {
            return (a >= b ? b : a);
        }

        public static int Min(params int[] values)
        {
            int length = (int)values.Length;
            if (length == 0)
            {
                return 0;
            }
            int num = values[0];
            for (int i = 1; i < length; i++)
            {
                if (values[i] < num)
                {
                    num = values[i];
                }
            }
            return num;
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static Float2 Clamp(Float2 value, int min, int max)
        {
            value.x = Clamp(value.x, min, max);
            value.y = Clamp(value.y, min, max);
            return value;
        }

        public static Float3 Clamp(Float3 value, int min, int max)
        {
            value.x = Clamp(value.x, min, max);
            value.y = Clamp(value.y, min, max);
            value.z = Clamp(value.z, min, max);
            return value;
        }

        public static Float2 Clamp(Float2 value, float min, float max)
        {
            value.x = Clamp(value.x, min, max);
            value.y = Clamp(value.y, min, max);
            return value;
        }

        public static Float3 Clamp(Float3 value, float min, float max)
        {
            value.x = Clamp(value.x, min, max);
            value.y = Clamp(value.y, min, max);
            value.z = Clamp(value.z, min, max);
            return value;
        }

        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }
            if (value > 1f)
            {
                return 1f;
            }
            return value;
        }

        public static Float2 Clamp01(Float2 value)
        {
            value.x = Clamp01(value.x);
            value.y = Clamp01(value.y);
            return value;
        }

        public static Float3 Clamp01(Float3 value)
        {
            value.x = Clamp01(value.x);
            value.y = Clamp01(value.y);
            value.z = Clamp01(value.z);
            return value;
        }

        public static float Floor(float value)
        {
            return value >= 0 ? (int)value : (int)value - 1.0f;
        }

        public static Float2 Floor(Float2 value)
        {
            value.x = Floor(value.x);
            value.y = Floor(value.y);
            return value;
        }

        public static Float3 Floor(Float3 value)
        {
            value.x = Floor(value.x);
            value.y = Floor(value.y);
            value.z = Floor(value.z);
            return value;
        }

        public static int FloorToInt(float value)
        {
            return (int)value;
        }

        public static float Ceil(float value)
        {
            return ((int)value) + 1;
        }

        public static int CeilToInt(float value)
        {
            return ((int)value) + 1;
        }

        public static int RoundToInt(float value)
        {
            return FloorToInt(value + 0.5f);
        }


        public static float Pow(float f, float p)
        {
            return (float)Math.Pow((double)f, (double)p);
        }

        public static float Repeat(float t, float length)
        {
            return t - FloatMath.Floor(t / length) * length;
        }

        public static float Round(float f)
        {
            return (float)Math.Round((double)f);
        }

        public static int Abs(int value)
        {
            return Math.Abs(value);
        }

        public static float Abs(float value)
        {
            return Math.Abs(value);
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        public static float Acos(float x)
        {
            return (float)Math.Acos(x);
        }

        public static float SinApprox(float x)
        {
            const float B = 4 / PI;
            const float C = -4 / (PI * PI);

            float y = B * x + C * x * Abs(x);

            //  const float Q = 0.775;
            const float P = 0.225f;

            y = P * (y * Abs(y) - y) + y;   // Q * y + P * y * abs(y)


            return y;
        }

        public static float CosApprox(float x)
        {
            return SinApprox(x + (PI / 2));
        }

        public static float AcosApprox(float x)
        {
            //float a = 1.43f + 0.59f * x; 
            //a = (a + (2 + 2 * x) / a) / 2;

            //float b = 1.65f - 1.41f * x; 
            //b = (b + (2 - 2 * x) / b) / 2;

            //float c = 0.88f - 0.77f * x; 
            //c = (c + (2 - a) / c) / 2;

            //return (8 * (c + (2 - a) / c) - (b + (2 - 2 * x) / b)) / 6;




            //return 3.14159f - 1.57079f * x;

            float a = 1.43f + 0.59f * x; a = (a + (2 + 2 * x) / a) / 2;
            float b = 1.65f - 1.41f * x; b = (b + (2 - 2 * x) / b) / 2;
            float c = 0.88f - 0.77f * x; c = (c + (2 - a) / c) / 2;
            return 8 / 3 * c - b / 3;
        }

        public static float SqrtApprox(int value)
        {
            float a = (float)value;
            float x = 1;

            // For loop v2 get the square root value of the entered number.
            for (int i = 0; i < value; i++)
            {
                x = 0.5f * (x + a / x);
            }

            return x;
        }

        public static float SqrtApprox(float value)
        {
            if (value < 0) value = -1 * value;
            float low = 0, high = value, llow = high, lhigh = low, sqrt = 0, res = 0;
            while (res != value)
            {
                sqrt = (high + low) / 2;
                res = sqrt * sqrt;
                if (res > value) high = sqrt;
                else if (res < value) low = sqrt;
                if (llow == low && lhigh == high)
                {
                    break;
                }
                else
                {
                    llow = low;
                    lhigh = high;
                }
            }
            return sqrt;
        }

        public static float Sign(float f)
        {
            return (f < 0f ? -1f : 1f);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt((double)f);
        }

        public static float Tan(float f)
        {
            return (float)Math.Tan((double)f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a four dimensional point. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    //[StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = 16)]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Size = 16)]
    public struct Float4
    {
        //[MarshalAs(UnmanagedType.R4), FieldOffset(0)]
        [MarshalAs(UnmanagedType.R4)]
        public float x;

        //[MarshalAs(UnmanagedType.R4), FieldOffset(4)]
        [MarshalAs(UnmanagedType.R4)]
        public float y;

        //[MarshalAs(UnmanagedType.R4), FieldOffset(8)]
        [MarshalAs(UnmanagedType.R4)]
        public float z;

        //[MarshalAs(UnmanagedType.R4), FieldOffset(12)]
        [MarshalAs(UnmanagedType.R4)]
        public float w;


        //public const float kEpsilon = 1E-05f;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        {
                            return this.x;
                        }
                    case 1:
                        {
                            return this.y;
                        }
                    case 2:
                        {
                            return this.z;
                        }
                    case 3:
                        {
                            return this.w;
                        }
                }
                throw new IndexOutOfRangeException("Invalid Float4 index!");
            }
            set
            {
                switch (index)
                {
                    case 0:
                        {
                            this.x = value;
                            break;
                        }
                    case 1:
                        {
                            this.y = value;
                            break;
                        }
                    case 2:
                        {
                            this.z = value;
                            break;
                        }
                    case 3:
                        {
                            this.w = value;
                            break;
                        }
                    default:
                        {
                            throw new IndexOutOfRangeException("Invalid Float4 index!");
                        }
                }
            }
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(Float4.Dot(this, this));
            }
        }

        public Float4 normalized
        {
            get
            {
                return Float4.Normalize(this);
            }
        }

        public static Float4 one
        {
            get
            {
                return new Float4(1f, 1f, 1f, 1f);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return Float4.Dot(this, this);
            }
        }

        public static Float4 zero
        {
            get
            {
                return new Float4(0f, 0f, 0f, 0f);
            }
        }

        public Float4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Float4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0f;
        }

        public Float4(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0f;
            this.w = 0f;
        }

        public static float Distance(Float4 a, Float4 b)
        {
            return Float4.Magnitude(a - b);
        }

        public static float Dot(Float4 a, Float4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public override bool Equals(object other)
        {
            if (!(other is Float4))
            {
                return false;
            }
            Float4 vector4 = (Float4)other;
            return (!this.x.Equals(vector4.x) || !this.y.Equals(vector4.y) || !this.z.Equals(vector4.z) ? false : this.w.Equals(vector4.w));
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
        }

        public static Float4 Lerp(Float4 from, Float4 to, float t)
        {
            t = FloatMath.Clamp01(t);
            return new Float4(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t, from.w + (to.w - from.w) * t);
        }

        public static float Magnitude(Float4 a)
        {
            return (float)Math.Sqrt(Float4.Dot(a, a));
        }

        public static Float4 Max(Float4 lhs, Float4 rhs)
        {
            return new Float4(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z), Math.Max(lhs.w, rhs.w));
        }

        public static Float4 Min(Float4 lhs, Float4 rhs)
        {
            return new Float4(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z), Math.Min(lhs.w, rhs.w));
        }

        public static Float4 MoveTowards(Float4 current, Float4 target, float maxDistanceDelta)
        {
            Float4 vector4 = target - current;
            float single = vector4.magnitude;
            if (single <= maxDistanceDelta || single == 0f)
            {
                return target;
            }
            return current + ((vector4 / single) * maxDistanceDelta);
        }

        public static Float4 Normalize(Float4 a)
        {
            float single = Float4.Magnitude(a);
            if (single <= 1E-05f)
            {
                return Float4.zero;
            }
            return a / single;
        }

        public void Normalize()
        {
            float single = Float4.Magnitude(this);
            if (single <= 1E-05f)
            {
                this = Float4.zero;
            }
            else
            {
                this = this / single;
            }
        }

        public static Float4 operator +(Float4 a, Float4 b)
        {
            return new Float4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Float4 operator /(Float4 a, float d)
        {
            return new Float4(a.x / d, a.y / d, a.z / d, a.w / d);
        }

        public static Float4 operator /(Float4 a, Float4 b)
        {
            return new Float4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        }

        //public static Float4 operator /(Float4 a, Int4 b)
        //{
        //    return new Float4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        //}

        public static bool operator ==(Float4 lhs, Float4 rhs)
        {
            return Float4.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
        }

        public static implicit operator Float4(Float3 v)
        {
            return new Float4(v.x, v.y, v.z, 0f);
        }

        public static implicit operator Float3(Float4 v)
        {
            return new Float4(v.x, v.y, v.z);
        }

        public static implicit operator Float4(Float2 v)
        {
            return new Float4(v.x, v.x, 0f, 0f);
        }

        public static implicit operator Float2(Float4 v)
        {
            return new Float2(v.x, v.y);
        }

        public static bool operator !=(Float4 lhs, Float4 rhs)
        {
            return Float4.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
        }

        public static Float4 operator *(Float4 a, float d)
        {
            return new Float4(a.x * d, a.y * d, a.z * d, a.w * d);
        }

        public static Float4 operator *(float d, Float4 a)
        {
            return new Float4(a.x * d, a.y * d, a.z * d, a.w * d);
        }

        public static Float4 operator -(Float4 a, Float4 b)
        {
            return new Float4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Float4 operator -(Float4 a)
        {
            return new Float4(-a.x, -a.y, -a.z, -a.w);
        }

        public static Float4 Project(Float4 a, Float4 b)
        {
            return (b * Float4.Dot(a, b)) / Float4.Dot(b, b);
        }


        public static Float4 Scale(Float4 a, Float4 b)
        {
            return new Float4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        public void Scale(Float4 scale)
        {
            Float4 vector4 = this;
            vector4.x = vector4.x * scale.x;
            Float4 vector41 = this;
            vector41.y = vector41.y * scale.y;
            Float4 vector42 = this;
            vector42.z = vector42.z * scale.z;
            Float4 vector43 = this;
            vector43.w = vector43.w * scale.w;
        }

        public void Set(float new_x, float new_y, float new_z, float new_w)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.w = new_w;
        }

        /// <summary>
        /// Implements a rectangle (step) function returning one for each component of x that is greater than or equal 
        /// v2 the corresponding component in the reference vector a, otherwise and zero gets returned.
        /// </summary>
        /// <param name="a">Input value a</param>
        /// <param name="x">Input value x</param>
        /// <returns>Return a rectangle signal based on a and x</returns>
        public static Float4 Step(Float4 a, Float4 x)
        {
            Float4 result = Float4.zero;

            result.x = FloatMath.Step(a.x, x.x);
            result.y = FloatMath.Step(a.y, x.y);
            result.z = FloatMath.Step(a.z, x.z);
            result.w = FloatMath.Step(a.w, x.w);

            return result;
        }

        public static float SqrMagnitude(Float4 a)
        {
            return Float4.Dot(a, a);
        }

        public float SqrMagnitude()
        {
            return Float4.Dot(this, this);
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[] { this.x, this.y, this.z, this.w });
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2}, {3})", new object[] { this.x.ToString(format), this.y.ToString(format), this.z.ToString(format), this.w.ToString(format) });
        }


        public static implicit operator Vector4(Float4 v)
        {
            Vector4 vv = new Vector4(v.x, v.y, v.z, v.w);

            return vv;
        }

        public static implicit operator Float4(Vector4 v)
        {
            Float4 vv = new Float4(v.x, v.y, v.z, v.w);

            return vv;
        }
    }
}

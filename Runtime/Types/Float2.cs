using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a two dimensional point. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Float2
    {
        [MarshalAs(UnmanagedType.R4)]
        public float x;

        [MarshalAs(UnmanagedType.R4)]
        public float y;
        public const float kEpsilon = 1E-05f;


        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }



        public float this[int index]
        {
            get
            {
                int num = index;
                if (num == 0)
                {
                    return this.x;
                }
                if (num != 1)
                {
                    throw new IndexOutOfRangeException("Invalid Float2 index!");
                }
                return this.y;
            }
            set
            {
                int num = index;
                if (num == 0)
                {
                    this.x = value;
                }
                else
                {
                    if (num != 1)
                    {
                        throw new IndexOutOfRangeException("Invalid Float2 index!");
                    }
                    this.y = value;
                }
            }
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(this.x * this.x + this.y * this.y);
            }
        }

        public Float2 normalized
        {
            get
            {
                Float2 vector2 = new Float2(this.x, this.y);
                vector2.Normalize();
                return vector2;
            }
        }

        public static Float2 one
        {
            get
            {
                return new Float2(1f, 1f);
            }
        }

        public static Float2 right
        {
            get
            {
                return new Float2(1f, 0f);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return this.x * this.x + this.y * this.y;
            }
        }

        public static Float2 up
        {
            get
            {
                return new Float2(0f, 1f);
            }
        }

        public static Float2 zero
        {
            get
            {
                return new Float2(0f, 0f);
            }
        }

        public static float Angle(Float2 from, Float2 to)
        {
            return (float)Math.Acos(FloatMath.Clamp(Float2.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
        }

        public static float AngleRad(Float2 from, Float2 to)
        {
            return (float)Math.Acos(FloatMath.Clamp(Float2.Dot(from.normalized, to.normalized), -1f, 1f));
        }

        public static Float2 ClampMagnitude(Float2 vector, float maxLength)
        {
            if (vector.sqrMagnitude <= maxLength * maxLength)
            {
                return vector;
            }
            return vector.normalized * maxLength;
        }

        public static float Distance(Float2 a, Float2 b)
        {
            return (a - b).magnitude;
        }

        public static float Dot(Float2 lhs, Float2 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        public override bool Equals(object other)
        {
            if (!(other is Float2))
            {
                return false;
            }
            Float2 vector2 = (Float2)other;
            return (!this.x.Equals(vector2.x) ? false : this.y.Equals(vector2.y));
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        public static Float2 Lerp(Float2 from, Float2 to, float t)
        {
            t = FloatMath.Clamp01(t);
            return new Float2(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
        }

        public static Float2 Max(Float2 lhs, Float2 rhs)
        {
            return new Float2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }

        public static Float2 Min(Float2 lhs, Float2 rhs)
        {
            return new Float2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        }

        public static Float2 MoveTowards(Float2 current, Float2 target, float maxDistanceDelta)
        {
            Float2 vector2 = target - current;
            float single = vector2.magnitude;
            if (single <= maxDistanceDelta || single == 0f)
            {
                return target;
            }
            return current + ((vector2 / single) * maxDistanceDelta);
        }

        public void Normalize()
        {
            float single = this.magnitude;
            if (single <= 1E-05f)
            {
                this = Float2.zero;
            }
            else
            {
                this = this / single;
            }
        }

        public static Float2 operator +(Float2 a, Float2 b)
        {
            return new Float2(a.x + b.x, a.y + b.y);
        }

        public static Float2 operator /(Float2 a, float d)
        {
            return new Float2(a.x / d, a.y / d);
        }

        public static Float2 operator /(Float2 a, Float2 b)
        {
            return new Float2(a.x / b.x, a.y / b.y);
        }

        public static Float2 operator /(Float2 a, Int2 b)
        {
            return new Float2(a.x / b.x, a.y / b.y);
        }

        public static Float2 operator *(Float2 a, Float2 b)
        {
            return new Float2(a.x * b.x, a.y * b.y);
        }

        public static bool operator ==(Float2 lhs, Float2 rhs)
        {
            return Float2.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
        }

        public static implicit operator Float2(Float3 v)
        {
            return new Float2(v.x, v.y);
        }

        public static implicit operator Float3(Float2 v)
        {
            return new Float3(v.x, v.y, 0f);
        }

        public static bool operator !=(Float2 lhs, Float2 rhs)
        {
            return Float2.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
        }

        public static Float2 operator *(Float2 a, float d)
        {
            return new Float2(a.x * d, a.y * d);
        }

        public static Float2 operator *(float d, Float2 a)
        {
            return new Float2(a.x * d, a.y * d);
        }

        public static Float2 operator -(Float2 a, Float2 b)
        {
            return new Float2(a.x - b.x, a.y - b.y);
        }

        public static Float2 operator -(Float2 a)
        {
            return new Float2(-a.x, -a.y);
        }


        public static Float2 operator %(Float2 a, float f)
        {
            return new Float2(a.x % f, a.y % f);
        }

        public static Float2 Scale(Float2 a, Float2 b)
        {
            return new Float2(a.x * b.x, a.y * b.y);
        }


        public static Float2 Rotate(float angle, Float2 vector)
        {
            angle *= FloatMath.Deg2Rad;

            float ca = (float)Math.Cos(angle);
            float sa = (float)Math.Sin(angle);

            return new Float2(ca * vector.x - sa * vector.y, sa * vector.x + ca * vector.y);
        }

        public void Rotate(float angle)
        {
            Float2 v = Rotate(angle, this);

            x = v.x;
            x = v.y;
        }

        public void Scale(Float2 scale)
        {
            Float2 vector2 = this;
            vector2.x = vector2.x * scale.x;
            Float2 vector21 = this;
            vector21.y = vector21.y * scale.y;
        }

        public void Set(float new_x, float new_y)
        {
            this.x = new_x;
            this.y = new_y;
        }

        /// <summary>
        /// Implements a rectangle (step) function returning one for each component of x that is greater than or equal 
        /// v2 the corresponding component in the reference vector a, otherwise and zero gets returned.
        /// </summary>
        /// <param name="a">Input value a</param>
        /// <param name="x">Input value x</param>
        /// <returns>Return a rectangle signal based on a and x</returns>
        public static Float2 Step(Float2 a, Float2 x)
        {
            Float2 result = Float2.zero;

            result.x = FloatMath.Step(a.x, x.x);
            result.y = FloatMath.Step(a.y, x.y);

            return result;
        }



        public static float SqrMagnitude(Float2 a)
        {
            return a.x * a.x + a.y * a.y;
        }

        public float SqrMagnitude()
        {
            return this.x * this.x + this.y * this.y;
        }

        public override string ToString()
        {
            return String.Format("({0:F3}, {1:F3})", new object[] { this.x, this.y });
        }

        public string ToString(string format)
        {
            return String.Format("({0}, {1})", new object[] { this.x.ToString(format), this.y.ToString(format) });
        }


        public static implicit operator Vector2(Float2 v)
        {
            Vector2 vv = new Vector2(v.x, v.y);

            return vv;
        }

        public static implicit operator Float2(Vector2 v)
        {
            Float2 vv = new Float2(v.x, v.y);

            return vv;
        }
    }
}

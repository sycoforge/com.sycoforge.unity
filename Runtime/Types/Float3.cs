using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a three dimensional point. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    //[StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Size = 12)]

    public struct Float3
    {
        public float x;
        public float y;
        public float z;

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Float3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0f;
        }

        public const float kEpsilon = 1E-05f;

        public static Float3 back
        {
            get
            {
                return new Float3(0f, 0f, -1f);
            }
        }

        public static Float3 down
        {
            get
            {
                return new Float3(0f, -1f, 0f);
            }
        }

        public static Float3 forward
        {
            get
            {
                return new Float3(0f, 0f, 1f);
            }
        }

        [Obsolete("Use Float3.forward instead.")]
        public static Float3 fwd
        {
            get
            {
                return new Float3(0f, 0f, 1f);
            }
        }

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
                }
                throw new IndexOutOfRangeException("Invalid Float3 index!");
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
                    default:
                        {
                            throw new IndexOutOfRangeException("Invalid Float3 index!");
                        }
                }
            }
        }

        public static Float3 left
        {
            get
            {
                return new Float3(-1f, 0f, 0f);
            }
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        public Float3 normalized
        {
            get
            {
                return Float3.Normalize(this);
            }
        }

        public static Float3 one
        {
            get
            {
                return new Float3(1f, 1f, 1f);
            }
        }

        public static Float3 right
        {
            get
            {
                return new Float3(1f, 0f, 0f);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return this.x * this.x + this.y * this.y + this.z * this.z;
            }
        }

        public static Float3 up
        {
            get
            {
                return new Float3(0f, 1f, 0f);
            }
        }

        public static Float3 zero
        {
            get
            {
                return new Float3(0f, 0f, 0f);
            }
        }


        public static float Angle(Float3 from, Float3 to)
        {
            float val = Float3.Dot(from.normalized, to.normalized);
            return (float)Math.Acos(FloatMath.Clamp(val, -1f, 1f)) * FloatMath.Deg2Rad;
        }


        public static Float3 ClampMagnitude(Float3 vector, float maxLength)
        {
            if (vector.sqrMagnitude <= maxLength * maxLength)
            {
                return vector;
            }
            return vector.normalized * maxLength;
        }

        public static Float3 Cross(Float3 lhs, Float3 rhs)
        {
            return new Float3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public static float Distance(Float3 a, Float3 b)
        {
            Float3 vector3 = new Float3(a.x - b.x, a.y - b.y, a.z - b.z);
            return (float)Math.Sqrt(vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z);
        }

        public static float Dot(Float3 lhs, Float3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public override bool Equals(object other)
        {
            if (!(other is Float3))
            {
                return false;
            }
            Float3 vector3 = (Float3)other;
            return (!this.x.Equals(vector3.x) || !this.y.Equals(vector3.y) ? false : this.z.Equals(vector3.z));
        }

        [Obsolete("Use Float3.ProjectOnPlane instead.")]
        public static Float3 Exclude(Float3 excludeThis, Float3 fromThat)
        {
            return fromThat - Float3.Project(fromThat, excludeThis);
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
        }

        public static Float3 Lerp(Float3 from, Float3 to, float t)
        {
            t = FloatMath.Clamp01(t);
            return new Float3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
        }

        public static float Magnitude(Float3 a)
        {
            return (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
        }

        public static Float3 Max(Float3 lhs, Float3 rhs)
        {
            return new Float3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        public static Float3 Min(Float3 lhs, Float3 rhs)
        {
            return new Float3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        public static Float3 MoveTowards(Float3 current, Float3 target, float maxDistanceDelta)
        {
            Float3 vector3 = target - current;
            float single = vector3.magnitude;
            if (single <= maxDistanceDelta || single == 0f)
            {
                return target;
            }
            return current + ((vector3 / single) * maxDistanceDelta);
        }

        public static Float3 Normalize(Float3 value)
        {
            float single = Float3.Magnitude(value);
            if (single <= 1E-05f)
            {
                return Float3.zero;
            }
            return value / single;
        }

        public void Normalize()
        {
            float single = Float3.Magnitude(this);
            if (single <= 1E-05f)
            {
                this = Float3.zero;
            }
            else
            {
                this = this / single;
            }
        }

        public static Float3 operator +(Float3 a, Float3 b)
        {
            return new Float3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Float3 operator /(Float3 a, float d)
        {
            return new Float3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Float3 lhs, Float3 rhs)
        {
            return Float3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
        }

        public static bool operator !=(Float3 lhs, Float3 rhs)
        {
            return Float3.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
        }

        public static Float3 operator *(Float3 a, float d)
        {
            return new Float3(a.x * d, a.y * d, a.z * d);
        }

        public static Float3 operator *(float d, Float3 a)
        {
            return new Float3(a.x * d, a.y * d, a.z * d);
        }

        public static Float3 operator *(Float3 a, Float3 b)
        {
            return new Float3(a.x * a.x, a.y * a.y, a.z * a.z);
        }

        public static Float3 operator /(Float3 a, Float3 b)
        {
            return new Float3(a.x / a.x, a.y / a.y, a.z / a.z);
        }

        public static Float3 operator /(Float3 a, Int3 b)
        {
            return new Float3(a.x / a.x, a.y / a.y, a.z / a.z);
        }

        public static Float3 operator -(Float3 a, Float3 b)
        {
            return new Float3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Float3 operator -(Float3 a)
        {
            return new Float3(-a.x, -a.y, -a.z);
        }

        public static Float3 Project(Float3 vector, Float3 onNormal)
        {
            float single = Float3.Dot(onNormal, onNormal);
            if (single < 1.401298E-45f)
            {
                return Float3.zero;
            }
            return (onNormal * Float3.Dot(vector, onNormal)) / single;
        }

        public static Float3 ProjectOnPlane(Float3 vector, Float3 planeNormal)
        {
            return vector - Float3.Project(vector, planeNormal);
        }

        public static Float3 Reflect(Float3 inDirection, Float3 inNormal)
        {
            return (-2f * Float3.Dot(inNormal, inDirection) * inNormal) + inDirection;
        }

        public static Float3 Scale(Float3 a, Float3 b)
        {
            return new Float3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public void Scale(Float3 scale)
        {
            Float3 vector3 = this;
            vector3.x = vector3.x * scale.x;
            Float3 vector31 = this;
            vector31.y = vector31.y * scale.y;
            Float3 vector32 = this;
            vector32.z = vector32.z * scale.z;
        }

        public void Set(float new_x, float new_y, float new_z)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
        }

        /// <summary>
        /// Implements a rectangle (step) function returning one for each component of x that is greater than or equal 
        /// v2 the corresponding component in the reference vector a, otherwise and zero gets returned.
        /// </summary>
        /// <param name="a">Input value a</param>
        /// <param name="x">Input value x</param>
        /// <returns>Return a rectangle signal based on a and x</returns>
        public static Float3 Step(Float3 a, Float3 x)
        {
            Float3 result = Float3.zero;

            result.x = FloatMath.Step(a.x, x.x);
            result.y = FloatMath.Step(a.y, x.y);
            result.z = FloatMath.Step(a.z, x.z);

            return result;
        }

        public static float SqrMagnitude(Float3 a)
        {
            return a.x * a.x + a.y * a.y + a.z * a.z;
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1})", new object[] { this.x, this.y, this.z });
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2})", new object[] { this.x.ToString(format), this.y.ToString(format), this.z.ToString(format) });
        }


        public static implicit operator Vector3(Float3 v)
        {
            Vector3 vv = new Vector3(v.x, v.y, v.z);

            return vv;
        }

        public static implicit operator Float3(Vector3 v)
        {
            Float3 vv = new Float3(v.x, v.y, v.z);

            return vv;
        }
    }
}


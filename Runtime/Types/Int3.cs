using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a three dimensinal integer point. 
    /// </summary>
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Int3
    {
        [MarshalAs(UnmanagedType.I4)]
        public int x;

        [MarshalAs(UnmanagedType.I4)]
        public int y;

        [MarshalAs(UnmanagedType.I4)]
        public int z;

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Int3 back
        {
            get
            {
                return new Int3(0, 0, -1);
            }
        }

        public static Int3 down
        {
            get
            {
                return new Int3(0, -1, 0);
            }
        }

        public static Int3 forward
        {
            get
            {
                return new Int3(0, 0, 1);
            }
        }

        public int this[int index]
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
                throw new IndexOutOfRangeException("Invalid Int3 index!");
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
                            throw new IndexOutOfRangeException("Invalid Int3 index!");
                        }
                }
            }
        }

        public static Int3 left
        {
            get
            {
                return new Int3(-1, 0, 0);
            }
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        public static Int3 one
        {
            get
            {
                return new Int3(1, 1, 1);
            }
        }

        public static Int3 right
        {
            get
            {
                return new Int3(1, 0, 0);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return this.x * this.x + this.y * this.y + this.z * this.z;
            }
        }

        public static Int3 up
        {
            get
            {
                return new Int3(0, 1, 0);
            }
        }

        public static Int3 zero
        {
            get
            {
                return new Int3(0, 0, 0);
            }
        }

        public static Int3 Cross(Int3 lhs, Int3 rhs)
        {
            return new Int3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public static float Distance(Int3 a, Int3 b)
        {
            Int3 vector3 = new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
            return (float)Math.Sqrt(vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z);
        }

        public static float Dot(Int3 lhs, Int3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public override bool Equals(object other)
        {
            if (!(other is Int3))
            {
                return false;
            }
            Int3 vector3 = (Int3)other;
            return (!this.x.Equals(vector3.x) || !this.y.Equals(vector3.y) ? false : this.z.Equals(vector3.z));
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
        }

        //public static Int3 Lerp(Int3 v1, Int3 v2, float t)
        //{
        //    t = FloatMath.Clamp01(t);
        //    return new Int3(v1.x + (v2.x - v1.x) * t, v1.y + (v2.y - v1.y) * t, v1.z + (v2.z - v1.z) * t);
        //}

        public static float Magnitude(Int3 a)
        {
            return (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
        }

        public static Int3 Max(Int3 lhs, Int3 rhs)
        {
            return new Int3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        public static Int3 Min(Int3 lhs, Int3 rhs)
        {
            return new Int3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        public static Int3 operator +(Int3 a, Int3 b)
        {
            return new Int3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Int3 operator /(Int3 a, int d)
        {
            return new Int3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Int3 lhs, Int3 rhs)
        {
            return Int3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
        }

        public static bool operator !=(Int3 lhs, Int3 rhs)
        {
            return Int3.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
        }

        public static Int3 operator *(Int3 a, int d)
        {
            return new Int3(a.x * d, a.y * d, a.z * d);
        }

        public static Int3 operator *(int d, Int3 a)
        {
            return new Int3(a.x * d, a.y * d, a.z * d);
        }

        public static Int3 operator -(Int3 a, Int3 b)
        {
            return new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Int3 operator -(Int3 a)
        {
            return new Int3(-a.x, -a.y, -a.z);
        }

        public static Int3 Scale(Int3 a, Int3 b)
        {
            return new Int3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public void Scale(Int3 scale)
        {
            Int3 vector3 = this;
            vector3.x = vector3.x * scale.x;
            Int3 vector31 = this;
            vector31.y = vector31.y * scale.y;
            Int3 vector32 = this;
            vector32.z = vector32.z * scale.z;
        }

        public void Set(int new_x, int new_y, int new_z)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
        }



        public static float SqrMagnitude(Int3 a)
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
    }
}

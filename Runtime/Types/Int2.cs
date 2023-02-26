using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a two dimensinal integer point. 
    /// </summary>
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Int2
    {
        [MarshalAs(UnmanagedType.I4)]
        public int x;

        [MarshalAs(UnmanagedType.I4)]
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("x: {0} y: {1}", x, y);
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator /(Int2 a, float d)
        {
            return new Int2((int)(a.x / d), (int)(a.y / d));
        }

        public static bool operator !=(Int2 lhs, Int2 rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }


        public static bool operator ==(Int2 lhs, Int2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }
        public static Int2 operator *(Int2 a, float d)
        {
            return new Int2((int)(a.x * d), (int)(a.y * d));
        }

        public static Int2 operator *(float d, Int2 a)
        {
            return new Int2((int)(a.x * d), (int)(a.y * d));
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return new Int2(a.x - b.x, a.y - b.y);
        }

        public static Int2 operator -(Int2 a)
        {
            return new Int2(-a.x, -a.y);
        }


        public static Int2 Scale(Int2 a, Int2 b)
        {
            return new Int2(a.x * b.x, a.y * b.y);
        }

        public static implicit operator Int2(Int3 v)
        {
            return new Int2(v.x, v.y);
        }

        public static implicit operator Int3(Int2 v)
        {
            return new Int3(v.x, v.y, 0);
        }
    }
}

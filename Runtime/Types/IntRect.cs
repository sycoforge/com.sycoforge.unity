using System;
using System.Runtime.InteropServices;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a three dimensinal integer point. 
    /// </summary>
    [Serializable]    
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct IntRect
    {
        public static IntRect Empty = new IntRect();

        [MarshalAs(UnmanagedType.R4)]
        private int m_XMin;

        [MarshalAs(UnmanagedType.R4)]
        private int m_YMin;

        [MarshalAs(UnmanagedType.R4)]
        private int m_Width;

        [MarshalAs(UnmanagedType.R4)]
        private int m_Height;

        public Int2 center
        {
            get
            {
                return new Int2(this.x + this.m_Width / 2, this.y + this.m_Height / 2);
            }
            set
            {
                this.m_XMin = value.x - this.m_Width / 2;
                this.m_YMin = value.y - this.m_Height / 2;
            }
        }

        public int height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        [Obsolete("use xMin")]
        public int left
        {
            get
            {
                return this.m_XMin;
            }
        }

        [Obsolete("use yMax")]
        public int bottom
        {
            get
            {
                return this.m_YMin + this.m_Height;
            }
        }

        public Int2 max
        {
            get
            {
                return new Int2(this.xMax, this.yMax);
            }
            set
            {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }

        public Int2 min
        {
            get
            {
                return new Int2(this.xMin, this.yMin);
            }
            set
            {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }

        public Int2 position
        {
            get
            {
                return new Int2(this.m_XMin, this.m_YMin);
            }
            set
            {
                this.m_XMin = value.x;
                this.m_YMin = value.y;
            }
        }

        [Obsolete("use xMax")]
        public float right
        {
            get
            {
                return this.m_XMin + this.m_Width;
            }
        }

        public Int2 size
        {
            get
            {
                return new Int2(this.m_Width, this.m_Height);
            }
            set
            {
                this.m_Width = value.x;
                this.m_Height = value.y;
            }
        }

        [Obsolete("use yMin")]
        public float top
        {
            get
            {
                return this.m_YMin;
            }
        }

        public int width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }

        public int x
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                this.m_XMin = value;
            }
        }

        public int xMax
        {
            get
            {
                return this.m_Width + this.m_XMin;
            }
            set
            {
                this.m_Width = value - this.m_XMin;
            }
        }

        public int xMin
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                int single = this.xMax;
                this.m_XMin = value;
                this.m_Width = single - this.m_XMin;
            }
        }

        public int y
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                this.m_YMin = value;
            }
        }

        public int yMax
        {
            get
            {
                return this.m_Height + this.m_YMin;
            }
            set
            {
                this.m_Height = value - this.m_YMin;
            }
        }

        public int yMin
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                int single = this.yMax;
                this.m_YMin = value;
                this.m_Height = single - this.m_YMin;
            }
        }

        //------------------------------
        // Constructor
        //------------------------------ 


        public IntRect(int left, int top, int width, int height)
        {
            this.m_XMin = left;
            this.m_YMin = top;
            this.m_Width = width;
            this.m_Height = height;
        }

        public IntRect(IntRect source)
        {
            this.m_XMin = source.m_XMin;
            this.m_YMin = source.m_YMin;
            this.m_Width = source.m_Width;
            this.m_Height = source.m_Height;
        }

        //------------------------------
        // Methods
        //------------------------------ 

        public bool Contains(Float2 point)
        {
            return (point.x < this.xMin || point.x >= this.xMax || point.y < this.yMin ? false : point.y < this.yMax);
        }

        public bool Contains(Float3 point)
        {
            return Contains(new Float2(point.x, point.y));
        }

        public bool Contains(Float3 point, bool allowInverse)
        {
            if (!allowInverse)
            {
                return this.Contains(point);
            }
            bool flag = false;
            if (this.width < 0f && point.x <= this.xMin && point.x > this.xMax || this.width >= 0f && point.x >= this.xMin && point.x < this.xMax)
            {
                flag = true;
            }
            if (flag && (this.height < 0f && point.y <= this.yMin && point.y > this.yMax || this.height >= 0f && point.y >= this.yMin && point.y < this.yMax))
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object other)
        {
            if (!(other is IntRect))
            {
                return false;
            }
            IntRect rect = (IntRect)other;
            return (!this.x.Equals(rect.x) || !this.y.Equals(rect.y) || !this.width.Equals(rect.width) ? false : this.height.Equals(rect.height));
        }

        public override int GetHashCode()
        {
            int hashCode = this.x.GetHashCode();
            float single = this.width;
            float single1 = this.y;
            float single2 = this.height;
            return hashCode ^ single.GetHashCode() << 2 ^ single1.GetHashCode() >> 2 ^ single2.GetHashCode() >> 1;
        }

        public static IntRect MinMaxRect(int left, int top, int right, int bottom)
        {
            return new IntRect(left, top, right - left, bottom - top);
        }

        public static Float2 NormalizedToPoint(IntRect rectangle, Float2 normalizedRectCoordinates)
        {
            return new Float2(FloatMath.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), FloatMath.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }

        public static bool operator ==(IntRect lhs, IntRect rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width ? false : lhs.height == rhs.height);
        }

        public static bool operator !=(IntRect lhs, IntRect rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width ? true : lhs.height != rhs.height);
        }

        private static IntRect OrderMinMax(IntRect rect)
        {
            if (rect.xMin > rect.xMax)
            {
                int single = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = single;
            }
            if (rect.yMin > rect.yMax)
            {
                int single1 = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = single1;
            }
            return rect;
        }

        public bool Overlaps(IntRect other)
        {
            return (other.xMax <= this.xMin || other.xMin >= this.xMax || other.yMax <= this.yMin ? false : other.yMin < this.yMax);
        }

        public bool Overlaps(IntRect other, bool allowInverse)
        {
            IntRect rect = this;
            if (allowInverse)
            {
                rect = IntRect.OrderMinMax(rect);
                other = IntRect.OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }

        public static Float2 PointToNormalized(IntRect rectangle, Float2 point)
        {
            return new Float2(FloatMath.InverseLerp(rectangle.x, rectangle.xMax, point.x), FloatMath.InverseLerp(rectangle.y, rectangle.yMax, point.y));
        }

        public void Set(int x, int y, int width, int height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public void Set(Int2 position, int width, int height)
        {
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public override string ToString()
        {
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", new object[] { this.x, this.y, this.width, this.height });
        }
    }
}

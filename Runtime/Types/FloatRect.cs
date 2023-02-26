using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a floating point rectangle. 
    /// </summary>
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct FloatRect
    {
        public static FloatRect Empty = new FloatRect();

        [MarshalAs(UnmanagedType.R4)]
        private float m_XMin;

        [MarshalAs(UnmanagedType.R4)]
        private float m_YMin;

        [MarshalAs(UnmanagedType.R4)]
        private float m_Width;

        [MarshalAs(UnmanagedType.R4)]
        private float m_Height;

        public Float2 center
        {
            get
            {
                return new Float2(this.x + this.m_Width / 2f, this.y + this.m_Height / 2f);
            }
            set
            {
                this.m_XMin = value.x - this.m_Width / 2f;
                this.m_YMin = value.y - this.m_Height / 2f;
            }
        }

        public float height
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
        public float left
        {
            get
            {
                return this.m_XMin;
            }
        }

        [Obsolete("use yMax")]
        public float bottom
        {
            get
            {
                return this.m_YMin + this.m_Height;
            }
        }

        public Float2 max
        {
            get
            {
                return new Float2(this.xMax, this.yMax);
            }
            set
            {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }

        public Float2 min
        {
            get
            {
                return new Float2(this.xMin, this.yMin);
            }
            set
            {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }

        public Float2 position
        {
            get
            {
                return new Float2(this.m_XMin, this.m_YMin);
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

        public Float2 size
        {
            get
            {
                return new Float2(this.m_Width, this.m_Height);
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

        public float width
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

        public float x
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

        public float xMax
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

        public float xMin
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                float single = this.xMax;
                this.m_XMin = value;
                this.m_Width = single - this.m_XMin;
            }
        }

        public float y
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

        public float yMax
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

        public float yMin
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                float single = this.yMax;
                this.m_YMin = value;
                this.m_Height = single - this.m_YMin;
            }
        }
        
        //------------------------------
        // Constructor
        //------------------------------ 


        public FloatRect(float left, float top, float width, float height)
        {
            this.m_XMin = left;
            this.m_YMin = top;
            this.m_Width = width;
            this.m_Height = height;
        }

        public FloatRect(Float2 position, Float2 size)
        {
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = size.x;
            this.m_Height = size.y;
        }

        public FloatRect(FloatRect source)
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
            if (!(other is FloatRect))
            {
                return false;
            }
            FloatRect rect = (FloatRect)other;
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

        public static FloatRect MinMaxRect(float left, float top, float right, float bottom)
        {
            return new FloatRect(left, top, right - left, bottom - top);
        }

        public static Float2 NormalizedToPoint(FloatRect rectangle, Float2 normalizedRectCoordinates)
        {
            return new Float2(FloatMath.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), FloatMath.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }

        public static bool operator ==(FloatRect lhs, FloatRect rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width ? false : lhs.height == rhs.height);
        }

        public static bool operator !=(FloatRect lhs, FloatRect rhs)
        {
            return (lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width ? true : lhs.height != rhs.height);
        }

        private static FloatRect OrderMinMax(FloatRect rect)
        {
            if (rect.xMin > rect.xMax)
            {
                float single = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = single;
            }
            if (rect.yMin > rect.yMax)
            {
                float single1 = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = single1;
            }
            return rect;
        }

        public bool Overlaps(FloatRect other)
        {
            return (other.xMax <= this.xMin || other.xMin >= this.xMax || other.yMax <= this.yMin ? false : other.yMin < this.yMax);
        }

        public bool Overlaps(FloatRect other, bool allowInverse)
        {
            FloatRect rect = this;
            if (allowInverse)
            {
                rect = FloatRect.OrderMinMax(rect);
                other = FloatRect.OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }

        public static Float2 PointToNormalized(FloatRect rectangle, Float2 point)
        {
            return new Float2(FloatMath.InverseLerp(rectangle.x, rectangle.xMax, point.x), FloatMath.InverseLerp(rectangle.y, rectangle.yMax, point.y));
        }

        public void Set(float x, float y, float width, float height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public void Set(Float2 position, float width, float height)
        {
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public override string ToString()
        {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", new object[] { this.x, this.y, this.width, this.height });
        }


        public static implicit operator Rect(FloatRect margin)
        {
            Rect offset = new Rect(margin.xMin, margin.yMin, margin.width, margin.height);

            return offset;
        }

        public static implicit operator FloatRect(Rect margin)
        {
            FloatRect offset = new FloatRect(margin.xMin, margin.yMin, margin.width, margin.height);

            return offset;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;


namespace ch.sycoforge.Types
{
    [Serializable]
    public struct IntThickness
    {
        public int bottom;


        public int horizontal
        {
            get
            {
                return left + right;
            }
        }

        public int left;

        public int right;

        public int top;

        public int vertical
        {
            get
            {
                return top + bottom;
            }
        }


        public IntThickness(int left, int right, int top, int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        public override string ToString()
        {
            return string.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", new object[] { this.left, this.right, this.top, this.bottom });
        }

        public static implicit operator RectOffset(IntThickness margin)
        {
            RectOffset offset = new RectOffset(margin.left, margin.right, margin.top, margin.bottom);

            return offset;
        }

        public static implicit operator IntThickness(RectOffset margin)
        {
            IntThickness offset = new IntThickness(margin.left, margin.right, margin.top, margin.bottom);

            return offset;
        }
    }
}

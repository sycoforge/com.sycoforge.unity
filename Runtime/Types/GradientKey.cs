using ch.sycoforge.Types;
using System;
using System.Runtime.InteropServices;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a two dimensional point. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    //[StructLayout(LayoutKind.Sequential, Size = 20)]
    public struct GradientKey
    {
        [MarshalAs(UnmanagedType.Struct), FieldOffset(0)]
        public Float4 Color;

        [MarshalAs(UnmanagedType.R4), FieldOffset(16)]
        public float Position;

        [MarshalAs(UnmanagedType.Struct), FieldOffset(20)]
        public Float3 Data;

        //public float Position
        //{
        //    get
        //    {
        //        return this.position;
        //    }
        //    set
        //    {
        //        this.position = value;
        //    }
        //}

        //public Float4 Color
        //{
        //    get
        //    {
        //        return this.color;
        //    }
        //    set
        //    {
        //        this.color = value;
        //    }
        //}


        public GradientKey(Float4 color, float position)
        {
            this.Color = color;
            this.Position = position;
            this.Data = new Float3();
        }
    }
}

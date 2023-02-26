using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Types
{
    public struct IntColor
    {
        public int argb;


        public IntColor(int argb)
        {
            this.argb = argb;
        }

        public static implicit operator IntColor(ByteColor c)
        {
            IntColor color = new IntColor();

            int a = ((int)c.a) << 24;
            int r = ((int)c.r) << 16;
            int g = ((int)c.g) << 8;
            int b = ((int)c.b);

            color.argb = a | r | g | b;

            return color;
        }

        public static implicit operator IntColor(int c)
        {
            IntColor color = new IntColor();
            color.argb = c;

            return color;
        }

        public static implicit operator int(IntColor c)
        {
            return c.argb;
        }

        public static implicit operator ByteColor(IntColor c)
        {
            ByteColor color = new ByteColor();

            color.a = (byte)((c.argb >> 24) & 0x0000FF);
            color.r = (byte)((c.argb >> 16) & 0x0000FF);
            color.g = (byte)((c.argb >> 8) & 0x0000FF);
            color.b = (byte)((c.argb) & 0x0000FF);

            return color;
        }
    }
}

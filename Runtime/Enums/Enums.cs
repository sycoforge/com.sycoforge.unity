using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Runtime.Enums
{
    public enum TargetChannel
    {
        Alpha, Red, Green, Blue, RGB
    }

    public enum Channel
    {
        RGB = 0,
        RGBA = 1,
        R = 2,
        G = 3,
        B = 4,
        A = 5,
        Grayscale = 6
    }

    public enum SampleMode
    {
        Tileable = 0,
        Clamp = 1,
    }

    public enum ColorSpaceMode
    {
        Gamma,
        Linear
    }
}

using System;
using System.Runtime.InteropServices;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a two dimensinal point. 
    /// </summary>
    [ComVisible(true)]
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Double2
    {
        private double x;
        private double y;

        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public Double2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

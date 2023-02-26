using System;
using System.Runtime.InteropServices;

namespace ch.sycoforge.Types
{
    /// <summary>
    /// Struct representing a two dimensinal integer point. 
    /// </summary>
    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct FloatMatrix
    {
        //------------------------------
        // Fields
        //------------------------------
        public float[] values;

        [MarshalAs(UnmanagedType.I4)]
        public int width;

        [MarshalAs(UnmanagedType.I4)]
        public int height;

        public int Size
        {
            get { return width * height; }
        }

        //------------------------------
        // Constructor
        //------------------------------

        public FloatMatrix(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.values = new float[width * height];
        }

        public FloatMatrix(float[,] matrix)
        {
            this.width = matrix.GetLength(0);
            this.height = matrix.GetLength(1);
            this.values = new float[width * height];

            Initialize(matrix);
        }

        //------------------------------
        // Methods
        //------------------------------

        public float GetValue(int x, int y)
        {
            int index = FloatMath.Clamp((y * width) + x, 0, Size - 1);

            return values[index];
        }

        public void SetValue(int x, int y, float value)
        {
            int index = FloatMath.Clamp((y * width) + x, 0, Size - 1);

            values[index] = value;
        }

        private void Initialize(float[,] matrix)
        {
            int i = 0;
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    values[i++] = matrix[x, y];
                }
            }
        }
    }
}

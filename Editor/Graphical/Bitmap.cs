using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Graphical
{
    public enum Format
    {
        PNG,
        JPEG
    }

    public class Bitmap
    {
        public Color[] Pixels
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public Bitmap(int width, int height)
        {
            Width = width;
            Height = height;

            Pixels = new Color[width * height];
        }

        public Bitmap(Texture2D texture)
        {
            Width = texture.width;
            Height = texture.height;

            Pixels = texture.GetPixels(); ;
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = (y * Width) + x;

            Pixels[index] = color;
        }

        public Color SetPixel(int x, int y)
        {
            int index = (y * Width) + x;

            return Pixels[index];
        }

        public static void Save(Bitmap bitmap, string path, Format format)
        {
            // Encode texture into a PNG byte buffer
            byte[] buffer = null;

            Texture2D output = new Texture2D(bitmap.Width, bitmap.Height);
            output.SetPixels(bitmap.Pixels);

            if (format == Format.PNG)
            {
                buffer = output.EncodeToPNG();
            }
            else if (format == Format.PNG)
            {
                buffer = output.EncodeToJPG();
            }

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                // Write the data to the file, byte by byte. 
                for (int i = 0; i < buffer.Length; i++)
                {
                    fileStream.WriteByte(buffer[i]);
                }
            }
        }
    }
}

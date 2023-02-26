using System.IO;
using UnityEngine;

namespace ch.sycoforge.Unity.TextureUtil
{
    public enum Format
    {
        PNG,
        JPEG
    }

    /// <summary>
    /// Texture sprite utillity methods.
    /// </summary>
    public static class TextureUtil
    {

        public static Color32 ToColor32(int c)
        {
            Color32 color = new Color32();

            color.a = (byte)((c >> 24) & 0x0000FF);
            color.r = (byte)((c >> 16) & 0x0000FF);
            color.g = (byte)((c >> 8) & 0x0000FF);
            color.b = (byte)((c) & 0x0000FF);

            return color;
        }

        public static void Save(Texture2D texture, string path, Format format)
        {
            // Encode texture into a PNG byte buffer
            byte[] buffer = null;

            if (format == Format.PNG)
            {
                buffer = texture.EncodeToPNG();
            }
            else if (format == Format.PNG)
            {
                buffer = texture.EncodeToJPG();
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

        public static void Save(Color[] texture, int width, int height, string path, Format format)
        {
            // Encode texture into a PNG byte buffer
            byte[] buffer = null;

            Texture2D output = new Texture2D(width, height);
            output.SetPixels(texture);

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

        public static void Save(int[] texture, int width, int height, string path, Format format)
        {
            // Encode texture into a PNG byte buffer
            byte[] buffer = null;

            Texture2D output = new Texture2D(width, height);
            Color32[] colors = new Color32[texture.Length];

            for (int i = 0; i < texture.Length; i++)
            {
                colors[i] = ToColor32(texture[i]);
            }

            output.SetPixels32(colors);

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

        public static Texture2D Load(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {

                // Read the source file into a byte array.
                byte[] buffer = new byte[fileStream.Length];

                int size = (int)fileStream.Length;
                int bytesRead = 0;

                while (size > 0)
                {
                    // Read return anything from 0 to bytesRead
                    int n = fileStream.Read(buffer, bytesRead, size);

                    // Stop when reach the end of the file
                    if (n == 0)
                    {
                        break;
                    }

                    bytesRead += n;
                    size -= n;
                }

                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(buffer);

                return texture;
            }
        }

        public static Color[] LoadColors(string path, out int width, out int height)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // Read the source file into a byte array.
                byte[] buffer = new byte[fileStream.Length];

                int size = (int)fileStream.Length;
                int bytesRead = 0;

                while (size > 0)
                {
                    // Read return anything from 0 to bytesRead
                    int n = fileStream.Read(buffer, bytesRead, size);

                    // Stop when reach the end of the file
                    if (n == 0)
                    {
                        break;
                    }

                    bytesRead += n;
                    size -= n;
                }

                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(buffer);

                width = texture.width;
                height = texture.height;

                return texture.GetPixels();
            }
        }


        public static void SetColor(Color[] colors, int x, int y, int width, Color color)
        {
            colors[width * y + x] = color;
        }

        public static Color GetColor(Color[] colors, int x, int y, int width)
        {
            return colors[width * y + x];
        }

        public enum PixelMode
        {
            Repeat, RepeatComplete, NearestNeighbour, Clamp, Bilinear
        }



        /// <summary>
        /// Returns a 1x1 texture in the specified color.
        /// </summary>
        /// <param name="r">The color of the texture.</param>
        /// <returns>Returns a 1x1 texture.</returns>
        public static Texture2D MakeTexture(Color color)
        {
            return MakeTexture(color.r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Returns a 1x1 texture in the specified color.
        /// </summary>
        /// <param name="r">The color of the texture.</param>
        /// <returns>Returns a texture in the specified size and color..</returns>
        public static Texture2D MakeTexture(Color color, int width, int height)
        {
            return MakeTexture(color.r, color.g, color.b, color.a, width, height);
        }

        /// <summary>
        /// Returns a 1x1 texture in the specified color.
        /// </summary>
        /// <param name="r">The red color component.</param>
        /// <param name="g">The green color component.</param>
        /// <param name="b">The blue color component.</param>
        /// <param name="a">The alpha component.</param>
        /// <returns>Returns a 1x1 texture.</returns>
        public static Texture2D MakeTexture(float r, float g, float b, float a)
        {
            return MakeTexture(r, g, b, a, 1, 1);
        }

        /// <summary>
        /// Returns a texture in the specified color and size.
        /// </summary>
        /// <param name="r">The red color component.</param>
        /// <param name="g">The green color component.</param>
        /// <param name="b">The blue color component.</param>
        /// <param name="a">The alpha component.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <returns></returns>
        public static Texture2D MakeTexture(float r, float g, float b, float a, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            Color color = new Color(r, g, b, a);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }


        /// <summary>
        /// Returns a blank texture in the specified size.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <returns></returns>
        public static Texture2D MakeTexture(int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            return texture;
        }

        public static Texture2D ScaleNineSliced(
            Texture2D sourceTexture,
            Rect targetRectangle,
            RectOffset sizingMargins, PixelMode mode)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            Texture2D targetTexture = TextureUtil.MakeTexture((int)targetRectangle.width, (int)targetRectangle.height);

            //calculate sizes for each slice to cut from the original image
            float leftX = 0;
            float rightX = sourceTexture.width - sizingMargins.right;
            float centerX = sizingMargins.left;

            float topY = 0;
            float bottomY = sourceTexture.height - sizingMargins.bottom;
            float centerY = sizingMargins.top;

            float topHeight = sizingMargins.top;
            float bottomHeight = sizingMargins.bottom;
            float centerHeight = sourceTexture.height - sizingMargins.vertical;

            float leftWidth = sizingMargins.left;
            float rightWidth = sizingMargins.right;
            float centerWidth = sourceTexture.width - sizingMargins.horizontal;

            //declare the bounds for each slice using the values above
            Rect topLeftSrc = new Rect(leftX, topY, leftWidth, topHeight);
            Rect topCenterSrc = new Rect(centerX, topY, centerWidth, topHeight);
            Rect topRightSrc = new Rect(rightX, topY, rightWidth, topHeight);

            Rect bottomLeftSrc = new Rect(leftX, bottomY, leftWidth, bottomHeight);
            Rect bottomCenterSrc = new Rect(centerX, bottomY, centerWidth, bottomHeight);
            Rect bottomRightSrc = new Rect(rightX, bottomY, rightWidth, bottomHeight);

            Rect centerLeftSrc = new Rect(leftX, centerY, leftWidth, centerHeight);
            Rect centerCenterSrc = new Rect(centerX, centerY, centerWidth, centerHeight);
            Rect centerRightSrc = new Rect(rightX, centerY, rightWidth, centerHeight);

            //calculate sizes for each slice to be drawn to the screen

            //x positions for left, right and center slices
            leftX = targetRectangle.x;
            rightX = targetRectangle.xMax - sizingMargins.right;
            centerX = targetRectangle.x + sizingMargins.left;

            //y positions for top, bottom and center slices
            topY = targetRectangle.y;
            bottomY = targetRectangle.yMax - sizingMargins.bottom;
            centerY = targetRectangle.y + sizingMargins.top;

            //heights for left, right and center slices
            topHeight = sizingMargins.top;
            bottomHeight = sizingMargins.bottom;
            centerHeight = targetRectangle.height - sizingMargins.vertical;

            //widths for top, bottom and center slices
            leftWidth = sizingMargins.left;
            rightWidth = sizingMargins.right;
            centerWidth = targetRectangle.width - sizingMargins.horizontal;

            //declare the bounds for each slice using the values above
            Rect topLeftDest = new Rect(leftX, topY, leftWidth, topHeight);
            Rect topCenterDest = new Rect(centerX, topY, centerWidth, topHeight);
            Rect topRightDest = new Rect(rightX, topY, rightWidth, topHeight);

            Rect bottomLeftDest = new Rect(leftX, bottomY, leftWidth, bottomHeight);
            Rect bottomCenterDest = new Rect(centerX, bottomY, centerWidth, bottomHeight);
            Rect bottomRightDest = new Rect(rightX, bottomY, rightWidth, bottomHeight);

            Rect centerLeftDest = new Rect(leftX, centerY, leftWidth, centerHeight);
            Rect centerCenterDest = new Rect(centerX, centerY, centerWidth, centerHeight);
            Rect centerRightDest = new Rect(rightX, centerY, rightWidth, centerHeight);

            //draw each slice to the screen
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, topLeftSrc, topLeftDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, topCenterSrc, topCenterDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, topRightSrc, topRightDest, mode);

            TextureUtil.DrawToTexture(sourceTexture, targetTexture, bottomLeftSrc, bottomLeftDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, bottomCenterSrc, bottomCenterDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, bottomRightSrc, bottomRightDest, mode);

            TextureUtil.DrawToTexture(sourceTexture, targetTexture, centerLeftSrc, centerLeftDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, centerCenterSrc, centerCenterDest, mode);
            TextureUtil.DrawToTexture(sourceTexture, targetTexture, centerRightSrc, centerRightDest, mode);

            stopWatch.Stop();


            return targetTexture;
        }

        public static void DrawToTexture(Texture2D source, Texture2D target, Rect from, Rect to, PixelMode mode = PixelMode.NearestNeighbour)
        {
            bool wrapX = from.width < to.width;
            bool wrapY = from.height < to.height;

            if (to.height == 0 || to.width == 0 || from.height == 0 || from.width == 0)
            {
                return;
            }

            Color[] colorBuffer = new Color[(int)to.width * (int)to.height];

            for (int x = 0; x < (int)to.width; x++)
            {
                int xSource = (int)from.x + x;
                int xTarget = (int)to.x + x;

                for (int y = 0; y < (int)to.height; y++)
                {
                    int ySource = (int)from.y + y;
                    int yTarget = (int)to.y + y;

                    int clampedSrc_X = xSource;
                    int clampedSrc_Y = ySource;

                    Color srcColor = source.GetPixel(clampedSrc_X, clampedSrc_Y);

                    if (wrapX || wrapY)
                    {
                        if (mode == PixelMode.NearestNeighbour)
                        {
                            float u = xTarget / to.width;
                            float v = yTarget / to.height;

                            clampedSrc_X = wrapX ? (int)Mathf.Clamp(u * from.width, from.x, from.width - 1) : clampedSrc_X;
                            clampedSrc_Y = wrapY ? (int)Mathf.Clamp(v * from.height, from.y, from.height - 1) : clampedSrc_Y;

                            srcColor = source.GetPixel(clampedSrc_X, clampedSrc_Y);
                        }
                        else if (mode == PixelMode.Repeat)
                        {
                            clampedSrc_X = wrapX ? (int)Mathf.Clamp(xSource, from.x, from.width - 1) : clampedSrc_X;
                            clampedSrc_Y = wrapY ? (int)Mathf.Clamp(ySource, from.y, from.height - 1) : clampedSrc_Y;

                            srcColor = source.GetPixel(clampedSrc_X, clampedSrc_Y);
                        }
                        else if (mode == PixelMode.RepeatComplete)
                        {
                            srcColor = source.GetPixelBilinear(xSource / (float)source.width, ySource / (float)source.height);
                        }
                        else if (mode == PixelMode.Bilinear)
                        {
                            float u = xTarget / (float)target.width;
                            float v = yTarget / (float)target.height;

                            srcColor = source.GetPixelBilinear(u, v);
                        }
                    }

                    //target.SetPixel(xTarget, yTarget, srcColor);
                    int index = x + (y * (int)to.width);
                    colorBuffer[index] = srcColor;
                }
            }

            target.SetPixels((int)to.x, (int)to.y, (int)to.width, (int)to.height, colorBuffer);
            target.Apply();
        }


        /// <summary>
        /// Returns a texture in the specified color and size.
        /// </summary>
        /// <param name="r">The red color component.</param>
        /// <param name="g">The green color component.</param>
        /// <param name="b">The blue color component.</param>
        /// <param name="a">The alpha component.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <returns></returns>
        public static Texture2D MakeTexture(float r, float g, float b, float a, int width, int height, bool linear = false)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, linear);

            Color color = new Color(r, g, b, a);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }

        /// <summary>
        /// Returns a 1x1 texture in the specified color.
        /// </summary>
        /// <param name="r">The color of the texture.</param>
        /// <returns>Returns a texture in the specified size and color..</returns>
        public static Texture2D MakeTexture(Color color, int width, int height, bool linear = false)
        {
            return MakeTexture(color.r, color.g, color.b, color.a, width, height, linear);
        }


        /// <summary>
        /// Returns a blank texture in the specified size.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <returns></returns>
        public static Texture2D MakeTexture(int width, int height, bool linear = false)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, linear);

            return texture;
        }
        /// <summary>
        /// Returns a blank color array (texture) in the specified size.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <returns></returns>
        public static Color[] MakeColors(int width, int height)
        {
            Color[] texture = new Color[width * height];

            return texture;
        }

    }

}


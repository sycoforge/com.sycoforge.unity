using System;
using UnityEngine;


namespace ch.sycoforge.Unity.Graphical
{
    public class Graphix
    {
        public static Texture2D aaLineTex = null;
        public static Texture2D lineTex = null;

        //public static void antiAliasCircle(Texture2D texture, int centerX, int centerY, int imgWidth, int[] srcData, int radius)
        //{
        //    // increase the  radius by 1 pixel and plot the pixel points on floor of radius
        //    //radius++;
        //    int x = 0;
        //    double yprev = radius;
        //    int y1 = radius;
        //    double ynew = radius;
        //    int ynewint = 0;
        //    int yprevint = 0;

        //    setPixelBinary(centerX + (centerY + radius) * imgWidth, srcData, 0);
        //    setPixelBinary(centerX + (centerY - (radius)) * imgWidth, srcData, 0);
        //    setPixelBinary(centerX + radius + centerY * imgWidth, srcData, 0);
        //    setPixelBinary(centerX - (radius) + centerY * imgWidth, srcData, 0);

        //    while (x < y1)
        //    {
        //        x++;
        //        ynew = Math.Sqrt(radius * radius - x * x);
        //        y1 = (int)Math.Ceiling(ynew);

        //        if (yprev - ipart(ynew) > 1)
        //        {
        //            ynewint = (int)Math.Ceiling(ynew);
        //            yprevint = (int)Math.Ceiling(yprev);

        //            setpixel8(centerX, centerY, x, ynewint, imgWidth, srcData, rfpart(ynew));
        //            setpixel8(centerX, centerY, x, yprevint, imgWidth, srcData, fpart(yprev));
        //        }
        //        else
        //        {
        //            setpixel8(centerX, centerY, x, y1, imgWidth, srcData, 0);
        //        }

        //        yprev = ynew;
        //    }
        //}


        private static int ipart(double x)
        {
            //is 'integer part of x'
            return (int)Math.Floor(x);
        }

        public static Color ColorFromHSV(float h, float s, float v, float a = 1)
        {
            // no saturation, we can return the value across the board (grayscale)
            if (s == 0)
                return new Color(v, v, v, a);

            // which chunk of the rainbow are we in?
            float sector = h / 60;

            // split across the decimal (ie 3.87 into 3 and 0.87)
            int i = (int)sector;
            float f = sector - i;

            float p = v * (1 - s);
            float q = v * (1 - s * f);
            float t = v * (1 - s * (1 - f));

            // build our rgb color
            Color color = new Color(0, 0, 0, a);

            switch (i)
            {
                case 0:
                    color.r = v;
                    color.g = t;
                    color.b = p;
                    break;

                case 1:
                    color.r = q;
                    color.g = v;
                    color.b = p;
                    break;

                case 2:
                    color.r = p;
                    color.g = v;
                    color.b = t;
                    break;

                case 3:
                    color.r = p;
                    color.g = q;
                    color.b = v;
                    break;

                case 4:
                    color.r = t;
                    color.g = p;
                    color.b = v;
                    break;

                default:
                    color.r = v;
                    color.g = p;
                    color.b = q;
                    break;
            }

            return color;
        }

        public static void ColorToHSV(Color color, out float h, out float s, out float v)
        {
            float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
            float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
            float delta = max - min;

            // value is our max color
            v = max;

            // saturation is percent of max
            if (!Mathf.Approximately(max, 0))
                s = delta / max;
            else
            {
                // all colors are zero, no saturation and hue is undefined
                s = 0;
                h = -1;
                return;
            }

            // grayscale image if min and max are the same
            if (Mathf.Approximately(min, max))
            {
                v = max;
                s = 0;
                h = -1;
                return;
            }

            // hue depends which color is max (this creates a rainbow effect)
            if (color.r == max)
                h = (color.g - color.b) / delta;            // between yellow & magenta
            else if (color.g == max)
                h = 2 + (color.b - color.r) / delta;                // between cyan & yellow
            else
                h = 4 + (color.r - color.g) / delta;                // between magenta & cyan

            // turn hue into 0-360 degrees
            h *= 60;
            if (h < 0)
                h += 360;
        }
        public static void DrawLine(Vector2 p0, Vector2 p1, Color color, float width, bool antiAlias)
        {
            Color savedColor = GUI.color;
            Matrix4x4 savedMatrix = GUI.matrix;

            if (!lineTex)
            {
                lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
                lineTex.SetPixel(0, 1, Color.white);
                lineTex.Apply();
            }
            if (!aaLineTex)
            {
                aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, true);
                aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
                aaLineTex.SetPixel(0, 1, Color.white);
                aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
                aaLineTex.Apply();
            }
            if (antiAlias) width *= 3;
            float angle = Vector3.Angle(p1 - p0, Vector2.right) * (p0.y <= p1.y ? 1 : -1);
            float m = (p1 - p0).magnitude;
            if (m > 0.01f)
            {
                Vector3 dz = new Vector3(p0.x, p0.y, 0);

                GUI.color = color;
                GUI.matrix = TransformationMatrix(dz) * GUI.matrix;
                GUIUtility.ScaleAroundPivot(new Vector2(m, width), new Vector3(-0.5f, 0, 0));
                GUI.matrix = TransformationMatrix(-dz) * GUI.matrix;
                GUIUtility.RotateAroundPivot(angle, Vector2.zero);
                GUI.matrix = TransformationMatrix(dz + new Vector3(width / 2, -m / 2) * Mathf.Sin(angle * Mathf.Deg2Rad)) * GUI.matrix;

                if (!antiAlias)
                    GUI.DrawTexture(new Rect(0, 0, 1, 1), lineTex);
                else
                    GUI.DrawTexture(new Rect(0, 0, 1, 1), aaLineTex);
            }
            GUI.matrix = savedMatrix;
            GUI.color = savedColor;
        }

        //Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent

        /// <summary>
        /// Draws the bezier line.
        /// </summary>
        /// <param name='p0'>
        /// Start point of curve.
        /// </param>
        /// <param name='p1'>
        /// Start points tangent vector.
        /// </param>
        /// <param name='p2'>
        /// End point of curve.
        /// </param>
        /// <param name='p3'>
        /// End points tangent vector.
        /// </param>
        /// <param name='color'>
        /// Curve Color.
        /// </param>
        /// <param name='width'>
        /// The curve width.
        /// </param>
        /// <param name='antiAlias'>
        /// Anti alias.
        /// </param>
        /// <param name='segments'>
        /// Amount of Segments.
        /// </param>
        public static void DrawBezierLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float width, bool antiAlias, int segments)
        {
            Vector2 lastV = CubeBezier(p0, p1, p2, p3, 0);
            for (int i = 1; i <= segments; ++i)
            {
                Vector2 v = CubeBezier(p0, p1, p2, p3, i / (float)segments);

                Graphix.DrawLine(lastV, v, color, width, antiAlias);
                lastV = v;
            }
        }

        public static void DrawBezierLine(Vector2 p0, Vector2 p1, Color color, Color shadow, float width)
        {

            DrawBezierLine(
                new Vector2(p0.x, p0.y),
                new Vector2(p0.x + Mathf.Abs(p1.x - (p0.x)) / 2, p0.y),
                new Vector2(p1.x, p1.y),
                new Vector2(p1.x - Mathf.Abs(p1.x - (p0.x)) / 2, p1.y), color, width, true, 40);
        }

        private static Vector2 CubeBezier(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
        {
            float rt = 1 - t;
            float rtt = rt * t;
            return rt * rt * rt * P0 + 3 * rt * rtt * P1 + 3 * rtt * t * P3 + t * t * t * P2;
        }

        private static Matrix4x4 TransformationMatrix(Vector3 v)
        {
            return Matrix4x4.TRS(v, Quaternion.identity, Vector3.one);
        }
    }
}
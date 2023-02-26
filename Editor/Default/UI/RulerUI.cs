using ch.sycoforge.Unity.Graphical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.UI
{
    public static class RulerUI
    {
        public static void DrawVerticalRuler(Rect rect, float pixelsPerUnit, Color color)
        {
            if (rect.width > 0 && rect.height > 0)
            {
                GraphixGL.BeginGroup(rect);
                Color c = Color.red;

                Vector2 unit = Vector2.up;

                DrawRuler(rect, color, unit, rect.height, pixelsPerUnit);

                GraphixGL.EndGroup();
            }
        }

        public static void DrawHorizontalRuler(Rect rect, float pixelsPerUnit, Color color)
        {
            if (rect.width > 0 && rect.height > 0)
            {
                GraphixGL.BeginGroup(rect);
                Color c = Color.red;

                Vector2 unit = Vector2.right;

                DrawRuler(rect, color, unit, rect.width, pixelsPerUnit);

                GraphixGL.EndGroup();
            }
        }

        private static void DrawRuler(Rect rect, Color color, Vector2 unit, float size, float pixelsPerUnit)
        {
            float d = Math.Min(rect.height, rect.width);

            float d1 = d;
            float d2 = d * 0.35f;
            float d3 = d * 0.15f;

            Vector2 offset = new Vector2(unit.y, unit.x);

            //for (float i = 1; i < (float)pixelsPerUnit * size + 10; i += 0.0625f)
            //for (float i = 0; i < (float)pixelsPerUnit * size + 10; i += 0.25f)
            //for (float i = 0; i < (float)size + 10; i += 0.1f)

            float w = Mathf.RoundToInt(size);
            float i = 0;
            while (i < (float)size+10)
            //for (float i = 0; i < (float)size + 10; i += 0.25f)
            //for (int i = 0; i < size + 10; i += 1)
            {
                Vector2 start = unit * i;
                Vector2 end = unit * i;
                

                if ((i % (pixelsPerUnit)) <= Mathf.Epsilon)
                {
                    GraphixGL.DrawLine(start, end + offset * d1, color, 2);
                    // Draw a Number at every inch mark
                    //g.DrawString(count.ToString(), TheFont, Brushes.Black, 25, i, new StringFormat));count++;
                }
                else if (((i * 2) % (pixelsPerUnit)) <= Mathf.Epsilon)
                {
                    GraphixGL.DrawLine(start, end + offset * d2, color, 1);
                }
                else if (((i * 8) % (pixelsPerUnit)) <= Mathf.Epsilon)
                {
                    GraphixGL.DrawLine(start, end + offset * d3, color, 1);
                }
                //else if (((i) % ((pixelsPerUnit/8))) == 0)
                //{
                //    GraphixGL.DrawLine(start, end + offset * 10, color, 1);
                //}
                //else if (((i) % ((pixelsPerUnit/16))) == 0)
                //{
                //    GraphixGL.DrawLine(start, end + offset * 5, color, 1);
                //}

                i += 0.125f;
            }
        }        
    }
}

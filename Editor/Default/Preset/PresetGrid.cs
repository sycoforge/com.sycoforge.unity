using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.UI;
//using ch.sycoforge.Unity.TextureUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    public class PresetGrid<P> : ItemGrid<PresetGridItem> where P : IGridItem, IWebPreset
    {
        //---------------------------------
        // Properties
        //---------------------------------


        //---------------------------------
        // Fields
        //---------------------------------
        private Texture2D background;
        private GUIStyle styleBox;

        //---------------------------------
        // Constructor
        //---------------------------------
        public PresetGrid(GUIStyle itemStyle, int itemsPerRow)
            : base(itemStyle, itemsPerRow)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0,0, new Color(0, 0, 0, 0.6f));
            texture.Apply();

            background = texture;
        }

        //---------------------------------
        // Methods
        //---------------------------------
        protected override void RenderItem(Rect rect, PresetGridItem preset)
        {
            preset.RenderItem(rect);
            //int labelHeight = 20;
            //int thumbMargin = 5;

            //CheckStyle(rect);

            //Rect labelTitleRect = rect;
            //labelTitleRect.yMin += rect.height - labelHeight;
            //labelTitleRect.height = labelHeight;

            //Rect labelStatusRect = rect;
            //labelStatusRect.yMin += 10;
            //labelStatusRect.height = labelHeight;

            //Rect labelRatingRect = rect;
            //labelRatingRect.yMin += rect.height - labelHeight * 2;
            //labelRatingRect.height = labelHeight;

            //Rect boxRect = rect;
            //boxRect.yMin += rect.height * (2 / 3f);
            //boxRect.height = rect.height / 3;

            //Rect thumbRect = rect;
            //thumbRect.yMin += thumbMargin;
            //thumbRect.yMax -= thumbMargin;
            //thumbRect.xMin += thumbMargin;
            //thumbRect.xMax -= thumbMargin;




            //if (preset.IsDownloading)
            //{
            //    BusyUI.Draw(rect);
            //    GUI.Label(labelStatusRect, "Loading...", Shared.SharedStyles.DefaultLabel);
            //}
            //else
            //{    
            //    DrawThumbnail(thumbRect, preset);


            //    //DrawRibbon(rect, preset);                
            //}

            //GUI.Box(boxRect, GUIContent.none, styleBox);


            //GUI.Label(labelTitleRect, preset.Title, Shared.SharedStyles.DefaultLabel);
            //GUI.Label(labelRatingRect, GetRatingString(preset.Rating), Shared.SharedStyles.DefaultRatingLabel);
        }

        //private void DrawRibbon(Rect parent, P preset)
        //{
        //    Texture ribbon = null;

        //    if (IsHot(preset))
        //    {
        //        ribbon = SharedGraphics.RibbonHot;
        //    }
        //    else if (IsNew(preset))
        //    {
        //        ribbon = SharedGraphics.RibbonNew;
        //    }

        //    if (ribbon != null)
        //    {
        //        Rect rectRibbon = parent;
        //        rectRibbon.width = parent.width / 4f;
        //        rectRibbon.height = parent.height / 4f;

        //        GUI.DrawTexture(rectRibbon, ribbon);
        //    }
        //}

        //private bool IsNew(P item)
        //{
        //    bool isNew = (DateTime.Now - item.Created) < TimeSpan.FromHours(48);

        //    return isNew;
        //}

        //private bool IsHot(P item)
        //{
        //    bool isHot = item.Downloads >= 100;

        //    return isHot;
        //}

        //private void CheckStyle(Rect rect)
        //{
        //    if (styleBox == null)
        //    {
        //        styleBox = new GUIStyle();
        //        styleBox.normal.background = background;
        //        //style.fixedWidth = rect.width;
        //        //style.fixedHeight = rect.height / 3;
        //    }
        //}

        //public static string GetRatingString(int rating)
        //{
        //    string ratingStarString = string.Empty;

        //    char black = '\u2605';
        //    char white = '\u2606';

        //    string b = "<color=#ffd800>" + black + "</color>";
        //    string w = "<color=white>" + black + "</color>";


        //    for (int i = 1; i < 6; i++)
        //    {
        //        string c = i <= rating ? b : w;

        //        ratingStarString += c.ToString();
        //    }

        //    return ratingStarString;
        //}


        //public static int DrawRatingUI(int rating)
        //{
        //    int r = -1;

        //    char black = '\u2605';
        //    char white = '\u2606';

        //    string b = "<color=#ffd800>" + black + "</color>";
        //    string w = "<color=white>" + black + "</color>";

        //    Rect rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(80));

        //    for (int i = 1; i < 6; i++)
        //    {
        //        string c = i <= rating ? b : w;

        //        if(GUILayout.Button(c, SharedStyles.RatingButton))
        //        {
        //            r = i;
        //        }
        //    }

        //    EditorGUILayout.EndHorizontal();

        //    EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

        //    return r;
        //}
    }
}

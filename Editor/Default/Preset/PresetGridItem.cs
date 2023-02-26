using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.Anim;
using ch.sycoforge.Unity.Editor.UI;
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    public class PresetGridItem : IGridItem //where P : IGridItem, IWebPreset
    {
        //---------------------------------
        // Event
        //---------------------------------
        public event Action OnNeedsRedraw;

        //---------------------------------
        // Properties
        //---------------------------------
        public Texture2D Thumbnail
        {
            get { return preset.Thumbnail; }
            set { preset.Thumbnail = value; }
        }

        public string DsiplayName
        {
            get { return preset.DsiplayName; }
            set { preset.DsiplayName = value; }
        }

        public BasePreset Preset
        {
            get { return preset; }
            set { preset = value; }
        }

        //---------------------------------
        // Fields
        //---------------------------------
        private Texture2D background;
        private GUIStyle itemStyle;
        private IconLayoutMode layoutMode;
        private IntThickness iconMargin;

        private FloatAnimation descAnimation;

        private GUIStyle styleDescBox;
        private BasePreset preset;

        //---------------------------------
        // Constructor
        //---------------------------------
        public PresetGridItem(GUIStyle itemStyle, IconLayoutMode layoutMode, IntThickness iconMargin, BasePreset preset)
        {
            this.itemStyle = itemStyle;
            this.layoutMode = layoutMode;
            this.iconMargin = iconMargin;
            this.preset = preset;

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0,0, new Color(0, 0, 0, 0.9f));
            texture.Apply();

            background = texture;

            descAnimation = new FloatAnimation(0);
            descAnimation.speed = 3f;
            descAnimation.valueChanged.AddListener(ValueChanged);
            //descAnimation.valueChanged.AddListener(Repaint);
        }

        //---------------------------------
        // Methods
        //---------------------------------
        public void RenderItem(Rect rect)
        {
            Event evt = Event.current;

            //if (evt.isMouse)
            //{
                if (rect.Contains(evt.mousePosition))
                {
                    descAnimation.Start(1);
                }
                else
                {
                    descAnimation.Start(0);
                }
            //}

            int labelHeight = 15;
            int thumbMargin = 5;

            CheckStyle(rect);

            Rect labelStatusRect = rect;
            labelStatusRect.yMin += 10;
            labelStatusRect.height = labelHeight;

            Rect thumbRect = rect;
            thumbRect.yMin += thumbMargin;
            thumbRect.yMax -= thumbMargin;
            thumbRect.xMin += thumbMargin;
            thumbRect.xMax -= thumbMargin;

            Rect descBoxRect = thumbRect;
            descBoxRect.yMin = thumbRect.yMax - thumbRect.height * (2 / 3f) * descAnimation.value;
            //descBoxRect.yMax += (rect.height / 3);
            //descBoxRect.height = (rect.height / 3);

            Rect labelRatingRect = descBoxRect;
            labelRatingRect.xMin += 2;
            labelRatingRect.yMin += labelHeight * 2;
            labelRatingRect.height = labelHeight;

            Rect labelTitleRect = descBoxRect;
            labelTitleRect.xMin += 2;
            labelTitleRect.yMin += labelHeight;
            labelTitleRect.height = labelHeight;


            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);


            

            if (preset.IsDownloading)
            {
                BusyUI.Draw(rect);
                GUI.Label(labelStatusRect, "Loading...", Shared.SharedStyles.DefaultLabel);
            }
            else
            {    
                //ItemGrid<BasePreset>.DrawThumbnail(thumbRect, this);
                ItemGrid<BasePreset>.DrawThumbnail(thumbRect, this, iconMargin, layoutMode);


                DrawRibbon(rect, preset);                
            }

            
            GUI.Box(descBoxRect, GUIContent.none, styleDescBox);

            GUI.BeginGroup(thumbRect);
            GUI.Label(labelTitleRect, preset.DsiplayName, Shared.SharedStyles.DefaultLabel);

            if (preset.IsWebPreset)
            {
                GUI.Label(labelRatingRect, GetRatingString(preset.Rating), Shared.SharedStyles.DefaultRatingLabel);
            }
            GUI.EndClip();
        }

        private void DrawRibbon(Rect parent, BasePreset preset)
        {
            Texture ribbon = null;

            if (IsHot(preset))
            {
                ribbon = SharedGraphics.RibbonHot;
            }
            else if (IsNew(preset))
            {
                ribbon = SharedGraphics.RibbonNew;
            }

            if (ribbon != null)
            {
                Rect rectRibbon = parent;
                rectRibbon.width = parent.width / 4f;
                rectRibbon.height = parent.height / 4f;

                GUI.DrawTexture(rectRibbon, ribbon);
            }
        }

        private bool IsNew(BasePreset item)
        {
            bool isNew = (DateTime.Now - item.Created) < TimeSpan.FromHours(48);

            return isNew;
        }

        private bool IsHot(BasePreset item)
        {
            bool isHot = item.Downloads >= 100;

            return isHot;
        }

        private void CheckStyle(Rect rect)
        {
            if (styleDescBox == null)
            {
                styleDescBox = new GUIStyle();
                styleDescBox.normal.background = background;
                //style.fixedWidth = rect.width;
                //style.fixedHeight = rect.height / 3;
            }
        }

        private void ValueChanged()
        {
            if(OnNeedsRedraw != null)
            {
                OnNeedsRedraw();
            }
        }

        //---------------------------------
        // Static Methods
        //---------------------------------

        public static string GetRatingString(int rating)
        {
            string ratingStarString = string.Empty;

            char black = '\u2605';
            //char white = '\u2606';

            string b = "<color=#ffd800>" + black + "</color>";
            string w = "<color=white>" + black + "</color>";


            for (int i = 1; i < 6; i++)
            {
                string c = i <= rating ? b : w;

                ratingStarString += c.ToString();
            }

            return ratingStarString;
        }


        public static int DrawRatingUI(int rating)
        {
            int r = -1;

            char black = '\u2605';
            //char white = '\u2606';

            string b = "<color=#ffd800>" + black + "</color>";
            string w = "<color=white>" + black + "</color>";

            Rect rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(80));

            for (int i = 1; i < 6; i++)
            {
                string c = i <= rating ? b : w;

                if(GUILayout.Button(c, SharedStyles.RatingButton))
                {
                    r = i;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            return r;
        }
    }
}

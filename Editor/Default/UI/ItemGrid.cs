using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.Workspace;
using ch.sycoforge.Unity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.UI
{
    public enum SizeMode
    {
        None,
        Square,
    }

    public enum ItemRenderMode
    {
        Icon,
        Text,
        IconText
    }

    public enum IconLayoutMode
    {
        Center,
        Left,
        Right
    }

    public class ItemGrid<P> where P : IGridItem
    {

        public delegate void ItemClicked(P item, int index);

        //--------------------------
        // Events
        //--------------------------
        public event ItemClicked OnItemClicked;

        //--------------------------
        // Properties
        //--------------------------

        public IList<P> Collection
        {
            get { return collection; }
            set 
            { 
                collection = value;

                if (collection != null)
                {
                    rects = new Rect[collection.Count()];
                }
            }
        }

        public int ItemsPerRow
        {
            get { return itemsPerRow; }
            set { itemsPerRow = value; }
        }

        public GUIStyle ItemStyle
        {
            get { return itemStyle; }
            set { itemStyle = value; }
        }

        public GUIStyle ContainerStyle
        {
            get { return containerStyle; }
            set { containerStyle = value; }
        }

        public SizeMode ItemSizeMode
        {
            get { return itemSizeMode; }
            set { itemSizeMode = value; }
        }

        public Vector2 MinItemSize
        {
            get { return minItemSize; }
            set { minItemSize = value; }
        }

        public Vector2 MaxItemSize
        {
            get { return maxItemSize; }
            set { maxItemSize = value; }
        }

        public Vector2 GridSize
        {
            get { return gridSize; }
            set { gridSize = value; }
        }

        //public Action<P, Rect> CustomItemRenderer
        //{
        //    get;
        //    set;
        //}

        public ItemRenderMode RenderMode
        {
            get;
            set;
        }

        public IconLayoutMode LayoutMode
        {
            get;
            set;
        }

        public IntThickness IconMargin
        {
            get;
            set;
        }

        public bool HideScrollbar
        {
            get;
            set;
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value != selectedIndex)
                {
                    selectedIndex = value;

                    P v = default(P);

                    if (selectedIndex < collection.Count && selectedIndex >= 0)
                    {
                        v = collection[selectedIndex];
                    }

                    ThrowOnControlClicked(v, value);
                }
            }
        }

        public P SelectedValue
        {
            get 
            {
                P v = default(P);

                if (selectedIndex < collection.Count && selectedIndex >= 0)
                {
                    v = collection[selectedIndex];
                }

                return v; 
            }
        }

        private int Rows
        {
            get
            {
                int rows = (int)Math.Ceiling((double)collection.Count / itemsPerRow);
                return rows;
            }
        }

        //public GUIStyle ItemStyleActive
        //{
        //    get { return itemStyleActive; }
        //    set { itemStyleActive = value; }
        //}

        //--------------------------
        // Fields
        //--------------------------
        private IList<P> collection;
        private int selectedIndex = -1;

        private Rect[] rects;
        private GUIStyle itemStyle;
        private GUIStyle containerStyle = GUIStyle.none;
        private SizeMode itemSizeMode = SizeMode.None;


        private const int cropWidth = 120;
        private int itemsPerRow = 4;
        private Vector2 scrollPos;
        private Vector2 minItemSize = Vector2.zero;
        private Vector2 maxItemSize = Vector2.one * 2000;
        private Vector2 gridSize = new Vector2(200, 200);

        private Rect lastContentRect;


        //--------------------------
        // Constructor
        //--------------------------
        public ItemGrid(IList<P> collection, GUIStyle itemStyle, int itemsPerRow)
        {
            this.Collection = collection;
            this.itemStyle = itemStyle;
            this.itemsPerRow = itemsPerRow;
        }

        public ItemGrid(GUIStyle itemStyle, int itemsPerRow) : this(new List<P>(), itemStyle, itemsPerRow)
        {

        }

        //--------------------------
        // Methods
        //--------------------------

        public void Draw(params GUILayoutOption[] options)
        {
            if (collection == null) { SelectedIndex = -1; return; }


            int count = (int)collection.Count();


            //Rect rect = GUILayoutUtility.GetRect(GridSize.x, GridSize.y, containerStyle, options);
            Rect rect = EditorGUILayout.BeginVertical(containerStyle, options);
            rect.width -= 16;//Scrollbar width

            Vector2 itemSize = GetItemDimension(rect, itemStyle);
            Rect contentRect = new Rect(rect.x, rect.y, rect.width, Rows * itemSize.y);


            if (!HideScrollbar)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);//);
            }

            SelectedIndex = DoButtonGrid(rect, SelectedIndex, itemSize.x, itemSize.y, itemStyle);


            GUILayout.Box(GUIContent.none, GUIStyle.none, GUILayout.Height(lastContentRect.height), GUILayout.MaxWidth(lastContentRect.width));
            //GUILayout.Box(GUIContent.none, GUIStyle.none, GUILayout.Height(lastContentRect.height), GUILayout.MaxWidth(lastContentRect.width - 40));
            //GUILayout.Box(GUIContent.none, new GUIStyle("Box"), GUILayout.Height(lastContentRect.height), GUILayout.MaxWidth(lastContentRect.width - 40));

            if (!HideScrollbar)
            {
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();

            if (contentRect.height > 0)
            {
                lastContentRect = contentRect;
            }
        }

        private Vector2 GetItemDimension(Rect gridSize, GUIStyle itemStyle)
        {
            int rows = (int)Math.Ceiling((double)collection.Count / itemsPerRow);

            float hSpacing = (itemsPerRow * itemStyle.margin.horizontal);
            float vSpacing = (float)(Mathf.Max(itemStyle.margin.top, itemStyle.margin.bottom) * (rows - 1));



            float width = (gridSize.width - hSpacing) / itemsPerRow;
            float height = 64;

            //Debug.Log("width: " + width + " rows: " + rows + " collection.Count: " + collection.Count + " itemsPerRow: " + itemsPerRow);
            


            if (itemSizeMode == SizeMode.Square)
            {
                height = width;
            }
            else
            {
                if (itemStyle.fixedHeight != 0f)
                {
                    height = itemStyle.fixedHeight;
                }
                else
                {
                    height = (gridSize.height - vSpacing) / (float)rows;
                }
            }

            width = Mathf.Clamp(width, minItemSize.x, maxItemSize.x <= 0 ? float.MaxValue : maxItemSize.x);
            height = Mathf.Clamp(height, minItemSize.y, maxItemSize.y <= 0 ? float.MaxValue : maxItemSize.y);


            return new Vector2(width, height);
        }

        private void ThrowOnControlClicked(P item, int index)
        {
            if(OnItemClicked != null)
            {
                OnItemClicked(item, index);
            }
        }

        //Rect[] rectArray;

        private int DoButtonGrid(Rect position, int selected, float itemWidth, float itemHeight, GUIStyle style)
        {
            position = new Rect(0f, 0f, position.width, position.height);

            int buttonGridHash = "ButtonGrid".GetHashCode();
            Rect[] rectArray;

            bool isHover;
            bool isActive;
            bool flag2;


            //Rect offset = position;

            //Caller.StaticCall(typeof(GUIUtility), "CheckOnGUI", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);

            int length = (int)collection.Count();
            if (length == 0)
            {
                return selected;
            }
            if (itemsPerRow <= 0)
            {
                Debug.LogWarning("You are trying to create a SelectionGrid with zero or less elements to be displayed in the horizontal direction. Set xCount to a positive value.");
                return selected;
            }
            int controlID = GUIUtility.GetControlID(buttonGridHash, FocusType.Passive, position);



            switch (Event.current.GetTypeForControl(controlID))
            {
                #region --- Mouse Down ---
                case EventType.MouseDown:
                {
                    if (position.Contains(Event.current.mousePosition))
                    {
                        //Debug.Log("MouseDown Contains: " + position + " mp: " + Event.current.mousePosition);
                        rectArray = CalcMouseRects(position, length, itemsPerRow, itemWidth, itemHeight, style, false);

                        if (GetButtonGridMouseSelection(rectArray, Event.current.mousePosition, true) != -1)
                        {
                            GUIUtility.hotControl = controlID;
                            Event.current.Use();
                        }
                    }
                    return selected;
                }
                #endregion

                #region --- Mouse Up ---

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        Vector2 mousePos = Event.current.mousePosition;

                        GUIUtility.hotControl = 0;
                        Event.current.Use();


                        rectArray = CalcMouseRects(position, length, itemsPerRow, itemWidth, itemHeight, style, false);

                        int buttonSelection = GetButtonGridMouseSelection(rectArray, mousePos, true);

                        GUI.changed = true;

                        return buttonSelection;
                    }
                    return selected;
                }

                #endregion

                case EventType.MouseMove:
                case EventType.KeyDown:
                case EventType.KeyUp:
                case EventType.ScrollWheel:
                {
                    return selected;
                }

                case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        Event.current.Use();
                    }
                    return selected;
                }
                //case EventType.Layout:
                //{
                //        //rectArray = CalcMouseRects(position, length, itemsPerRow, itemWidth, itemHeight, style, false);

                //        for (int i = 0; i < length; i++)
                //        {
                //            Rect rect = rectArray[i];
                //            //Debug.Log("r: " + rect);

                //            GUI.Box(rect, GUIContent.none, GUIStyle.none);
                //        }

                //        return selected;
                        
     
                    //GUI.Box(position, GUIContent.none, GUIStyle.none);
                    //return selected;  
              
                //}


                case EventType.Repaint:
                    {



                        rectArray = CalcMouseRects(position, length, itemsPerRow, itemWidth, itemHeight, style, false);


                        int buttonGridMouseSelection1 = GetButtonGridMouseSelection(rectArray, Event.current.mousePosition, controlID == GUIUtility.hotControl);
                        bool mouseInside = position.Contains(Event.current.mousePosition);

                        Caller.SetStaticProperty(typeof(GUIUtility), "mouseUsed", mouseInside, BindingFlags.Static | BindingFlags.NonPublic);

                        for (int i = 0; i < length; i++)
                        {
                            bool isSelected = i == selected;

                            Rect rect = rectArray[i];

                            //Debug.Log("r: " + rect+ " style: " + style.normal.background);

                            if (i != buttonGridMouseSelection1 || !GUI.enabled && controlID != GUIUtility.hotControl)
                            {
                                isHover = false;
                            }
                            else
                            {
                                isHover = (controlID == GUIUtility.hotControl ? true : GUIUtility.hotControl == 0);
                            }

                            //GUI.Box(rect, GUIContent.none, GUIStyle.none);
                            isActive = (controlID != GUIUtility.hotControl ? false : GUI.enabled);
                            style.Draw(rect, GUIContent.none, isHover, isActive, false, false);


                            RenderItem(rect, collection[i]);


                            rects[i] = rect;
                        }

                        if (selected < length && selected > -1)
                        {
                            Rect rect = rectArray[selected];

 

                            if (selected != buttonGridMouseSelection1 || !GUI.enabled && controlID != GUIUtility.hotControl)
                            {
                                flag2 = false;
                            }
                            else
                            {
                                flag2 = (controlID == GUIUtility.hotControl ? true : GUIUtility.hotControl == 0);
                            }


                            style.Draw(rect, GUIContent.none, flag2, controlID == GUIUtility.hotControl, true, false);



                            RenderItem(rect, collection[selected]);


                            rects[selected] = rect;

                        }
                        if (buttonGridMouseSelection1 >= 0)
                        {
                            //GUI.tooltip = collection.Regions[buttonGridMouseSelection1].Name;
                        }

                        return selected;
                    }
                default:
                    {
                        return selected;
                    }
            }
        }

        protected virtual void RenderItem(Rect rect, P item)
        {
            if (RenderMode == ItemRenderMode.Icon)
            {
                DrawThumbnail(rect, item, IconMargin, LayoutMode);
            }
            else if (RenderMode == ItemRenderMode.Text)
            {
                GUI.Label(rect, item.DsiplayName, Shared.SharedStyles.DefaultLabel);
            }
            else if (RenderMode == ItemRenderMode.IconText)
            {
                DrawThumbnail(rect, item, IconMargin, LayoutMode);

                Rect labelRect = rect;
                labelRect.xMin += rect.height;

                GUI.Label(labelRect, item.DsiplayName, Shared.SharedStyles.DefaultLabel);
            }
        }

        protected void DrawThumbnail(Rect position, IGridItem item)
        {
            DrawThumbnail(position, item, IconMargin, LayoutMode);
        }

        public static void DrawThumbnail(Rect position, IGridItem item, Rect uv, IntThickness iconMargin, IconLayoutMode layoutMode = IconLayoutMode.Center)
        {
            float imageAspect = uv.width / uv.height;

            Texture text = item.Thumbnail;

            if (text != null)
            {
                FloatRect screenRect = new FloatRect();
                FloatRect rect1 = new FloatRect();
                CanvasBase.CalculateScaledTextureRects(position, ScaleMode.ScaleToFit, imageAspect, ref screenRect, ref rect1);

                if (layoutMode == IconLayoutMode.Left)
                {
                    screenRect.position = new Float2(0, screenRect.position.y);
                }

                screenRect.xMin += iconMargin.left;
                screenRect.xMax -= iconMargin.right;
                screenRect.yMin += iconMargin.top;
                screenRect.yMax -= iconMargin.bottom;

                GUI.DrawTextureWithTexCoords(screenRect, text, uv, true);
            }
        }

        public static void DrawThumbnail(Rect position, IGridItem item, IntThickness iconMargin, IconLayoutMode layoutMode = IconLayoutMode.Center)
        {
            Rect uv = new Rect(0, 0, 1, 1);

            DrawThumbnail(position, item, uv, iconMargin, layoutMode);
        }

        private GUIContent Cropped(GUIContent original, Rect rect)
        {
            bool displayText = rect.width > cropWidth;
            GUIContent gUIContent = displayText ? original : new GUIContent(original.image);

            return gUIContent;
        }

        private static int GetButtonGridMouseSelection(Rect[] buttonRects, Vector2 mousePos, bool findNearest)
        {
            for (int i = 0; i < (int)buttonRects.Length; i++)
            {
                if (buttonRects[i].Contains(mousePos))
                {
                    return i;
                }
            }
            if (!findNearest)
            {
                return -1;
            }

            float single = 1E+07f;
            int num = -1;

            for (int j = 0; j < (int)buttonRects.Length; j++)
            {
                Rect rect = buttonRects[j];
                Vector2 vector2 = new Vector2(Mathf.Clamp(mousePos.x, rect.xMin, rect.xMax), Mathf.Clamp(mousePos.y, rect.yMin, rect.yMax));
                float single1 = (mousePos - vector2).sqrMagnitude;
                if (single1 < single)
                {
                    num = j;
                    single = single1;
                }
            }
            return num;
        }

        private static Rect[] CalcControlRects(Rect[] tabRects, float tabWidth, float tabHeight, float ctrlWidth, float ctrlMargintRight)
        {
            Rect[] rects = new Rect[tabRects.Length];

            for(int i = 0; i < tabRects.Length; i++)
            {
                Rect r = tabRects[i];

                Rect soloRect = new Rect(r);
                soloRect.xMin += tabWidth - ctrlMargintRight - ctrlWidth;
                soloRect.width = ctrlWidth;

                rects[i] = soloRect;
            }

            return rects;
        }

        private static Rect[] CalcMouseRects(Rect position, int count, int xCount, float elemWidth, float elemHeight, GUIStyle style, bool addBorders)
        {
            int num = 0;
            int num1 = 0;
            float left = position.xMin;
            float top = position.yMin;

            Rect[] rects = new Rect[count];

            for (int i = 0; i < count; i++)
            {
                if (addBorders)
                {
                    rects[i] = style.margin.Add(new Rect(left, top, elemWidth, elemHeight));
                }
                else
                {
                    rects[i] = new Rect(left, top, elemWidth, elemHeight);
                }
                rects[i].width = Mathf.Round(rects[i].xMax) - Mathf.Round(rects[i].x);
                rects[i].x = Mathf.Round(rects[i].x);


                left = left + (elemWidth + (float)Mathf.Max(style.margin.right, style.margin.left));
                num1++;
                if (num1 >= xCount)
                {
                    num++;
                    num1 = 0;
                    top = top + (elemHeight + (float)Mathf.Max(style.margin.top, style.margin.bottom));
                    left = position.xMin;
                }
            }
            return rects;
        }

        private static int CalcTotalHorizSpacing(int xCount, GUIStyle style, GUIStyle firstStyle, GUIStyle midStyle, GUIStyle lastStyle)
        {
            if (xCount < 2)
            {
                return 0;
            }
            if (xCount == 2)
            {
                return Mathf.Max(firstStyle.margin.right, lastStyle.margin.left);
            }
            int num = Mathf.Max(midStyle.margin.left, midStyle.margin.right);
            return Mathf.Max(firstStyle.margin.right, midStyle.margin.left) + Mathf.Max(midStyle.margin.right, lastStyle.margin.left) + num * (xCount - 3);
        }
    }
}




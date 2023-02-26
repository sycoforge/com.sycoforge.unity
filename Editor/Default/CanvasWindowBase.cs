using ch.sycoforge.Device;

using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor;
using ch.sycoforge.Unity.Editor.Workspace;

using System;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public enum DefaultEditTool
    {
        Default = 0,
        SingleSelection = 1,
        PanZoom = 2,
        AdditiveSelection,
    }

    public enum SelectionResizeMode
    {
        None = -1,

        ResizeSelectionTop = 0,
        ResizeSelectionBottom = 1,
        ResizeSelectionLeft = 2,
        ResizeSelectionRight = 3,
        MoveSelection = 4,
    }

    public abstract class CanvasWindowBase<T, C> : EditorWindowExtension
        where T : CanvasWindowBase<T, C>
        where C : CanvasBase
    {
        //------------------------------
        // Events
        //------------------------------
        public event Action<DefaultEditTool> OnToolChanged;
        public event Action<IEditorWindow> OnWindowSizeChanged;

        //------------------------------
        // Properties
        //------------------------------
        public Texture2D PreviewTexture
        {
            get { return previewTexture; }
            set { previewTexture = value; }
        }

        protected bool ShowMenuBar
        {
            get;
            set;
        }

        protected bool ShowToolbar
        {
            get;
            set;
        }

        protected bool ToolbarEnabled
        {
            get { return toolbarEnabled; }
            set { toolbarEnabled = value; }
        }

        protected bool ShowSettingsArea
        {
            get;
            set;
        }  

        protected bool ShowStatusbar
        {
            get;
            set;
        }

        protected bool EnablePreviewPan
        {
            get;
            set;
        }

        protected bool EnableSnapPan
        {
            get;
            set;
        }

        protected bool EnablePreviewZoom
        {
            get;
            set;
        }


        protected bool ShowMapSeams
        {
            get;
            set;
        }

        /// <summary>
        /// When enabled the pan operation gets clamped to to the displayed image.
        /// </summary>
        protected bool ClampPan
        {
            get;
            set;
        }

        protected bool RenderAlpha
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether an active selection can be moved and/or scaled.
        /// </summary>
        public bool AllowSelectionChange
        {
            get;
            set;
        }
        
        private int activeToolIndex;
        //{
        //    get { return activeToolIndex; }
        //    private set
        //    {
        //        int transform = value;

        //        if (transform != activeToolIndex)
        //        {
        //            activeToolIndex = value;
        //            if (Enum.IsDefined(typeof(DefaultEditTool), activeToolIndex))
        //            {
        //                ActiveTool = (DefaultEditTool)activeToolIndex;
        //            }
        //        }
        //    }
        //}

        protected DefaultEditTool ActiveTool
        {
            get { return activeTool; }
            set
            {
                DefaultEditTool t = value;

                if(t != activeTool)
                {
                    activeTool = value;
                    activeToolIndex = (int)value;
                    //ThrowToolChanged();
                }
            }
        }

        protected SelectionResizeMode SelectionResize
        {
            get { return selectionResize; }
            set { selectionResize = value; }
        }

        protected bool FixedAspectRatio
        {
            get { return fixedAspectRatio; }
            set
            {
                bool last = value;

                if (last != fixedAspectRatio)
                {
                    fixedAspectRatio = value;
                    CorrectAspect();
                }
            }
        }

        //-----------------------------
        // Static Fields
        //-----------------------------
        protected static T WindowInstance;

        //------------------------------
        // Private Fields
        //------------------------------  
        private DefaultEditTool activeTool = DefaultEditTool.Default;
        private SelectionResizeMode selectionResize = SelectionResizeMode.None;
        //private int activeToolIndex = 0;

        [SerializeField]
        private Texture2D previewTexture;

        [SerializeField]
        private bool toolbarEnabled = true;

        //protected int tileX = 1, tileY = 1;
        protected GUIStyle styleContainer;
        //protected Material canvasMaterial;
        //protected RectOffset previewMargin = new RectOffset(5, 5, 5, 5);

        ///// <summary>
        ///// Normalized preview brickSize.
        ///// </summary>
        protected Float2 previewOffsetNormalized = Float2.zero;
        protected Float2 previewOffset = Float2.zero;
        protected float previewZoom = 1.0f;
        protected float previewZoomSpeed = 0.02f;
        protected bool showSeams = false;


        
        /// <summary>
        /// Locks preview dragging and zooming.
        /// </summary>
        [SerializeField]        
        protected bool locked;

        /// <summary>
        /// Draw canvas with fixed aspect ratio.
        /// </summary>
        [SerializeField]        
        protected bool fixedAspectRatio;

        protected C canvas;


        protected Float2 SelectionStartPoint;
        protected int sideToolbarWidth = 50;
        protected int settingsAreaWidth = 360;
        protected int menubarHeight = 16;

        [SerializeField]
        protected GUIContent[] toolIcons;


        private Float2 accuDeltaDragNormalized;
        //private Texture2D selectionTetxure;

        [SerializeField]
        private Material selectionMaterial;

        [SerializeField]
        private GUIStyle settingsAreaStyle;

        private Vector2 lastWindowSize;

        //------------------------------
        // Constructor
        //------------------------------

        //------------------------------
        // Methods
        //------------------------------

        public override void OnDisable()
        {
            base.OnDisable();

            WindowInstance = null;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            canvas.OnMouseDragged += canvas_OnMouseDragged;
            canvas.OnMouseWheel += canvas_OnMouseWheel;
            canvas.OnSizeChanged += canvas_OnSizeChanged;
            canvas.OnMouseDown += canvas_OnMouseDown;
            canvas.OnMouseUp += canvas_OnMouseUp;
            canvas.OnMouseMove += canvas_OnMouseMove;
            canvas.OnKeyPressed += canvas_OnKeyPressed;
            canvas.Selection.OnSelectionChanged += Selection_OnSelectionChanged;

            OnWindowSizeChanged += CanvasWindowBase_OnWindowSizeChanged;
            //OnToolChanged += MapWindowBase_OnToolChanged;

            styleContainer = new GUIStyle();

            this.selectionMaterial = new Material(Shader.Find("Hidden/nu Assets/UI/CanvasSelection"));

            this.lastWindowSize = position.size;
            
            wantsMouseMove = true;

            CorrectAspect();
        }

        void CanvasWindowBase_OnWindowSizeChanged(IEditorWindow obj)
        {
            CorrectAspect();
        }


        void Selection_OnSelectionChanged()
        {
            //UpdateSelectionTexture(false);
        }

        void canvas_OnKeyPressed(NativeKeyCode key, NativeEventModifiers modifier)
        {
            if (key == NativeKeyCode.D && modifier == NativeEventModifiers.Control)
            {
                canvas.Selection.Clear();
            }
            if (key == NativeKeyCode.A && modifier == NativeEventModifiers.Control)
            {
                canvas.Selection.Clear();
                canvas.Selection.Add(new FloatRect(0, 0, 1, 1));
            }
            else if (key == NativeKeyCode.M)
            {
                ActiveTool = DefaultEditTool.SingleSelection;
            }
            else if (key == NativeKeyCode.V)
            {
                ActiveTool = DefaultEditTool.Default;
            }
            else if (key == NativeKeyCode.H)
            {
                ActiveTool = DefaultEditTool.PanZoom;
            }
        }

        public virtual void OnGUI()
        //public void OnGUI()
        {


            if (ShowMenuBar)
            {
                DrawMenubar();
            }

            //canvasMaterial.SetTextureScale("_MainTex", new Float2(tileX, tileY));

            Event evt = Event.current;

            canvas.Update(evt);

            if (EnablePreviewPan && canvas.IsDragging && ActiveTool == DefaultEditTool.PanZoom)
            {
                EditorGUIUtility.AddCursorRect(canvas.CanvasRect, MouseCursor.Pan);

                //Debug.Log("PAN @ " + canvas.AbsoluteCanvasRect);
            }

            #region ---- Selection Change ----
            if (activeTool == DefaultEditTool.Default && canvas.Selection.HasSelection && AllowSelectionChange)
            {
                float border = 5;
                float borderNorm = border / canvas.Width;

                foreach (FloatRect sel in canvas.Selection)
                {
                    //FloatRect screenRect = InverseTransformRectNorm(sel, true);
                    FloatRect rect = canvas.ScaleOffsetRect(sel, previewZoom, previewOffsetNormalized);
                    FloatRect rectBorder = rect;
                    rectBorder.xMin += borderNorm;
                    rectBorder.yMin += borderNorm;
                    rectBorder.xMax -= borderNorm;
                    rectBorder.yMax -= borderNorm;

                    bool inBorderRect = rectBorder.Contains(canvas.MouseCanvasPositionNormalized);
                    bool inSelectionRect = rect.Contains(canvas.MouseCanvasPositionNormalized);

                        FloatRect rectNormUI = canvas.InverseTransformRectNorm(sel, previewZoom, previewOffsetNormalized);
                        FloatRect rectUI = canvas.DenormalizeRect(rectNormUI);
                        rectUI.position += canvas.CanvasPosition;

                    if (!inBorderRect && inSelectionRect)
                    {


                        FloatRect rectInnerUI = rectUI;
                        rectInnerUI.xMax -= border;
                        rectInnerUI.yMax -= border;
                        rectInnerUI.xMin += border;
                        rectInnerUI.yMin += border;

                        FloatRect borderTop = rectUI;
                        borderTop.yMax -= rectUI.height - border;

                        FloatRect borderBottom = rectUI;
                        borderBottom.yMin += rectUI.height - border;


                        FloatRect borderLeft = rectUI;
                        borderLeft.xMax -= rectUI.width - border;

                        FloatRect borderRight = rectUI;
                        borderRight.xMin += rectUI.width - border;

                        if (evt.isMouse && evt.type == EventType.MouseDown && evt.button == 0)
                        {
                            if (borderTop.Contains(evt.mousePosition))
                            {
                                selectionResize = SelectionResizeMode.ResizeSelectionTop;
                            }
                            else if (borderBottom.Contains(evt.mousePosition))
                            {
                                selectionResize = SelectionResizeMode.ResizeSelectionBottom;
                            }
                            else if (borderLeft.Contains(evt.mousePosition))
                            {
                                selectionResize = SelectionResizeMode.ResizeSelectionLeft;
                            }
                            else if (borderRight.Contains(evt.mousePosition))
                            {
                                selectionResize = SelectionResizeMode.ResizeSelectionRight;
                            }
                        }

                        EditorGUIUtility.AddCursorRect(borderTop, MouseCursor.ResizeVertical);
                        EditorGUIUtility.AddCursorRect(borderBottom, MouseCursor.ResizeVertical);

                        EditorGUIUtility.AddCursorRect(borderLeft, MouseCursor.ResizeHorizontal);
                        EditorGUIUtility.AddCursorRect(borderRight, MouseCursor.ResizeHorizontal);
                    }
                    else if (inSelectionRect)
                    {
                        EditorGUIUtility.AddCursorRect(rectUI, MouseCursor.MoveArrow);

                        if (evt.isMouse && evt.type == EventType.MouseDown && evt.button == 0)
                        {
                            if (rectUI.Contains(evt.mousePosition))
                            {
                                selectionResize = SelectionResizeMode.MoveSelection;
                            }
                        }
                    }
                }
            }
            #endregion

            DrawUIContent();
            GUILayout.FlexibleSpace();

            if (ShowStatusbar)
            {
                DrawStatusbar();
            }

            if (lastWindowSize != this.position.size)
            {
                ThrowWindowSizeChanged();

                lastWindowSize = this.position.size;
            }
        }

        /// <summary>
        /// Renders the main UI between toolbar and status bar.
        /// Place your main UI code here.
        /// </summary>
        protected void DrawUIContent()
        {
            if (settingsAreaStyle == null)
            {
                settingsAreaStyle = new GUIStyle("Box");
                //settingsAreaStyle = EditorStyles.inspectorDefaultMargins;
                settingsAreaStyle.stretchHeight = true;
                settingsAreaStyle.stretchWidth = false;
                //settingsAreaStyle.fixedWidth = settingsAreaWidth;
                //settingsAreaStyle.margin = new RectOffset(0, 0, menubarHeight+10, 0);
                //settingsAreaStyle.iconMargin = new RectOffset(0, 0, canvas.Margin.top - menubarHeight, 0);
            }

            EditorGUILayout.BeginVertical();

            if (ShowToolbar)
            {
                //DrawSideToolbar(toolIcons, new Float2(0, canvas.Margin.top - menubarHeight));
                DrawHorizontalToolbar(toolIcons, new Float2(0, canvas.Margin.top - menubarHeight));
            }

            EditorGUILayout.BeginHorizontal();



            DrawPreview();

            if (ShowSettingsArea)
            {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(settingsAreaWidth), GUILayout.ExpandHeight(true));


                //GUILayout.Space(canvas.Margin.top);
                DrawSettingsContent();


                EditorGUILayout.EndVertical();
            }


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawMenubar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true), GUILayout.Height(menubarHeight));
            DrawMenubarContent();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawStatusbar()
        {
            int labelWidth = 140;

            Float2 p = CanvasToTexture(canvas.MouseCanvasPositionNormalized);

            //float x = (float)Math.Round(canvas.MouseCanvasPositionNormalized.x, 3);
            //float y = (float)Math.Round(canvas.MouseCanvasPositionNormalized.y, 3);

            //float offsetx = (float)Math.Round(previewOffsetNormalized.x, 3);
            //float offsety = (float)Math.Round(previewOffsetNormalized.y, 3);

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true), GUILayout.Height(menubarHeight));
            EditorGUILayout.LabelField(string.Format("Canvas {0}", canvas.MouseCanvasPositionNormalized), SharedStyles.DefaultLabel, GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
            EditorGUILayout.LabelField(string.Format("Offset {0}", previewOffsetNormalized), SharedStyles.DefaultLabel, GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
            EditorGUILayout.LabelField(string.Format("UV {0}", p), SharedStyles.DefaultLabel, GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
            DrawStatusbarContent();

            GUILayout.FlexibleSpace();

            RenderAlpha = EditorGUILayout.Toggle(RenderAlpha, SharedStyles.ColorChannelToggleStyle, GUILayout.MinWidth(25), GUILayout.MaxWidth(25));
            FixedAspectRatio = EditorGUILayout.Toggle(FixedAspectRatio, SharedStyles.AspectToggleStyle, GUILayout.MinWidth(25), GUILayout.MaxWidth(25));
            locked = EditorGUILayout.Toggle(locked, SharedStyles.LockToggleStyle, GUILayout.MinWidth(25), GUILayout.MaxWidth(25));

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawMenubarContent()
        {

        }

        protected virtual void DrawSettingsContent()
        {

        }

        protected virtual void DrawStatusbarContent()
        {

        }

        protected void DrawMenubarTransform()
        {
            EditorGUI.BeginChangeCheck();



            EditorGUILayout.LabelField("Show Seams", GUILayout.MaxWidth(80));
            showSeams = EditorGUILayout.Toggle(showSeams, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));

            EditorGUILayout.LabelField("Tiling", GUILayout.MaxWidth(80));
            previewZoom = EditorGUILayout.Slider(previewZoom, 0.1f, 4, GUILayout.MinWidth(130), GUILayout.MaxWidth(180));

            EditorGUILayout.LabelField("Offset", GUILayout.MaxWidth(80));
            previewOffsetNormalized = EditorGUILayout.Vector2Field(GUIContent.none, previewOffsetNormalized, GUILayout.MinWidth(130), GUILayout.MaxWidth(180));

            if(EditorGUI.EndChangeCheck())
            {
                previewOffset = Float2.right * canvas.Width * previewOffsetNormalized.x + Float2.up * canvas.Height * previewOffsetNormalized.y;

                //UpdatePreviewTransform();
            }
        }

        protected void CorrectAspect()
        {
            if (FixedAspectRatio && canvas != null)
            {
                Vector2 size = this.position.size;

                Rect rect = canvas.MaxCanvasRect;
                Vector2 center = rect.center;

                float acWidth = rect.width;
                float acHeight = rect.height;

                if (acWidth <= acHeight)
                {
                    rect.size = new Vector3(acWidth, acWidth);
                }
                else
                {
                    rect.size = new Vector3(acHeight, acHeight);
                }

                rect.center = center;

                canvas.Margin = new RectOffset(Mathf.RoundToInt(rect.xMin), Mathf.RoundToInt(this.position.size.x - rect.xMax),
                                               Mathf.RoundToInt(rect.yMin), Mathf.RoundToInt(this.position.size.y - rect.yMax));
            }
            else
            {
                canvas.Reset();
            }
        }

        //protected void DrawToolbarSaveAs()
        //{
        //    bool save = GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.MaxWidth(100));

        //    if(save)
        //    {
        //        SaveAs();
        //    }
        //}

        //protected void UpdateSelectionTexture(bool isDragging)
        //{
        //    int size = 256;

            //selectionTetxure = new Texture2D(size, size, TextureFormat.ARGB32, false);
            //selectionTetxure.filterMode = FilterMode.Point;
            //selectionTetxure.wrapMode = TextureWrapMode.Repeat;
            //selectionTetxure.alphaIsTransparency = true;



            //Color outsideColor = new Color(0, 0, 0, 0);
            //Color insideColorFixed = new Color(1, 1, 0.5f, 1f);
            //Color insideColorDragging = new Color(0, 1, 0.5f, 1f);

            //Color insideColor = isDragging ? insideColorDragging : insideColorFixed;

            //Color[] pixels = new Color[size * size];

            //foreach (FloatRect screenRect in canvas.Selection)
            //{            
            //    int maxX = Mathf.RoundToInt(screenRect.xMax * size);
            //    int minX = Mathf.RoundToInt(screenRect.xMin * size);

            //    int maxY = Mathf.RoundToInt(screenRect.yMax * size);
            //    int minY = Mathf.RoundToInt(screenRect.yMin * size);

            //    //int fromIndex = 0;
            //    for (int x = 0; x < size; x++)
            //    {
            //        bool insideX = x >= minX && x <= maxX;

            //        for (int y = 0; y < size; y++)
            //        {
            //            bool insideY = y >= minY && y <= maxY;

            //            Color color = insideX && insideY ? insideColor : outsideColor;

            //            //selectionTetxure.SetPixel(x, y, color);
            //            pixels[(y * size) + x] = color;
            //        }
            //    }
            //}

            //selectionTetxure.SetPixels(pixels);
            //selectionTetxure.Apply();

            //return selectionTetxure;
        //}

        protected void DrawSelection()
        {
            //if (selectionTetxure != null)
            //{
                Rect r = canvas.Selection.GetFirst();

                Vector2 texelSize = new Vector2(1.0f / canvas.Width, 1.0f / canvas.Height) * previewZoom;
                selectionMaterial.SetVector("_TexelSize", texelSize);
                selectionMaterial.SetVector("_Selection", new Vector4(r.xMin, r.yMin, r.xMax, r.yMax));
                selectionMaterial.SetFloat("_EditorTime", (float)EditorApplication.timeSinceStartup*0.01f);


                canvas.DrawTexture(Texture2D.whiteTexture, previewZoom, previewOffsetNormalized, selectionMaterial);
                //canvas.DrawTexture(selectionTetxure, previewZoom, previewOffsetNormalized, selectionMaterial);
            //}
        }

        protected void DrawPreview()
        {
            EditorGUILayout.BeginHorizontal(styleContainer);
            EditorGUILayout.BeginHorizontal();
            
            if(RenderAlpha)
            {
                canvas.DrawTransparencyCheckerTexture();
            }

            if (PreviewTexture != null)
            {
                canvas.DrawTexture(PreviewTexture, previewZoom, previewOffsetNormalized, showSeams, RenderAlpha);
            }

            DrawSelection();
            //DrawMapSeams();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSideToolbar(GUIContent[] toolIcons, Float2 offset)
        {
            if (toolIcons != null)
            {
                bool en = GUI.enabled;
                GUI.enabled = toolbarEnabled;

                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                GUILayout.Space(offset.x);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(offset.y);

                EditorGUILayout.BeginVertical(Shared.SharedStyles.SideToolbarStyle);

                int lastIndex = activeToolIndex;
                activeToolIndex = GUILayout.SelectionGrid(activeToolIndex, toolIcons, 1, EditorStyles.toolbarButton);
                if (lastIndex != activeToolIndex) { ThrowToolChanged(); }

                GUILayout.Space(50);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                GUI.enabled = en;
            }
        }

        private void DrawHorizontalToolbar(GUIContent[] toolIcons, Float2 offset)
        {
            if (toolIcons != null)
            {
                bool en = GUI.enabled;
                GUI.enabled = toolbarEnabled;

                GUIStyle toolbar = new GUIStyle(EditorStyles.toolbar);
                toolbar.stretchWidth = true;

                EditorGUILayout.BeginHorizontal(toolbar);//, GUILayout.ExpandWidth(true));

                var s = new GUIStyle( EditorStyles.toolbarButton);
                s.fixedWidth = 60;

                int lastIndex = activeToolIndex;
                activeToolIndex = GUILayout.SelectionGrid(activeToolIndex, toolIcons, toolIcons.Length, s);
                if (lastIndex != activeToolIndex) { ThrowToolChanged(); }

                GUILayout.Space(50);

                EditorGUILayout.EndHorizontal();

                GUI.enabled = en;
            }
        }

        protected int Clamp(int value, int max)
        {
            int result;

            if (value < 0)
            {
                result = max - (Math.Abs(value) % max);
                //result = (Math.Abs(value) % max);
            }
            else
            {
                result = value % max;
            }

            return result;
        }

        protected float Clamp(float value, float max)
        {
            float result;

            if (value < 0)
            {
                result = max - (Math.Abs(value) % max);
                //result = (Math.Abs(value) % max);
            }
            else
            {
                result = value % max;
            }

            return result;
        }


        //------------------------------
        // Handler Methods
        //------------------------------


        void canvas_OnMouseMove(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            //Debug.Log("posnorm: " + positionNormalized);
        }
        

        void canvas_OnMouseDragged(Float2 delta, Float2 deltaNormalized, 
                                   Float2 position, Float2 positionNormalized, 
                                   NativeEventModifiers modifier, MouseButton btn)
        {
            if(SelectionResize != SelectionResizeMode.None)
            {
                if (canvas.Selection.HasSelection)
                {
                    FloatRect r = canvas.Selection.GetFirst();

                    Float2 deltaScaled = deltaNormalized * previewZoom;

                    if(SelectionResize == SelectionResizeMode.ResizeSelectionTop)
                    {
                        r.yMax -= deltaScaled.y;
                    }
                    else if (SelectionResize == SelectionResizeMode.ResizeSelectionBottom)
                    {
                        r.yMin -= deltaScaled.y;
                    }
                    else if (SelectionResize == SelectionResizeMode.ResizeSelectionLeft)
                    {
                        r.xMin += deltaScaled.x;
                    }
                    else if (SelectionResize == SelectionResizeMode.ResizeSelectionRight)
                    {
                        r.xMax += deltaScaled.x;
                    }

                    else if (SelectionResize == SelectionResizeMode.MoveSelection)
                    {
                        deltaScaled.y *= -1;
                        r.position += deltaScaled;
                    }

                    canvas.Selection.UpdateFirst(r);
                }

            }

            //if (modifier == NativeEventModifiers.Control)
            if (ActiveTool == DefaultEditTool.SingleSelection && btn == MouseButton.Left)
            {
                if (canvas.Selection.Count > 0)
                {
                    canvas.Selection.UpdateFirst(CreateSelectionRect(positionNormalized, SelectionStartPoint));

                    //UpdateSelectionTexture(true);
                }
            }
            else if (ActiveTool == DefaultEditTool.PanZoom || btn == MouseButton.Middle)
            {


                //ActiveTool = DefaultEditTool.PanZoom;

                if (EnablePreviewPan && !locked)
                {

                    //EditorGUIUtility.AddCursorRect(canvas.AbsoluteCanvasRect, MouseCursor.Pan);

                    #region --- Snapped Pan ---
                    if (EnableSnapPan && modifier == NativeEventModifiers.Shift)
                    {
                        accuDeltaDragNormalized += deltaNormalized;
                        float threshold = 0.2f;
                        if (accuDeltaDragNormalized.magnitude > threshold)
                        {

                            deltaNormalized = GetSnappedDelta(accuDeltaDragNormalized, threshold);
                            delta = new Float2(deltaNormalized.x * canvas.Width, deltaNormalized.y * canvas.Height);

                            //Debug.Log(string.Format("norm:({0}, {1}) s:({2}, {3})", transform.x, transform.y, deltaNormalized.x, deltaNormalized.y));


                            accuDeltaDragNormalized = Float2.zero;
                        }
                        else
                        {
                            deltaNormalized = Float2.zero;
                            delta = Float2.zero;
                        }
                    }
                    #endregion

                    deltaNormalized.y *= -1;

                    previewOffsetNormalized += deltaNormalized;
                    previewOffset += delta;

                    if (ClampPan)
                    {
                        float limit = 1;
                        if(previewZoom < 1)
                        {
                            limit /= previewZoom;
                        }
                        previewOffsetNormalized = FloatMath.Clamp(previewOffsetNormalized, -limit, limit);
                        previewOffset = new Float2(previewOffsetNormalized.x * canvas.Width, previewOffsetNormalized.y * canvas.Height);
                    }

                    if (EnableSnapPan && modifier == NativeEventModifiers.Shift)
                    {
                        previewOffsetNormalized.x = (float)Math.Round(previewOffsetNormalized.x * 5f, MidpointRounding.ToEven) / 5f;
                        previewOffsetNormalized.y = (float)Math.Round(previewOffsetNormalized.y * 5f, MidpointRounding.ToEven) / 5f;

                        previewOffset = new Float2(previewOffsetNormalized.x * canvas.Width, previewOffsetNormalized.y * canvas.Height);
                    }

                    //UpdatePreviewTransform();
                }
            }
        }

        void canvas_OnMouseWheel(Float2 position, Float2 positionNormalized, Float2 delta, NativeEventModifiers modifier)
        {
            if (EnablePreviewZoom && !locked)
            {
                Float2 pointBefore = CanvasToTexture(positionNormalized);

                //ActiveTool = DefaultEditTool.PanZoom;
                previewZoom += previewZoomSpeed * delta.y;
                previewZoom = Mathf.Clamp(previewZoom, 0.1f, 4.0f);
                Float2 pointAfter = CanvasToTexture(positionNormalized);



                Float2 c1 = TextureToCanvas(pointBefore);
                Float2 c2 = TextureToCanvas(pointAfter);
                Float2 d1 = c1 - c2;
                Float2 d2 = pointBefore - pointAfter;



                

                previewOffsetNormalized -= d1;
                previewOffset = new Float2(previewOffsetNormalized.x * canvas.Width, previewOffsetNormalized.y * canvas.Height);


                //Debug.Log(string.Format("uv before: {0} uv after: {1}  c before: {2}  c after: {3} d1: {4}  d2: {5} pn: {6} po: {7} ",
                //    pointBefore, pointAfter, c1, c2, d1, d2, positionNormalized, previewOffsetNormalized));
            }
        }

        void canvas_OnMouseDown(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            //if (modifier == NativeEventModifiers.Control)
            if (ActiveTool == DefaultEditTool.SingleSelection)
            {
                SelectionStartPoint = positionNormalized;

                //if(ActiveTool == DefaultEditTool.SingleSelection)
                //{
                    canvas.Selection.Clear();
                    canvas.Selection.Add(FloatRect.Empty);
                //}
            }
        }

        void canvas_OnMouseUp(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            SelectionResize = SelectionResizeMode.None;

            if(ActiveTool == DefaultEditTool.SingleSelection)
            {
                //CurrentSelection = CreateSelectionRect(positionNormalized, SelectionStartPoint);
                //UpdateSelectionTexture(false);


                if (canvas.Selection.Count > 0)
                {
                    canvas.Selection.UpdateFirst(CreateSelectionRect(positionNormalized, SelectionStartPoint));
                }
            }

            //ActiveTool = DefaultEditTool.Default;
        }

        void canvas_OnSizeChanged(Float2 sizeOld, Float2 sizeNew)
        {
            previewOffset = Float2.one;
            previewOffset.x = previewOffsetNormalized.x * sizeNew.x;
            previewOffset.y = previewOffsetNormalized.y * sizeNew.y;




        }

        /// <summary>
        /// Transforms a point from normalized canvas space to normalized texture space.
        /// </summary>
        /// <param name="point">Point in normalized canvas space.</param>
        /// <returns>Returns point in texture space.</returns>
        public Float2 CanvasToTexture(Float2 point)
        {
            Float2 p = point;

            //p.x -= previewOffsetNormalized.x;
            //p.y -= previewOffsetNormalized.y;

            p -= previewOffsetNormalized;


            p *= previewZoom;

            return p;
        }

        /// <summary>
        /// Transforms a point from normalized canvas space to normalized texture space and clamps it to [0..1].
        /// </summary>
        /// <param name="point">Point in normalized canvas space.</param>
        /// <returns>Returns point in texture space.</returns>
        public Float2 CanvasToTextureClamped(Float2 point)
        {
            Float2 p = point;

            //p.x -= previewOffsetNormalized.x;
            //p.y -= previewOffsetNormalized.y;
            p -= previewOffsetNormalized;

            p *= previewZoom;

            p.x = p.x % 1.0f;
            p.y = p.y % 1.0f;

            return p;
        }

        /// <summary>
        /// Transforms a point from normalized texture space to normalized canvas space.
        /// </summary>
        /// <param name="point">Point in normalized texture space.</param>
        /// <returns>Returns point in canvas space.</returns>
        public Float2 TextureToCanvas(Float2 point)
        {
            Float2 p = point;
            p /= previewZoom;

            //p.x += previewOffsetNormalized.x;
            //p.y += previewOffsetNormalized.y;

            p += previewOffsetNormalized;


            return p;
        }

        private FloatRect CreateSelectionRect(Float2 normalizedStart, Float2 normalizedEnd)
        {
            Float2 offset = Float2.Scale(previewOffsetNormalized, new Float2(-1, -1));
            float zoomInv = 1.0f / previewZoom;


            //normalizedStart.y *= -1;
            //normalizedEnd.y *= -1;
            //Correct size


            normalizedStart += offset;
            normalizedEnd += offset;

            normalizedStart = new Float2(RepeatClamp(normalizedStart.x, zoomInv), RepeatClamp(normalizedStart.y, zoomInv));
            normalizedEnd = new Float2(RepeatClamp(normalizedEnd.x, zoomInv), RepeatClamp(normalizedEnd.y, zoomInv));

            normalizedStart *= previewZoom;
            normalizedEnd *= previewZoom;

            Float2 start = new Float2(Mathf.Min(normalizedStart.x, normalizedEnd.x), Mathf.Min(normalizedStart.y, normalizedEnd.y));


            Float2 size = normalizedStart - normalizedEnd;
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);




            //Debug.Log(string.Format("start: {0} end: {1} size:{2}", start, start, size));
            FloatRect rect = new FloatRect(start, size);

            return rect;
        }

        private float RepeatClamp(float x, float size)
        {
            float result = 0;

            if (size > 0)
            {
                //if (mode == CLAMP)
                //{
                //    result = ClampInt(x, 0, size - 1);
                //}
                //else
                //{
                    if (x < 0)
                    {
                        result = size - (Mathf.Abs(x) % size);
                    }
                    else
                    {
                        result = x % size;
                    }
                //}
            }
            //Debug.Log(string.Format("x: {0} size: {1} result: {2}", x, size, result));

            return result;
        }

        private Float2 GetSnappedDelta(Float2 n, float threshold)
        {
            float t = 0.02f;// Mathf.Sqrt(threshold);
            Float2 nn = new Float2(Math.Abs(n.x) > t ? Math.Sign(n.x) : 0, Math.Abs(n.y) > t ? Math.Sign(n.y) : 0) * 0.2f;
            return nn;
        }

        //public static void UpdatePreview(Texture2D texture)
        //{
        //    if (WindowInstance != null)
        //    {
        //        WindowInstance.PreviewTexture = texture;
        //        WindowInstance.Repaint();
        //    }
        //}

        private void ThrowToolChanged()
        {
            if (Enum.IsDefined(typeof(DefaultEditTool), activeToolIndex))
            {
                ActiveTool = (DefaultEditTool)activeToolIndex;
            }

            if (OnToolChanged != null)
            {
                OnToolChanged(activeTool);
            }
        }

        private void ThrowWindowSizeChanged()
        {
            if (OnWindowSizeChanged != null)
            {
                OnWindowSizeChanged(this);
            }
        }

        //------------------------------
        // Static Methods
        //------------------------------
        protected static void ShowWindow()
        {
            if (WindowInstance == null)
            {
                T window = (T)EditorWindow.GetWindow(typeof(T));
                
                WindowInstance = window;
            }

            WindowInstance.Show();
            WindowInstance.Repaint();
        }
    }
}

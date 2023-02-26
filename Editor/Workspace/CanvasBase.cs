
using ch.sycoforge.Device;
using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Runtime.Enums;
using System;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Workspace
{
    public abstract class CanvasBase : IDisposable
    {
        /// <summary>
        /// Delegate for canvas mouse events.
        /// </summary>
        /// <param name="position">The position within the canvas. Origin (0/0) is located in the bottom-left corner.</param>
        /// <param name="positionNormalized">The normalized position within the canvas [0..1].</param>
        public delegate void MouseEventDelegte(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier);

        /// <summary>
        /// Delegate for canvas mouse wheel events.
        /// </summary>
        /// <param name="position">The position within the canvas. Origin (0/0) is located in the bottom-left corner.</param>
        /// <param name="positionNormalized">The normalized position within the canvas [0..1].</param>
        /// <param name="delta">The delta mouse wheel movement.</param>
        public delegate void MouseWheelEventDelegte(Float2 position, Float2 positionNormalized, Float2 delta, NativeEventModifiers modifier);


        /// <summary>
        /// Delegate for canvas mouse wheel events.
        /// </summary>
        /// <param name="delta">The delta mouse wheel movement.</param>
        /// <param name="deltaNormalized">The normalized delta mouse wheel movement.</param>
        /// <param name="position">The position within the canvas.</param>
        /// <param name="positionNormalized">The normalized position within the canvas [0..1].</param>
        public delegate void MouseDraggedEventDelegte(Float2 delta, Float2 deltaNormalized, Float2 position, Float2 positionNormalized, NativeEventModifiers modifier, MouseButton mouseButton);

        /// <summary>
        /// Delegate for key events.
        /// </summary>
        /// <param name="key">The key that was pressed during the event.</param>
        /// <param name="modifier">The modifier key that was pressed during the event.</param>
        public delegate void KeyPressedEventDelegte(NativeKeyCode key, NativeEventModifiers modifier);

        /// <summary>
        /// Delegate for canvas size change events.
        /// </summary>
        /// <param name="sizeOld">The old size in pixels.</param>
        /// <param name="sizeNew">The new size in pixels</param>
        public delegate void CanvasSizeEventDelegte(Float2 sizeOld, Float2 sizeNew);

        //------------------------------
        // Events
        //------------------------------
        public event MouseDraggedEventDelegte OnMouseDragged;
        public event MouseEventDelegte OnMouseClicked;
        public event MouseEventDelegte OnMouseDown;
        public event MouseEventDelegte OnMouseUp;
        public event MouseEventDelegte OnMouseMove;
        public event MouseWheelEventDelegte OnMouseWheel;
        public event CanvasSizeEventDelegte OnSizeChanged;
        public event KeyPressedEventDelegte OnKeyPressed;

        //------------------------------
        // Properties
        //------------------------------
        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }

        public float AspectRatio
        {
            get { return width/ Mathf.Max(height, 0.000001f); }
        }

        /// <summary>
        /// Only throws the <c>OnMouseDragged</c> when inside of canvas.
        /// </summary>
        public bool ConstrainDraggedEvent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value specifying whether the preview texture should be clamped when out of bounds [0..1].
        /// </summary>
        public bool ClampPreview
        {
            get;
            set;
        }

        /// <summary>
        /// Only throws mouse events when mouse is inside of canvas.
        /// </summary>
        public bool ConstrainMouseEvents
        {
            get { return constrainMouseEvents; }
            set { constrainMouseEvents = value; }
        }

        /// <summary>
        /// Gets the top left corner of the canvas within the parent window.
        /// </summary>
        public Float2 CanvasPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// The addtive position of the mouse in window coordinates.
        /// Note: this position can contains coordinates outside the actual window.
        /// </summary>
        public Float2 AdditiveMouseWindowPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// The position of the mouse in window coordinates.
        /// </summary>
        public Float2 MouseWindowPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// The position of the mouse in canvas coordinates. Origin (0/0) is located in the bottom-left corner.
        /// </summary>
        public Float2 MouseCanvasPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// The position of the mouse in normalized canvas coordinates [0..1]. Origin (0/0) is located in the bottom-left corner.
        /// </summary>
        public Float2 MouseCanvasPositionNormalized
        {
            get;
            private set;
        }

        //public UnityBitmap Target
        //{
        //    get;
        //    set;
        //}

        public bool ClampCursor
        {
            get;
            set;
        }

        public bool CursorInsideCanvas
        {
            get
            {
                return isInCanvas;
            }
        }

        public bool CursorWasRepeated
        {
            get
            {
                return cursorWasRepeated;
            }
        }

        /// <summary>
        /// The actual margin of the canvas.
        /// </summary>
        public RectOffset Margin
        {
            get
            {
                return margin;
            }
            
            set
            {
                margin = value;

                CanvasPosition = new Float2(margin.left, margin.top);    
            }
        }

        /// <summary>
        /// FloatRect of canvas in absoulte display coordinates.
        /// </summary>
        public FloatRect AbsoluteCanvasRect
        {
            get
            {
                FloatRect rect = new FloatRect(window.Position.position + CanvasPosition, new Float2(Width, Height));
                return rect;
            }
        }

        /// <summary>
        /// FloatRect of canvas in window coordinates.
        /// </summary>
        public FloatRect CanvasRect
        {
            get
            {
                FloatRect rect = new FloatRect(new Float2(margin.left, margin.top), new Float2(Width, Height));
                return rect;
            }
        }

        /// <summary>
        /// The maximum FloatRect of canvas in window coordinates.
        /// </summary>
        public FloatRect MaxCanvasRect
        {
            get
            {
                float w = window.Position.size.x - maxMargin.horizontal;
                float h = window.Position.size.y - maxMargin.vertical;

                FloatRect rect = new FloatRect(new Float2(maxMargin.left, maxMargin.top), new Float2(w, h));
                return rect;
            }
        }

        public bool IsDragging
        {
            get
            {
                return isMouseDragging;
            }
        }

        public SampleMode Mode
        {
            get
            {
                return sampleMode;
            }

            set
            {
                sampleMode = value;
            }
        }

        public CanvasSelection Selection
        {
            get
            {
                return selection;
            }
        }

        //-----------------------------
        // Static Fields
        //-----------------------------

        //------------------------------
        // Private Fields
        //------------------------------ 

        protected CanvasSelection selection = new CanvasSelection();
        protected IEditorWindow window;
        protected Material canvasMaterial;
        protected Material canvasMaterialOpaque;

        protected RectOffset margin;
        protected RectOffset maxMargin;
        protected Float2 lastCanvasPosition;
        protected Float2 lastCanvasPositionNormalized;
        //private Texture2D textureResult;
        //private FloatRect cursorRect;

        protected float width, height;
        protected bool isMouseDown;
        protected bool isMouseDragging;
        protected bool textureDidChange;

        private bool isInCanvas;
        private bool constrainMouseEvents = true;
        private bool cursorWasRepeated;
        private SampleMode sampleMode;
        //private Texture2D textureCheckerBoard;

        //------------------------------
        // Constructor
        //------------------------------
        public CanvasBase(IEditorWindow window, RectOffset margin)
            : this(window, margin, SampleMode.Tileable)
        {      
        }

        public CanvasBase(IEditorWindow window, RectOffset margin, SampleMode sampleMode) 
        {
            this.window = window;
            this.Margin = margin;
            this.sampleMode = sampleMode;
            this.canvasMaterial = new Material(Shader.Find("Hidden/nu Assets/UI/Canvas"));
            this.canvasMaterialOpaque = new Material(Shader.Find("Hidden/nu Assets/UI/CanvasOpaque"));
            this.maxMargin = margin;

            UpdateSize();  
        }

        //------------------------------
        // Public Methods
        //------------------------------

        public void Reset()
        {
            Margin = maxMargin;
        }


        public virtual void Update(NativeEvent evt)
        {
            float widthOld = width;
            float heightOld = height;

            UpdateSize();

            if (widthOld != width || heightOld != height)
            {
                ThrowCanvasSizeChanged(new Float2(widthOld, heightOld), new Float2(width, height));
            }

            UpdateInputState(evt);

            window.Repaint();
            //return;

            //if (isMouseDragging)
            //{
            //    Float2 pos = evt.mousePosition;

            //    bool isInCanvas = IsInCanvas(pos);

            //    if (Target != null && ActiveBrush != null)// && isInCanvas)
            //    {
            //        pos = RepeatCursor(pos);

            //        Float2 bitmapPos = GetRelMousePosition(pos);
            //        Float2 bitmapPosNormalized = NormalizePosition(bitmapPos);

            //        ActiveBrush.DrawBrush(bitmapPosNormalized.x, bitmapPosNormalized.y, Target);

            //        lastCanvasPosition = bitmapPos;
            //        window.Repaint();
            //    }
            //}
        }

        //public Texture2D GetResult()
        //{
        //    if (Target != null && textureDidChange)
        //    {
        //        textureResult = Target.ToTexture2D(MapLabSettings.PreviewColorSpace);

        //        textureDidChange = false;
        //    }

        //    return textureResult;
        //}

        //public FloatRect GetCursorRect()
        //{
        //    if (ActiveBrush != null)// && textureDidChange)
        //    {
        //        float rHalf = ActiveBrush.Radius * 0.5f;
        //        cursorRect = new FloatRect(MouseWindowPosition.x -rHalf, MouseWindowPosition.y - rHalf, ActiveBrush.Radius, ActiveBrush.Radius);
        //    }

        //    return cursorRect;
        //}

        /// <summary>
        /// Transforms the specified normalized rectangle from UI space (origin top-left) to the canvas space (origin bottom-left).
        /// </summary>
        /// <param name="rect">The rectangle to transform.</param>
        /// <returns>The transformed rectangle.</returns>
        public FloatRect TransformRectNorm(FloatRect rect)
        {
            Vector2 pos = rect.position;
            pos.y = 1 - pos.y - rect.height;
            FloatRect r = new FloatRect(pos, rect.size);

            return r;
        }

        /// <summary>
        /// Inverse transforms the specified normalized rectangle from the canvas space (origin bottom-left) to UI space (origin top-left).
        /// </summary>
        /// <param name="rect">The rectangle to transform.</param>
        /// <returns>The transformed rectangle.</returns>
        public FloatRect InverseTransformRectNorm(FloatRect rect, bool toWindowSpace = false)
        {
            Float2 pos = rect.position;
            pos.y = 1 - pos.y - rect.height;

            if (toWindowSpace)
            {
                Float2 offset = CanvasPosition;
                offset.x /= width;
                offset.y /= height;
                pos = pos + offset;
            }

            FloatRect r = new FloatRect(pos, rect.size);

            return r;
        }

        /// <summary>
        /// Inverse transforms the specified normalized rectangle from the canvas space (origin bottom-left) to UI space (origin top-left).
        /// </summary>
        /// <param name="rect">The rectangle to transform.</param>
        /// <returns>The transformed rectangle.</returns>
        public FloatRect InverseTransformRectNorm(FloatRect rect, float tile, Float2 offset, bool toWindowSpace = false)
        {
            float scale = (1 / tile);

            Float2 o = offset;
            //o.y *= -1;

            rect.size *= scale;


            Float2 position = (rect.position * scale) + o;

            Float2 pos = position;
            pos.y = 1 - rect.height - pos.y;

            if (toWindowSpace)
            {
                Float2 cOffset = CanvasPosition;
                cOffset.x /= width;
                cOffset.y /= height;
                pos = pos + cOffset;
            }

            FloatRect r = new FloatRect(pos, rect.size);

            return r;
        }

        /// <summary>
        /// Transforms the specified normalized point from UI space (origin top-left) to the canvas space (origin bottom-left).
        /// </summary>
        /// <param name="rect">The point to transform.</param>
        /// <returns>The point.</returns>
        public Float2 TransformPointNorm(Float2 p)
        {
            Vector2 pos = p;
            pos.y = 1 - pos.y;

            return pos;
        }

        /// <summary>
        /// Scales and offsets the specified rectangle (origin top-left) to match the canvas' current transformation.
        /// </summary>
        /// <param name="rect">The rectangle to transform.</param>
        /// <returns>The transformed rectangle.</returns>
        public FloatRect ScaleOffsetRect(FloatRect rect, float tile, Float2 offset)
        {
            Float2 o = offset;
            //o.y *= -1;
            float scale = 1 / tile;
            Float2 pos = (rect.position * scale) + o;
            Float2 size = rect.size * scale;

            return new FloatRect(pos, size);
        }

        ///// <summary>
        ///// Scales and offsets the specified point to match the canvas' current transformation.
        ///// </summary>
        ///// <param name="rect">The point to transform.</param>
        ///// <returns>The rect rectangle.</returns>
        //public Float2 ScaleOffsetPoint(Float2 rect, float tile, Float2 offset)
        //{
        //    float scale = 1 / tile;
        //    Float2 pos = (rect * scale) - offset;

        //    return pos;
        //}

        /// <summary>
        /// Draws a texture within the canvas.
        /// </summary>
        /// <param name="tetxure">The texture to draw</param>
        /// <param name="position">The normalized position in canvas coordinates. Origin (0/0) is the bottom-left corner.</param>
        /// <param name="size">The normalized size.</param>
        /// <param name="material">The material used for drawing the texture.</param>
        public void DrawTextureNormalized(Texture2D tetxure, Float2 position, Float2 size, Material material)
        {
            Float2 p = DenormalizePosition(position);
            Float2 s = DenormalizePosition(size);
            FloatRect groupRect = new FloatRect(Margin.left + p.x, Margin.top + p.y, s.x, s.y);

            //EditorGUI.DrawPreviewTexture(groupRect, tetxure, material);
            DrawTextureClipped(groupRect, tetxure, material);
        }

        /// <summary>
        /// Draws a texture within the canvas.
        /// </summary>
        /// <param name="tetxure">The texture to draw</param>
        /// <param name="position">The normalized position in canvas coordinates. Origin (0/0) is the bottom-left corner.</param>
        /// <param name="size">The normalized size.</param>
        /// <param name="tile">The uniform tiling</param>
        /// <param name="offset">The normalized offset used to draw.</param>
        /// <param name="material">The material used for drawing the texture.</param>
        public void DrawTextureNormalized(Texture2D tetxure, Float2 position, Float2 size, float tile, Float2 offset, Material material)
        {
            float scale = (1.0f / tile);

            Float2 o = offset;
            //o.y *= -1;

            size *= scale;


            position = (position * scale) + o;
            position.y = 1 - (size.y) - position.y;

            Float2 p = DenormalizePosition(position);
            Float2 s = DenormalizePosition(size);

            //Float2 pIn = position;
            //pIn = (pIn* scale)  + o;
            //pIn.y = 1 - (size.y * scale) -pIn.y;

            //Float2 rect = DenormalizePosition(pIn);
            //Float2 s = DenormalizePosition(size * scale);


            FloatRect groupRect = new FloatRect(Margin.left + p.x, Margin.top + p.y, s.x, s.y);

            //EditorGUI.DrawPreviewTexture(groupRect, tetxure, material);
            DrawTextureClipped(groupRect, tetxure, material);
        }

        /// <summary>
        /// Draws a texture within the canvas.
        /// </summary>
        /// <param name="tetxure">The texture to draw</param>
        /// <param name="position">The absolute position in canvas coordinates. Origin (0/0) is the bottom-left corner.</param>
        /// <param name="size">The absolute size.</param>
        /// <param name="material">The material used for drawing the texture.</param>
        public void DrawTexture(Texture2D tetxure, Float2 position, Float2 size, Material material)
        {
            FloatRect groupRect = new FloatRect(Margin.left + position.x, Margin.top + position.y, size.x, size.y);

            //EditorGUI.DrawPreviewTexture(groupRect, tetxure, material);
            DrawTextureClipped(groupRect, tetxure, material);
        }

        /// <summary>
        /// Draws a fullscreen texture within the canvas.
        /// </summary>
        /// <param name="tetxure">The texture to draw</param>
        /// <param name="tile">The uniform tiling</param>
        /// <param name="offset">The normalized offset used to draw.</param>
        /// <param name="material">The material used for drawing the texture.</param>
        public void DrawTexture(Texture2D tetxure, float tile, Float2 offset, Material material)
        {
            float w = (this.window.Position.width - Margin.horizontal);
            float h = (this.window.Position.height - Margin.vertical);

            material.SetFloat("_OffsetX", offset.x);
            material.SetFloat("_OffsetY", offset.y);
            material.SetFloat("_Tile", tile);

            FloatRect groupRect = new FloatRect(Margin.left, Margin.top, w, h);


            EditorGUI.DrawPreviewTexture(groupRect, tetxure, material);
        }

        /// <summary>
        /// Draws a fullscreen texture within the canvas using the default canvas shader.
        /// </summary>
        /// <param name="tetxure">The texture to draw</param>
        /// <param name="tile">The uniform tiling</param>
        /// <param name="offset">The offset used to draw.</param>
        public void DrawTexture(Texture2D tetxure, float tile, Float2 offset, bool showSeams, bool renderAlpha)
        {
            //float w = (this.window.Position.width - Margin.horizontal);
            //float h = (this.window.Position.height - Margin.vertical);

            //canvasMaterial.SetFloat("_OffsetX", offset.x);
            //canvasMaterial.SetFloat("_OffsetY", offset.y);

            //canvasMaterial.SetFloat("_Tile", tile);

            Material mat = renderAlpha ? canvasMaterial : canvasMaterialOpaque;

            if (mat != null)
            {
                mat.SetColor("_SeamColor", showSeams ? new Color(1, 1, 1, 0.5f) : Color.clear);
                mat.SetFloat("_ClampPreview", ClampPreview ? 1 : 0);

                DrawTexture(tetxure, tile, offset, mat);
            }

            //FloatRect groupRect = new FloatRect(Margin.left, Margin.top, w, h);


            //EditorGUI.DrawPreviewTexture(groupRect, tetxure, canvasMaterial);
        }

        /// <summary>
        /// Draws a fullscreen texture within the canvas.
        /// </summary>
        public void DrawTexture(Texture2D tetxure, bool renderAlpha)
        {
            DrawTexture(tetxure, 1, Float2.zero, false, renderAlpha);
        }

        public void DrawTransparencyCheckerTexture()
        {
            DrawTransparencyCheckerTexture(CanvasRect, width / height);
        }

        public static void DrawTransparencyCheckerTexture(FloatRect position, float aspect, ScaleMode scaleMode = ScaleMode.ScaleAndCrop)
        {
            //Texture2D text = textureCheckerBoard;
            Texture2D texture = SharedGraphics.AlphaBackground;
            FloatRect rect = new FloatRect();
            FloatRect rect1 = new FloatRect();
            
            CalculateScaledTextureRects(position, scaleMode, aspect, ref rect, ref rect1);

            FloatRect uv = new Rect((float)rect.width * -0.5f / (float)texture.width, (float)rect.height * -0.5f / (float)texture.height, (float)rect.width / (float)texture.width, (float)rect.height / (float)texture.height);


            GUI.DrawTextureWithTexCoords(rect, texture, uv, false);
        }

        private void DrawTextureClipped(Rect rect, Texture texture, Material material)
        {
            EditorGUI.DrawPreviewTexture(ClipToCanvas(rect), texture, material);
            //EditorGUI.DrawPreviewTexture(rect, texture, material);
        }

        /// <summary>
        /// Clips the specified rectangle to the canvas' bounds.
        /// </summary>
        /// <param name="rect">The rectangle to clip.</param>
        /// <returns>The clipped rectangle.</returns>
        public FloatRect ClipToCanvas(FloatRect rect)
        {
            FloatRect r = new FloatRect();
            r.xMin = FloatMath.Clamp(rect.xMin, CanvasRect.xMin, CanvasRect.xMax);
            r.yMin = FloatMath.Clamp(rect.yMin, CanvasRect.yMin, CanvasRect.yMax);
            r.xMax = FloatMath.Clamp(rect.xMax, CanvasRect.xMin, CanvasRect.xMax);
            r.yMax = FloatMath.Clamp(rect.yMax, CanvasRect.yMin, CanvasRect.yMax);

            return r;
        }

        /// <summary>
        /// Clips the specified rectangle to the canvas' bounds.
        /// </summary>
        /// <param name="rect">The rectangle to clip.</param>
        /// <returns>The clipped rectangle.</returns>
        public FloatRect ClipToCanvasNorm(FloatRect rect)
        {
            FloatRect r = new FloatRect();
            r.xMin = FloatMath.Clamp01(rect.xMin);
            r.yMin = FloatMath.Clamp01(rect.yMin);
            r.xMax = FloatMath.Clamp01(rect.xMax);
            r.yMax = FloatMath.Clamp01(rect.yMax);

            return r;
        }


        public static bool CalculateScaledTextureRects(FloatRect position, ScaleMode scaleMode, float imageAspect, ref FloatRect outScreenRect, ref FloatRect outSourceRect)
        {
            float currentAspect = position.width / position.height;
            bool flag = false;
            switch (scaleMode)
            {
                case ScaleMode.StretchToFill:
                    {
                        outScreenRect = position;
                        outSourceRect = new FloatRect(0f, 0f, 1f, 1f);
                        flag = true;
                        break;
                    }
                case ScaleMode.ScaleAndCrop:
                    {
                        if (currentAspect <= imageAspect)
                        {
                            float single1 = currentAspect / imageAspect;
                            outScreenRect = position;
                            outSourceRect = new FloatRect(0.5f - single1 * 0.5f, 0f, single1, 1f);
                            flag = true;
                        }
                        else
                        {
                            float single2 = imageAspect / currentAspect;
                            outScreenRect = position;
                            outSourceRect = new FloatRect(0f, (1f - single2) * 0.5f, 1f, single2);
                            flag = true;
                        }
                        break;
                    }
                case ScaleMode.ScaleToFit:
                    {
                        if (currentAspect <= imageAspect)
                        {
                            float single3 = currentAspect / imageAspect;
                            outScreenRect = new FloatRect(position.xMin, position.yMin + position.height * (1f - single3) * 0.5f, position.width, single3 * position.height);
                            outSourceRect = new FloatRect(0f, 0f, 1f, 1f);
                            flag = true;
                        }
                        else
                        {
                            float single4 = imageAspect / currentAspect;
                            outScreenRect = new FloatRect(position.xMin + position.width * (1f - single4) * 0.5f, position.yMin, single4 * position.width, position.height);
                            outSourceRect = new FloatRect(0f, 0f, 1f, 1f);
                            flag = true;
                        }
                        break;
                    }
            }
            return flag;
        }


        //------------------------------
        // Protected Methods
        //------------------------------

        //protected bool CanDraw()
        //{
        //    return Target != null && ActiveBrush != null;
        //}

        protected void UpdateInputState(NativeEvent evt)
        {
            if (evt.type == NativeEventType.ScrollWheel)
            {
                isInCanvas = IsInCanvas(evt.mousePosition);

                if (isInCanvas || !ConstrainMouseEvents)
                {
                    ThrowMouseWheel(MouseCanvasPosition, MouseCanvasPositionNormalized, evt.delta, evt.modifiers);
                }
            }

            if(evt.isKey)
            {
                ThrowKeyPressed(evt.keyCode, evt.modifiers);
            }

            if (evt.isMouse)
            {
                MouseWindowPosition = evt.mousePosition;

                //Float2 mousePosition = evt.mousePosition;

                //if(ClampCursor && isMouseDown)
                //{
                //    //mousePosition = RepeatCursor(mousePosition);
                //    RepeatCursor(MouseWindowPosition);
                //}

                //Float2 canvasPosition = GetRelMousePosition(mousePosition);
                //Float2 canvasPositionNormalized = NormalizePosition(canvasPosition);

                //isInCanvas = IsInCanvas(mousePosition);



                //Float2 canvasPosition = Float2.zero;
                //Float2 canvasPositionNormalized = Float2.zero;




                bool lastMouseDown = isMouseDown;


                #region --- Mouse Down ---
                if (evt.type == NativeEventType.MouseDown && evt.button == 0)
                {
                    AdditiveMouseWindowPosition = evt.mousePosition;

                    UpdateCanvasPositions(evt);

                    //isInCanvas = IsInCanvas(AdditiveMouseWindowPosition);
                    isInCanvas = IsInCanvas(evt.mousePosition);

                    isMouseDown = isInCanvas;

                    if(isMouseDown)
                    {
                        ThrowMouseDown(MouseCanvasPosition, MouseCanvasPositionNormalized, evt.modifiers);

                        lastCanvasPosition = MouseCanvasPosition;
                        lastCanvasPositionNormalized = MouseCanvasPositionNormalized;

                    }
                }
                else
                {
                    //UpdateCanvasPositions();

                    //isInCanvas = IsInCanvas(AdditiveMouseWindowPosition);
                    isInCanvas = IsInCanvas(evt.mousePosition);
                }
                #endregion

                #region --- Mouse Up ---
                if (evt.type == NativeEventType.MouseUp && evt.button == 0)
                {
                    if (isMouseDown && !isMouseDragging && isInCanvas)
                    {
                        ThrowMouseClicked(MouseCanvasPosition, MouseCanvasPositionNormalized,evt.modifiers);
                    }
                    
                    if(isMouseDown)
                    {
                        ThrowMouseUp(MouseCanvasPosition, MouseCanvasPositionNormalized, evt.modifiers);
                    }

                    isMouseDown = false;
                    isMouseDragging = false;
                }
                #endregion

                #region --- Mouse Move ---
                if (evt.type == NativeEventType.MouseMove)
                {
                    //NEW: 26.05.2016
                    //if (isMouseDown && !isMouseDragging && isInCanvas)
                    //{
                    //    ThrowMouseClicked(MouseCanvasPosition, MouseCanvasPositionNormalized, evt.modifiers);
                    //}
                    UpdateCanvasPositions(evt);

                }
                #endregion


                #region --- Mouse Drag ---

                if (evt.type == NativeEventType.MouseDrag)
                {
                    Float2 delta = evt.delta;
                    Float2 deltaNormalized = evt.delta;
                    deltaNormalized.x /= width;
                    deltaNormalized.y /= height;
                    //Debug.Log("REPEATED: " + delta);


                    if (cursorWasRepeated)
                    {
                        delta = lastCanvasPosition - MouseCanvasPosition;

                        //Debug.Log("REPEATED: " + delta);
                    }

                    AdditiveMouseWindowPosition += delta;

                    //CorrectPositions(evt);

                    UpdateCanvasPositions(evt);

                    if (!ConstrainDraggedEvent || (ConstrainDraggedEvent && CursorInsideCanvas))
                    {
                        MouseButton btn = evt.button < 3 ? (MouseButton)evt.button : MouseButton.Other;

                        ThrowMouseDragged(delta, deltaNormalized, MouseCanvasPosition, MouseCanvasPositionNormalized, evt.modifiers, btn);
                    }



                    lastCanvasPosition = MouseCanvasPosition;
                    lastCanvasPositionNormalized = MouseCanvasPositionNormalized;

                    isMouseDragging = isMouseDown;
                    cursorWasRepeated = false;
                    //Debug.Log("isMouseDragging: " + isMouseDragging);
                }
                #endregion

                #region --- Mouse Move ---

                if (evt.type == NativeEventType.MouseMove)
                {
                    ThrowMouseMove(MouseCanvasPosition, MouseCanvasPositionNormalized, evt.modifiers);

                //    if (selection.HasSelection && AllowSelectionChange)
                //    {
                //        float border = 5;

                //        foreach (FloatRect sel in selection)
                //        {
                //            //FloatRect rect = InverseTransformRectNorm(sel, true);
                //            FloatRect rect = InverseTransformRectNorm(sel, prev,true);
                //            FloatRect rectBorder = rect;
                //            rect.xMin += border;
                //            rect.yMin += border;
                //            rect.xMax -= border;
                //            rect.yMax -= border;

                //            if (sel.Contains(MouseCanvasPositionNormalized))
                //            {
                //                Debug.Log("inside");
                //                //EditorGUIUtility.AddCursorRect(rect, MouseCursor.ArrowPlus);
                //            }
                //        }
                //    }
                }
                
                #endregion

                if (ClampCursor && isMouseDown)
                {
                    //mousePosition = RepeatCursor(mousePosition);
                    RepeatCursor(evt.mousePosition);
                }

                if (lastMouseDown != isMouseDown || isMouseDragging)
                {
                    textureDidChange = true;

                    //Debug.Log("textureDidChange : " + (lastMouseDown != isMouseDown) + " : " + isMouseDragging);

                }
            }
        }

        /// <summary>
        /// Clamps a mouse position in window coordinates to the canvas space.
        /// </summary>
        /// <param name="mousePosition">The mouse position in window coordinates.</param>
        /// <returns>The clamped position.</returns>
        protected Float2 RepeatCursor(Float2 mousePosition)
        {
            Float2 pos = mousePosition;

            int maxX = (int)(CanvasPosition.x + width);
            int minX = (int)(CanvasPosition.x);

            int maxY = (int)(CanvasPosition.y + height);
            int minY = (int)(CanvasPosition.y);


            int x, y;
            bool clampedX = Clamp((int)mousePosition.x, minX, maxX, out x);
            bool clampedY = Clamp((int)mousePosition.y, minY, maxY, out y);

            if (clampedX || clampedY)
            {
                cursorWasRepeated = true;

                //Debug.Log("Clamped: from " + mousePosition + " to " + pos);

                int displayX = (int)window.Position.x + x;
                int displayY = (int)window.Position.y + y;

                Mouse.SetPosition(displayX, displayY);

                pos = new Float2(x, y);

            }



            return pos;
        }

        protected bool Clamp(int value, int min, int max, out int result)
        {
            //Debug.Log("Clamp: min " + min + " max " + max + " val " + value);
            result = value;

            //try
            //{                
                if (value < min || value > max)
                {
                    if (Mode == SampleMode.Clamp)
                    {

                        result = Mathf.Clamp(value, min, max);


                    }
                    else if (Mode == SampleMode.Tileable)
                    {
                        if (value < min)
                        {
                            result = max - (Math.Abs(value) % max);
                        }
                        else
                        {
                            result = min + (value % max);
                        }
                    }

                    return true;
                }

            //}
            //catch (OverflowException)
            //{
            //    Debug.Log("Unable to calculate the absolute value of "+value);
            //}

            return false;
        }

        /// <summary>
        /// Checks whether the specified position in located in the canvas.
        /// </summary>
        /// <param name="position">Mouse position within the window.</param>
        /// <returns>returns <c>true</c> the specified position is inside the current canvas, otherwise <c>false</c>.</returns>
        protected bool IsInCanvas(Float2 position)
        {
            // Top left corner of canvas
            //Float2 canvasPosition = new Float2(margin.left, margin.top);

            FloatRect r = new FloatRect(CanvasPosition.x, CanvasPosition.y, width, height);

            return r.Contains(position);
        }

        /// <summary>
        /// Returns the mouse position relative to the drawing canvas (bitmap).
        /// Origin (0/0) is bottom-left corner.
        /// </summary>
        /// <param name="position">Mouse position within the window.</param>
        /// <returns></returns>
        protected Float2 GetRelMousePosition(Float2 position)
        {
            Float2 relativeCanvasPos = position - new Float2(margin.left, margin.top);
            relativeCanvasPos.y = height - relativeCanvasPos.y;

            return relativeCanvasPos;
        }

        /// <summary>
        /// Returns the mouse position absolute to display.
        /// </summary>
        /// <param name="position">Mouse position within the window.</param>
        /// <returns></returns>
        protected Float2 GetAbsMousePosition(Float2 position)
        {
            Float2 absolutePos = position + window.Position.position;
            return absolutePos;
        }

        /// <summary>
        /// Normalizes the specified rect in canvas space.
        /// </summary>
        /// <param name="rect">Absolute rect within the canvas.</param>
        /// <returns>The normalized rect.</returns>
        public Float2 NormalizePosition(Float2 point)
        {
            Float2 p = new Float2(point.x / width, point.y / height);
            return p;
        }

        /// <summary>
        /// Denormalizes the specified rect to match the absolute canvas space.
        /// </summary>
        /// <param name="rect">Normalized rect within the canvas.</param>
        /// <returns>The absolute rect.</returns>
        public Float2 DenormalizePosition(Float2 point)
        {
            Float2 p = new Float2(point.x * width, point.y * height);
            return p;
        }

        /// <summary>
        /// Normalizes the specified rectangle in canvas space.
        /// </summary>
        /// <param name="rect">Absolute rect within the canvas.</param>
        /// <returns>The normalized rect.</returns>
        public FloatRect NormalizeRect(FloatRect rect)
        {
            FloatRect p = rect;
            p.position = NormalizePosition(rect.position);
            p.width /= width;
            p.height /= height;

            return p;
        }

        /// <summary>
        /// Denormalizes the specified rectangle to match the absolute canvas space.
        /// </summary>
        /// <param name="rect">Normalized rect within the canvas.</param>
        /// <returns>The absolute rect.</returns>
        public FloatRect DenormalizeRect(FloatRect rect)
        {
            FloatRect p = rect;
            p.position = DenormalizePosition(rect.position);
            p.width *= width;
            p.height *= height;

            return p;
        }

        private void UpdateCanvasPositions(NativeEvent evt)
        {
            //MouseCanvasPosition = GetRelMousePosition(AdditiveMouseWindowPosition);
            MouseCanvasPosition = GetRelMousePosition(evt.mousePosition);
            MouseCanvasPositionNormalized = NormalizePosition(MouseCanvasPosition);
        }

        private void CorrectPositions(NativeEvent evt)
        {
            if (!cursorWasRepeated) { return; }

            Float2 ab = AdditiveMouseWindowPosition - evt.mousePosition;

            if(ab.sqrMagnitude > 1)
            {
                AdditiveMouseWindowPosition = evt.mousePosition;
            }
        }

        private void UpdateSize()
        {
            width = (int)(window.Position.width - margin.horizontal);
            height = (int)(window.Position.height - margin.vertical);
        }

        //------------------------------
        // Event Methods
        //------------------------------

        private void ThrowMouseMove(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            ThrowMouseEvent(OnMouseMove, position, positionNormalized, modifier);
        }

        private void ThrowMouseClicked(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            ThrowMouseEvent(OnMouseClicked, position, positionNormalized, modifier);
        }

        private void ThrowMouseUp(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            ThrowMouseEvent(OnMouseUp, position, positionNormalized, modifier);
        }

        private void ThrowMouseDown(Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            ThrowMouseEvent(OnMouseDown, position, positionNormalized, modifier);
        }

        private void ThrowMouseEvent(MouseEventDelegte del, Float2 position, Float2 positionNormalized, NativeEventModifiers modifier)
        {
            if(del != null)
            {
                del(position, positionNormalized, modifier);
            }
        }

        private void ThrowMouseDragged(Float2 delta, Float2 deltaNormalized, Float2 position, 
            Float2 positionNormalized, NativeEventModifiers modifier, MouseButton mouseButton)
        {
            if (OnMouseDragged != null)
            {
                OnMouseDragged(delta, deltaNormalized, position, positionNormalized, modifier, mouseButton);
            }
        }

        private void ThrowCanvasSizeChanged(Float2 sizeOld, Float2 sizeNew)
        {
            if (OnSizeChanged != null)
            {
                OnSizeChanged(sizeOld, sizeNew);
            }
        }

        private void ThrowKeyPressed(NativeKeyCode key, NativeEventModifiers modifier)
        {
            if (OnKeyPressed != null)
            {
                OnKeyPressed(key, modifier);
            }
        }

        private void ThrowMouseWheel(Float2 position, Float2 positionNormalized, Float2 delta, NativeEventModifiers modifier)
        {
            if (OnMouseWheel != null)
            {
                OnMouseWheel(position, positionNormalized, delta, modifier);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

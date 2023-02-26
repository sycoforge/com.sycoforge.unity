
using ch.sycoforge.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public static class EditorGUILayoutExt
    {
        private static Color s_MixedValueContentColorTemp;
        private static Color s_MixedValueContentColor;


        internal delegate UnityEngine.Object ObjectFieldValidator(UnityEngine.Object[] references, Type objType, SerializedProperty property);
        internal enum ObjectFieldVisualType
        {
            IconAndText,
            LargePreview,
            MiniPreivew
        }

        public static bool LinkLabel(string text, string url, GUIStyle style)
        {
            bool pressed = false;
            if (GUILayout.Button(text, style))
            {
                Application.OpenURL(url);

                pressed = true;
            }
            Rect r = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);

            return pressed;
        }

        public static string LayerMaskField(string label, string layer)
        {
            int layerSelection = 0;
            List<string> optionsList = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                // get layer names
                string name = LayerMask.LayerToName(i);

                if (!string.IsNullOrEmpty(name))
                {
                    optionsList.Add(name);
                }
            }

            for (int i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i] == layer)
                {
                    layerSelection = i;
                }

                //if (i == layer)
                //{
                //    layerSelection = i;

                //    break;
                //}
            }

            string[] options = optionsList.ToArray();

            //flare.Layer = EditorGUILayout.MaskField("Layer Mask", flare.Layer, optionsList.ToArray());
            layerSelection = EditorGUILayout.Popup(label, layerSelection, optionsList.ToArray());

            //return LayerMask.NameToLayer(options[layerSelection]);
            return options[layerSelection];
        }

        internal static UnityEngine.Object ObjectField(Rect position, UnityEngine.Object obj, Type objType, bool allowSceneObjects, Texture icon)
        {
            

#if UNITY_5_3
            int controlID = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), EditorGUIUtility.native, position);
#else
            int controlID = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), FocusType.Passive, position);
#endif
            return DoObjectField(EditorGUI.IndentedRect(position), EditorGUI.IndentedRect(position), controlID, obj, objType, null, null, allowSceneObjects, icon);
        }

        internal static UnityEngine.Object ObjectField(Rect position, GUIContent label, UnityEngine.Object obj, Type objType, bool allowSceneObjects, Texture icon)
        {
#if UNITY_5_3
            int controlID = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), EditorGUIUtility.native, position);
#else
            int controlID = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), FocusType.Passive, position);
#endif
            //int controlID = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), EditorGUIUtility.native, position);
            //int controlID = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);

            position = EditorGUI.PrefixLabel(position, controlID, label);
            if (EditorGUIUtility.HasObjectThumbnail(objType) && position.height > 16f)
            {
                float single = Mathf.Min(position.width, position.height);
                position.height = single;
                position.xMin = position.xMax - single;
            }
            return DoObjectField(position, position, controlID, obj, objType, null, null, allowSceneObjects, icon);
        }

        public static UnityEngine.Object ObjectField(GUIContent label, UnityEngine.Object obj, Type objType, bool allowSceneObjects, Texture icon, params GUILayoutOption[] options)
        {
            float single;
            single = (!EditorGUIUtility.HasObjectThumbnail(objType) ? 16f : 64f);
            Rect controlRect = EditorGUILayout.GetControlRect(true, single, options);

            Caller.SetStaticField(typeof(EditorGUILayout), "s_LastRect", controlRect);

            return ObjectField(controlRect, label, obj, objType, allowSceneObjects, icon);
        }

        public static UnityEngine.Object ObjectField(GUIContent label, UnityEngine.Object obj, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            float single;
            single = (!EditorGUIUtility.HasObjectThumbnail(objType) ? 16f : 64f);
            Rect controlRect = EditorGUILayout.GetControlRect(true, single, options);

            Caller.SetStaticField(typeof(EditorGUILayout), "s_LastRect", controlRect);

            return ObjectField(controlRect, label, obj, objType, allowSceneObjects, null);
        }

        internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, Type objType, SerializedProperty property, ObjectFieldValidator validator, bool allowSceneObjects, Texture icon)
        {
            return DoObjectField(position, dropRect, id, obj, objType, property, validator, allowSceneObjects, EditorStyles.objectField, icon);
        }

        internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, Type objType, SerializedProperty property, ObjectFieldValidator validator, bool allowSceneObjects, GUIStyle style, Texture icon)
        {
            Rect rect;
            GUIContent sMixedValueContent = null;
            if (validator == null)
            {
                validator = new ObjectFieldValidator(ValidateObjectFieldAssignment);
            }
            Event evt = Event.current;
            EventType eventType = evt.type;
            if (!GUI.enabled && Event.current.rawType == EventType.MouseDown)
            {
                eventType = Event.current.rawType;
            }
            bool flag = EditorGUIUtility.HasObjectThumbnail(objType);
            ObjectFieldVisualType objectFieldVisualType = ObjectFieldVisualType.IconAndText;
            if (flag && position.height <= 18f && position.width <= 32f)
            {
                objectFieldVisualType = ObjectFieldVisualType.MiniPreivew;
            }
            else if (flag && position.height > 16f)
            {
                objectFieldVisualType = ObjectFieldVisualType.LargePreview;
            }
            Vector2 iconSize = EditorGUIUtility.GetIconSize();
            if (objectFieldVisualType == ObjectFieldVisualType.IconAndText)
            {
                EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));
            }
            EventType eventType1 = eventType;
            switch (eventType1)
            {
                case EventType.KeyDown:
                    {
                        if (GUIUtility.keyboardControl == id)
                        {
                            if (evt.keyCode == KeyCode.Backspace || evt.keyCode == KeyCode.Delete)
                            {
                                if (property == null)
                                {
                                    obj = null;
                                }
                                else
                                {
                                    property.objectReferenceValue = null;
                                }
                                GUI.changed = true;
                                evt.Use();
                            }
                            //if (evt.MainActionKeyForControl(id))
                            if (Caller.Call<bool>(evt, "MainActionKeyForControl", BindingFlags.NonPublic, id))
                            {
                                Type[] types = new Type[]{ typeof(UnityEngine.Object), typeof(Type), typeof(SerializedProperty), typeof(bool) };

                                Type type = FindType("ObjectSelector");
                                object o = Caller.GetStaticProperty(type, "get", BindingFlags.Public | BindingFlags.Static);
                                Caller.Call(o, "Show", BindingFlags.Public, types, obj, objType, property, allowSceneObjects);
                                Caller.SetField(o, "objectSelectorID", id, BindingFlags.NonPublic);


                                evt.Use();
                                GUIUtility.ExitGUI();
                            }
                        }
                        break;
                    }
                case EventType.Repaint:
                    {
                        if (EditorGUI.showMixedValue)
                        {
                            sMixedValueContent = Caller.GetStaticField<GUIContent>(typeof(EditorGUI), "s_MixedValueContent");
                            //sMixedValueContent = EditorGUI.s_MixedValueContent;
                        }
                        else if (property == null)
                        {
                            sMixedValueContent = EditorGUIUtility.ObjectContent(obj, objType);
                        }
                        //else
                        //{
                        //    string v = Caller.GetField<string>(property, "objectReferenceStringValue");
                        //    //UnityEngine.Object o = Caller.GetField<UnityEngine.Object>(property, "objectReferenceStringValue");
                        //    GUIContent color = Caller.StaticCall<GUIContent>(typeof(EditorGUIUtility), "TempContent", BindingFlags.Static | BindingFlags.NonPublic, v, AssetPreview.GetMiniThumbnail(property.objectReferenceValue));

                        //    sMixedValueContent = color;
                        //    //sMixedValueContent = EditorGUIUtility.TempContent(property.objectReferenceStringValue, AssetPreview.GetMiniThumbnail(property.objectReferenceValue));
                            
                        //    obj = property.objectReferenceValue;
                        //    if (obj != null)
                        //    {
                        //        if (validator(new UnityEngine.Object[] { obj }, objType, property) == null)
                        //        {
                        //            GUIContent c2 = Caller.StaticCall<GUIContent>(typeof(EditorGUIUtility), "TempContent", BindingFlags.Static | BindingFlags.NonPublic, "Type mismatch");

                        //            sMixedValueContent = c2;
                        //            //sMixedValueContent = EditorGUIUtility.TempContent("Type mismatch");
                        //        }
                        //    }
                        //}
                        switch (objectFieldVisualType)
                        {
                            case ObjectFieldVisualType.IconAndText:
                                {
                                   //BeginHandleMixedValueContentColor();
                                    Caller.StaticCall(typeof(EditorGUI), "BeginHandleMixedValueContentColor", BindingFlags.NonPublic);

                                    style.Draw(position, sMixedValueContent, id, DragAndDrop.activeControlID == id);
                                    Caller.StaticCall(typeof(EditorGUI), "EndHandleMixedValueContentColor", BindingFlags.NonPublic);

                                    //EndHandleMixedValueContentColor();
                                    break;
                                }
                            case ObjectFieldVisualType.LargePreview:
                                {
                                    DrawObjectFieldLargeThumb(position, id, obj, sMixedValueContent, icon);
                                    break;
                                }
                            case ObjectFieldVisualType.MiniPreivew:
                                {
                                    DrawObjectFieldMiniThumb(position, id, obj, sMixedValueContent);
                                    break;
                                }
                            default:
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                        }
                        break;
                    }
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    {
                        if (dropRect.Contains(Event.current.mousePosition) && GUI.enabled)
                        {
                            UnityEngine.Object obj1 = validator(DragAndDrop.objectReferences, objType, property);
                            if (obj1 != null && !allowSceneObjects && !EditorUtility.IsPersistent(obj1))
                            {
                                obj1 = null;
                            }
                            if (obj1 != null)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                                if (eventType != EventType.DragPerform)
                                {
                                    DragAndDrop.activeControlID = id;
                                }
                                else
                                {
                                    if (property == null)
                                    {
                                        obj = obj1;
                                    }
                                    else
                                    {
                                        property.objectReferenceValue = obj1;
                                    }
                                    GUI.changed = true;
                                    DragAndDrop.AcceptDrag();
                                    DragAndDrop.activeControlID = 0;
                                }
                                Event.current.Use();
                            }
                        }
                        break;
                    }
                case EventType.ExecuteCommand:
                    {
                        Type type = FindType("ObjectSelector");
                        object o = Caller.GetStaticProperty(type, "get", BindingFlags.Public | BindingFlags.Static);

                        int objSelID = Caller.GetField<int>(o, "objectSelectorID", BindingFlags.NonPublic);

                        //if (evt.commandName == "ObjectSelectorUpdated" && ObjectSelector.@get.objectSelectorID == id && GUIUtility.keyboardControl == id)
                        if (evt.commandName == "ObjectSelectorUpdated" && objSelID== id && GUIUtility.keyboardControl == id)
                        {
                            UnityEngine.Object ob = Caller.StaticCall<UnityEngine.Object>(type, "GetCurrentObject", BindingFlags.Public);

                            UnityEngine.Object[] objs = new UnityEngine.Object[] 
                            { 
                                ob
                            };
                            UnityEngine.Object obj2 = validator(objs, objType, property);

                            //UnityEngine.Object obj2 = validator(new UnityEngine.Object[] { ObjectSelector.GetCurrentObject() }, objType, property);
                            if (property != null)
                            {
                                property.objectReferenceValue = obj2;
                            }
                            GUI.changed = true;
                            evt.Use();
                            return obj2;
                        }
                        break;
                    }
                case EventType.DragExited:
                    {
                        if (GUI.enabled)
                        {
                            HandleUtility.Repaint();
                        }
                        break;
                    }
                default:
                    {
                        if (eventType1 != EventType.MouseDown)
                        {
                            break;
                        }
                        else if (Event.current.button == 0)
                        {
                            if (position.Contains(Event.current.mousePosition))
                            {
                                switch (objectFieldVisualType)
                                {
                                    case ObjectFieldVisualType.IconAndText:
                                        {
                                            rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
                                            break;
                                        }
                                    case ObjectFieldVisualType.LargePreview:
                                        {
                                            rect = new Rect(position.xMax - 32f, position.yMax - 14f, 32f, 14f);
                                            break;
                                        }
                                    case ObjectFieldVisualType.MiniPreivew:
                                        {
                                            rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
                                            break;
                                        }
                                    default:
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }
                                }
                                EditorGUIUtility.editingTextField = false;
                                if (!rect.Contains(Event.current.mousePosition))
                                {
                                    UnityEngine.Object obj3 = (property == null ? obj : property.objectReferenceValue);
                                    Component component = obj3 as Component;
                                    if (component)
                                    {
                                        obj3 = component.gameObject;
                                    }
                                    if (EditorGUI.showMixedValue)
                                    {
                                        obj3 = null;
                                    }
                                    if (Event.current.clickCount == 1)
                                    {
                                        GUIUtility.keyboardControl = id;
                                        if (obj3)
                                        {
                                            bool flag1 = (evt.shift ? true : evt.control);
                                            if (!flag1)
                                            {
                                                EditorGUIUtility.PingObject(obj3);
                                            }
                                            if (flag1 && obj3 is Texture)
                                            {
                                                PopupWindowWithoutFocus.Show((new RectOffset(6, 3, 0, 3)).Add(position), new ObjectPreviewPopup(obj3), new PopupLocationHelper.PopupLocation[] { PopupLocationHelper.PopupLocation.Left, PopupLocationHelper.PopupLocation.Below, PopupLocationHelper.PopupLocation.Right });
                                            }
                                        }
                                        evt.Use();
                                    }
                                    else if (Event.current.clickCount == 2)
                                    {
                                        if (obj3)
                                        {
                                            AssetDatabase.OpenAsset(obj3);
                                            GUIUtility.ExitGUI();
                                        }
                                        evt.Use();
                                    }
                                }
                                else if (GUI.enabled)
                                {
                                    GUIUtility.keyboardControl = id;


                                    Type[] types = new Type[] { typeof(UnityEngine.Object), typeof(Type), typeof(SerializedProperty), typeof(bool) };
                                    Type type = FindType("ObjectSelector");
                                    object o = Caller.GetStaticProperty(type, "get", BindingFlags.Public | BindingFlags.Static);
                                    Caller.Call(o, "Show", BindingFlags.Public, types, obj, objType, property, allowSceneObjects);
                                    Caller.SetField(o, "objectSelectorID", id, BindingFlags.NonPublic);

                                    
                                    evt.Use();
                                    GUIUtility.ExitGUI();
                                }
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
            }
            EditorGUIUtility.SetIconSize(iconSize);
            return obj;
            throw new ArgumentOutOfRangeException();
        }


        internal static UnityEngine.Object ValidateObjectFieldAssignment(UnityEngine.Object[] references, Type objType, SerializedProperty property)
        {
            if ((int)references.Length > 0)
            {
                bool length = (int)DragAndDrop.objectReferences.Length > 0;
                bool flag = (references[0] == null ? false : references[0].GetType() == typeof(Texture2D));

                if (property == null)
                {
                    if (references[0] != null && references[0].GetType() == typeof(GameObject) && typeof(Component).IsAssignableFrom(objType))
                    {
                        references = ((GameObject)references[0]).GetComponents(typeof(Component));
                    }
                    UnityEngine.Object[] objArray = references;
                    for (int i = 0; i < (int)objArray.Length; i++)
                    {
                        UnityEngine.Object obj = objArray[i];
                        if (obj != null && objType.IsAssignableFrom(obj.GetType()))
                        {
                            return obj;
                        }
                    }
                }
                else
                {
                    bool valid = Caller.Call<bool>(property, "ValidateObjectReferenceValue", BindingFlags.NonPublic, references[0]);
                    if (references[0] != null && valid)
                    //if (references[0] != null && property.ValidateObjectReferenceValue(references[0]))
                    {
                        return references[0];
                    }

                }
            }
            return null;
        }


        private static Type FindType(string name)
        {
            Assembly assembly = typeof(EditorWindow).Assembly;

            Type type = new List<Type>(assembly.GetTypes()).Find(t => t.ToString().EndsWith(name));

            return type;
        }


        private static void DrawObjectFieldLargeThumb(Rect position, int id, UnityEngine.Object obj, GUIContent content, Texture icon)
        {
            GUIStyle gUIStyle = EditorStyles.objectFieldThumb;
            gUIStyle.Draw(position, GUIContent.none, id, DragAndDrop.activeControlID == id);
            if (!(obj != null) || EditorGUI.showMixedValue)
            {
                GUIStyle gUIStyle1 = string.Concat(gUIStyle.name, "Overlay");

                //BeginHandleMixedValueContentColor();
                Caller.StaticCall(typeof(EditorGUI), "BeginHandleMixedValueContentColor", BindingFlags.NonPublic);

                gUIStyle1.Draw(position, content, id);

                //EndHandleMixedValueContentColor();
                Caller.StaticCall(typeof(EditorGUI), "EndHandleMixedValueContentColor", BindingFlags.NonPublic);

            }
            else
            {
                bool flag = obj is Cubemap;
                Rect rect = gUIStyle.padding.Remove(position);
                if (!flag)
                {
                    Texture2D texture2D = content.image as Texture2D;
                    if (!(texture2D != null) || !texture2D.alphaIsTransparency)
                    {
                        EditorGUI.DrawPreviewTexture(rect, content.image);
                    }
                    else
                    {
                        EditorGUI.DrawTextureTransparent(rect, texture2D);
                    }
                }
                else
                {
                    rect.x = rect.x + (rect.width - (float)content.image.width) / 2f;
                    rect.y = rect.y + (rect.height - (float)content.image.width) / 2f;
                    GUIStyle.none.Draw(rect, content.image, false, false, false, false);
                }
            }

            //GUIStyle gUIStyle2 = string.Concat(gUIStyle.name, "Overlay2");

            GUIStyle gUIStyle2 = "Box";
            gUIStyle2.alignment = TextAnchor.LowerRight;
            gUIStyle2.padding = new RectOffset(2, 2, 2, 2);
            gUIStyle2.margin = new RectOffset();


            //GUIContent color = Caller.StaticCall<GUIContent>(typeof(EditorGUIUtility), "TempContent", BindingFlags.NonPublic, " [+] ");

            GUIContent c;
            if(icon != null)
            {
                c = Caller.StaticCall<GUIContent>(typeof(EditorGUIUtility), "TempContent", BindingFlags.NonPublic, icon);
            }
            else
            {
                c = Caller.StaticCall<GUIContent>(typeof(EditorGUIUtility), "TempContent", BindingFlags.NonPublic, "Open");
            }

            //gUIStyle2.Draw(position, color, id);
            gUIStyle2.Draw(position, new GUIContent("", icon, "Add new"), id);


           // gUIStyle2.Draw(position, EditorGUIUtility.TempContent("Select"), id);
        }

        private static void DrawObjectFieldMiniThumb(Rect position, int id, UnityEngine.Object obj, GUIContent content)
        {
            GUIStyle gUIStyle = GUI.skin.FindStyle("ObjectFieldMiniThumb") ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("ObjectFieldMiniThumb");
            //GUIStyle gUIStyle = EditorStyles.objectFieldMiniThumb;
            position.width = 32f;

            //BeginHandleMixedValueContentColor();
            Caller.StaticCall(typeof(EditorGUI), "BeginHandleMixedValueContentColor", BindingFlags.NonPublic);

            bool flag = obj != null;
            bool flag1 = DragAndDrop.activeControlID == id;
            bool flag2 = GUIUtility.keyboardControl == id;
            gUIStyle.Draw(position, flag, false, flag1, flag2);

            //EndHandleMixedValueContentColor();
            Caller.StaticCall(typeof(EditorGUI), "EndHandleMixedValueContentColor", BindingFlags.NonPublic);


            if (obj != null && !EditorGUI.showMixedValue)
            {
                Rect rect = new Rect(position.x + 1f, position.y + 1f, position.height - 2f, position.height - 2f);
                Texture2D texture2D = content.image as Texture2D;
                if (!(texture2D != null) || !texture2D.alphaIsTransparency)
                {
                    EditorGUI.DrawPreviewTexture(rect, content.image);
                }
                else
                {
                    EditorGUI.DrawTextureTransparent(rect, texture2D);
                }
                if (rect.Contains(Event.current.mousePosition))
                {
                    GUIContent c = Caller.StaticCall<GUIContent>(typeof(GUIContent), "Temp", BindingFlags.NonPublic, string.Empty, "Ctrl + Click to show preview");
                    GUI.Label(rect, c);
                    //GUI.Label(screenRect, GUIContent.Temp(string.Empty, "Ctrl + Click v2 show preview"));
                }
            }
        }


        //internal static void BeginHandleMixedValueContentColor()
        //{
        //    s_MixedValueContentColor = GUI.contentColor;
        //    GUI.contentColor = (!EditorGUI.showMixedValue ? GUI.contentColor : GUI.contentColor * s_MixedValueContentColorTemp);
        //}

        //internal static void EndHandleMixedValueContentColor()
        //{
        //    GUI.contentColor = s_MixedValueContentColorTemp;
        //}
    }

    internal class ObjectPreviewPopup : PopupWindowContent
    {
        private const float kToolbarHeight = 17f;

        private readonly UnityEditor.Editor m_Editor;

        private readonly GUIContent m_ObjectName;

        private ObjectPreviewPopup.Styles s_Styles;

        public ObjectPreviewPopup(UnityEngine.Object previewObject)
        {
            if (previewObject == null)
            {
                Debug.LogError("ObjectPreviewPopup: Check object is not null, before trying to show it!");
                return;
            }
            this.m_ObjectName = new GUIContent(previewObject.name, AssetDatabase.GetAssetPath(previewObject));
            this.m_Editor = UnityEditor.Editor.CreateEditor(previewObject);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(300f, 317f);
        }

        public override void OnClose()
        {
            if (this.m_Editor != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_Editor);
            }
        }

        public override void OnGUI(Rect rect)
        {
            if (this.m_Editor == null)
            {
                base.editorWindow.Close();
                return;
            }
            if (this.s_Styles == null)
            {
                this.s_Styles = new ObjectPreviewPopup.Styles();
            }
            GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width, 17f), this.s_Styles.toolbar);
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            this.m_Editor.OnPreviewSettings();
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUI.Label(new Rect(rect.x + 5f, rect.y, rect.width - 140f, 17f), this.m_ObjectName, this.s_Styles.toolbarText);
            Rect rect1 = new Rect(rect.x, rect.y + 17f, rect.width, rect.height - 17f);
            this.m_Editor.OnPreviewGUI(rect1, this.s_Styles.background);
        }

        internal class Styles
        {
            public readonly GUIStyle toolbar = "preToolbar";

            public readonly GUIStyle toolbarText = "preToolbar2";

            public GUIStyle background = "preBackground";

            public Styles()
            {
            }
        }
    }

    internal class PopupWindowWithoutFocus : EditorWindow
    {
        private static PopupWindowWithoutFocus s_PopupWindowWithoutFocus;

        private static double s_LastClosedTime;

        private static Rect s_LastActivatorRect;

        private PopupWindowContent m_WindowContent;

        private PopupLocationHelper.PopupLocation[] m_LocationPriorityOrder;

        private Vector2 m_LastWantedSize = Vector2.zero;

        private Rect m_ActivatorRect;

        private float m_BorderWidth = 1f;

        private PopupWindowWithoutFocus()
        {
            base.hideFlags = HideFlags.DontSave;
        }

        private void FitWindowToContent()
        {
            Vector2 windowSize = this.m_WindowContent.GetWindowSize();
            if (this.m_LastWantedSize != windowSize)
            {
                this.m_LastWantedSize = windowSize;
                Vector2 vector2 = windowSize + new Vector2(2f * this.m_BorderWidth, 2f * this.m_BorderWidth);
                Rect dropDownRect = PopupLocationHelper.GetDropDownRect(this.m_ActivatorRect, vector2, vector2, this.m_LocationPriorityOrder);
                //this.m_Pos = dropDownRect;

                Caller.SetField(this, "m_Pos", dropDownRect);

                Vector2 vector21 = new Vector2(dropDownRect.width, dropDownRect.height);
                base.maxSize = vector21;
                base.minSize = vector21;
            }
        }

        public static void Hide()
        {
            if (PopupWindowWithoutFocus.s_PopupWindowWithoutFocus != null)
            {
                PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
            }
        }

        private void Init(Rect activatorRect, PopupWindowContent windowContent, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
        {
            this.m_WindowContent = windowContent;
            //this.m_WindowContent.editorWindow = this;

            Caller.SetProperty(this.m_WindowContent, "editorWindow", this);

            this.m_ActivatorRect = Caller.StaticCall<Rect>(typeof(GUIUtility), "GUIToScreenRect", BindingFlags.NonPublic , activatorRect);
            //this.m_ActivatorRect = GUIUtility.GUIToScreenRect(activatorRect);
            this.m_LastWantedSize = windowContent.GetWindowSize();
            this.m_LocationPriorityOrder = locationPriorityOrder;
            Vector2 windowSize = windowContent.GetWindowSize() + new Vector2(this.m_BorderWidth * 2f, this.m_BorderWidth * 2f);
            base.position = PopupLocationHelper.GetDropDownRect(this.m_ActivatorRect, windowSize, windowSize, this.m_LocationPriorityOrder);
            base.ShowPopup();
            base.Repaint();
        }

        public static bool IsVisible()
        {
            return PopupWindowWithoutFocus.s_PopupWindowWithoutFocus != null;
        }

        private void OnDisable()
        {
            PopupWindowWithoutFocus.s_LastClosedTime = EditorApplication.timeSinceStartup;
            if (this.m_WindowContent != null)
            {
                this.m_WindowContent.OnClose();
            }
            PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = null;
        }

        private void OnEnable()
        {
            PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = this;
        }

        private static bool OnGlobalMouseOrKeyEvent(EventType type, KeyCode keyCode, Vector2 mousePosition)
        {
            if (PopupWindowWithoutFocus.s_PopupWindowWithoutFocus == null)
            {
                return false;
            }
            if (type == EventType.KeyDown && keyCode == KeyCode.Escape)
            {
                PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
                return true;
            }
            if (type != EventType.MouseDown || PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.position.Contains(mousePosition))
            {
                return false;
            }
            PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
            return true;
        }

        internal void OnGUI()
        {
            this.FitWindowToContent();
            float mBorderWidth = this.m_BorderWidth;
            float single = this.m_BorderWidth;
            Rect rect = base.position;
            float mBorderWidth1 = rect.width - 2f * this.m_BorderWidth;
            Rect rect1 = base.position;
            Rect rect2 = new Rect(mBorderWidth, single, mBorderWidth1, rect1.height - 2f * this.m_BorderWidth);
            this.m_WindowContent.OnGUI(rect2);
            float single1 = base.position.width;
            Rect rect3 = base.position;
            GUI.Label(new Rect(0f, 0f, single1, rect3.height), GUIContent.none, "grey_border");
        }

        private static bool ShouldShowWindow(Rect activatorRect)
        {
            if (EditorApplication.timeSinceStartup - PopupWindowWithoutFocus.s_LastClosedTime < 0.2 && !(activatorRect != PopupWindowWithoutFocus.s_LastActivatorRect))
            {
                return false;
            }
            PopupWindowWithoutFocus.s_LastActivatorRect = activatorRect;
            return true;
        }

        public static void Show(Rect activatorRect, PopupWindowContent windowContent)
        {
            PopupWindowWithoutFocus.Show(activatorRect, windowContent, null);
        }

        internal static void Show(Rect activatorRect, PopupWindowContent windowContent, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
        {
            if (PopupWindowWithoutFocus.ShouldShowWindow(activatorRect))
            {
                if (PopupWindowWithoutFocus.s_PopupWindowWithoutFocus == null)
                {
                    PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = ScriptableObject.CreateInstance<PopupWindowWithoutFocus>();
                }
                PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Init(activatorRect, windowContent, locationPriorityOrder);
            }
        }
    }

    internal static class PopupLocationHelper
    {
        private static float k_SpaceFromBottom
        {
            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    return 10f;
                }
                return 0f;
            }
        }

        private static Rect FitRect(Rect rect)//, ContainerWindow popupContainerWindow)
        {
            //if (!popupContainerWindow)
            //{
                return (Rect)Caller.StaticCall("ContainerWindow", "FitRectToScreen", rect, true, true);
                //return ContainerWindow.FitRectToScreen(screenRect, true, true);
            //}


            //return popupContainerWindow.FitWindowRectToScreen(screenRect, true, true);
        }

        public static Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize)
        {
            return PopupLocationHelper.GetDropDownRect(buttonRect, minSize, maxSize, null);
        }

        public static Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
        {
            Rect rect;
            if (locationPriorityOrder == null)
            {
                locationPriorityOrder = new PopupLocationHelper.PopupLocation[] { PopupLocationHelper.PopupLocation.Below, PopupLocationHelper.PopupLocation.Above };
            }
            List<Rect> rects = new List<Rect>();
            PopupLocationHelper.PopupLocation[] popupLocationArray = locationPriorityOrder;
            for (int i = 0; i < (int)popupLocationArray.Length; i++)
            {
                switch (popupLocationArray[i])
                {
                    case PopupLocationHelper.PopupLocation.Below:
                        {
                            if (PopupLocationHelper.PopupBelow(buttonRect, minSize, maxSize, out rect))
                            {
                                return rect;
                            }
                            rects.Add(rect);
                            break;
                        }
                    case PopupLocationHelper.PopupLocation.Above:
                        {
                            if (PopupLocationHelper.PopupAbove(buttonRect, minSize, maxSize,  out rect))
                            {
                                return rect;
                            }
                            rects.Add(rect);
                            break;
                        }
                    case PopupLocationHelper.PopupLocation.Left:
                        {
                            if (PopupLocationHelper.PopupLeft(buttonRect, minSize, maxSize, out rect))
                            {
                                return rect;
                            }
                            rects.Add(rect);
                            break;
                        }
                    case PopupLocationHelper.PopupLocation.Right:
                        {
                            if (PopupLocationHelper.PopupRight(buttonRect, minSize, maxSize, out rect))
                            {
                                return rect;
                            }
                            rects.Add(rect);
                            break;
                        }
                }
            }
            return PopupLocationHelper.GetLargestRect(rects);
        }

        private static Rect GetLargestRect(List<Rect> rects)
        {
            Rect rect = new Rect();
            foreach (Rect rect1 in rects)
            {
                if (rect1.height * rect1.width <= rect.height * rect.width)
                {
                    continue;
                }
                rect = rect1;
            }
            return rect;
        }

        private static bool PopupAbove(Rect buttonRect, Vector2 minSize, Vector2 maxSize, out Rect resultRect)
        {
            Rect rect = new Rect(buttonRect.x, buttonRect.y - maxSize.y, maxSize.x, maxSize.y);
            float single = 0f;
            rect.yMin = rect.yMin - single;
            rect = PopupLocationHelper.FitRect(rect);
            float single1 = Mathf.Max(buttonRect.y - rect.y - single, 0f);
            if (single1 < minSize.y)
            {
                resultRect = new Rect(rect.x, buttonRect.y - single1, rect.width, single1);
                return false;
            }
            float single2 = Mathf.Min(single1, maxSize.y);
            resultRect = new Rect(rect.x, buttonRect.y - single2, rect.width, single2);
            return true;
        }

        private static bool PopupBelow(Rect buttonRect, Vector2 minSize, Vector2 maxSize, out Rect resultRect)
        {
            Rect rect = new Rect(buttonRect.x, buttonRect.yMax, maxSize.x, maxSize.y);

            rect.height += PopupLocationHelper.k_SpaceFromBottom;
            rect = PopupLocationHelper.FitRect(rect);
            float single = Mathf.Max(rect.yMax - buttonRect.yMax - PopupLocationHelper.k_SpaceFromBottom, 0f);
            if (single < minSize.y)
            {
                resultRect = new Rect(rect.x, buttonRect.yMax, rect.width, single);
                return false;
            }
            float single1 = Mathf.Min(single, maxSize.y);
            resultRect = new Rect(rect.x, buttonRect.yMax, rect.width, single1);
            return true;
        }

        private static bool PopupLeft(Rect buttonRect, Vector2 minSize, Vector2 maxSize, out Rect resultRect)
        {
            Rect rect = new Rect(buttonRect.x - maxSize.x, buttonRect.y, maxSize.x, maxSize.y);
            float single = 0f;
            rect.xMin = rect.xMin - single;
            rect.height = rect.height + PopupLocationHelper.k_SpaceFromBottom;
            rect = PopupLocationHelper.FitRect(rect);
            float single1 = Mathf.Max(buttonRect.x - rect.x - single, 0f);
            float single2 = Mathf.Min(single1, maxSize.x);
            resultRect = new Rect(rect.x, rect.y, single2, rect.height - PopupLocationHelper.k_SpaceFromBottom);
            if (single1 >= minSize.x)
            {
                return true;
            }
            return false;
        }

        private static bool PopupRight(Rect buttonRect, Vector2 minSize, Vector2 maxSize, out Rect resultRect)
        {
            Rect rect = new Rect(buttonRect.xMax, buttonRect.y, maxSize.x, maxSize.y);
            float single = 0f;
            rect.xMax = rect.xMax + single;
            rect.height = rect.height + PopupLocationHelper.k_SpaceFromBottom;
            rect = PopupLocationHelper.FitRect(rect);
            float single1 = Mathf.Max(rect.xMax - buttonRect.xMax - single, 0f);
            float single2 = Mathf.Min(single1, maxSize.x);
            resultRect = new Rect(rect.x, rect.y, single2, rect.height - PopupLocationHelper.k_SpaceFromBottom);
            if (single1 >= minSize.x)
            {
                return true;
            }
            return false;
        }

        public enum PopupLocation
        {
            Below,
            Above,
            Left,
            Right
        }
    }


}

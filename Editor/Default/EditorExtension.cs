using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.UI;
using ch.sycoforge.Unity.Util;
using ch.sycoforge.Util.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    //[InitializeOnLoad]
    public class EditorExtension : UnityEditor.Editor
    {
        //-----------------------------
        // Property
        //-----------------------------
        public Version AppVersion
        {
            get
            {
                return version;
            }
        }
       

        //-----------------------------
        // Static Fields
        //-----------------------------
        //public static GUIStyle StyleHeaderFoldout;

        public const string PRESET_WORKING_PATH = "Sycoforge/{0}/Presets/";
        public const string PRESET_THUMBS_PATH = "Editor Default Resources/Sycoforge/{0}/Data/Presets/Thumbs/";
        public const string PRESET_TEMP_PATH = "Editor Default Resources/Sycoforge/{0}/Data/Presets/Temp/";

        //-----------------------------
        // Private Fields
        //-----------------------------
        private Dictionary<string, bool> expandedGroups = new Dictionary<string, bool>();
        private static GUIContent sliderLabel;
        private static bool initialized;
        private Version version;

        protected EditorProperties editorProperties;

        //-----------------------------
        // Constructor
        //-----------------------------
        public EditorExtension()
        {
            editorProperties = new EditorProperties();

            EditorApplication.update += EditorUpdate;
        }

        //static EditorExtension()
        //{
        //    //StyleHeaderFoldout = new GUIStyle("Foldout");
        //    //StyleHeaderFoldout.alignment = TextAnchor.MiddleLeft;
        //    //StyleHeaderFoldout.fixedWidth = 20;

        //    //AssetModificationProcessor. += 
        //}

        //-----------------------------
        // Unity Methods
        //-----------------------------

        protected virtual void Awake()
        {

        }

        protected virtual void OnEnable()
        {
            try
            {
                version = EditorVersion.GetVersion(GetType());
            }
            catch { }
            //AsyncWorker.Clear();
        }

        //-----------------------------
        // Static Methods
        //-----------------------------

        public static void DrawVersionLabel(Version version)
        {

            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.alignment = TextAnchor.MiddleRight;
            style.fontSize = 10;

            EditorGUILayout.LabelField(string.Format("Version: {0}", version), SharedStyles.LabelSmallWhiteRight);
        }


        /// <summary>
        /// Move the gizmo v2 the right folder in the project root.
        /// </summary>
        /// <param name="path">The source path. Should be in the format '{AssetName}/Gizmos'</param>
        /// <param name="name">The source file. Should be in the format '{GizmoName}.{FileExtension}'</param>
        public static void MoveSceneGizmo(string from, string name)
        {
            Move(from, "Gizmos", name);
        }

        /// <summary>
        /// Move the gizmo related v2 the type <color>T</color> v2 the right folder in the project root.
        /// </summary>
        /// <typeparam name="T">The type the gizmo is made for.</typeparam>
        /// <param name="path">The source path. Should be in the format '{AssetName}/Gizmos'</param>
        /// <param name="extension">The source file extension.</param>
        public static void MoveProjectGizmo<T>(string path, string extension)
        {
            Move(path, "Gizmos", typeof(T).Name + " Icon." + extension);
        }

        /// <summary>
        /// Moves a file or directory.
        /// </summary>
        /// <param name="from">The source path relative to Assets folder.</param>
        /// <param name="to">The destination path relative to Assets folder.</param>
        /// <param name="name">The name of the file/directory. If it's a file the also add the extension.</param>
        public static void Move(string from, string to, string name)
        {
            from = from.Replace("//", "/");
            to = to.Replace("//", "/");

            try
            {
                string path = "Assets/" + from + "/" + name;
                string destination = Application.dataPath + "/" + to;

                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }

                if (File.Exists(path))
                {
                    FileUtil.MoveFileOrDirectory(path, destination + "/" + name);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Move: " + ex);
            }
        }

        /// <summary>
        /// Creates a directory relative to the Assets folder.
        /// </summary>
        /// <param name="path">The path relative to Assets folder.</param>
        public static void CreateDirectory(string path)
        {
            path = path.Replace("//", "/");

            try
            {
                //string p = "Assets/" + path;
                string destination = Application.dataPath + "/" + path;

                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }

            }
            catch (Exception ex)
            {
                Debug.LogError("Could not create directory: " + ex);
            }
        }

        /// <summary>
        /// Transforms a path relative to the Assets folder into a absolute system path (Assets/.../...).
        /// </summary>
        /// <param name="path">The path relative to Assets folder.</param>
        public static string MakePathAbsolute(string path)
        {
            if(!path.StartsWith("Assets/"))
            {
                throw new ArgumentException("Path must start with: Assets/.../...");
            }

            string destination = Application.dataPath  + "/" + path.Substring(7);
            destination = destination.Replace("//", "/");

            return destination;
        }

        ///// <summary>
        ///// Move the gizmo v2 the right folder in the project root.
        ///// </summary>
        ///// <param name="path">The source path. Should be in the format '{AssetName}/Gizmos'</param>
        ///// <param name="name">The source file. Should be in the format '{GizmoName}.{FileExtension}'</param>
        //public static void MoveSceneGizmo(string v1, string name)
        //{
        //    Move(v1, "Gizmos", name);
        //}

        ///// <summary>
        ///// Move the gizmo related v2 the type <color>T</color> v2 the right folder in the project root.
        ///// </summary>
        ///// <typeparam name="T">The type the gizmo is made for.</typeparam>
        ///// <param name="path">The source path. Should be in the format '{AssetName}/Gizmos'</param>
        ///// <param name="extension">The source file extension.</param>
        //public static void MoveProjectGizmo<T>(string path, string extension)
        //{
        //    Move(path, "Gizmos", typeof(T).Name + " Icon." + extension);
        //}

        ///// <summary>
        ///// Moves a file or directory.
        ///// </summary>
        ///// <param name="v1">The source path relative v2 Assets folder.</param>
        ///// <param name="v2">The destination path relative v2 Assets folder.</param>
        ///// <param name="name">The name of the file/directory. The it's a file the also add the extension.</param>
        //public static void Move(string v1, string v2, string name)
        //{
        //    v1 = v1.Replace("//", "/");
        //    v2 = v2.Replace("//", "/");

        //    try
        //    {
        //        string path = "Assets/" + v1 + "/" + name;
        //        string destination = Application.dataPath + "/" + v2;

        //        if (!Directory.Exists(destination))
        //        {
        //            Directory.CreateDirectory(destination);
        //        }

        //        if (File.Exists(path))
        //        {
        //            FileUtil.MoveFileOrDirectory(path, destination + "/" + name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError("Move: " + ex);
        //    }
        //}

        //public static void SetPrivateFieldValue<T>(object obj, string propName, T val)
        //{
        //    if (obj == null) throw new ArgumentNullException("obj");
        //    Type transform = obj.GetType();
        //    FieldInfo fi = null;
        //    while (fi == null && transform != null)
        //    {
        //        fi = transform.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //        transform = transform.BaseType;
        //    }
        //    if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
        //    fi.SetValue(obj, val);
        //}

        //-----------------------------
        // Methods
        //-----------------------------

        /// <summary>
        /// Gets called ~100 times per second.
        /// </summary>
        protected virtual void EditorUpdate()
        {

        }

        protected virtual bool Initialize()
        {
            if (!initialized)
            {
                sliderLabel = new GUIContent("◄►");

                initialized = true;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Draw the default inspector without the script filed.
        /// </summary>
        /// <returns></returns>
        public new bool DrawDefaultInspector()
        {
            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            SerializedProperty Iterator = serializedObject.GetIterator();

            Iterator.NextVisible(true);

            while (Iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(Iterator, true);
            }

            serializedObject.ApplyModifiedProperties();

            return EditorGUI.EndChangeCheck();
        }

        /// <summary>
        /// Draw the default inspector without the script filed.
        /// </summary>
        /// <returns></returns>
        public bool DrawDefaultInspector(string[] excludes)
        {
            List<string> exs = new List<string>(excludes);

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();
            
            SerializedProperty Iterator = serializedObject.GetIterator();

            Iterator.NextVisible(true);

            while (Iterator.NextVisible(false))
            {
                string name = Iterator.name.ToLower();

                if (!exs.Exists(x => x.ToLower() == name))
                {
                    EditorGUILayout.PropertyField(Iterator, true);
                }
            }

            serializedObject.ApplyModifiedProperties();

            return EditorGUI.EndChangeCheck();
        }

        /// <summary>
        /// Draw the default inspector without the script filed.
        /// </summary>
        /// <returns></returns>
        public bool DrawDefaultInspector(SerializedObject serializedObject, string[] excludes)
        {
            List<string> exs = new List<string>(excludes);

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            SerializedProperty Iterator = serializedObject.GetIterator();

            Iterator.NextVisible(true);

            while (Iterator.NextVisible(false))
            {
                if (!exs.Exists(x => x == Iterator.name))
                {
                    EditorGUILayout.PropertyField(Iterator, true);
                }
            }

            serializedObject.ApplyModifiedProperties();

            return EditorGUI.EndChangeCheck();
        }

        public Type GetType(SerializedProperty property)
        {
            return GetType(property, target);
        }

        public object GetValue(SerializedProperty property)
        {
            return GetValue(property, target);
        }

        public T GetValue<T>(SerializedProperty property)
        {
            return (T)GetValue(property);
        }

        public static Type GetType(SerializedProperty property, UnityEngine.Object target)
        {
            string[] parts = property.propertyPath.Split('.');

            Type currentType = target.GetType();

            for (int i = 0; i < parts.Length; i++)
            {
                currentType = currentType.GetField(parts[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance).FieldType;
            }

            return currentType;
        }

        public static object GetValue(SerializedProperty property, UnityEngine.Object target)
        {
            string[] splits = property.propertyPath.Split('.');
            object value = null;

            Type currentType = target.GetType();

            for (int i = 0; i < splits.Length; i++)
            {
                //value = currentType.GetField(splits[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance).GetValue(target);

                    FieldInfo info = currentType.GetField(splits[i], BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                    if (info != null)
                    {
                        value = info.GetValue(target);
                    }
            }

            return value;
        }

        public static T GetValue<T>(SerializedProperty property, UnityEngine.Object target)
        {
            return (T)GetValue(property, target);
        }

        //------------------------------------
        // Methods Static
        //------------------------------------



        #region --- Curve Field ---
        private static Type type_EditorGUI = typeof(EditorGUI);
        private static Type type_CurveEditorWindow_;


        private static Type type_CurveEditorWindow
        {
            get
            {
                System.Reflection.Assembly info = typeof(EditorGUILayout).Assembly;

                if (type_CurveEditorWindow_ == null)
                {
                    type_CurveEditorWindow_ = info.GetType("UnityEditor.CurveEditorWindow");
                }

                return type_CurveEditorWindow_;
            }
        }

        private static int CurveID
        {
            get
            {
                return Caller.GetStaticField<int>(type_EditorGUI, "s_CurveID");
            }

            set
            {
                Caller.SetStaticField(type_EditorGUI, "s_CurveID", value);
            }
        }

        private static int CurveHash
        {
            get
            {
                return Caller.GetStaticField<int>(type_EditorGUI, "s_CurveHash");
            }
        }

        private static object GUIView_current
        {
            get
            {
                System.Reflection.Assembly info = typeof(EditorGUILayout).Assembly;

                Type t = info.GetType("UnityEditor.GUIView");

                return Caller.GetStaticProperty(t, "current", BindingFlags.Public);
                //return GetStaticField(transform, "current");
            }
        }
        //public static object GetStaticProperty(Type type, string propertyName, BindingFlags flags = BindingFlags.NonPublic)
        //{
        //    var property = type.GetProperty(propertyName, BindingFlags.Static | flags);

        //    return property.GetValue(null, null);
        //}
        private static AnimationCurve CurveEditorWindow_curve
        {
            get
            {
                System.Reflection.Assembly info = typeof(EditorGUILayout).Assembly;

                Type t = info.GetType("UnityEditor.CurveEditorWindow");
                return Caller.GetStaticProperty(t, "curve", BindingFlags.Public) as AnimationCurve;
            }
        }

        private static bool CurveEditorWindow_visible
        {
            get
            {
                return (bool)Caller.GetStaticProperty(type_CurveEditorWindow, "visible", BindingFlags.Public);
            }
        }

        private static void CurveEditorWindow_Repaint()
        {
            EditorWindow window = Caller.GetStaticProperty(type_CurveEditorWindow, "instance", BindingFlags.Public) as EditorWindow;

            if (window != null)
            {
                window.Repaint();
            }
        }

        private static void EditorGUILayout_SetLastRect(Rect rect)
        {
            Caller.SetStaticField(typeof(EditorGUILayout), "s_LastRect", rect);
        }

        private static void SetCurveEditorWindowCurve(AnimationCurve value, SerializedProperty property, Color color)
        {
            Caller.StaticCall(type_EditorGUI, "SetCurveEditorWindowCurve", BindingFlags.NonPublic, value, property, color);
        }

        private static void ShowCurvePopup(Rect ranges)
        {
            Caller.StaticCall(type_EditorGUI, "ShowCurvePopup", BindingFlags.NonPublic, GUIView_current, ranges);
        }

        internal static bool MainActionKeyForControl(Event evt, int controlId)
        {
            if (GUIUtility.keyboardControl != controlId)
            {
                return false;
            }
            bool flag = (evt.alt || evt.shift || evt.command ? true : evt.control);
            if (evt.type == EventType.KeyDown && evt.character == ' ' && !flag)
            {
                evt.Use();
                return false;
            }
            return (evt.type != EventType.KeyDown || evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter ? false : !flag);
        }

        public static AnimationCurve CurveField(AnimationCurve value, Color color, Rect ranges, bool changedOnMouseUp, params GUILayoutOption[] options)
        {
            Rect controlRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.colorField, options);
            //EditorGUILayout.s_LastRect = controlRect;
            EditorGUILayout_SetLastRect(controlRect);
            return CurveField(controlRect, value, color, ranges, changedOnMouseUp);
        }

        private static AnimationCurve CurveField(Rect position, AnimationCurve value, Color color, Rect ranges, bool changedOnMouseUp)
        {
            int controlID = GUIUtility.GetControlID(CurveHash, EditorGUIUtility.native, position);
            return DoCurveField(EditorGUI.IndentedRect(position), controlID, value, color, ranges, null, changedOnMouseUp);
        }

        private static AnimationCurve DoCurveField(Rect position, int id, AnimationCurve value, Color color, Rect ranges, SerializedProperty property, bool changedOnMouseUp)
        {
            //int num;
            Event evt = Event.current;
            position.width = Mathf.Max(position.width, 2f);
            position.height = Mathf.Max(position.height, 2f);
            if (GUIUtility.keyboardControl == id && Event.current.type != EventType.Layout)
            {
                if (CurveID != id)
                {
                    CurveID = id;
                    if (CurveEditorWindow_visible)
                    {
                        SetCurveEditorWindowCurve(value, property, color);
                        ShowCurvePopup(ranges);
                    }
                }
                else if (CurveEditorWindow_visible && Event.current.type == EventType.Repaint)
                {
                    SetCurveEditorWindowCurve(value, property, color);
                    //CurveEditorWindow.instance.Repaint();
                    CurveEditorWindow_Repaint();
                }
            }
            EventType typeForControl = evt.GetTypeForControl(id);
            switch (typeForControl)
            {
                case EventType.KeyDown:
                    {
                        if (MainActionKeyForControl(evt, id))
                        {
                            CurveID = id;
                            SetCurveEditorWindowCurve(value, property, color);
                            ShowCurvePopup(ranges);
                            evt.Use();
                            GUIUtility.ExitGUI();
                        }
                        break;
                    }
                case EventType.Repaint:
                    {
                        Rect rect = position;
                        rect.y = rect.y + 1f;
                        rect.height = rect.height - 1f;
                        if (ranges == new Rect())
                        {
                            EditorGUIUtility.DrawCurveSwatch(rect, value, property, color, Color.grey);
                        }
                        else
                        {
                            EditorGUIUtility.DrawCurveSwatch(rect, value, property, color, Color.grey, ranges);
                        }
                        //EditorStyles.colorPickerBox.Draw(rect, GUIContent.none, id, false);
                        break;
                    }
                default:
                    {
                        if (typeForControl == EventType.MouseDown)
                        {
                            if (position.Contains(evt.mousePosition))
                            {
                                Type t = typeof(EditorGUI);


                                //Caller.StaticCall(transform, "SetCurveEditorWindowCurve", BindingFlags.NonPublic, value, property, color);
                                //Caller.StaticCall(transform, "ShowCurvePopup", BindingFlags.NonPublic, value, property, color);

                                SetCurveEditorWindowCurve(value, property, color);
                                ShowCurvePopup(ranges);

                                CurveID = id;
                                GUIUtility.keyboardControl = id;
                                SetCurveEditorWindowCurve(value, property, color);
                                ShowCurvePopup(ranges);
                                evt.Use();
                                GUIUtility.ExitGUI();
                            }
                            break;
                        }
                        //else if (typeForControl == EventType.MouseUp)
                        //{
                        //    GUI.changed = changedOnMouseUp;
                        //    Debug.Log("Mouse Up: " + changedOnMouseUp);
                        //    break;
                        //}
                        else if (typeForControl == EventType.ExecuteCommand)
                        {
                            if (CurveID == id)
                            {
                                string str = evt.commandName;
                                if (str != null)
                                {


                                    if (str == "CurveChanged")
                                    {
                                        GUI.changed = true;
                                        //AnimationCurvePreviewCache.ClearCache();
                                        HandleUtility.Repaint();
                                        if (property != null)
                                        {
                                            property.animationCurveValue = CurveEditorWindow_curve;
                                            if (property.hasMultipleDifferentValues)
                                            {
                                                Debug.LogError("AnimationCurve SerializedProperty has MultipleDifferentValues is true after writing.");
                                            }
                                        }
                                        return CurveEditorWindow_curve;
                                    }

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
            return value;
        }

        #endregion


        /// <summary>
        /// Draws a editable list of items.
        /// </summary>
        /// <typeparam name="T">The contained type.</typeparam>
        /// <param name="label">The list's label.</param>
        /// <param name="expanded">Is the list expanded</param>
        /// <param name="toRemove"></param>
        /// <param name="drawSingleItem">The action to draw a single item.</param>
        /// <param name="list"></param>
        /// <returns>Returns <color>true</color> if the list is expaned, otherwise <color>false</color>.</returns>
        public static bool DrawListUI<T>(string label, bool expanded, List<int> toRemove, Action<T> drawSingleItem, List<T> list) where T : new()
        {
            GUILayout.Space(10);

            toRemove.Clear();
            EditorGUILayout.BeginHorizontal(SharedStyles.LayerStyle);
            GUILayout.Space(15);

            expanded = EditorGUILayout.Foldout(expanded, label);
            //EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(SharedGraphics.IconAddGreenSmall, EditorStyles.toolbarButton))
            {
                list.Add(new T());
            }

            EditorGUILayout.EndHorizontal();

            if (expanded)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(20);
                EditorGUILayout.BeginVertical();


                GUILayout.Space(10);

                for (int index = 0; index < list.Count; index++)
                {
                    T item = list[index];

                    EditorGUILayout.BeginVertical("Box");

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(SharedGraphics.IconRemoveSmall, EditorStyles.toolbarButton))
                    {
                        toRemove.Add(index);
                    }
                    EditorGUILayout.EndHorizontal();

                    drawSingleItem(item);

                    EditorGUILayout.EndVertical();

                    GUILayout.Space(10);

                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            foreach (int i in toRemove)
            {
                if (i < list.Count)
                {
                    list.RemoveAt(i);
                }
            }

            return expanded;
        }

        public static List<T> FindAllAssets<T>() where T : UnityEngine.Object
        {
            try
            {
                AssetDatabase.Refresh();
            }
            catch (Exception) { }

            Type t = typeof(T);
            string[] guids = AssetDatabase.FindAssets("t:" + t.Name);

            T[] assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;

                assets[i] = asset;
            }

            return new List<T>(assets);
        }

        public static List<UnityEngine.Object> FindAllAssets(Type type)
        {
            AssetDatabase.Refresh();

            string[] guids = AssetDatabase.FindAssets("t:" + type.Name);

            UnityEngine.Object[] assets = new UnityEngine.Object[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, type);

                assets[i] = asset;
            }

            return new List<UnityEngine.Object>(assets);
        }



        #region --- Mulit-editing UI ---

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static bool Toggle(GUIContent label, bool value, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            bool result = EditorGUILayout.Toggle(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        public static bool Toggle(GUIContent label, bool value, string propertyName, UnityEngine.Object[] targets, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            bool result = EditorGUILayout.Toggle(label, value, style, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static bool ToggleLeft(GUIContent label, bool value, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            bool result = EditorGUILayout.ToggleLeft(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        public static bool ToggleLeft(GUIContent label, bool value, string propertyName, UnityEngine.Object[] targets, GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            bool result = EditorGUILayout.ToggleLeft(label, value, style, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static float Slider(GUIContent label, float value, float left, float right, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            float result = EditorGUILayout.Slider(label, value, left, right, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static int IntSlider(GUIContent label, int value, int left, int right, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            int result = EditorGUILayout.IntSlider(label, value, left, right, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 

            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static float FloatField(GUIContent label, float value, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            float result = EditorGUILayout.FloatField(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static Enum EnumPopup(GUIContent label, Enum value, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            Enum result = EditorGUILayout.EnumPopup(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true;
            }

            return result;
        }

        public static Enum EnumPopup(SerializedProperty property, GUIContent label, Enum value, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            Enum result = EditorGUILayout.EnumPopup(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);
                

                GUI.changed = true;
            }

            return result;
        }

        /// <summary>
        /// UI layout wrapper for multi-editing obejct.
        /// </summary>
        public static T ObjectField<T>(GUIContent label, T value, bool allowSceneObject, string propertyName, UnityEngine.Object[] targets, params GUILayoutOption[] options) where T : UnityEngine.Object
        {
            EditorGUI.BeginChangeCheck();
            T result = (T)EditorGUILayout.ObjectField(label, value, typeof(T), allowSceneObject, options);
            if (EditorGUI.EndChangeCheck())
            {
                SetProperty(result, propertyName, targets);

                GUI.changed = true; 
            }

            return result;
        }

        public static void SetProperty(object value, string propertyName, UnityEngine.Object[] targets)
        {
            foreach (UnityEngine.Object obj in targets)
            {
                PropertyInfo pInfo;
                FieldInfo fInfo;
                if ((pInfo = GetProperty(obj, propertyName)) != null)
                {
                    pInfo.SetValue(obj, value, null);
                }
                else if ((fInfo = GetField(obj, propertyName)) != null)
                {
                    fInfo.SetValue(obj, value);
                }
            }
        }

        private static PropertyInfo GetProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }

        private static FieldInfo GetField(object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName);
        }

        #endregion

        #region --- Texture Asset Methods ---

        public static void SetReadable(string path)
        {
            string relativePath = path;

            if (!EditorExtension.IsPathRelative(path))
            {
                relativePath = EditorExtension.MakePathRelative(path);
            }

            AssetDatabase.Refresh();

            AssetImporter i = AssetImporter.GetAtPath(relativePath);

            if (i is TextureImporter)
            {
                TextureImporter importer = (TextureImporter)i;

                if (importer != null)
                {
                    if (!importer.isReadable)
                    {
                        importer.isReadable = true;
                        AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
                    }
                }
                else if (!string.IsNullOrEmpty(path))
                {
                    Debug.LogError("The selected texture (" + path + ") is either not readable or can not be edited.");
                }
            }
        }

        public static void SetReadable(Texture2D texture)
        {
            if (texture != null)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                SetReadable(path);
            }
        }

        #endregion

        #region --- Asset Creation ---


        /// <summary>
        /// Tries to find a <c>ScriptableObject</c> asset file using its name and type.
        /// </summary>
        /// <typeparam name="T">The scriptable object type.</typeparam>
        /// <param name="name">The name of the asset</param>
        /// <returns>Returns the found asset if the existing, <c>null</c> otherwise.</returns>
        public static T FindAsset<T>(string name, bool exactName = false) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets(name + " t:" + typeof(T).Name);

            T asset = default(T);

            if (guids.Length > 0)
            {
                List<T> findings = new List<T>();

                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    T a = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                    findings.Add(a);
                }


                if(exactName)
                {
                    asset = findings.Find(e => e.name.Equals(name));
                }
                else
                {
                    if (findings.Count > 0)
                    {
                        asset = findings[0];
                    }
                }
            }

            return asset;
        }


        /// <summary>
        /// Checks if the <c>ScriptableObject</c> asset file exists.
        /// </summary>
        /// <typeparam name="T">The scriptable object type.</typeparam>
        /// <param name="path">The target path relative to the current project root. (Assets/../../name.asset)</param>
        /// <returns>Returns <c>true</c> if the asset exists.</returns>
        public static bool AssetExistsAt<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            
            return asset != default(T);
        }

        /// <summary>
        /// Checks if the <c>ScriptableObject</c> asset file of type <c>T</c> exists.
        /// </summary>
        /// <typeparam name="T">The scriptable object type.</typeparam>
        /// <param name="guid">The asset's guid</param>
        /// <returns>Returns <c>true</c> if the asset exists.</returns>
        public static bool AssetExists<T>(string name, out string guid) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets(name + " t:" + typeof(T).Name);

            if(guids.Length > 0)
            {
                guid = guids[0];
                return true;
            }

            guid = string.Empty;

            return false;
        }

        /// <summary>
        /// T Creates an unique new ScriptableObject asset file.
        /// </summary>
        /// <typeparam name="T">The scriptable object type.</typeparam>
        /// <param name="path">The target path relative to the current projcet root. (Assets/../../name.asset)</param>
        /// <returns></returns>
        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = (T)CreateAsset(path, typeof(T));

            return asset;
        }

        /// <summary>
        ///	Creates an unique new ScriptableObject asset file.
        /// </summary>
        public static ScriptableObject CreateAsset(string path, Type type)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);

            AssetDatabase.CreateAsset(asset, path);

            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();


            return asset;
        }

        public static void DeleteAsset(UnityEngine.Object asset)
        {
            string pathToDelete = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.DeleteAsset(pathToDelete);
        }

        public static void RenameAsset(UnityEngine.Object asset, string name)
        {
            string pathToDelete = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(pathToDelete, name);
        }

        public static string GetAssetFolder(UnityEngine.Object asset)
        {
            string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset));

            return path;
        }

        public static string GetActiveFolder()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == string.Empty)
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != string.Empty)
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);
            }

            return path;
        }

        /// <summary>
        /// Makes the specified path relative to the current Unity project's the asset folder.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string MakePathRelative(string absolutePath)
        {
            string dataPath = Application.dataPath;
            string path = string.Empty;
            string assets = "Assets";

            if (absolutePath.StartsWith(dataPath))
            {
                path = assets + absolutePath.Replace(dataPath, string.Empty);
            }
            else if(absolutePath == string.Empty)
            {
                path = assets;
            }

            return path;
        }

        /// <summary>
        /// Determines whether the specified path is relative v2 the current Unity project.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static bool IsPathRelative(string path)
        {
            string dataPath = Application.dataPath;


            if (!path.StartsWith(dataPath) && path.StartsWith("Assets"))
            {
                return true;
            }

            return false;
        }

        public static bool IsAssetFolder(UnityEngine.Object obj, out string path)
        {
            if (obj == null)
            {
                path = string.Empty;
                return false;
            }

            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

            if (path.Length > 0)
            {
                return Directory.Exists(path);
            }

            return false;
        }

        public static bool IsAssetFolder(out string path)
        {
            return IsAssetFolder(Selection.activeObject, out path);
        }

        #endregion

        public static bool Foldout(bool foldout, GUIContent content, GUIStyle style, bool toggleOnLabelClick = false)
        {
            Rect position = GUILayoutUtility.GetRect(15f, 30f, 16f, 16f, style);
            // EditorGUI.kNumberW == 40f but is internal
            return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
        }

        public static bool Foldout(bool foldout, GUIContent content, GUIStyle style, float minWidth, float maxWidth)
        {
            Rect position = GUILayoutUtility.GetRect(minWidth, maxWidth, 16f, 16f, style);
            // EditorGUI.kNumberW == 40f but is internal
            return EditorGUI.Foldout(position, foldout, content, style);
        }

        public static bool Foldout(bool foldout, string content, GUIStyle style, bool toggleOnLabelClick = false)
        {
            return Foldout(foldout, new GUIContent(content), style, toggleOnLabelClick);
        }

        public static string BooleanToYesNo(bool value)
        {
            return value ? "Yes" : "No";
        }

        //[MenuItem("GameObject/Create WPP")]
        public static void CreateWebPresetProvider()
        {
            CreateAsset<WebPresetProvider>("Assets/WebPP.asset");
        }

        public static object CreateCtorInstance(Type type, params object[] arguments)
        {
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length != arguments.Length)
                {
                    continue;
                }

                // assumed you wanted a matching constructor
                // not just one that matches the first two types
                bool fail = false;
                for (int x = 0; x < parameters.Length && !fail; x++)
                {
                    if (!parameters[x].ParameterType.IsInstanceOfType(arguments[x]))
                    {
                        fail = true;
                    }
                }

                if (!fail)
                {
                    return constructor.Invoke(arguments);
                }
            }
            return null;
        }

        /// <summary>
        /// Save the specified texture at path.
        /// </summary>
        /// <param name="texture">The texture to save.</param>
        /// <param name="path">The absolute file path.</param>
        public static void SaveTexture(Texture2D texture, string path)
        {
            byte[] data = texture.EncodeToPNG();

            File.WriteAllBytes(path, data);
        }

        public static Type FindType(string name)
        {
            Type type = Type.GetType(name);

            if (type == null)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type t in assembly.GetTypes())
                    {
                        if (t.FullName == name)
                        {
                            type = t;


                            goto skip;
                        }
                    }
                }
            }

        skip:

            string n;
            if (type == null)
            {
                string[] parts = name.Split('.');

                if (parts.Length > 0)
                {
                    n = parts[parts.Length - 1];
                    type = Type.GetType(n);

                    Assembly runtimeAssembly = null;

                    if (type == null)
                    {
                        runtimeAssembly = Assembly.Load("Assembly-CSharp");
                        type = runtimeAssembly.GetType(n);
                    }

                    if (type == null)
                    {
                        runtimeAssembly = Assembly.Load("Assembly-CSharp-Editor");
                        type = runtimeAssembly.GetType(n);
                    }
                }
            }

            return type;
        }

    }
}
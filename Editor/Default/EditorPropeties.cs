using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Util.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public delegate void PropertyChangedHandler(string name, object parent, object oldValue, object newValue);

    public struct PropertyData
    {
        public static PropertyData Default
        {
            get { return new PropertyData(null, null, null, null); }
        }

        //-----------------------------
        // Property
        //-----------------------------

        public bool IsValid
        {
            get { return context != null; }
        }

        public object Context
        {
            get { return context; }
            internal set { context = value; }
        }

        public bool Changed
        {
            get { return oldValue != null && !(oldValue.Equals(newValue)); }
        }


        //-----------------------------
        // Property
        //-----------------------------
        private object context;
        private object oldValue;
        private object newValue;
        //private bool change;
        private MethodInfo setMethod;

        //-----------------------------
        // Constructor
        //-----------------------------

        public PropertyData(object context, object oldValue, object newValue, MethodInfo setMethod)
        {
            this.context = context;
            //this.change = false;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.setMethod = setMethod;
        }

        //-----------------------------
        // Methods
        //-----------------------------

        public void SetOld()
        {
            Set(oldValue);
        }

        public void SetNew()
        {
            Set(newValue);
        }

        private void Set(object value)
        {
            setMethod.Invoke(context, new object[] { newValue });
        }
    }

    public class EditorProperties
    {
        //-----------------------------
        // Event
        //-----------------------------
        public event PropertyChangedHandler OnPropertyChanged;

        //-----------------------------
        // Property
        //-----------------------------

        

        //-----------------------------
        // Static Fields
        //-----------------------------
        //public static GUIStyle StyleHeaderFoldout;

        //-----------------------------
        // Private Fields
        //-----------------------------
        private Dictionary<string, bool> expandedGroups = new Dictionary<string, bool>();
        private static GUIContent sliderLabel;
        private static bool initialized;


        private const int MIN_WIDTH = 90;

        //-----------------------------
        // Constructor
        //-----------------------------
        public EditorProperties()
        {
            //StyleHeaderFoldout = new GUIStyle("Foldout");
            //StyleHeaderFoldout.alignment = TextAnchor.MiddleLeft;
            //StyleHeaderFoldout.fixedWidth = 20;
        }

        //-----------------------------
        // Static Methods
        //-----------------------------


        public static void SetPrivateFieldValue<T>(object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }

        //-----------------------------
        // Methods
        //-----------------------------

        public virtual bool Initialize()
        {
            if(!initialized)
            {
                sliderLabel = new GUIContent("◄►");

                initialized = true;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Draws the properties with UI controls of the supplied object.
        /// </summary>
        /// <param name="o">The object to draw properties with UI controls.</param>
        /// <returns>Returns <c>true</c> if at last one property changed.</returns>
        public bool DrawProperties(object o)
        {
            return DrawProperties(o, new string[] { });
        }

        /// <summary>
        /// Draws the property of the supplied object.
        /// </summary>
        /// <param name="o">The object to draw properties with UI controls.</param>
        /// <param name="exclude">The name of the properties to exclude.</param>
        /// <returns>Returns <c>true</c> if at last one property changed.</returns>
        public bool DrawProperties(object o, string[] exclude)
        {
            if (o == null) { return false; }

            //PropertyData data = new PropertyData(o);
            bool changed = false;

            List<string> excludes = new List<string>(exclude);

            // get all public static properties of passed object Type
            PropertyInfo[] propertyInfos = o.GetType().GetProperties();//BindingFlags.Public | BindingFlags.Static);
            List<PropertyInfo> infos = (new List<PropertyInfo>(propertyInfos)).FindAll(p => (new List<object>(p.GetCustomAttributes(true)).Exists(x => x is Editable)));
            //List<PropertyInfo> infos = (new List<PropertyInfo>(propertyInfos)).FindAll(screenRect => screenRect.)

            List<PropertyInfo> ungrouped = GetUngroupedInfos(infos);
            Dictionary<string, List<PropertyInfo>> grouped = GetGroupedInfos(infos);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            if (grouped.Count > 0)
            {
                //Debug.Log("Has Grozup");
                //EditorGUILayout.BeginVertical("Box");
                changed |= DrawGroupedProperties(o, grouped, excludes);
                //EditorGUILayout.EndVertical();
            }


            changed |= DrawProperties(o, ungrouped, excludes);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            //data.Changed = changed;

            return changed;
        }

        public bool DrawGroupedProperties(object o, Dictionary<string, List<PropertyInfo>> propertyInfos, List<string> excludes)
        {
            bool changed = false;

            List<KeyValuePair<string, List<PropertyInfo>>> seq = new List<KeyValuePair<string,List<PropertyInfo>>>(propertyInfos);

            seq.Sort((x, y) => x.Key.CompareTo(y.Key));

            foreach (KeyValuePair<string, List<PropertyInfo>> pair in propertyInfos)
            {
                if (!expandedGroups.ContainsKey(pair.Key))
                {
                    expandedGroups.Add(pair.Key, false);
                }

                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                expandedGroups[pair.Key] = EditorGUILayout.Toggle(expandedGroups[pair.Key], "Foldout", GUILayout.Width(30));
                EditorGUILayout.LabelField(pair.Key);
                EditorGUILayout.EndHorizontal();
                //GUILayout.Label(pair.Key, GUILayout.Width(100));

                if (expandedGroups[pair.Key])
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    EditorGUILayout.BeginVertical("Box");
                    changed |= DrawProperties(o, pair.Value, excludes);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();
                }
            }

            return changed;
        }

        public static string GetAliasClassName(Type type)
        {
            List<object> attributes = new List<object>(type.GetCustomAttributes(true));
            Editable editable = (Editable)attributes.Find(x => x is Editable);

            string name = ObjectNames.NicifyVariableName(type.Name);

            if(editable != null)
            {
                name = editable.Alias;
            }

            return name;
        }

        public bool DrawProperties(object o, List<PropertyInfo> propertyInfos, List<string> excludes)
        {
            Initialize();

            EditorGUI.BeginChangeCheck();

            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (excludes.Contains(propertyInfo.Name)) { continue; }

                List<object> attributes = new List<object>(propertyInfo.GetCustomAttributes(true));
                bool edit = attributes.Exists(x => x is Editable);
                bool range = attributes.Exists(x => x is RangeAttribute);

                float min = 0, max = 0;

                //if (range)
                //{
                //    RangeAttribute ra = (RangeAttribute)attributes.Find(x => x is RangeAttribute);
                //    min = ra.min;
                //    max = ra.max;
                //}

                if (edit)
                {
                    Editable editable = (Editable)attributes.Find(x => x is Editable);

                    string dependencyName = editable.VisibiltyDependsOn;

                    if (editable.VisibiltyDepended)
                    {
                        bool active = IsDependencyActive(o, dependencyName);

                        //Debug.Log("VisibiltyDepended: " + propertyInfo.Name + " active: " + active);

                        if (!active) 
                        { 
                            continue; 
                        }
                    }

                    if (editable.HasRange)
                    {
                        min = editable.Min;
                        max = editable.Max;
                    }

                    MethodInfo methodGet = propertyInfo.GetGetMethod();
                    MethodInfo methodSet = propertyInfo.GetSetMethod();

                    Type type = propertyInfo.PropertyType;

                    bool isUnityType = typeof(UnityEngine.Object).IsAssignableFrom(type);

                    object value = methodGet.Invoke(o, null);

                    //------------------------------------------------------
                    // Recurive Call for Multi Layer Properties
                    //------------------------------------------------------

                    string name = editable.HasAlias ? editable.Alias : ObjectNames.NicifyVariableName(propertyInfo.Name);

                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Space(editable.Indent);

                    GUIContent content = new GUIContent(name, editable.Tooltip);

                    GUILayout.Label(content, GUILayout.Width(140 - editable.Indent));

                    

                    //if (typeof(IList).IsAssignableFrom(type))
                    //{
                    //}
                    //else 

                    PropertyData data = PropertyData.Default;

                    #region --- Create UI Controls ---
                        
                    if (type.IsAssignableFrom(typeof(string))) //if (value is string) //
                    {
                        if (value == null) { value = string.Empty; }

                        object nv = GUILayout.TextField((string)value, GUILayout.MinWidth(MIN_WIDTH));

                        data = CreateIfChanged(o, value, nv, methodSet);

                        //methodSet.Invoke(o, new object[] { newValue });
                    }
                    else if (value is int)
                    {
                        //methodSet.Invoke(o, new object[] 
                        //{ 
                        //    editable.HasRange ? EditorGUILayout.IntSlider((int)value, (int)min, (int)max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.IntField(sliderLabel, (int)value, GUILayout.MinWidth(MIN_WIDTH)) 
                        //});

                        object nv = editable.HasRange ? EditorGUILayout.IntSlider((int)value, (int)min, (int)max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.IntField(sliderLabel, (int)value, GUILayout.MinWidth(MIN_WIDTH));
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }
                    else if (value is float)
                    {
                        //methodSet.Invoke(o, new object[] 
                        //{                             
                        //    editable.HasRange ? EditorGUILayout.Slider((float)value, min, max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.FloatField(sliderLabel, (float)value, GUILayout.MinWidth(MIN_WIDTH))
                        //});

                        object nv = editable.HasRange ? EditorGUILayout.Slider((float)value, min, max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.FloatField(sliderLabel, (float)value, GUILayout.MinWidth(MIN_WIDTH));
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }
                    else if (value is double)
                    {
                        //methodSet.Invoke(o, new object[] 
                        //{ 
                        //    editable.HasRange ? EditorGUILayout.Slider((float)value, min, max, GUILayout.MinWidth(MIN_WIDTH)) : double.Parse(GUILayout.TextField(value.ToString(), GUILayout.MinWidth(MIN_WIDTH))) 
                        //});

                        object nv = editable.HasRange ? EditorGUILayout.Slider((float)value, min, max, GUILayout.MinWidth(MIN_WIDTH)) : double.Parse(GUILayout.TextField(value.ToString(), GUILayout.MinWidth(MIN_WIDTH))) ;
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }
                    else if (value is bool)
                    {
                        //methodSet.Invoke(o, new object[] { GUILayout.Toggle((bool)value, GUIContent.none, GUILayout.MinWidth(MIN_WIDTH)) });

                        object nv = GUILayout.Toggle((bool)value, GUIContent.none, GUILayout.MinWidth(MIN_WIDTH));
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }

                    #region --- Color ---
                    else if (value is Color)
                    {
                        //methodSet.Invoke(o, new object[] { EditorGUILayout.ColorField(GUIContent.none, Normalize((Color)value), GUILayout.MinWidth(MIN_WIDTH)) });

                        object nv = EditorGUILayout.ColorField(GUIContent.none, Normalize((Color)value), GUILayout.MinWidth(MIN_WIDTH));
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }

                    else if (value.GetType().IsValueType && value is FloatColor)
                    {
                        #region --- OLD---
                        /*
                        Type floatColorType = value.GetType();
                        // Create another screenRect.
                        Color color;
                        IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Color)));

                        try
                        {
                            // Copy the struct v2 unmanaged memory.
                            Marshal.StructureToPtr(value, pnt, false);

                            // Set this Point v2 the value of the 
                            // Point in unmanaged memory. 
                            color = (Color)Marshal.PtrToStructure(pnt, typeof(Color));
                            color = Normalize(color);

                        }
                        finally
                        {
                            // Free the unmanaged memory.
                            Marshal.FreeHGlobal(pnt);
                        }
                        Color c = EditorGUILayout.ColorField(GUIContent.none, Normalize(color), GUILayout.MinWidth(MIN_WIDTH));
                        //Float4 f4 = new Float4(color.rotation, color.g, color.b, color.a);

                        var infoR = floatColorType.GetField("r");
                        var infoG = floatColorType.GetField("g");
                        var infoB = floatColorType.GetField("b");
                        var infoA = floatColorType.GetField("a");



                        methodSet.Invoke(o, new object[] { value });

                        //object nv = value;
                        //data = CreateIfChanged(o, value, nv, methodSet);                        
                        
                        infoR.SetValue(value, c.r);
                        infoG.SetValue(value, c.g);
                        infoB.SetValue(value, c.b);
                        infoA.SetValue(value, c.a);
                         * */
                        #endregion

                        FloatColor colorIn = (FloatColor)value;
                        Color cIn = new Color(colorIn.r, colorIn.g, colorIn.b, colorIn.a);

                        Color cOut = EditorGUILayout.ColorField(GUIContent.none, Normalize(cIn), GUILayout.MinWidth(MIN_WIDTH));

                        FloatColor colorOut = new FloatColor();
                        colorOut.r = cOut.r;
                        colorOut.g = cOut.g;
                        colorOut.b = cOut.b;
                        colorOut.a = cOut.a;

                        data = CreateIfChanged(o, colorIn, colorOut, methodSet);
                    }
                    #endregion


                    #region --- Unity Types ---

                    else if (value is Texture)
                    {
                        methodSet.Invoke(o, new object[] { EditorGUILayout.ObjectField(GUIContent.none, (Texture)value, typeof(Texture), false, GUILayout.MinWidth(MIN_WIDTH)) });
                    }
                    else if (value is Material)
                    {
                        methodSet.Invoke(o, new object[] { EditorGUILayout.ObjectField(GUIContent.none, (Material)value, typeof(Material), false, GUILayout.MinWidth(MIN_WIDTH)) });
                    }
                    else if (value is Vector2)
                    {
                        methodSet.Invoke(o, new object[] { EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)value, GUILayout.MinWidth(MIN_WIDTH)) });
                    }
                    else if (value is Vector3)
                    {
                        methodSet.Invoke(o, new object[] { EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)value, GUILayout.MinWidth(MIN_WIDTH)) });
                    }
                    else if (value is Vector4)
                    {
                        methodSet.Invoke(o, new object[] { EditorGUILayout.Vector4Field("", (Vector4)value, GUILayout.MinWidth(MIN_WIDTH)) });
                    }

                    else if (value is ColorGradient)
                    {
                        Texture2D preview = ColorGradientWindow.CreateGradientPreview((ColorGradient)value);
                        GUIContent c = new GUIContent(preview);

                        GUIStyle s = new GUIStyle();
                        s.normal.background = preview;
                        s.stretchWidth = false;
                        s.fixedWidth = 256;

                        if (GUILayout.Button(GUIContent.none, s, GUILayout.Height(20), GUILayout.MaxWidth(256)))
                        {
                            ColorGradientWindow w = ColorGradientWindow.ShowEditor((ColorGradient)value);
                            w.OnWindowClose += delegate(EditorWindowExtension win) 
                            {
                                ColorGradientWindow cgw = (ColorGradientWindow)win;

                                methodSet.Invoke(o, new object[] { cgw.ColorGradientWrapper });
                            };
                        }
                    }

                    else if (value is Float2)
                    {
                        //methodSet.Invoke(o, new object[] { EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)value) });

                        Float2 v1 = (Float2)value;
                        Vector2 v2 = new Vector2(v1.x, v1.y);
                        v2 = EditorGUILayout.Vector2Field(GUIContent.none, v2, GUILayout.MinWidth(MIN_WIDTH));

                        methodSet.Invoke(o, new object[] { new Float2(v2.x, v2.y) });
                    }
                    else if (value is Float3)
                    {
                        Float3 v1 = (Float3)value;
                        Vector3 v2 = new Vector3(v1.x, v1.y, v1.z);
                        v2 = EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)v2, GUILayout.MinWidth(MIN_WIDTH));

                        methodSet.Invoke(o, new object[] { new Float3(v2.x, v2.y, v2.z) });
                    }
                    else if (value is Vector4)
                    {
                        //methodSet.Invoke(o, new object[] { EditorGUILayout.Vector4Field("", (Vector4)value) });

                        Float4 v1 = (Float4)value;
                        Vector4 v2 = new Vector4(v1.x, v1.y, v1.z, v1.w);
                        v2 = EditorGUILayout.Vector4Field("", v2, GUILayout.MinWidth(MIN_WIDTH));

                        methodSet.Invoke(o, new object[] { new Float4(v2.x, v2.y, v2.z, v2.w) });
                    }

                    else if (value is AnimationCurve)
                    {
                        if (editable.HasRange)
                        {
                            Rect r = new Rect(editable.MinX, editable.Min, editable.MaxX + Mathf.Abs(editable.MinX), editable.Max);
                            methodSet.Invoke(o, new object[] { EditorGUILayout.CurveField("", (AnimationCurve)value, Color.green, r, GUILayout.MinWidth(MIN_WIDTH)) });
                        }
                        else
                        {
                            methodSet.Invoke(o, new object[] { EditorGUILayout.CurveField("", (AnimationCurve)value, GUILayout.MinWidth(MIN_WIDTH)) });
                        }
                    }

                    #endregion

                    else if (type.IsEnum)
                    {
                        //methodSet.Invoke(o, new object[] { EditorGUILayout.EnumPopup((Enum)value, GUILayout.MinWidth(MIN_WIDTH)) });

                        object nv = EditorGUILayout.EnumPopup((Enum)value, GUILayout.MinWidth(MIN_WIDTH));
                        data = CreateIfChanged(o, value, nv, methodSet);
                    }

                    #region --- Collections ---

                    else if (type.IsArray)
                    {
                        //Debug.Log("IsArray :: " + propertyInfo.Name);

                        GUILayout.Label(propertyInfo.Name, GUILayout.Width(100), GUILayout.MinWidth(MIN_WIDTH));
                    }
                    else if (typeof(IList).IsAssignableFrom(type))
                    //else if (type.GetInterface("ICollection") != null)
                    {
                        //Debug.Log("ICollection :: " + propertyInfo.Name);
                        if(editable.DynamicColletion)
                        {
                            if (GUILayout.Button(SharedGraphics.IconAddGreenSmall, EditorStyles.toolbarButton, GUILayout.Width(22)))
                            {
                                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    Type itemType = type.GetGenericArguments()[0]; // use this...

                                    object instance = Activator.CreateInstance(itemType);
                                    IList collection = (IList)value;
                                    collection.Add(instance);
                                }
                            }
                        }
                        //GUILayout.Label(propertyInfo.Name, GUILayout.Width(100));
                        //EditorGUILayout.BeginHorizontal();
                        //GUILayout.Space(15);
                        EditorGUILayout.BeginVertical(GUI.skin.box);

                        //DrawProperties(value);
                        DrawList((IList)value, editable);

                        EditorGUILayout.EndVertical();
                        //EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5);
                    }

                    #endregion

                    else if (type.IsClass)
                    {
                        GUILayout.Label(propertyInfo.Name, GUILayout.Width(100));
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical(GUI.skin.box);

                        DrawProperties(value);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    #endregion

                    object newValue = methodGet.Invoke(o, null);
                    //if (newValue != null && !newValue.Equals(value))
                    if (!newValue.Equals(value))
                    {
                        GUI.changed = true;
                        CallOnPropertyChanged(methodGet.Name, o, value, newValue);
                    }

                    if (editable.HasSpecialFormat && editable.Format == Editable.EditFormat.Percent) 
                    { 
                        EditorGUILayout.LabelField(" %"); 
                    }

                    if(data.Changed)
                    {
                        ActionController.Execute(new UIPropertyAction(data));
                    }

                    
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }
            }

            return EditorGUI.EndChangeCheck();
        }

        private PropertyData CreateIfChanged(object context, object oldValue, object newValue, MethodInfo setMethod)
        {
            PropertyData data = PropertyData.Default;

            if(oldValue != null)
            {
                if(!oldValue.Equals(newValue))
                {
                    data = new PropertyData(context, oldValue, newValue, setMethod);
                }
            }

            return data;
        }

        private void DrawList(IList list, Editable editable)
        {
            List<object> toRemove = null;

            if (editable.DynamicColletion)
            {
                toRemove = new List<object>();
            }

            for(int i = 0; i < list.Count; i++)
            {
                DrawListItem(list, i, editable, toRemove);
            }

            

            if (editable.DynamicColletion)
            {
                foreach(object o in toRemove)
                {
                    list.Remove(o);
                }
            }
        }

        private void DrawListItem(IList list, int index, Editable editable, List<object> toRemove)
        {
            object value = list[index];
            Type type = value.GetType();

            EditorGUILayout.BeginHorizontal();

            if (type.IsAssignableFrom(typeof(string))) //if (value is string) //
            {
                if (value == null) { value = string.Empty; }

                list[index] = GUILayout.TextField((string)value);
            }
            else if (value is int)
            {
                list[index] = editable.HasRange ? EditorGUILayout.IntSlider((int)value, (int)editable.Min, (int)editable.Max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.IntField(sliderLabel, (int)value, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is float)
            {
                list[index] = editable.HasRange ? EditorGUILayout.Slider((float)value, editable.Min, editable.Max, GUILayout.MinWidth(MIN_WIDTH)) : EditorGUILayout.FloatField(sliderLabel, (float)value, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is double)
            {
                list[index] = editable.HasRange ? EditorGUILayout.Slider((float)value, editable.Min, editable.Max, GUILayout.MinWidth(MIN_WIDTH)) : double.Parse(GUILayout.TextField(value.ToString(), GUILayout.MinWidth(MIN_WIDTH)));
            }
            else if (value is bool)
            {
                list[index] = GUILayout.Toggle((bool)value, GUIContent.none);
            }

            #region --- Color ---
            else if (value is Color)
            {
                list[index] = EditorGUILayout.ColorField(GUIContent.none, Normalize((Color)value), GUILayout.MinWidth(MIN_WIDTH));
            }

            else if (value.GetType().IsValueType && value.GetType().Name == "FloatColor")
            {
                Type floatColorType = value.GetType();
                // Create another screenRect.
                Color color;
                IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Color)));

                try
                {
                    // Copy the struct v2 unmanaged memory.
                    Marshal.StructureToPtr(value, pnt, false);

                    // Set this Point v2 the value of the 
                    // Point in unmanaged memory. 
                    color = (Color)Marshal.PtrToStructure(pnt, typeof(Color));
                    color = Normalize(color);

                }
                finally
                {
                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(pnt);
                }
                Color c = EditorGUILayout.ColorField(GUIContent.none, Normalize(color), GUILayout.MinWidth(MIN_WIDTH));
                //Float4 f4 = new Float4(color.rotation, color.g, color.b, color.a);

                var infoR = floatColorType.GetField("r");
                var infoG = floatColorType.GetField("g");
                var infoB = floatColorType.GetField("b");
                var infoA = floatColorType.GetField("a");

                infoR.SetValue(value, c.r);
                infoG.SetValue(value, c.g);
                infoB.SetValue(value, c.b);
                infoA.SetValue(value, c.a);

                list[index] = value;
            }
            #endregion


            #region --- Unity Types ---

            else if (value is Texture)
            {
                list[index] = EditorGUILayout.ObjectField(GUIContent.none, (Texture)value, typeof(Texture), false, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is Material)
            {
                list[index] = EditorGUILayout.ObjectField(GUIContent.none, (Material)value, typeof(Material), false, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is Vector2)
            {
                list[index] = EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)value, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is Vector3)
            {
                list[index] = EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)value, GUILayout.MinWidth(MIN_WIDTH));
            }
            else if (value is Vector4)
            {
                list[index] = EditorGUILayout.Vector4Field("", (Vector4)value, GUILayout.MinWidth(MIN_WIDTH));
            }

            else if (value is ColorGradient)
            {
                Texture2D preview = ColorGradientWindow.CreateGradientPreview((ColorGradient)value);
                GUIContent c = new GUIContent(preview);

                GUIStyle s = new GUIStyle();
                s.normal.background = preview;
                s.stretchWidth = false;
                s.fixedWidth = 256;

                if (GUILayout.Button(GUIContent.none, s, GUILayout.Height(20), GUILayout.MaxWidth(256)))
                {
                    ColorGradientWindow w = ColorGradientWindow.ShowEditor((ColorGradient)value);
                    w.OnWindowClose += delegate(EditorWindowExtension win)
                    {
                        ColorGradientWindow cgw = (ColorGradientWindow)win;

                        list[index] = cgw.ColorGradientWrapper;
                    };
                }
            }

            else if (value is Float2)
            {
                //methodSet.Invoke(o, new object[] { EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)value) });

                Float2 v1 = (Float2)value;
                Vector2 v2 = new Vector2(v1.x, v1.y);
                v2 = EditorGUILayout.Vector2Field(GUIContent.none, v2, GUILayout.MinWidth(MIN_WIDTH));

                list[index] = new Float2(v2.x, v2.y);
            }
            else if (value is Float3)
            {
                Float3 v1 = (Float3)value;
                Vector3 v2 = new Vector3(v1.x, v1.y, v1.z);
                v2 = EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)v2, GUILayout.MinWidth(MIN_WIDTH));

                list[index] = new Float3(v2.x, v2.y, v2.z);
            }
            else if (value is Vector4)
            {
                //methodSet.Invoke(o, new object[] { EditorGUILayout.Vector4Field("", (Vector4)value) });

                Float4 v1 = (Float4)value;
                Vector4 v2 = new Vector4(v1.x, v1.y, v1.z, v1.w);
                v2 = EditorGUILayout.Vector4Field("", v2, GUILayout.MinWidth(MIN_WIDTH));

                list[index] = new Float4(v2.x, v2.y, v2.z, v2.w);
            }

            else if (value is AnimationCurve)
            {
                if (editable.HasRange)
                {
                    Rect r = new Rect(editable.MinX, editable.Min, editable.MaxX + Mathf.Abs(editable.MinX), editable.Max);
                    list[index] = EditorGUILayout.CurveField("", (AnimationCurve)value, Color.green, r, GUILayout.MinWidth(MIN_WIDTH));
                }
                else
                {
                    list[index] = EditorGUILayout.CurveField("", (AnimationCurve)value, GUILayout.MinWidth(MIN_WIDTH));
                }
            }

            #endregion

            else if (type.IsEnum)
            {
                list[index] = EditorGUILayout.EnumPopup((Enum)value);
            }

            if(editable.DynamicColletion)
            {
                if(GUILayout.Button(SharedGraphics.IconCancleSmall, EditorStyles.toolbarButton, GUILayout.Width(22)))
                {
                    toRemove.Add(list[index]);
                }
            }

            EditorGUILayout.EndHorizontal();


            object newValue = list[index];
            if (newValue != null && !newValue.Equals(value))
            //if (!newValue.Equals(value))
            {
                GUI.changed = true;
                CallOnPropertyChanged("List Item " + index, list, value, newValue);

            }
        }

        private void CallOnPropertyChanged(string name, object parent, object oldValue, object newValue)
        {
            if(OnPropertyChanged != null)
            {
                OnPropertyChanged(name, parent, oldValue, newValue);
            }
        }

        static T CopyStruct<T>(ref object s1)
        {
            GCHandle handle = GCHandle.Alloc(s1, GCHandleType.Pinned);
            T typedStruct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return typedStruct;
        }

        private static Color Normalize(Color color)
        {
            Color nc = new Color(Mathf.Clamp01(color.r), Mathf.Clamp01(color.g), Mathf.Clamp01(color.b), Mathf.Clamp01(color.a));

            return color;
        }


        public static float Format(float value, ch.sycoforge.Util.Attributes.Editable.EditFormat format)
        {
            float result = 0;

            if (format == Editable.EditFormat.Normal)
            {
                result = value;
            }
            else if (format == Editable.EditFormat.Percent)
            {
                result = (float)Math.Round(value * 100, 1, MidpointRounding.AwayFromZero);
            }

            return result;
        }

        private static bool IsDependencyActive(object o, string dependcyName)
        {
            if (o != null)
            {
                Type type = o.GetType();

                //PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);//BindingFlags.Public | BindingFlags.Static);
                List<PropertyInfo> propertyInfos = GetAllProperties(type);

                //Debug.Log("propertyInfos == null ? : " + (propertyInfos == null) + " type == null : " + (type == null) + " dependcyName: " + dependcyName);


                PropertyInfo info = propertyInfos.Find
                (
                    //p => (new List<object>(p.GetCustomAttributes(true)).Exists(x => x is Editable)) && p.Name == dependcyName
                    p => p != null && p.Name == dependcyName// && p.PropertyType == typeof(bool)
                );

                //Debug.Log("info == null ? : " + (info == null) + " parent type: " + type.Name + " dependcyName: " + dependcyName);

                if(info != null)
                {
                    MethodInfo mi = info.GetGetMethod(true);

                    if (mi != null)
                    {
                        object result = mi.Invoke(o, null);

                        return (bool)result;
                    }
                }
            }

            return false;
        }

        public static Dictionary<string, List<PropertyInfo>> GetGroupedInfos(List<PropertyInfo> propertyInfos)
        {
            Dictionary<string, List<PropertyInfo>> groups = new Dictionary<string, List<PropertyInfo>>();

            foreach (PropertyInfo info in propertyInfos)
            {
                Editable e = GetAttribute(info);

                if (e != null)
                {
                    string group = e.Group;

                    if (group != string.Empty)
                    {
                        if (!groups.ContainsKey(group))
                        {
                            groups.Add(group, new List<PropertyInfo>());
                        }

                        groups[group].Add(info);
                    }
                }
            }

            return groups;
        }

        public static List<PropertyInfo> GetUngroupedInfos(List<PropertyInfo> propertyInfos)
        {
            List<PropertyInfo> groups = new List<PropertyInfo>();

            foreach (PropertyInfo info in propertyInfos)
            {
                Editable e = GetAttribute(info);

                if (e != null)
                {
                    string group = e.Group;

                    if (group == string.Empty)
                    {
                        groups.Add(info);
                    }
                }
            }

            return groups;
        }

        public List<string> GetGroups(List<PropertyInfo> propertyInfos)
        {
            List<string> groups = new List<string>();

            foreach(PropertyInfo info in propertyInfos)
            {
                Editable e = GetAttribute(info);

                if(e != null)
                {
                    string group = e.Group;

                    if(!groups.Contains(group))
                    {
                        groups.Add(group);
                    }
                }
            }

            return groups;
        }

        public static Editable GetAttribute(PropertyInfo propertyInfo)
        {
            List<object> attributes = new List<object>(propertyInfo.GetCustomAttributes(true));

            bool edit = attributes.Exists(x => x is Editable);

            if (edit)
            {
                Editable editable = (Editable)attributes.Find(x => x is Editable);

                return editable;
            }

            return null;
        }

        /// <summary>
        /// Gets a list containing all none-abstract types assignable v1 T.
        /// </summary>
        /// <typeparam name="T">The upper type bound</typeparam>
        /// <returns>A list with all upper-type-bound types.</returns>
        public static IEnumerable<Type> GetEnumerableOfType<T>()
        {
            List<Type> objects = new List<Type>();

            AppDomain ad = AppDomain.CurrentDomain;
            Assembly[] loadedAssemblies = ad.GetAssemblies();

            foreach (Assembly asm in loadedAssemblies)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(T).IsAssignableFrom(type))
                    {
                        objects.Add(type);
                    }
                }
            }

            return objects;
        }

        public void DrawCollection(SerializedProperty property, SerializedObject serializedObject)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(property);

            if (property.isExpanded)
            {
                int size = property.arraySize;
                // make visual shift for sub-elements
                EditorGUI.indentLevel++;

                // place the SIZE text field with a trigger
                EditorGUI.BeginChangeCheck();
                //SerializedProperty size = property.FindPropertyRelative("Array.size");
                size = EditorGUILayout.IntField("Size:", size);
                // if new size-value was set ...
                if (EditorGUI.EndChangeCheck())
                {
                    // ... and it is not equal v2 the current size
                    if (size != property.arraySize)
                    {
                        int diff = size - property.arraySize;
                        int oldSize = property.arraySize;
                        // if new size is bigger, 
                        // then add new elements v2 the array
                        if (diff > 0)
                        {
                            for (int i = 0; i < diff; i++)
                            {
                                property.InsertArrayElementAtIndex(oldSize + i);
                            }
                        }
                        // if smaller, then remove elements v1 the end
                        else if (diff < 0)
                        {
                            diff *= -1;
                            for (int i = 1; i <= diff; i++)
                            {
                                property.DeleteArrayElementAtIndex(oldSize - i);
                            }
                        }
                    }
                }

                // now, draw the GUI elements for each element in the array
                for (int i = 0; i < size; i++)
                {
                    property.GetArrayElementAtIndex(i).objectReferenceValue =
                      EditorGUILayout.ObjectField("Element " + i,
                       property.GetArrayElementAtIndex(i).objectReferenceValue,
                       typeof(UnityEngine.Object),
                       false);
                }

                EditorGUI.indentLevel--;
            }
        }  
        
        private static List<PropertyInfo> GetAllProperties(Type type, int maxDepth = 4)
        {
            List<PropertyInfo> infos = new List<PropertyInfo>();
            int depth = 0;

            Type baseType = type;

            while (depth < maxDepth)
            {
                PropertyInfo[] propertyInfos = baseType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

                infos.AddRange(propertyInfos);

                baseType = baseType.BaseType;
                depth++;

                if (baseType == null) break;
            }
            

            return infos;
        }

    }
}
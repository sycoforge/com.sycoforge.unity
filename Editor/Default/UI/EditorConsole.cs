using ch.sycoforge.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.UI
{
    [Serializable]
    public class EditorConsole
    {
        //---------------------------
        // Events
        //---------------------------
        public event Action OnClear;


        //---------------------------
        // Properties
        //---------------------------
        public bool ClearOnPlay
        {
            get { return clearOnPlay; }
            private set { clearOnPlay = value; }
        }

        public Action OnEnterPlayMode
        {
            get { return onEnterPlayMode; }
            set { onEnterPlayMode = value; }
        }

        //---------------------------
        // Fields
        //---------------------------
        private Dictionary<int, ConsoleEntry> entries = new Dictionary<int, ConsoleEntry>();
        private EntryType filter;
        private Vector2 scrollPos;
        private GUIStyle filterButtonStyle;

        [SerializeField]
        private bool clearOnPlay;

        [SerializeField]
        private bool showInfo = true;

        [SerializeField]
        private bool showEvents = true;

        [SerializeField]
        private bool showWarnings = true;

        [SerializeField]
        private bool showErrors = true;

        [SerializeField]
        private bool showClearOnPlay;

        private Action onEnterPlayMode;

        private Dictionary<EntryType, int> logCounts;

        //---------------------------
        // Constructor
        //---------------------------

        public EditorConsole(bool showClearOnPlay)
        {
            this.showClearOnPlay = showClearOnPlay;
        }

        //---------------------------
        // Methods
        //---------------------------

        //---------------------------
        // Static Methods
        //---------------------------

        public static Texture2D GetIcon(EntryType type)
        {
            Texture2D icon = SharedGraphics.IconDialogInfo;

            if (type == EntryType.Info)
            {
                icon = SharedGraphics.IconDialogInfo;
            }
            else if (type == EntryType.Success)
            {
                icon = SharedGraphics.IconDialogSuccess;
            }
            else if (type == EntryType.Warning)
            {
                icon = SharedGraphics.IconDialogWarning;
            }
            else if (type == EntryType.Event)
            {
                icon = SharedGraphics.IconDialogEvent;
            }
            else if (type == EntryType.Error)
            {
                icon = SharedGraphics.IconDialogError;
            }

            return icon;
        }

        public void Clear()
        {
            entries.Clear();
            logCounts[EntryType.Error] = 0;
            logCounts[EntryType.Event] = 0;
            logCounts[EntryType.Info] = 0;
            logCounts[EntryType.Warning] = 0;

            if(OnClear !=  null)
            {
                OnClear();
            }
        }

        public void Log(ConsoleEntry entry)
        {
            if(entry != null)
            {
                int key = entry.GetHashCode();

                if(!entries.ContainsKey(key))
                {
                    entries.Add(key, entry);
                }
                else
                {
                    entries[key].Increase();
                }

                logCounts[entry.Type]++;
            }
        }

        public void Log(string message, EntryType type, object context)
        {
            Log(new ConsoleEntry(message, type, context));

            //GUIUtility.ExitGUI();
        }

        public void LogError(string message, object context, FixDelegate fixRoutine = null)
        {
            Log(new ConsoleEntry(message, EntryType.Error, context, fixRoutine));

            //GUIUtility.ExitGUI();
        }

        public void Draw(Rect position)
        {
            CheckDictionary();
            InitStyles();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(50)))
            {
                Clear();
            }

            if (showClearOnPlay)
            {
                //clearOnPlay = EditorGUILayout.Toggle(clearOnPlay, new GUIStyle("ToolbarButton"));
                clearOnPlay = GUILayout.Toggle(clearOnPlay, "Clear on Play", new GUIStyle("ToolbarButton"), new GUILayoutOption[0]);
            }

            GUILayout.FlexibleSpace();
            showInfo = GUILayout.Toggle(showInfo, new GUIContent(logCounts[EntryType.Info].ToString(), SharedGraphics.IconDialogInfo, "Show info logs."), filterButtonStyle, new GUILayoutOption[0]);
            showEvents = GUILayout.Toggle(showEvents, new GUIContent(logCounts[EntryType.Event].ToString(), SharedGraphics.IconDialogEvent, "Show event logs."), filterButtonStyle, new GUILayoutOption[0]);
            showWarnings = GUILayout.Toggle(showWarnings, new GUIContent(logCounts[EntryType.Warning].ToString(), SharedGraphics.IconDialogWarning, "Show warning logs."), filterButtonStyle, new GUILayoutOption[0]);
            showErrors = GUILayout.Toggle(showErrors, new GUIContent(logCounts[EntryType.Error].ToString(), SharedGraphics.IconDialogError, "Show error logs."), filterButtonStyle, new GUILayoutOption[0]);


            EditorGUILayout.EndHorizontal();
            SetFilter();
            

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(200));
            EditorGUILayout.BeginVertical(GUILayout.MaxHeight(200));

            List<ConsoleEntry> toRemove = new List<ConsoleEntry>();



            int i = 0;
            //for (int i = 0; i < ent.Count; i++)
            foreach (ConsoleEntry entry in entries.Values)
            {
                if (IsFilterActive(entry.Type))
                {
                    bool even = (i % 2) == 0;

                    //Texture2D icon = SharedGraphics.IconDialogInfo;

                    //if (entry.Type == EntryType.Info)
                    //{
                    //    icon = SharedGraphics.IconDialogInfo;
                    //}
                    //else if (entry.Type == EntryType.Warning)
                    //{
                    //    icon = SharedGraphics.IconDialogWarning;
                    //}
                    //else if (entry.Type == EntryType.Event)
                    //{
                    //    icon = SharedGraphics.IconDialogEvent;
                    //}
                    //else if (entry.Type == EntryType.Error)
                    //{
                    //    icon = SharedGraphics.IconDialogError;
                    //}

                    Texture2D icon = GetIcon(entry.Type);

                    EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(new GUIContent(icon), GUILayout.MaxWidth(25));

                    EditorGUILayout.LabelField(entry.Message, SharedStyles.DefaultLabel, GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();

                    if(entry.HasFixRoutine)
                    {
                        if(GUILayout.Button(" Fix "))
                        {
                            bool success = entry.TryFixing();
                            if(!success)
                            {
                                EditorUtility.DisplayDialog("Error", "Could not auto-fix: " + entry.Message, "OK");
                            }
                            else
                            {
                                toRemove.Add(entry);
                            }
                        }
                    }

                    EditorGUILayout.LabelField(entry.ThrowCount.ToString(), GUILayout.MaxWidth(50));
                    EditorGUILayout.EndHorizontal();
                }

                i++;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            foreach(ConsoleEntry e in toRemove)
            {
                Remove(e);
            }
        }

        private void Remove(ConsoleEntry entry)
        {
            if (entry != null)
            {
                int key = entry.GetHashCode();

                if (entries.ContainsKey(key))
                {

                    logCounts[entry.Type]--;

                    entries.Remove(key);
                }
            }
        }

        private void SetFilter()
        {
            EntryType info = showInfo ? EntryType.Info : EntryType.None;
            EntryType events = showEvents ? EntryType.Event : EntryType.None;
            EntryType warnings = showWarnings ? EntryType.Warning : EntryType.None;
            EntryType errors = showErrors ? EntryType.Error : EntryType.None;

            filter = info | events | warnings | errors;
        }

        private bool IsFilterActive(EntryType type)
        {
            return (filter & type) > 0;
        }

        private void CheckDictionary()
        {
            if(entries == null)
            {
                entries = new Dictionary<int, ConsoleEntry>();
            }

            if(logCounts == null)
            {
                logCounts = new Dictionary<EntryType, int>();
                logCounts.Add(EntryType.Error, 0);
                logCounts.Add(EntryType.Event, 0);
                logCounts.Add(EntryType.Info, 0);
                logCounts.Add(EntryType.Warning, 0);
            }
        }

        private void InitStyles()
        {
            if (filterButtonStyle == null)
            {
                filterButtonStyle = new GUIStyle(new GUIStyle("ToolbarButton"));
                filterButtonStyle.padding = new RectOffset(0, 0, 1, 1);
            }
        }
    }
}

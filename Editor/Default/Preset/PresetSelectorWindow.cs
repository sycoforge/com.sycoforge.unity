using ch.sycoforge.Shared;
using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    internal class PresetSelectorWindow : EditorWindow, ISerializationCallbackReceiver //where P : BasePreset
    {
        //------------------------------
        // Events
        //------------------------------

        //-----------------------------
        // Static Fields
        //-----------------------------
        internal static PresetSelectorWindow WindowInstance;


        //------------------------------
        // Private Fields
        //------------------------------ 

        //private string username = "ismix";
        //private string password = "crossfader87";

        private string searchTag = string.Empty;


        //private PresetWebservice<MapLabPreset> manager;
        private PresetGrid<BasePreset> presetGrid;
        //private PresetBrowserBase browser;
        private WebPresetProvider provider;

        private Action<BasePreset> onPresetSelected;
        private Type presetType;

        //------------------------------
        // Methods
        //------------------------------
        public virtual void OnEnable()
        {


            WindowInstance = this;

            EditorApplication.update -= EditorUpdate;
            EditorApplication.update += EditorUpdate;
        }

        private void Initialize<P>() where P : BasePreset
        {

            //WebPresetProvider provider = PresetUtility.FindProvider("nu-artifactz");

            //Debug.Log("2) WebPresetProvider = null? " + (provider == null));
            //Debug.Log("WebPresetProvider :: " + provider.AppSKU);

            //browser = PresetBrowserBase.CreateBrowserInstance(provider, ViewMode.UserView);
            //browser.SetPresetCategory("Artifactz");

            //List<P> presets = EditorExtension.FindAllAssets<P>();
            //List<BasePreset> castedPresets = new List<BasePreset>();

            //foreach(P p in presets)
            //{
            //    castedPresets.Add(p);
            //}

            presetType = EditorExtension.FindType(provider.PresetTypeName);

            presetGrid = new PresetGrid<BasePreset>(SharedStyles.BoxStyle, 4);
            presetGrid.Collection = FindPresets();
            presetGrid.ItemSizeMode = Unity.Editor.UI.SizeMode.Square;
            presetGrid.OnItemClicked += grid_OnItemClicked;

            


            Debug.Log("Initialize :: " +presetType);
        }



        public virtual void OnDisable()
        {
            //grid.Dispose();
        }


        protected static void EditorUpdate()
        {
            //AsyncWorker.Process();

            //BusyUI.Update();

            if(WindowInstance != null)
            {
                WindowInstance.Repaint();
            }
            else
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        //void grid_OnItemClicked(P item, int index)
        //{
        //    Debug.Log("Clicked: " + item.Title);
        //}

        public void OnLostFocus()
        {
            Close();
        }

        public void OnGUI()
        {
            if (presetGrid == null) { return; }

            Event evt = Event.current;


            bool searchChanged = false;
            string result = string.Empty;

            #region --- Search Filter ---
            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            //GUILayout.FlexibleSpace();

            EditorGUI.BeginChangeCheck();

            searchTag = GUILayout.TextField(searchTag, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(220));
            //GUILayout.Space(50);

            searchChanged = EditorGUI.EndChangeCheck();

            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared
                searchTag = string.Empty;
                GUI.FocusControl(null);

                searchChanged = true;
            }            
            
            GUILayout.FlexibleSpace();

            if(GUILayout.Button("Remove", EditorStyles.toolbarButton))
            {
                onPresetSelected(null);
                Close();
            }

            GUILayout.EndHorizontal();
            #endregion

            if(searchChanged)
            {
                presetGrid.Collection = FindPresets(searchTag);
            }

            presetGrid.Draw(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if(evt.clickCount > 1)
            {
                Close();
            }
        }

        private List<PresetGridItem> FindPresets(string search = "")
        {
            search = search.ToLower();
            
            //Type type = presetGrid.Collection.GetType().GetGenericArguments()[0];
            Type type = presetType;

            List<UnityEngine.Object> findings = EditorExtension.FindAllAssets(type);

            List<PresetGridItem> presets = new List<PresetGridItem>();

            foreach(UnityEngine.Object o in findings)
            {
                BasePreset p = (BasePreset)o;

                PresetGridItem item = null;

                if(search == string.Empty)
                {
                    item = new PresetGridItem(presetGrid.ItemStyle, presetGrid.LayoutMode, presetGrid.IconMargin, p);
                    presets.Add(item);
                }
                else
                {
                    string title = p.DsiplayName.ToLower();
                    string name = p.name.ToLower();

                    if(title.Contains(search) || name.Contains(search))
                    {
                        item = new PresetGridItem(presetGrid.ItemStyle, presetGrid.LayoutMode, presetGrid.IconMargin, p);

                        presets.Add(item);
                    }
                }

                if (item != null)
                {
                    item.OnNeedsRedraw += item_OnNeedsRedraw;
                }
            }


            return presets;
        }

        private void item_OnNeedsRedraw()
        {
            //Debug.Log("PresetSelector: REPAINT");

            //Repaint();
        }

        private void grid_OnItemClicked(PresetGridItem item, int index)
        {
            if(onPresetSelected != null)
            {
                //GUI.changed = true;
                onPresetSelected(item.Preset);
            }
            //else
            //{
            //    Debug.Log("Handler is null");
            //}
        }

        //------------------------------
        // Static Methods
        //------------------------------    




        public static void ShowWindow<P>(WebPresetProvider provider, Action<BasePreset> onPresetSelected) where P : BasePreset
        {
            PresetSelectorWindow window = (PresetSelectorWindow)EditorWindow.GetWindow(typeof(PresetSelectorWindow));
            window.position = new Rect(100, 100, 500, 400);
            window.minSize = new Vector2(400, 400);
            window.titleContent = new GUIContent("Preset Selector");
            window.provider = provider;
            window.onPresetSelected = onPresetSelected;
            window.Initialize<P>();

            Debug.Log("1) WebPresetProvider = null? " + (provider == null));


            //window.ShowAuxWindow();
            //window.ShowUtility();
            //window.ShowAsDropDown(new Rect(), Vector2.one * 300);
            window.Show();

            WindowInstance = window;
        }

        public void OnAfterDeserialize()
        {
            Debug.Log("OnAfterDeserialize");
        }

        public void OnBeforeSerialize()
        {
            Debug.Log("OnBeforeSerialize");
        }
    }
}



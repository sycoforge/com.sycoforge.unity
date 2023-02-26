using ch.sycoforge.Types;
using System;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    public enum ViewMode
    {
        PublisherView,
        UserView
    }

    public abstract class PresetBrowserBase : ScriptableObject
    {
        //------------------------------
        // Events
        //------------------------------
        public event Action OnNeedsRedraw;

        //------------------------------
        // Properties
        //------------------------------

        /// <summary>
        /// The current page.
        /// </summary>
        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// The items per page.
        /// </summary>
        public int ItemsPerPage
        {
            get { return itemsPerPage; }
            set { itemsPerPage = value; }
        }

        /// <summary>
        /// The overall page count in the current category.
        /// </summary>
        public int PageCount
        {
            get { return pageCount; }
        }

        /// <summary>
        /// The overall amount of items in the current category.
        /// </summary>
        public int ItemsInCategory
        {
            get { return itemsInCategory; }
        }

        ///// <summary>
        ///// The app SKU.
        ///// </summary>
        //public string AppSKU
        //{
        //    get { return appSKU; }
        //    protected set { appSKU = value; }
        //}

        ///// <summary>
        ///// The app name.
        ///// </summary>
        //public string AppName
        //{
        //    get { return appName; }
        //    protected set { appName = value; }
        //}

        /// <summary>
        /// The preset provider object.
        /// </summary>
        public WebPresetProvider Provider
        {
            get { return provider; }
            protected set { provider = value; }
        }

        /// <summary>
        /// The current category name.
        /// </summary>
        public string Category
        {
            get { return category; }
            protected set { category = value; }
        }

        /// <summary>
        /// The current view mode.
        /// </summary>
        public ViewMode Mode
        {
            get { return mode; }
            protected set { mode = value; }
        }

        public PresetManager PresetManager
        {
            get { return presetManager; }
        }

        //------------------------------
        // Private Fields
        //------------------------------ 

        protected WebPresetProvider provider;
        //protected string appSKU;
        //protected string appName;
        protected int itemsPerPage;

        protected string category;
        protected int page;
        protected int itemsInCategory;
        protected int pageCount = 1;
        protected ViewMode mode;

        protected PresetManager presetManager;

        //------------------------------
        // Constructor
        //------------------------------
        /// <summary>
        /// Constructs a new preset browser.
        /// </summary>
        /// <param name="appSKU">The application GUID in the web database. E.g. nu-map-lab</param>
        /// <param name="appName">The application name.</param>
        /// <param name="itemsPerPage">The amount of item per single page.</param>
        /// <param name="itemStyle">The style used to render a single item</param>
        public PresetBrowserBase(WebPresetProvider provider, int itemsPerPage, ViewMode mode)
        {
            presetManager = new PresetManager(provider);

            //this.appSKU = appSKU;
            //this.appName = appName;
            this.provider = provider;
            this.itemsPerPage = itemsPerPage;
            this.mode = mode;

        }

        //------------------------------
        // Methods
        //------------------------------


        public abstract void Dispose();

        public abstract void DownloadPublisherPresetList();

        public void SetPresetCategory(string category)
        {
            this.category = category;
        }

     
        public virtual void Draw(params GUILayoutOption[] options)
        {
          
        }

        protected void CallOnNeedsRedraw()
        {
            if (OnNeedsRedraw != null)
            {
                OnNeedsRedraw();
            }
        }

        //------------------------------
        // Static Methods
        //------------------------------

        public static PresetBrowserBase CreateBrowserInstance(WebPresetProvider provider, ViewMode mode)
        {
            //Type type = Type.GetType(provider.QualifiedTypeName);FindType
            Type type = EditorExtension.FindType(provider.BrowserTypeName);

            if (type != null)
            {
                if (typeof(PresetBrowserBase).IsAssignableFrom(type))
                {
                    object instance = EditorExtension.CreateCtorInstance(type, provider, mode);

                    return (PresetBrowserBase)instance;
                }
            }
            else
            {
                string msg = string.Format("Type ({0}) could not be reflected. Are you sure the needed plugin is installed?", provider.BrowserTypeName);
                string msgShort = string.Format("Plugin ({0}) could not be found.", provider.App);

                NotificationMessage.ShowMessage("Error", msgShort, UI.EntryType.Error);
                Debug.LogError(msg);
            }

            return null;
            //throw new Exception("Sycoforge: Plugin not found.");
        }




    }
}

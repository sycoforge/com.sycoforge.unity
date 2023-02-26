using UnityEngine;

namespace ch.sycoforge.Types
{
    public sealed class WebPresetProvider : ScriptableObject, IGridItem
    {
        //------------------------------
        // Properties
        //------------------------------


        public string DsiplayName
        {
            get { return app; }
            set { app = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string AppSKU
        {
            get { return appSKU; }
            set { appSKU = value; }
        }

        public string App
        {
            get { return app; }
            set { app = value; }
        }

        /// <summary>
        /// The fully qualified browser type name. The specified type has to to a sub class of <c>PresetBrowserBase</c>.
        /// </summary>
        public string BrowserTypeName
        {
            get { return browserTypeName; }
            set { browserTypeName = value; }
        }

        /// <summary>
        /// The fully qualified preset renderer type name. The specified type has to implement <c>IPresetThumbRenderer</c> interface.
        /// </summary>
        public string RendererTypeName
        {
            get { return rendererTypeName; }
            set { rendererTypeName = value; }
        }

        /// <summary>
        /// The fully qualified preset type name.
        /// </summary>
        public string PresetTypeName
        {
            get { return presetTypeName; }
            set { presetTypeName = value; }
        }


        public Texture2D Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        //------------------------------
        // Fields
        //------------------------------

        [SerializeField]
        private string appSKU;

        [SerializeField]
        private string app;

        [SerializeField]
        private string version;

        [SerializeField]
        private string browserTypeName;

        [SerializeField]
        private string rendererTypeName;

        [SerializeField]
        private string presetTypeName;

        [SerializeField]
        private Texture2D thumbnail;

    }
}

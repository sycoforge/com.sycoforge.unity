using System;
using System.Collections.Generic;
using UnityEngine;

namespace ch.sycoforge.Types
{
    public abstract class BasePreset : ScriptableObject, IWebPreset, IGridItem
    {
        //---------------------------------
        // Properties
        //---------------------------------

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public int RatingCount
        {
            get { return ratingCount; }
            set { ratingCount = value; }
        }

        public int Downloads
        {
            get { return downloads; }
            set { downloads = value; }
        }

        public int UserRating
        {
            get { return userRating; }
            set { userRating = value; }
        }

        public Texture2D Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        public string DsiplayName
        {
            get { return title; }
            set { title = value; }
        }

        public string AppSKU
        {
            get { return appSKU; }
            set { appSKU = value; }
        }

        public string ThumbExtension
        {
            get { return thumbExt; }
            set { thumbExt = value; }
        }

        public string FileExtension
        {
            get { return fileExt; }
            set { fileExt = value; }
        }

        public int FileVersion
        {
            get { return fileVersion; }
            set { fileVersion = value; }
        }

        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public bool External
        {
            get { return external; }
            set { external = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public bool IsDownloading
        {
            get;
            set;
        }

        public bool IsWebPreset
        {
            get { return !string.IsNullOrEmpty(guid); }
        }

        //---------------------------------
        // Fields
        //---------------------------------


        [SerializeField]
        private string title;

        [SerializeField]
        private string appSKU;

        [SerializeField]
        private bool external;

        [SerializeField]
        private Texture2D thumbnail;

        [SerializeField]
        private int rating = -1;

        [SerializeField]
        private int ratingCount = 0;

        [SerializeField]
        private int downloads = 0;

        [SerializeField]
        private int userRating = -1;

        [SerializeField]
        private int id = -1;

        [SerializeField]
        private string thumbExt;

        [SerializeField]
        private string fileExt;

        [SerializeField]
        private int fileVersion;

        [SerializeField]
        private string appVersion;

        [SerializeField]
        private string guid;

        [SerializeField]
        private string createdBy;

        [SerializeField]
        private DateTime created;

        [SerializeField]
        private string category;

        //---------------------------------
        // Methods
        //---------------------------------

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDestroy()
        {
            Debug.Log("OnDestroy Preset:: " + name);

            if (thumbnail != null)
            {
                ScriptableObject.DestroyImmediate(thumbnail, true);
            }
        }

        protected virtual void OnDisable()
        {
            Debug.Log("OnDisable Preset:: " + name);

            if (thumbnail != null)
            {
                //ScriptableObject.DestroyImmediate(thumbnail, true);
            }
        }
    }
}

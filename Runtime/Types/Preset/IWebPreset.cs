using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Types
{
    public interface IWebPreset
    {
        int ID
        {
            get;
            set;
        }

        string GUID
        {
            get;
            set;
        }

        string ThumbExtension
        {
            get;
            set;
        }

        string FileExtension
        {
            get;
            set;
        }

        int FileVersion
        {
            get;
            set;
        }

        string AppVersion
        {
            get;
            set;
        }

        string AppSKU
        {
            get;
            set;
        }

        string CreatedBy
        {
            get;
            set;
        }

        string Category
        {
            get;
            set;
        }

        DateTime Created
        {
            get;
            set;
        }

        bool IsDownloading
        {
            get;
            set;
        }

        int Downloads
        {
            get;
            set;
        }

        int Rating
        {
            get;
            set;
        }

        int RatingCount
        {
            get;
            set;
        }

        int UserRating
        {
            get;
            set;
        }
    }
}

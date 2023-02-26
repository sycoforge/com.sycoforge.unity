using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Types
{
    public interface IPresetThumbRenderer//<P> where P : BasePreset
    {
        Texture2D RenderThumbnail(BasePreset preset);
    }
}

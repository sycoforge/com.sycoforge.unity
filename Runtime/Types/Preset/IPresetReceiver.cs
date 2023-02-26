using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Types
{
    public interface IPresetReceiver<P> where P : BasePreset
    {
        P Preset
        {
            get;
            set;
        }

    }
}

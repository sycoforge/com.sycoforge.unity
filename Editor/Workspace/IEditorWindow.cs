using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Editor.Workspace
{
    public interface IEditorWindow
    {
        FloatRect Position
        {
            get;
            set;
        }

        void Repaint();

    }
}

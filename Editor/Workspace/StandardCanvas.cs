using ch.sycoforge.Device;
using ch.sycoforge.Unity.Editor.Workspace;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public class StandardCanvas : CanvasBase
    {

        //------------------------------
        // Events
        //------------------------------


        //------------------------------
        // Properties
        //------------------------------


        //------------------------------
        // Static Fields
        //------------------------------

        //------------------------------
        // Private Fields
        //------------------------------ 
        //protected Vector2 lastPositionNormalized;



        //------------------------------
        // Constructor
        //------------------------------

        public StandardCanvas(IEditorWindow window, RectOffset margin)
            : base(window, margin)
        {

        }

        //------------------------------
        // Public Methods
        //------------------------------

        public override void Update(NativeEvent evt)
        {
            base.Update(evt);
        }

        //------------------------------
        // Handler Methods
        //------------------------------



        //------------------------------
        // Event Methods
        //------------------------------

    }
}

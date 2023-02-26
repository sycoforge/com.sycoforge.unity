using ch.sycoforge.Unity.Editor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public class UIPropertyAction : BaseAction
    {
        //------------------------------
        // Properties
        //------------------------------

        //------------------------------
        // Fields
        //------------------------------
        private PropertyData data;

        //------------------------------
        // Constructor
        //------------------------------
        public UIPropertyAction(PropertyData data)
        {
            this.data = data;
        }

        //------------------------------
        // Methods
        //------------------------------

        public override bool Do()
        {
            bool success = false;

            try
            {
                data.SetNew();

                success = true;
            }
            catch { }

            return success;
        }

        public override void Undo()
        {
            try
            {
                data.SetOld();
            }
            catch { }
        }
    }
}

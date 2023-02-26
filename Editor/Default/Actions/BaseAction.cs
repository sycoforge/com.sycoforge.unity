using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Editor
{
    public abstract class BaseAction
    {
        //------------------------------
        // Properties
        //------------------------------


        //------------------------------
        // Fields
        //------------------------------

        //------------------------------
        // Constructor
        //------------------------------

        public BaseAction()
        {

        }

        //------------------------------
        // Methods
        //------------------------------

        /// <summary>
        /// Executes the actions
        /// </summary>
        /// <returns>Returns <c>true</c> when the action successfully executed, <c>false</c> otherwise.</returns>
        public abstract bool Do();

        public abstract void Undo();
    }
}

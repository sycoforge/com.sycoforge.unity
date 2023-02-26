using ch.sycoforge.Types;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    

    public static class ActionController
    {

        //------------------------------
        // Fields
        //------------------------------
        private static BufferStack<BaseAction> undoneActions = new BufferStack<BaseAction>(20);
        private static BufferStack<BaseAction> doneActions = new BufferStack<BaseAction>(20);

        //------------------------------
        // Methods
        //------------------------------

        public static void Reset()
        {
            undoneActions.Clear();
            doneActions.Clear();
        }

        public static void Execute(BaseAction action)
        {
            bool success = action.Do();

            if (success)
            {
                doneActions.Push(action);
            }
        }

        public static void Undo()
        {
            if (doneActions.Count > 0)
            {
                BaseAction action = doneActions.Pop();
                action.Undo();

                //Debug.Log("Undo: " + action);


                undoneActions.Push(action);

                EditorGUIUtility.ExitGUI();
            }
        }


        public static void Redo()
        {
            if (undoneActions.Count > 0)
            {
                BaseAction action = undoneActions.Pop();
                action.Do();

                //Debug.Log("Do: " + action);

                doneActions.Push(action);

                EditorGUIUtility.ExitGUI();
            }
        }
    }
}

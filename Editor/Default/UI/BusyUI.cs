using ch.sycoforge.Shared;
using ch.sycoforge.Unity.Graphical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.UI
{
    [InitializeOnLoad]
    //public static class BusyUI
    public class BusyUI
    {
        //------------------------------
        // Fields
        //------------------------------
        private static float angularSpeed = 80;
        private static float angle;
        private static double lastUpdate = EditorApplication.timeSinceStartup;

        //------------------------------
        // Ctor
        //------------------------------
        //static BusyUI()
        public BusyUI()
        {
            EditorApplication.update -= EditorUpdate;
            EditorApplication.update += EditorUpdate;
        }
        
        //------------------------------
        // Methods
        //------------------------------
        //private static void EditorUpdate()
        private void EditorUpdate()
        {
            BusyUI.Update();
        }

        public static void Draw(Rect rect)
        {
            return;
            //Debug.Log("Busy Draw");
            if (rect.width > 0 && rect.height > 0)
            {
                float dx = rect.width / 6;
                float dy = rect.height / 6;

                Rect center = new Rect();
                center.width = 32;
                center.height = 32;
                center.center = rect.center;

                //Debug.Log("Draw: " + center + " angle: " + angle);

                GUIUtility.RotateAroundPivot(angle, center.center);

                //GUI.Box(center, new GUIContent(SharedGraphics.IconWait_32), SharedStyles.WaitBoxStyle);
                GUI.Box(center, GUIContent.none, SharedStyles.WaitBoxStyle);
                GUI.matrix = Matrix4x4.identity;
            }
        }

        private static void Update()
        {
            float dt = (float)(EditorApplication.timeSinceStartup - lastUpdate);

            angle += angularSpeed * dt;
            angle = angle % 360;

            lastUpdate = EditorApplication.timeSinceStartup;
        }
    }
}

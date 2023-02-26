using System;
using System.Collections.Generic;
using UnityEngine;


namespace ch.sycoforge.Unity.Runtime
{
    public static class CameraExtension
    {
        public static Camera Main
        {
            get
            {
                Camera c = Camera.main;

                if (c == null)
                {
                    Debug.LogError("No main camera found in scene.");
                }

                return c;
            }
        }
    }
}

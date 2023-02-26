using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Export
{
    /// <summary>
    /// Base class for all mesh exporters.
    /// </summary>
    public abstract class MeshExporter
    {
        //--------------------------------
        // Properties
        //--------------------------------

        //--------------------------------
        // Fields
        //--------------------------------
        protected string filetype;

        protected const string CREATOR = "&u Assets by Scoforge";

        //--------------------------------
        // Methods
        //--------------------------------


        public abstract bool SaveToFile(string path, Mesh mesh, Material[] matrials, Matrix4x4 rts, bool flipNormals);
    }
}

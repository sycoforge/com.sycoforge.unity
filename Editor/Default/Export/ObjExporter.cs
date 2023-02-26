using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Export
{
    public class ObjExporter : MeshExporter
    {
        //--------------------------------
        // Properties
        //--------------------------------


        //--------------------------------
        // Fields
        //--------------------------------
        private int startIndex = 0;

        private const string ERROR = "####Error####";

        //--------------------------------
        // Constructor
        //--------------------------------
        public ObjExporter()
        {
            this.filetype = "OBJ File";
        }

        //--------------------------------
        // Methods
        //--------------------------------

        public override bool SaveToFile(string path, Mesh mesh, Material[] materials, Matrix4x4 rts, bool flipNormals)
        {
            bool success = false;

            //try
            //{
                string content = MeshToString(mesh, materials, rts, flipNormals);
                File.WriteAllText(path, content);

                success = true;
            //}
            //catch { }

            return success;
        }

        private string MeshToString(Mesh mesh, Material[] materials, Matrix4x4 rts, bool flipNormals)
        {
            if (mesh == null)
            {
                return ERROR;
            }

            Quaternion rotation = Quaternion.LookRotation(rts.GetColumn(2), rts.GetColumn(1));

            int startIndex = 0;
            int numVertices = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append("#" + CREATOR + Environment.NewLine);

            foreach (Vector3 vv in mesh.vertices)
            {
                Vector3 v = rts.MultiplyPoint(vv);
                numVertices++;
                sb.Append(string.Format("v {0} {1} {2}{3}", v.x, v.y, v.z, Environment.NewLine));
            }
            sb.Append(Environment.NewLine);

            foreach (Vector3 nn in mesh.normals)
            {
                Vector3 v = rotation * nn;
                //v = new Vector3(-v.x, -v.y, v.z);
                v = new Vector3(v.x, v.y, v.z);
                v = flipNormals ? -v : v;

                sb.Append(string.Format("vn {0} {1} {2}{3}", v.x, v.y, v.z, Environment.NewLine));
            }
            sb.Append(Environment.NewLine);

            foreach (Vector3 v in mesh.uv)
            {
                sb.Append(string.Format("vt {0} {1}{2}", v.x, v.y, Environment.NewLine));
            }

            for (int matID = 0; matID < mesh.subMeshCount; matID++)
            {
                sb.Append(Environment.NewLine);

                if (matID < materials.Length)
                {
                    sb.Append("usemtl ").Append(materials[matID].name).Append(Environment.NewLine);
                    sb.Append("usemap ").Append(materials[matID].name).Append(Environment.NewLine);
                }

                int[] triangles = mesh.GetTriangles(matID);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}{3}",
                        triangles[i] + 1 + startIndex, triangles[i + 1] + 1 + startIndex, triangles[i + 2] + 1 + startIndex, Environment.NewLine));
                }
            }

            startIndex += numVertices;
            return sb.ToString();
        }
    }
}


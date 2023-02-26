using ch.sycoforge.Types;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Export
{
    public enum MeshFormat
    {
        OBJ
    }

    public class MeshExportertWindow : EditorWindowExtension
    {


        //--------------------------------
        // Fields
        //--------------------------------
        public Mesh Mesh;
        public Matrix4x4 TRS;
        public string Path = string.Empty;
        public MeshFormat Format;
        public bool ApplyTransform;

        private MeshExporter exporter;

        //--------------------------------
        // Methods
        //--------------------------------
        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Mesh export is currently in beta.", MessageType.Info);
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Path");

            EditorGUILayout.BeginHorizontal();
            Path = EditorGUILayout.TextField(Path);

            if (GUILayout.Button("...", EditorStyles.toolbarButton, GUILayout.Width(30)))
            {
                Path = EditorUtility.SaveFilePanel("Output File", Path, "decal-export", "obj");
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Mesh");
            Mesh = EditorGUILayout.ObjectField(Mesh, typeof(Mesh), true) as Mesh;

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Format");
            Format = (MeshFormat)EditorGUILayout.EnumPopup(Format);

            if (EditorGUI.EndChangeCheck() || exporter == null)
            {
                if (Format == MeshFormat.OBJ)
                {
                    exporter = new ObjExporter();
                }
            }

            GUILayout.Space(10);
            ApplyTransform = EditorGUILayout.Toggle(new GUIContent("Apply Transform", "Applies the world space transform to the mesh."), ApplyTransform);

            GUILayout.FlexibleSpace();

            if(GUILayout.Button("Export"))
            {
                if(exporter != null)
                {
                    if(!ApplyTransform || TRS == default(Matrix4x4))
                    {
                        TRS = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
                    }

                    exporter.SaveToFile(Path, Mesh, new Material[0], TRS, false);
                }
            }

            GUILayout.Space(10);
        }

        public static MeshExportertWindow ShowEditor(Mesh mesh, Matrix4x4 rts)
        {
            MeshExportertWindow window = (MeshExportertWindow)EditorWindow.GetWindow(typeof(MeshExportertWindow), true, "Mesh Export", false);
            window.Mesh = mesh;
            window.TRS = rts;

            return window;
        }

        public static MeshExportertWindow ShowEditor()
        {
            return ShowEditor(null, default(Matrix4x4));
        }
    }
}

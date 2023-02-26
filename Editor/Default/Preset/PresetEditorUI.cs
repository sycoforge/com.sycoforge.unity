using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Preset
{
    public static class PresetEditorUI
    {
        private static bool initializedStyles;
        private static GUIStyle styleDropArea;
        private static GUIStyle styleDropAreaLabel;

        public static P PresetField<P>(string name, P preset, WebPresetProvider provider, IPresetReceiver<P> receiver, Action onChanged) where P : BasePreset
        {
            P result = preset;

            Init();

            string title = preset != null ? preset.DsiplayName : "(None) " + typeof(P).Name;
            Texture2D thumbnail = preset != null ? preset.Thumbnail : null;

            Event evt = Event.current;

            Rect dropRect = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
            dropRect.x += 5;
            dropRect.y += 5;
            dropRect.width -= 10;
            dropRect.height = 25;

            Rect nameRect = dropRect;
            //nameRect.x += 5;
            nameRect.width = 150;
            nameRect.height = 25;

            Rect styleRect = dropRect;
            styleRect.x += nameRect.width;
            styleRect.width += 20 - nameRect.width;

            Rect thumbRect = dropRect;
            thumbRect.x += 3 + nameRect.width;
            thumbRect.y += 3;
            thumbRect.height -= 6;
            thumbRect.width = thumbRect.height;


            Rect titleRect = thumbRect;
            titleRect.x += thumbRect.width + 5;
            titleRect.width = dropRect.width - thumbRect.width;

            GUI.Label(nameRect, name);
            GUI.Box(styleRect, GUIContent.none, styleDropArea);
            GUI.DrawTexture(thumbRect, thumbnail);
            GUI.Label(titleRect, title);


            EditorGUIUtility.AddCursorRect(dropRect, MouseCursor.Link);
            

            switch (evt.type)
            {
                #region ---- Open Selector Window ----

                    
                case EventType.MouseUp:

                    if (dropRect.Contains(evt.mousePosition))
                    {
                        Action<BasePreset> onPresetSelected = delegate(BasePreset p)
                        {
                            if (receiver != null)
                            {
                                receiver.Preset = (P)p;
                            }

                            if (onChanged != null)
                            {
                                onChanged();
                            }
                        };

                        PresetSelectorWindow.ShowWindow<P>(provider, onPresetSelected);
                    }  
                  
                    break;
                #endregion

                #region ---- Drag and Drop ----

                case EventType.DragUpdated:
                case EventType.DragPerform:

                    if (!dropRect.Contains(evt.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        if (DragAndDrop.objectReferences.Length > 0)
                        {
                            object o = DragAndDrop.objectReferences[0];

                            if (o is P)
                            {
                                result = o as P;

                                GUI.changed = true;
                            }
                            else
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                            }
                        }
                    }
                    break;

                #endregion
            }

            return result;
        }

        private static void Init()
        {
            if(!initializedStyles)
            {
                styleDropArea = new GUIStyle("ObjectField");

                styleDropAreaLabel = new GUIStyle("Label");
                styleDropAreaLabel.alignment = TextAnchor.MiddleLeft;

                initializedStyles = true;
            }
        }
    }
}

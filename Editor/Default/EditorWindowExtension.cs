using ch.sycoforge.Types;
using ch.sycoforge.Unity.Editor.Workspace;
using ch.sycoforge.Util.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;

using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public delegate void OnWindowCloseHandler(EditorWindowExtension instance);
    public delegate void OnSizedChangedHandler(EditorWindowExtension instance, Vector2 oldSize, Vector2 newSize);

    public class EditorWindowExtension : UnityEditor.EditorWindow, IEditorWindow
    {
        //-----------------------------
        // Event
        //-----------------------------
        public event OnWindowCloseHandler OnWindowClose;
        public event OnSizedChangedHandler OnSizeChanged;

        /// <summary>
        /// Gets called before entering the play mode.
        /// </summary>
        public event Action OnPlayModeEntering;

        /// <summary>
        /// Gets called when entered the play mode.
        /// </summary>
        public event Action OnPlayModeEntered;

        /// <summary>
        /// Gets called when left the play mode.
        /// </summary>
        public event Action OnPlayModeLeft;

        /// <summary>
        /// Gets called when entered the pause mode.
        /// </summary>
        public event Action OnPauseModeEntered;

        /// <summary>
        /// Gets called when a new scne gets loaded. The name of the new scene gets passed to the handler method.
        /// </summary>
        public event Action<string> OnSceneChanged;

        //-----------------------------
        // Property
        //-----------------------------
        public Version AppVersion
        {
            get
            {
                return version;
            }
        }

        public FloatRect Position
        {
            get
            {
                FloatRect r = new FloatRect(position.xMin, position.yMin, position.width, position.height);
                return r;
            }
            set
            {
                position.Set(value.xMin, value.yMin, value.width, value.height);
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(position.width, position.height);
            }
            set
            {
                position.Set(position.xMin, position.yMin, value.x, value.y);
            }
        }


        //-----------------------------
        // Static Fields
        //-----------------------------

        //-----------------------------
        // Private Fields
        //-----------------------------
        private Version version;
        private Vector2 lastSize;
        private string lastSceneName;

        //-----------------------------
        // Constructor
        //-----------------------------
        public EditorWindowExtension()
        {

            //EditorApplication.update += EditorUpdate;
        }


        //-----------------------------
        // Static Methods
        //-----------------------------


        //-----------------------------
        // Methods
        //-----------------------------

        private Version GetVersion()
        {
            //string version = "-.-.-";

            //try
            //{
            //    System.Reflection.Assembly assembly = GetType().Assembly;
            //    System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            //    version = fvi.FileVersion;
            //}
            //catch (Exception ex) { }

            //return version;

            return EditorVersion.GetVersion(GetType());
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void OnEnable()
        {
            EditorApplication.update -= EditorUpdate;
            EditorApplication.update += EditorUpdate;

            EditorApplication.playmodeStateChanged -= EditorPlayModeChanged;
            EditorApplication.playmodeStateChanged += EditorPlayModeChanged;


            version = GetVersion();
            //AsyncWorker.Clear();
        }

        public virtual void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;

            if(OnWindowClose != null)
            {
                OnWindowClose(this);
            }

            //AsyncWorker.Clear();
        }

        public virtual void Update()
        {
            Vector2 size = Size;

            if(lastSize.magnitude != size.magnitude)
            {
                if (OnSizeChanged != null)
                {
                    OnSizeChanged(this, lastSize, size);
                }
            }

            lastSize = size;
        }

        private void EditorPlayModeChanged()
        {
            //if (!EditorApplication.isPlayingOrWillChangePlaymode)
            //{

            //    EditorCompositionControl.Stop();
            //}

            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                CallDelegate(OnPlayModeEntering);
            }
            else if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPaused)
            {
                CallDelegate(OnPauseModeEntered);

            }
            else if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
            {
                CallDelegate(OnPlayModeEntered);

            }
            else if (!EditorApplication.isPlaying)
            {
                CallDelegate(OnPlayModeLeft);

            }
        }

        /// <summary>
        /// Gets called ~100 times per second.
        /// </summary>
        protected virtual void EditorUpdate()
        {
            //AsyncWorker.Process();
            string sceneName = string.Empty;
            //sceneName = EditorApplication.currentScene;
#if UNITY_5_2 || UNITY_4
            sceneName = EditorApplication.currentScene;
#else
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            sceneName = scene.name;
#endif

            if (lastSceneName != sceneName)
            {
                CallOnSceneLoaded(sceneName);
            }

            lastSceneName = sceneName;
        }

        private void CallDelegate(Action d)
        {
            if(d != null)
            {
                d();
            }
        }

        private void CallOnSceneLoaded(string newSceneName)
        {
            if(OnSceneChanged != null)
            {
                OnSceneChanged(newSceneName);
            }
        }
    }
}
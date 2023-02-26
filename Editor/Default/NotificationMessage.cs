using ch.sycoforge.Shared;
using ch.sycoforge.Unity.Editor;
using ch.sycoforge.Unity.Editor.UI;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    public enum FadeDirection
    {
        Horizontal,
        Vertical
    }

    public enum FadeMode
    {
        None,
        FadeIn,
        FadeOut,
        Both
    }

    public class NotificationMessage : EditorWindow
    {


        protected enum State
        {
            FadeIn,
            Idle,
            FadeOut,
            Died
        }

        //------------------------------
        // Events
        //------------------------------

        //-----------------------------
        // Static Properties
        //-----------------------------
        private const string KEY_SHOW_NOTIFICATIONS = "SYCOFORGE_SHOW_NOTIFICATIONS";

        //-----------------------------
        // Static Properties
        //-----------------------------

        public static bool ShowNotifications
        {
            get
            {
                if (EditorPrefs.HasKey(KEY_SHOW_NOTIFICATIONS))
                {
                    showNotifications = EditorPrefs.GetBool(KEY_SHOW_NOTIFICATIONS);
                }

                return showNotifications;
            }

            set
            {
                if (showNotifications != value)
                {
                    EditorPrefs.SetBool(KEY_SHOW_NOTIFICATIONS, value);
                }

                showNotifications = value;
            }
        }

        //------------------------------
        // Properties
        //------------------------------  
        public float Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        public FadeDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public FadeMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public EntryType Type
        {
            get { return type; }
            set { type = value; }
        }


        public string Message
        {
            get;
            protected set;
        }

        public string Title
        {
            get;
            protected set;
        }

        //-----------------------------
        // Static Fields
        //-----------------------------

        //------------------------------
        // Private Fields
        //------------------------------  

        protected Texture2D icon;
        protected float progress;
        protected FadeDirection direction = FadeDirection.Horizontal;
        protected FadeMode mode = FadeMode.Both;
        protected EntryType type = EntryType.Info;

        protected Vector2 currentPosition;

        protected float lifetime = 2;
        protected float fadeTime = 0.5f;
        protected float age = 0;
        protected float lastUpdate;
        protected float speed = 20;

        private State state, lastState;

        private bool isClosing;
        private bool isInitialized;

        private static bool showNotifications = true;

        //------------------------------
        // Static Constructor
        //------------------------------

        //------------------------------
        // Constructor
        //------------------------------

        //------------------------------
        // Methods
        //------------------------------
        public virtual void OnEnable()
        {
            state = State.Idle;
            age = 0;
            lastUpdate = Time.realtimeSinceStartup;

            EditorApplication.update += EditorUpdate;

            //SetStartPosition();
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnGUI()
        {
            #region --- Close on Click ---
            Event evt = Event.current;

            if(evt.isMouse)
            {
                Rect r = new Rect(0, 0, this.position.width, this.position.height);

                if(r.Contains(evt.mousePosition))
                {
                    this.Close();
                }
            }
            #endregion

            EditorGUILayout.BeginHorizontal(SharedStyles.NotificationBoxStyle);
            EditorGUILayout.LabelField(new GUIContent(icon), GUILayout.Width(30), GUILayout.Height(30));

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(Title, SharedStyles.DefaultTitleLabel, GUILayout.ExpandWidth(true), GUILayout.Height(30));
            EditorGUILayout.LabelField(Message, SharedStyles.DefaultLabelWrap, GUILayout.ExpandWidth(true), GUILayout.Height(30));
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        //public void OnInspectorUpdate()
        private void EditorUpdate()
        {
            if (isClosing) { return; }

            if(!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }

            float dt = UpdateTime();
            UpdateState();

            if (state == State.FadeIn)
            {
                UpdatePosition(dt, -1);
            }
            else if (state == State.Idle && lastState == State.FadeIn)
            {
                currentPosition = Vector2.zero;
                this.position = new Rect(currentPosition, new Vector2(this.position.width, this.position.height));
            }
            else if (state == State.FadeOut)
            {
                UpdatePosition(dt, 1);
            }
            else if (state == State.Died)
            {
                this.Close();

                isClosing = true;
            }

            //Debug.Log("State: " + state);
        }

        private void UpdatePosition(float dt, float dir)
        {
            Vector2 d = direction == FadeDirection.Horizontal ? Vector2.left : Vector2.down;


            Rect rect = this.position;
            currentPosition = currentPosition + (d * dt * speed) * dir;


            this.position = new Rect(currentPosition, new Vector2(rect.width, rect.height));

            //Debug.Log("newPos: " + currentPosition + " p: " + this.position.position.x);

        }

        private void UpdateState()
        {
            lastState = state;

            if (age < fadeTime)
            {
                state = State.FadeIn;
            }
            else if (age < lifetime + fadeTime)
            {
                state = State.Idle;
            }
            else if (age < lifetime + (2 * fadeTime))
            {
                state = State.FadeOut;
            }
            else
            {
                state = State.Died;
            }
        }

        private float UpdateTime()
        {
            float dt = Time.realtimeSinceStartup - lastUpdate;

            age += dt;

            lastUpdate = Time.realtimeSinceStartup;

            return dt;
        }

        private void Initialize()
        {
            float d = (direction == FadeDirection.Horizontal ? this.position.width : this.position.height) - 20;
            speed = d / fadeTime;


            icon = EditorConsole.GetIcon(type);
        }

        private void SetStartPosition(int w = 300, int h = 100)
        {
            Vector2 size = new Vector2(w, h);
            currentPosition = new Vector2(0, 0);

            float d = direction == FadeDirection.Horizontal ? w : h;
            Vector2 dir = direction == FadeDirection.Horizontal ? Vector2.left : Vector2.down;

            if(mode == FadeMode.FadeIn || mode == FadeMode.Both)
            {
                currentPosition = dir * d;
            }

            
            //else if (mode == FadeMode.None)
            //{
            //    pos = dir * d;
            //}

            this.maxSize = size;
            this.minSize = size;

            this.position = new Rect(currentPosition, size);
        }

        //------------------------------
        // Static Methods
        //------------------------------    

        public static NotificationMessage ShowMessage(string title, string message, EntryType type, FadeDirection direction = FadeDirection.Horizontal)
        {
            NotificationMessage window = null;

            if (showNotifications)
            {
                window = ScriptableObject.CreateInstance<NotificationMessage>();
                window.Direction = direction;
                window.Type = type;
                window.Message = message;
                window.Title = title;
                window.SetStartPosition();
                window.ShowPopup();
            }

            return window;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ch.sycoforge.Unity.Runtime.Pooling
{
    [Serializable]
    public class GenericPool : BasePool<GameObject>
    {
        //---------------------------
        // Events
        //---------------------------

        //---------------------------
        // Properties
        //---------------------------

        /// <summary>
        /// Items in pool.
        /// </summary>
        public Stack<GameObject> PoolItems
        {
            get { return poolItems; }
            protected set { poolItems = value; }
        }

        /// <summary>
        /// Items spawned in scene.
        /// </summary>
        public Dictionary<int, GameObject> SceneItems
        {
            get { return sceneItems; }
            protected set { sceneItems = value; }
        }

        /// <summary>
        /// All items.
        /// </summary>
        public List<GameObject> Items
        {
            get { return items; }
            protected set { items = value; }
        }

        public string Name
        {
            get { return name; }
            protected set { name = value; }
        }

        public bool HasFixedSize
        {
            get { return hasFixedSize; }
            set { hasFixedSize = value; }
        }

        public int OverflowCount
        {
            get { return overflowCount; }
        }

        /// <summary>
        /// Amount of spawned items in scene.
        /// </summary>
        public int SpawnedCount
        {
            get { return sceneItems != null ? sceneItems.Count : 0; }
        }


        /// <summary>
        /// Amount of ready items waiting in pool.
        /// </summary>
        public int ReadyCount
        {
            get { return poolItems != null ? poolItems.Count : 0; }
        }

        /// <summary>
        /// The initial pool size.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// The absolute size of the pool (inclusive overflow). 
        /// </summary>
        public int AbsoluteSize
        {
            get { return items != null ? items.Count : -1; }
        }

        //---------------------------
        // Fields
        //---------------------------
        [SerializeField]
        private bool hasFixedSize = true;

        [SerializeField]
        private int size;

        [SerializeField]
        private int overflowCount;

        private Dictionary<int, GameObject> sceneItems;
        private Stack<GameObject> poolItems;

        private Action<GameObject, GenericPool> onInitialized;

        [SerializeField]
        private List<GameObject> items;


        [SerializeField]
        private string name;

        [SerializeField]
        private HideFlags flags;

        //[SerializeField]
        private GameObject[] prototypes;

        //---------------------------
        // Constructor
        //---------------------------
        public GenericPool()
            : this(string.Empty)
        {

        }

        public GenericPool(string name)
        {
            this.name = name;
        }

        //---------------------------
        // Methods
        //---------------------------

        public void Rebuild(Action<GameObject, GenericPool> onInitialized = null)
        {
            if (sceneItems != null)
            {
                sceneItems.Clear();
            }
            else
            {
                sceneItems = new Dictionary<int, GameObject>();
            }

            if (poolItems != null)
            {
                poolItems.Clear();
            }
            else
            {
                poolItems = new Stack<GameObject>();
            }

            foreach (GameObject o in items)
            {
                if (o != null)
                {
                    if (o.activeSelf)
                    {
                        int id = o.GetInstanceID();

                        if (!sceneItems.ContainsKey(id))
                        {
                            sceneItems.Add(id, o);
                        }
                    }
                    else
                    {
                        poolItems.Push(o);
                    }

                    if (onInitialized != null)
                    {
                        onInitialized(o, this);
                    }
                }
            }
        }

        public void Initialize<T>(int count, Action<GameObject, GenericPool> onOnitialized, HideFlags flags = HideFlags.None) where T : MonoBehaviour
        {
            Type type = typeof(T);
            string name = type.Name;

            GameObject obj = new GameObject(name, type);

            Initialize(obj, count, onOnitialized, flags);
        }

        public void Initialize(GameObject[] prefabs, int count, Action<GameObject, GenericPool> onIntialized, HideFlags flags = HideFlags.None)
        {
            InternalInitialize(count, prefabs, onIntialized, flags);
        }

        public void Initialize(GameObject prefab, int count, Action<GameObject, GenericPool> onIntialized, HideFlags flags = HideFlags.None)
        {
            InternalInitialize(count, new GameObject[]{prefab}, onIntialized, flags);
        }

        private void InternalInitialize(int count, GameObject[] prototypes, Action<GameObject, GenericPool> onIntialized, HideFlags flags)
        {
            //if(prototype.GetComponent<PoolElement>() == null)
            //{
            //    prototype.AddComponent<PoolElement>();
            //}

            this.size = Math.Max(count, 0);

            this.prototypes = prototypes;

            for (int i = 0; i < prototypes.Length; i++)
            {
                this.prototypes[i].SetActive(false);
            }
            
            //this.prototype.hideFlags = flags;

            this.flags = flags;
            this.onInitialized = onIntialized;

            sceneItems = new Dictionary<int, GameObject>(count);
            poolItems = new Stack<GameObject>(count);
            items = new List<GameObject>(count);

            FillPool(flags);
        }

        private void FillPool(HideFlags flags)
        {
            for (int i = 0; i < size; i++)
            {
                var p = InstantiatePrototype(i, flags);
                
                if(p.GetComponent<PoolElement>() == null)
                {
                    p.AddComponent<PoolElement>();
                }

                poolItems.Push(p);
            }
        }

        private GameObject InstantiatePrototype(int id, HideFlags flags)
        {
            if(prototypes == null)
            {
                Debug.LogError("Pool: Prototype has been destroyed.");
            }

            int index = prototypes.Length > 1 ? UnityEngine.Random.Range(0, prototypes.Length) : 0;
            var p = prototypes[index];

            GameObject clone = GameObject.Instantiate(p);
            clone.name = clone.name + " " + id;
            clone.SetActive(false);
            clone.transform.SetParent(p.transform.parent);

            //clone.hideFlags = flags;

            if (Application.isPlaying)
            {
                GameObject.DontDestroyOnLoad(clone);
            }

            if(onInitialized != null)
            {
                onInitialized(clone, this);
            }

            items.Add(clone);

            return clone;
        }

        public void Clear(bool destroy = true)
        {
            if(sceneItems != null)
            {
                sceneItems.Clear();
            }
            if (poolItems != null)
            {
                poolItems.Clear();
            }

            foreach (GameObject o in items)
            {
                if (destroy)
                {
                    if (Application.isPlaying)
                    {
                        GameObject.Destroy(o);
                    }
                    else
                    {
                        GameObject.DestroyImmediate(o);
                    }
                }
            }

            if (destroy)
            {
                if (Application.isPlaying)
                {
                    foreach (var o in prototypes) { GameObject.Destroy(o); }
                }
                else
                {
                    foreach (var o in prototypes) { GameObject.DestroyImmediate(o); }
                }
            }

            items.Clear();
        }

        public GameObject Spawn(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            CheckValidity();

            GameObject o = null;

            if (poolItems == null) { return null; }

            if (poolItems.Count == 0)
            {
                if (!hasFixedSize)
                {
                    o = InstantiatePrototype(overflowCount++, flags);

                    //items.Add(o);
                }
            }
            else
            {
                o = poolItems.Pop();
            }

            if(o != null)
            {
                o.SetActive(true);

                int id = o.GetInstanceID();

                if(!sceneItems.ContainsKey(id))
                {
                    sceneItems.Add(id, o);
                }

                sceneItems[id] = o;
            }

            CallOnPoolChanged();
            CallItemSpawned();



            return o;
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            return Spawn(position, Vector3.one, rotation);
        }

        public GameObject Spawn(Vector3 position)
        {
            return Spawn(position, Vector3.one, Quaternion.identity);
        }

        public override GameObject Spawn()
        {
            return Spawn(Vector3.one, Vector3.one, Quaternion.identity);
        }

        public override void Despawn(GameObject obj)
        {
            CheckValidity();

            int id = obj.GetInstanceID();

            if (sceneItems.ContainsKey(id))
            {
                GameObject o = sceneItems[id];

                Recycle(o);

                sceneItems.Remove(id);

                poolItems.Push(o);

                CallOnPoolChanged();
                CallItemDespawned();
            }
        }

        public void Recycle(GameObject item)
        {
            item.transform.SetParent(parent);
            item.SetActive(false);
        }

        public override void DespawnAll()
        {
            foreach (GameObject o in items)
            {
                Recycle(o);
            }
        }

        public override string ToString()
        {
            return name;
        }

        private void CheckValidity()
        {
            if(sceneItems == null || poolItems == null)
            {
                Rebuild();
            }
        }
    }
}


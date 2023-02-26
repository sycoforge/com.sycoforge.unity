using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ch.sycoforge.Unity.Runtime.Pooling
{
    public abstract class SimpleGameObjectPool : BasePool<UnityEngine.Object>//
    {
        //void j()
        //{
        //    UnityEngine.Object j;j.
        //}
    }

    public abstract class BasePool<T> : IPool//
    {
        //---------------------------
        // Events
        //---------------------------

        public event Action<BasePool<T>> PoolChanged
        {
            add { poolChanged += value; }
            remove { poolChanged -= value; }
        }

        public event Action ItemSpawned
        {
            add { itemSpawned += value; }
            remove { itemSpawned -= value; }
        }

        public event Action ItemDespawned
        {
            add { itemDespawned += value; }
            remove { itemDespawned -= value; }
        }

        private Action<BasePool<T>> poolChanged;
        private Action itemDespawned;
        private Action itemSpawned;

        //---------------------------
        // Properties
        //---------------------------

        //---------------------------
        // Fields
        //---------------------------


        protected Transform parent;

        //---------------------------
        // Constructor
        //---------------------------
        public BasePool()
        {

        }

        //---------------------------
        // Methods
        //---------------------------

        public abstract void DespawnAll();

        //public abstract void Recycle(T item);
        //{
        //    item.transform.SetParent(parent);
        //    item.SetActive(false);
        //}

        public abstract T Spawn();

        public abstract void Despawn(T item);

        /// <summary>
        /// Sets the parent transform where all pool items get spawned.
        /// </summary>
        /// <param name="parent"></param>
        public void SetItemParent(Transform parent)
        {
            this.parent = parent;
        }

        protected void CallOnPoolChanged()
        {
            if (poolChanged != null) { poolChanged(this); }
        }

        protected void CallItemSpawned()
        {
            if (itemSpawned != null) { itemSpawned(); }
        }

        protected void CallItemDespawned()
        {
            if (itemDespawned != null) { itemDespawned(); }
        }
    }
}


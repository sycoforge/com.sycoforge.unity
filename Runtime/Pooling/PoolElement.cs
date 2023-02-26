using ch.sycoforge.Unity.Runtime.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Runtime.Pooling
{
    public class PoolElement : MonoBehaviour
    {
        //--------------------------------
        // Events
        //--------------------------------

        public event Action<PoolElement> Spawned;
        public event Action<PoolElement> Despawned;

        //--------------------------------
        // Unity Methods
        //--------------------------------
        private SimpleGameObjectPool parent;

        //--------------------------------
        // Unity Methods
        //--------------------------------

        protected virtual void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }

        protected virtual void OnDestroy()
        {
            Debug.Log("On Pool Item, Destroy :: " + name);
        }

        //--------------------------------
        // Public Methods
        //--------------------------------

        protected void IntializeItem(SimpleGameObjectPool parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets called by the IObjectPool when being spawned.
        /// </summary>
        public void OnSpawn()
        {
            CallSpawned();
        }

        /// <summary>
        /// Gets called by the IObjectPool when being despawned.
        /// </summary>
        public void OnDespawn()
        {
            this.parent.Despawn(this.gameObject);

            CallDespawned();
        }

        /// <summary>
        /// Despawns the item.
        /// </summary>
        public void Despawn()
        {
            this.parent.Despawn(this.gameObject);
        }

        private void CallSpawned()
        {
            if(Spawned != null)
            {
                Spawned(this);
            }
        }

        private void CallDespawned()
        {
            if (Despawned != null)
            {
                Despawned(this);
            }
        }
    }
}

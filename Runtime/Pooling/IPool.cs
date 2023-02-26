using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Runtime.Pooling
{
    public interface IPool
    {
        void DespawnAll();
        //GameObject Spawn(Vector3 position, Quaternion rotation);

        //GameObject Spawn(Vector3 position);

        //GameObject Spawn();

        //void Despawn(GameObject obj);
    }
}

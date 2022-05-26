using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooler
{
    public class PooledObject : MonoBehaviour
    {
        internal ObjectPooler pool;

        void OnDisable()
        {
            pool.Release(this.gameObject);
        }
    }
}

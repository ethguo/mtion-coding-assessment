using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooler
{
    /// <summary>This non-generic ObjectPooler manages a pool of GameObjects.</summary>
    public class ObjectPooler
    {
        private GameObject m_prefab;
        private int m_maxPoolSize;
        private Stack<GameObject> m_inactiveStack;

        /// <summary>Constructs an ObjectPooler that manages a pool of GameObjects.</summary>
        /// <param name="prefab">The prefab to instantiate when creating new objects in the pool.</param>
        /// <param name="initialPoolSize">The number of objects to instantiate when the pool is created.</param>
        /// <param name="maxPoolSize">The maximum number of objects this pool will hold to reuse --
        ///     objects created beyond this limit will be destroyed.</param>
        public ObjectPooler(GameObject prefab, int initialPoolSize = 0, int maxPoolSize = 10)
        {
            m_prefab = prefab;
            m_maxPoolSize = maxPoolSize;
            m_inactiveStack = new Stack<GameObject>(maxPoolSize);

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject go = InstantiateNew();
                go.SetActive(false);
                m_inactiveStack.Push(go);
            }
        }

        /// <summary>Activates (or creates) and returns a GameObject from the pool.</summary>
        public GameObject Get()
        {
            if (m_inactiveStack.Count > 0)
            {
                GameObject go = m_inactiveStack.Pop();
                go.SetActive(true);
                return go;
            }
            else
            {
                return InstantiateNew();
            }
        }

        /// <summary>Returns a GameObject to the pool.</summary>
        internal void Release(GameObject obj)
        {
            if (m_inactiveStack.Count < m_maxPoolSize)
            {
                m_inactiveStack.Push(obj);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        private GameObject InstantiateNew()
        {
            GameObject go = Object.Instantiate(m_prefab);
            PooledObject po = go.AddComponent<PooledObject>();
            po.pool = this;
            return go;
        }

    }

    /// <summary>This generic version of ObjectPooler manages a pool of GameObjects with a specific Component.</summary>
    public class ObjectPooler<T> : ObjectPooler where T : Component
    {
        /// <summary>Constructs an ObjectPooler that manages a pool of GameObjects with a specific Component.</summary>
        /// <param name="prefab">The prefab to instantiate when creating new objects in the pool.</param>
        /// <param name="initialPoolSize">The number of objects to instantiate when the pool is created.</param>
        /// <param name="maxPoolSize">The maximum number of objects this pool will hold to reuse --
        ///     objects created beyond this limit will be destroyed.</param>
        public ObjectPooler(T prefab, int initialPoolSize = 0, int maxPoolSize = 10)
            : base(prefab.gameObject, initialPoolSize, maxPoolSize)
        { }

        /// <summary>Activates (or creates) a GameObject from the pool and returns the relevant Component.</summary>
        public new T Get()
        {
            return base.Get().GetComponent<T>();
        }
    }
}

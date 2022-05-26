using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooler
{
    public class ObjectPooler
    {

        private GameObject m_prefab;

        private int m_maxPoolSize;

        private Stack<GameObject> m_inactiveStack;

        public ObjectPooler(GameObject prefab, int maxPoolSize = 10)
        {
            m_prefab = prefab;
            m_maxPoolSize = maxPoolSize;
            m_inactiveStack = new Stack<GameObject>(maxPoolSize);
        }

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
                return GetNew();
            }
        }

        public void Release(GameObject obj)
        {
            m_inactiveStack.Push(obj);
        }

        private GameObject GetNew()
        {
            GameObject go = Object.Instantiate(m_prefab);
            PooledObject po = go.AddComponent<PooledObject>();
            po.pool = this;
            return go;
        }

    }

    public class ObjectPooler<T> : ObjectPooler where T : Component
    {
        public ObjectPooler(T prefab, int maxPoolSize = 10)
            : base(prefab.gameObject, maxPoolSize)
        { }

        public new T Get()
        {
            return base.Get().GetComponent<T>();
        }

        public void Release(T obj)
        {
            base.Release(obj.gameObject);
        }
    }
}

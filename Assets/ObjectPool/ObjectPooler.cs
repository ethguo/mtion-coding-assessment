using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooler
{
    public class ObjectPooler<T> where T : Object
    {
        private T m_prefab;

        private int m_maxPoolSize;

        private Stack<T> m_stack;

        public ObjectPooler(T prefab, int maxPoolSize = 10)
        {
            m_prefab = prefab;
            m_maxPoolSize = maxPoolSize;
            m_stack = new Stack<T>(maxPoolSize);
        }

        public T Get()
        {
            if (m_stack.Count > 0)
            {
                return m_stack.Pop();
            }
            else
            {
                return GetNew();
            }
        }

        private T GetNew()
        {
            return Object.Instantiate(m_prefab);
        }
    }
}

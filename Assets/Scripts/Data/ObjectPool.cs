using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Data
{
    public class ObjectPool<T> where T : class
    {
        private T prefab;
        private Stack<T> objectPool = new Stack<T>();

        public void SetObject(T prefab)
        {
            this.prefab = prefab;
        }

        public int GetLength()
        {
            return objectPool.Count;
        }

        public void Fill(int count, Transform t)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(prefab as Object, t) as T;
                
                Push(obj);
            }
        }

        public T Pull(Transform t)
        {
            if (objectPool.Count > 0)
            {
                T obj = objectPool.Pop();

                return obj;
            }

            return Object.Instantiate(prefab as Object, t) as T;
        }

        public T Pop()
        {
            if (objectPool.Count > 0)
                return objectPool.Pop();
            return null;
        }

        public void Push(T obj)
        {
            if (obj != null)
                objectPool.Push(obj);
        }

        public bool IsObjectNull()
        {
            return prefab == null;
        }
    }
}
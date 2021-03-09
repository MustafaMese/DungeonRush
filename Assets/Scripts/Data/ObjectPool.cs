using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Data
{
    public class ObjectPool
    {
        private GameObject prefab;
        private Stack<GameObject> objectPool = new Stack<GameObject>();

        public void SetObject(GameObject prefab)
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
                GameObject obj = Object.Instantiate(prefab, t) as GameObject;
                
                Push(obj);
            }
        }

        public GameObject Pull(Transform t)
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Pop();

                return obj;
            }

            return Object.Instantiate(prefab, t);
        }

        public GameObject Pop()
        {
            if (objectPool.Count > 0)
                return objectPool.Pop();
            return null;
        }

        public void Push(GameObject obj)
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
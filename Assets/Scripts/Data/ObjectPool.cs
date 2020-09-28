using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Data
{
    public class ObjectPool : MonoBehaviour
    {
        private GameObject prefab;
        private Stack<GameObject> objectPool = new Stack<GameObject>();

        public void SetObject(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public void FillPool(int count, Transform t)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab, t);
                AddObjectToPool(obj);
            }
        }

        public GameObject PullObjectFromPool(Transform t)
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Pop();
                obj.gameObject.SetActive(true);

                return obj;
            }

            return Instantiate(prefab, t);
        }

        public void AddObjectToPool(GameObject obj)
        {
            print("1");
            if (obj != null)
            {
                print("2");
                obj.gameObject.SetActive(false);
                objectPool.Push(obj);
            }
        }

        public void DeleteObjectsInPool()
        {
            int count = objectPool.Count;
            for(int i = 0; i < count; i++)
            {
                GameObject obj = objectPool.Pop();
                Destroy(obj.gameObject);
            }
        }

        public bool IsObjectNull()
        {
            return prefab == null;
        }
    }
}
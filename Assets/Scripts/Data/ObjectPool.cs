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

        public void FillPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                AddObjectToPool(obj);
            }
        }

        public GameObject PullObjectFromPool()
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Pop();
                obj.gameObject.SetActive(true);

                return obj;
            }

            return Instantiate(prefab);
        }

        public void AddObjectToPool(GameObject obj)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                objectPool.Push(obj);
            }
        }

        public void DeleteObjectsInPool()
        {
            print("Siliyorum");
            int count = objectPool.Count;
            for(int i = 0; i < count; i++)
            {
                GameObject obj = objectPool.Pop();
                Destroy(obj.gameObject);
                print("Sildim");
            }
        }
    }
}
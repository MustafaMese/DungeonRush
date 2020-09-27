using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class MjollnirPositioning : MonoBehaviour
    {
        [SerializeField] GameObject lightiningPrefab;
        private ObjectPool poolForEffect = new ObjectPool();

        [SerializeField] LineRenderer lineRenderer;
        List<GameObject> objects = new List<GameObject>();


        public void Execute(List<Vector3> targetPositions, float time)
        {
            lineRenderer.positionCount = targetPositions.Count;

            for (int i = 0; i < targetPositions.Count; i++)
                lineRenderer.SetPosition(i, targetPositions[i]);
            print("hey brah");
            objects.Clear();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                if (poolForEffect.IsObjectNull())
                {
                    print("yee");
                    poolForEffect.SetObject(lightiningPrefab);
                    poolForEffect.FillPool(3);
                }

                GameObject obj = poolForEffect.PullObjectFromPool();
                objects.Add(obj);

                obj.transform.position = targetPositions[i];
            }

            Invoke("Deactive", time);
        }

        public void Deactive()
        {
            for (int i = 0; i < objects.Count; i++)
                poolForEffect.AddObjectToPool(objects[i]);
            objects.Clear();
        }

    }
}
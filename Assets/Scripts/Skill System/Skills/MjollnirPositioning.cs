using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class MjollnirPositioning : MonoBehaviour
    {
        [SerializeField] GameObject lightiningPrefab;
        private ObjectPool<GameObject> poolForEffect = new ObjectPool<GameObject>();

        [SerializeField] LineRenderer lineRenderer;
        List<GameObject> objects = new List<GameObject>();

        public void Execute(List<Vector3> targetPositions, float time)
        {
            lineRenderer.positionCount = targetPositions.Count;
            objects.Clear();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                lineRenderer.SetPosition(i, targetPositions[i]);

                if (poolForEffect.IsObjectNull())
                {
                    poolForEffect.SetObject(lightiningPrefab);
                    poolForEffect.FillPool(3, lineRenderer.transform);
                }

                GameObject obj = poolForEffect.Pull(lineRenderer.transform);
                obj.SetActive(true);
                objects.Add(obj);

                obj.transform.position = targetPositions[i];
            }

            Invoke("Deactive", time);
        }

        public void Deactive()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(false);
                poolForEffect.AddObjectToPool(objects[i]);
            }
            objects.Clear();
        }

    }
}
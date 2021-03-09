using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class MjollnirPositioning : MonoBehaviour
    {
        [SerializeField] GameObject lightiningPrefab;
        private ObjectPool pool = new ObjectPool();

        [SerializeField] LineRenderer lineRenderer;
        private List<GameObject> activeList = new List<GameObject>();

        public void Initialize(GameObject obje)
        {
            pool.SetObject(lightiningPrefab);
            pool.Fill(3, transform);

            for (var i = 0; i < 3; i++)
            {
                GameObject obj = pool.Pull(transform);
                obj.transform.SetParent(obje.transform);
                obj.SetActive(false);
                pool.Push(obj);
            }
        }

        public void Execute(List<Vector3> targetPositions, float time)
        {
            lineRenderer.positionCount = targetPositions.Count;
            activeList.Clear();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                lineRenderer.SetPosition(i, targetPositions[i]);

                GameObject obj = pool.Pull(lineRenderer.transform);
                obj.SetActive(true);
                obj.transform.position = targetPositions[i];
                activeList.Add(obj);
            }
        }

        public void Deactivate()
        {
            for (var i = 0; i < activeList.Count; i++)
            {
                GameObject obj = activeList[i];
                obj.SetActive(false);
                pool.Push(obj);
            }

            activeList.Clear();
        }

    }
}
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

        public void Initialize()
        {
            poolForEffect.SetObject(lightiningPrefab);
            poolForEffect.FillPool(3, transform);
        }

        public void Execute(List<Vector3> targetPositions, float time)
        {
            lineRenderer.positionCount = targetPositions.Count;
            objects.Clear();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                lineRenderer.SetPosition(i, targetPositions[i]);

                GameObject obj = poolForEffect.Pull(lineRenderer.transform);
                obj.SetActive(true);
                objects.Add(obj);

                obj.transform.position = targetPositions[i];
            }

            StartCoroutine(Deactivate(time));
        }

        private IEnumerator Deactivate(float time)
        {
            time = time / 2;

            yield return new WaitForSeconds(time);

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(false);
                poolForEffect.AddObjectToPool(objects[i]);
            }
            objects.Clear();
        }

    }
}
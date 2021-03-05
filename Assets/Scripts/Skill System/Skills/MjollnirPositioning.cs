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

        List<GameObject> activeObjects = new List<GameObject>();
        List<GameObject> deactiveObjects = new List<GameObject>();

        public void Initialize()
        {
            poolForEffect.SetObject(lightiningPrefab);
            poolForEffect.Fill(3, transform);

            for (var i = 0; i < 3; i++)
            {
                GameObject obj = poolForEffect.Pull(transform);
                obj.SetActive(false);
                poolForEffect.Push(obj);
            }
        }

        public void Execute(List<Vector3> targetPositions, float time)
        {
            lineRenderer.positionCount = targetPositions.Count;
            activeObjects.Clear();

            for (int i = 0; i < targetPositions.Count; i++)
            {
                print(targetPositions.Count);

                lineRenderer.SetPosition(i, targetPositions[i]);

                GameObject obj = poolForEffect.Pull(lineRenderer.transform);
                obj.SetActive(true);
                activeObjects.Add(obj);

                obj.transform.position = targetPositions[i];
            }

            //StartCoroutine(Deactivate(time));
        }

        private IEnumerator Deactivate(float time)
        {
            for (var i = 0; i < activeObjects.Count; i++)
            {
                GameObject obj = activeObjects[i];

                deactiveObjects.Add(obj);
                activeObjects.Remove(obj);
            }

            yield return new WaitForSeconds(time / 2);

            for (int i = 0; i < deactiveObjects.Count; i++)
            {
                deactiveObjects[i].SetActive(false);
                poolForEffect.Push(deactiveObjects[i]);
            }
            deactiveObjects.Clear();
        }

    }
}
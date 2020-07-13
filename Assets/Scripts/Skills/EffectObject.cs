using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Effects
{
    public class EffectObject : MonoBehaviour
    {
        [HideInInspector] public EffectObject prefab;

        public void InitializeObject(float existence, Vector3 position, Transform parent, bool disable)
        {
            prefab = Instantiate(this, position, Quaternion.identity, parent);
            if(disable)
                Invoke("DisableObject", existence);
        }

        public void DisableObject()
        {
            if (prefab.gameObject.activeInHierarchy)
            {
                prefab.gameObject.SetActive(false);
            }
        }

        public void EnableObject(float existence, Vector3 position)
        {
            prefab.gameObject.SetActive(true);
            prefab.transform.position = position;
            Invoke("DisableObject", existence);
        }

    }
}

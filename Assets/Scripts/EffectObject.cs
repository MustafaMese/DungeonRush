using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Effects
{
    public class EffectObject : MonoBehaviour
    {
        private EffectObject prefab;

        public void InitializeObject(float existence, Transform t)
        {
            prefab = Instantiate(this, t);
            Invoke("DestroyObject", existence);
        }

        public void DestroyObject()
        {
            Destroy(prefab.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using DungeonRush.Traits;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status Object")]
public class StatusObject : ScriptableObject
{
    [SerializeField] Status statusPrefab;

    public Status Create(Transform t)
    {
        Status s = Instantiate(statusPrefab, t);
        return s;
    }
}

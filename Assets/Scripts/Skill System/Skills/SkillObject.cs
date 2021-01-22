using System;
using DungeonRush.Skills;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill Object")]
public class SkillObject : ScriptableObject
{
    [SerializeField] Skill statusPrefab;
    private string ID = Guid.NewGuid().ToString("N");

    public Skill Create(Transform t)
    {
        Skill s = Instantiate(statusPrefab, t);
        return s;
    }

    public string GetID()
    {
        return ID;
    }
}
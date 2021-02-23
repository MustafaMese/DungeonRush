using System;
using DungeonRush.Skills;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill Object")]
public class SkillObject : ScriptableObject
{
    public Skill skillPrefab;
    private string ID = Guid.NewGuid().ToString("N");

    public Skill Create(Transform t)
    {
        Skill s = Instantiate(skillPrefab, t);
        return s;
    }

    public string GetID()
    {
        return ID;
    }
}
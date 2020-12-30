using DungeonRush.Skills;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill Object")]
public class SkillObject : ScriptableObject
{
    [SerializeField] Skill statusPrefab;

    public Skill Create(Transform t)
    {
        Skill s = Instantiate(statusPrefab, t);
        return s;
    }
}
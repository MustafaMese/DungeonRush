using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;
using UnityEngine;

public abstract class PassiveSkill : Skill
{
    [SerializeField] int chanceFactor;

    public override void Initialize(Card card)
    {
        base.Initialize(card);
        skillType = SkillType.PASSIVE;
    }

    public override void Adjust(Move move)
    {
        if(CooldownControl())
        {
            if(!CalculateChance()) 
                canExecute = false;
            else
            {
                StartCoroutine(Animate(move));
                IncreaseCooldown();
                canExecute = true;
            }
        }
        else
            DecreaseCooldown();
    }

    private bool CooldownControl()
    {
        if(tempCooldown > 0)
            return false;
        return true;
    }

    private bool CalculateChance()
    {
        int number = Random.Range(0, 101);
        if(chanceFactor <= number)
            return true;
        return false;
    }
}
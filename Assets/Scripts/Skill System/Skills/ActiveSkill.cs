using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;
using DungeonRush.UI;

public abstract class ActiveSkill : Skill
{
    public override void Initialize(Card card)
    {
        base.Initialize(card);
        skillType = SkillType.ACTIVE;
    }

    public override void Adjust(Move move)
    {
        if(CooldownControl())
        {
            IncreaseCooldown();
            move.GetCard().GetController().Stop();

            Execute(move);
            StartCoroutine(Animate(move));
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

    public override void DecreaseCooldown()
    {
        base.DecreaseCooldown();

        SkillButtonControl();
    }

    protected void SkillButtonControl()
    {
        if(tempCooldown <= 0)
            UIManager.Instance.ButtonControl(this, true);
        else
            UIManager.Instance.ButtonControl(this, false);
        
    }
}
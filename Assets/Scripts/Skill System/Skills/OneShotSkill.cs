using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;

public abstract class OneShotSkill : Skill
{
    private bool used;

    public override void Initialize(Card card)
    {
        base.Initialize(card);
        used = false;
        skillType = SkillType.ONESHOT;
    }

    public override void Adjust(Move move)
    {
        if(!used)
        {
            Execute(move);
            StartCoroutine(Animate(move));
            used = true;
        }
    }
}
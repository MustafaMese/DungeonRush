using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Skills;

public abstract class OneShotSkill : Skill
{
    public override void Initialize(Card card)
    {
        base.Initialize(card);
        skillType = SkillType.ONESHOT;
    }

    public override void Adjust(Move move)
    {
        //StartCoroutine(Animate(move));
        canExecute = true;
    }
}
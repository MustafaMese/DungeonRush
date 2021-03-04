using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class HealYourself : ActiveSkill
    {
        public override void Execute(Move move)
        {
            Card card = move.GetCard();
            card.GetDamagable().IncreaseHealth(Power);

            SkillButtonControl();
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetCard().transform;
            effect.transform.SetParent(t);
            effect.transform.position = t.position;
        }
    }
}

using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Traits;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class StatusCreateOnTargetSkill : PassiveSkill
    {
        [SerializeField] StatusObject status;

        public override void Execute(Move move)
        {
            Card target = move.GetTargetTile().GetCard();

            if (target != null)
                target.GetStatusController().AddStatus(status);

        }
    }
}
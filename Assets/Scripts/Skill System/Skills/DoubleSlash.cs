using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class DoubleSlash : PassiveSkill
    {
        public override void Execute(Move move)
        {
            if(!canExecute) return;

            Tile targetTile = move.GetTargetTile();
            Card targetCard = targetTile.GetCard();
            
            if(targetCard != null)
                targetCard.DecreaseHealth(Power);
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetTargetTile().transform;
            effect.transform.SetParent(t);
            effect.transform.position = t.position;
        }
    }
}

using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/DoubleSlash")]
    public class DoubleSlash : Skill
    {
        public override void Execute(Move move)
        {
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

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            Transform t = move.GetTargetTile().transform;
            textPopup.transform.SetParent(t);
            textPopup.transform.position = t.position;

            Vector3 targetPosition = t.position;
            return targetPosition;
        }
    }
}

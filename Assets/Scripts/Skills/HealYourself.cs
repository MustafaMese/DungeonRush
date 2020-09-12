using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/Healing")]
    public class HealYourself : Skill
    {
        public override void Execute(Move move)
        {
            Card card = move.GetCard();
            card.IncreaseHealth(Power);
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetCard().transform;
            effect.transform.SetParent(t);
            effect.transform.position = t.position;
        }

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            Transform t = move.GetCard().transform;
            textPopup.transform.SetParent(t);
            textPopup.transform.position = t.position;

            Vector3 targetPosition = t.position;
            return targetPosition;
        }
    }
}

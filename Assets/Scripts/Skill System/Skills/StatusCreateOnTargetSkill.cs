using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Traits;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/StatusCreateOnTargetSkill")]
    public class StatusCreateOnTargetSkill : Skill
    {
        [SerializeField] StatusObject status;

        public override void Execute(Move move)
        {
            Card target = move.GetTargetTile().GetCard();

            if (target != null)
                target.GetComponent<StatusController>().AddStatus(status);

        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            return;
        }

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            return Vector3.zero;
        }
    }
}
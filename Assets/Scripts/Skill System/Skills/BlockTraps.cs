using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/BlockTrap")]
    public class BlockTraps : Skill
    {
        public override void Execute(Move move)
        {
            move.GetCard().CanBlockTraps = true;
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

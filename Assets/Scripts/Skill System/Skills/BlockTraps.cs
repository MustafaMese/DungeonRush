using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class BlockTraps : OneShotSkill
    {
        public override void Execute(Move move)
        {
            move.GetCard().GetStats().CanBlockTraps = true;
        }
    }
}

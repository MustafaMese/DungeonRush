using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class BlockTraps : OneShotSkill
    {
        public override void Execute(Move move)
        {
            if(canExecute)
                move.GetCard().CanBlockTraps = true;
        }
    }
}

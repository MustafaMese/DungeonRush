using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Controller;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            [SerializeField] EventMover eventMover = null;
            public bool isEventMove = false;

            [SerializeField] bool isFirstLevel = false;

            protected override void Initialize()
            {
                base.Initialize();
                if(!isFirstLevel)
                    GetComponent<PlayerController>().LoadPlayer();
            }

            public override void ExecuteMove()
            {
                if(GetMove().GetMoveType() != MoveType.EVENT)
                    mover.Move();
                else
                {
                    eventMover.Move();
                    isEventMove = true;
                } 
            }

            public override bool IsMoveFinished()
            {
                if (!isEventMove)
                    return mover.IsMoveFinished();
                else
                    return eventMover.IsMoveFinished();
            }

            public override void SetIsMoveFinished(bool b)
            {
                if (!isEventMove)
                    mover.SetIsMoveFinished(b);
                else
                {
                    isEventMove = b;
                    eventMover.SetIsMoveFinished(b);
                }
            }
        }
    }
}

using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Controller;
using System.Collections.Generic;
using DungeonRush.Items;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            [SerializeField] EventMover eventMover = null;
            private bool isEventMove = false;
            [SerializeField] bool isFirstLevel = false;

            PlayerAttacker playerAttacker;
            ItemUser itemUser;

            [SerializeField] private int experience = 0;
            public int Experience
            {
                get { return experience; }
                set { experience = value; }
            }

            [SerializeField] private int coins = 0;
            public int Coins
            {
                get { return coins; }
                set { coins = value; }
            }

            protected override void Initialize()
            {
                base.Initialize();
                playerAttacker = GetComponent<PlayerAttacker>();
                itemUser = GetComponent<ItemUser>();

                if (!isFirstLevel)
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

            public int GetDamage()
            {
                return playerAttacker.GetDamage();
            }

            public List<string> GetItemNames()
            {
                return itemUser.GetItemsNames();
            }
        }
    }
}

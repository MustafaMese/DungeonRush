using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Controller;
using System.Collections.Generic;
using DungeonRush.Items;
using DungeonRush.Camera;
using DungeonRush.Traits;
using TMPro;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            [SerializeField] TextMeshPro nameText = null;
            [SerializeField] EventMover eventMover = null;
            [SerializeField] bool isFirstLevel = false;

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

            private bool isEventMove = false;
            private PlayerAttacker playerAttacker;
            private ItemUser itemUser;

            protected override void Initialize()
            {
                base.Initialize();
                health = GetComponent<Health>();
                mover = GetComponent<Mover>();
                attacker = GetComponent<Attacker>();
                Controller = GetComponent<IMoveController>();
                playerAttacker = GetComponent<PlayerAttacker>();
                statusController = GetComponent<StatusController>();
                SetStats();
                move = new Move();
                itemUser = GetComponent<ItemUser>();

                float z = PlayerCamera.Instance.transform.position.z;
                PlayerCamera.Instance.transform.position = new Vector3(transform.position.x, transform.position.y + 1, z);
              
                InstantMoveCount = TotalMoveCount;
                if (!isFirstLevel)
                    GetComponent<PlayerController>().LoadPlayer();
                else
                    SetCurrentHealth(GetMaxHealth());

                nameText.text = cardName;
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

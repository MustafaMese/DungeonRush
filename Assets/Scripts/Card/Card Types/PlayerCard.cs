﻿using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Controller;
using System.Collections.Generic;
using DungeonRush.Items;
using DungeonRush.Camera;
using DungeonRush.Traits;
using TMPro;
using DungeonRush.Shifting;
using DungeonRush.Attacking;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card, IMovable, IDamagable, IFighter
        {
            [SerializeField] TextMeshPro nameText = null;
            [SerializeField] EventMover eventMover = null;
            [SerializeField] Animator animator = null;
            [SerializeField] bool isFirstLevel = false;

            private int experience = 0;
            public int Experience
            {
                get { return experience; }
                set { experience = value; }
            }

            private int coins = 0;
            public int Coins
            {
                get { return coins; }
                set { coins = value; }
            }

            private bool isEventMove = false;

            private Health health = null;
            private Mover mover = null;
            private PlayerAttacker playerAttacker;
            private ItemUser itemUser;
            private IMoveController controller;
            private StatusController statusController;

            private IMovable movable;
            private IDamagable damagable;
            private IFighter fighter;
            private Stats stats;

            protected override void Initialize()
            {
                base.Initialize();
                health = GetComponent<Health>();
                mover = GetComponent<Mover>();
                playerAttacker = GetComponent<PlayerAttacker>();
                controller = GetComponent<IMoveController>();
                statusController = GetComponent<StatusController>();
                movable = GetComponent<IMovable>();
                fighter = GetComponent<IFighter>();
                damagable = GetComponent<IDamagable>();
                SetStats();
                move = new Move();
                itemUser = GetComponent<ItemUser>();

                float z = PlayerCamera.Instance.transform.position.z;
                PlayerCamera.Instance.transform.position = new Vector3(transform.position.x, transform.position.y + 1, z);

                if (!isFirstLevel)
                    GetComponent<PlayerController>().LoadPlayer();
                else
                    SetCurrentHealth(GetMaxHealth());

                nameText.text = cardName;
            }

            public override IDamagable GetDamagable()
            {
                return damagable;
            }

            public override IFighter GetFighter()
            {
                return fighter;
            }

            public override IMovable GetMovable()
            {
                return movable;
            }

            public override Stats GetStats()
            {
                return stats;
            }

            protected void SetStats()
            {
                if (cardProperties.cardStats != null)
                {
                    stats = new Stats();

                    stats.MaximumHealth = cardProperties.cardStats.maximumHealth;
                    stats.CriticChance = cardProperties.cardStats.criticChance;
                    stats.DodgeChance = cardProperties.cardStats.dodgeChance;
                    stats.LifeCount = cardProperties.cardStats.lifeCount;
                    stats.TotalMoveCount = cardProperties.cardStats.moveCount;
                    stats.LootChance = cardProperties.cardStats.lootChance;
                    stats.InstantMoveCount = stats.TotalMoveCount;
                    if (stats.MaximumHealth > 0)
                        SetMaxHealth(stats.MaximumHealth);
                }
            }

            public override Animator GetAnimator()
            {
                return animator;
            }

            public override IMoveController GetController()
            {
                return controller;
            }

            public override StatusController GetStatusController()
            {
                return statusController;
            }

            public void ExecuteMove()
            {
                if(GetMove().GetMoveType() != MoveType.EVENT)
                    mover.Move();
                else
                {
                    eventMover.Move();
                    isEventMove = true;
                } 
            }

            public bool IsMoveFinished()
            {
                if (!isEventMove)
                    return mover.IsMoveFinished();
                else
                    return eventMover.IsMoveFinished();
            }

            public void SetIsMoveFinished(bool b)
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

            public bool CanAttack(Card enemy)
            {
                return playerAttacker.CanMove(enemy);
            }
            public void ExecuteAttack()
            {
                playerAttacker.Attack();
            }
            public Shift GetShift()
            {
                return mover.GetShift();
            }
            public AttackStyle GetAttackStyle()
            {
                return playerAttacker.GetAttackStyle();
            }

            public void IncreaseMaxHealth(int h)
            {
                health.IncreaseMaxHealth(h);
            }

            public void DecreaseMaxHealth(int h)
            {
                health.DecreaseMaxHealth(h);
            }

            public void IncreaseHealth(int h)
            {
                health.ChangeHealth(false, h);
            }
            public void DecreaseHealth(int h)
            {
                health.ChangeHealth(true, h);
            }
            public void SetCurrentHealth(int amount)
            {
                health.SetCurrentHealth(amount);
            }

            public void SetMaxHealth(int amount)
            {
                health.SetMaxHealth(amount);
            }

            public int GetMaxHealth()
            {
                return health.GetMaxHealth();
            }

            public int GetHealth()
            {
                return health.GetCurrentHealth();
            }

            
        }
    }
}

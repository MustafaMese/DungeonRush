using DungeonRush.Data;
using DungeonRush.Property;
using DungeonRush.Controller;
using DungeonRush.Traits;
using UnityEngine;
using TMPro;
using DungeonRush.Shifting;
using DungeonRush.Attacking;

namespace DungeonRush
{
    namespace Cards
    {
        public class EnemyCard : Card, IMovable, IFighter, IDamagable
        {
            [SerializeField] TextMeshPro nameText = null;
            [SerializeField] Animator animator;

            private Mover mover = null;
            private Health health;
            private Attacker attacker = null;
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
                attacker = GetComponent<Attacker>();
                controller = GetComponent<IMoveController>();
                statusController = GetComponent<StatusController>();
                movable = GetComponent<IMovable>();
                fighter = GetComponent<IFighter>();
                damagable = GetComponent<IDamagable>();
                SetStats();

                move = new Move();
                nameText.text = cardName;
            }

            protected void SetStats()
            {
                if (cardProperties.cardStats != null)
                {
                    stats = new Stats();

                    stats.MaximumHealth = cardProperties.cardStats.maximumHealth;
                    stats.Damage = cardProperties.cardStats.damage;
                    stats.CriticChance = cardProperties.cardStats.criticChance;
                    stats.DodgeChance = cardProperties.cardStats.dodgeChance;
                    stats.LifeCount = cardProperties.cardStats.lifeCount;
                    stats.TotalMoveCount = cardProperties.cardStats.moveCount;
                    stats.LootChance = cardProperties.cardStats.lootChance;
                    stats.InstantMoveCount = stats.TotalMoveCount;
                    if (stats.MaximumHealth > 0)
                    {
                        SetMaxHealth(stats.MaximumHealth);
                        SetCurrentHealth(stats.MaximumHealth);
                    }
                }
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

            public bool CanAttack(Card enemy)
            {
                return attacker.CanMove(enemy);
            }
            public void ExecuteAttack()
            {
                attacker.Attack();
            }
            public Shift GetShift()
            {
                return mover.GetShift();
            }
            public AttackStyle GetAttackStyle()
            {
                return attacker.GetAttackStyle();
            }
            public bool IsMoveFinished()
            {
                return mover.IsMoveFinished();
            }
            public void SetIsMoveFinished(bool b)
            {
                mover.SetIsMoveFinished(b);
            }

            public void ExecuteMove()
            {
                mover.Move();
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
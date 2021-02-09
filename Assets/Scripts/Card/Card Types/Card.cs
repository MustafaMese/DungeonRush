using DungeonRush.Attacking;
using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Shifting;
using DungeonRush.Traits;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public abstract class Card : MonoBehaviour
        {
            protected Tile tile = null;
            protected Health health = null;
            protected Move move;
            protected string cardName = " ";
            protected Mover mover = null;
            protected Attacker attacker = null;
            protected IMoveController controller;
            protected StatusController statusController;

            [Header("General Properties")]
            [SerializeField] CardProperties cardProperties = null;
            [SerializeField] Character characterType;
            [SerializeField] TextMeshPro nameText = null;
            [SerializeField] Animator animator;

            #region CARD STATS
            protected int maximumHealth = 0;
            protected int criticChance = 0;
            protected int dodgeChance = 0;
            protected int lifeCount = 0;
            protected int totalMoveCount = 0;
            protected int instantMoveCount = 0;
            protected int lootChance = 0;
            protected bool canBlockTraps = false;
            public int MaximumHealth { get => maximumHealth; set => maximumHealth = value; }
            public int CriticChance { get => criticChance; set => criticChance = value; }
            public int DodgeChance { get => dodgeChance; set => dodgeChance = value; }
            public int LifeCount { get => lifeCount; set => lifeCount = value; }
            public int TotalMoveCount { get => totalMoveCount; set => totalMoveCount = value; }
            public int InstantMoveCount { get => instantMoveCount; set => instantMoveCount = value; }
            public int LootChance { get => lootChance; set => lootChance = value; }
            public bool CanBlockTraps { get => canBlockTraps; set => canBlockTraps = value; }
            #endregion

            public IMoveController Controller { get => controller; set => controller = value; }
            public Animator Animator { get => animator; }
            public StatusController StatusController { get => statusController; }

            public void Start()
            {
                Initialize();
            }

            protected virtual void Initialize()
            {
                health = GetComponent<Health>();
                cardName = cardProperties.cardName;
                mover = GetComponent<Mover>();
                attacker = GetComponent<Attacker>();
                Controller = GetComponent<IMoveController>();
                statusController = GetComponent<StatusController>();
                SetStats();
                
                move = new Move();

                if(GetCardType() != CardType.EVENT && GetCardType() != CardType.TRAP)
                    nameText.text = cardName;
            }

            protected void SetStats()
            {
                if(cardProperties.cardStats != null)
                {
                    maximumHealth = cardProperties.cardStats.maximumHealth;
                    criticChance = cardProperties.cardStats.criticChance;
                    dodgeChance = cardProperties.cardStats.dodgeChance;
                    lifeCount = cardProperties.cardStats.lifeCount;
                    totalMoveCount = cardProperties.cardStats.moveCount;
                    lootChance = cardProperties.cardStats.lootChance;
                    if (maximumHealth > 0)
                        SetMaxHealth(maximumHealth);
                }
            }

            public int GetLevel()
            {
                return cardProperties.level;
            }

            public Sprite GetCharacterIcon()
            {
                return cardProperties.characterIcon;
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

            public string GetCardName()
            {
                return cardName;
            }

            public Tile GetTile()
            {
                return tile;
            }

            public CardType GetCardType()
            {
                return cardProperties.cardType;
            }

            public void SetTile(Tile coordinate)
            {
                this.tile = coordinate;
            }

            public void SetCardType(CardType type)
            {
                cardProperties.cardType = type;
            }

            public Move GetMove()
            {
                return move;
            }

            public void SetMove(Move move)
            {
                this.move = move;
            }

            public virtual void ExecuteMove()
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
            public Character GetCharacterType()
            {
                return characterType;
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
            public virtual bool IsMoveFinished()
            {
                return mover.IsMoveFinished();
            }
            public virtual void SetIsMoveFinished(bool b)
            {
                mover.SetIsMoveFinished(b);
            }
        }
    }
}

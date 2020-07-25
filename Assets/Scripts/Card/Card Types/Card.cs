using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using DungeonRush.Shifting;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public abstract class Card : MonoBehaviour
        {
            public Tile tile = null;
            protected Health health = null;
            protected string cardName = " ";
            protected Move move;
            protected Mover mover = null;
            protected bool isAlive = false;
            private IMoveController controller;

            [SerializeField, Range(0, 1)] float disappearing = 0.1f;

            [Header("General Properties")]
            [HideInInspector] public bool isMoving = false;
            [HideInInspector] public bool isBossCard = false;
            public CardProperties cardProperties = null;
            public Character characterType;
            public Shift shifting = null;

            public IMoveController Controller { get => controller; set => controller = value; }

            public void Start()
            {
                isMoving = false;
                isBossCard = false;
                health = GetComponent<Health>();
                cardName = cardProperties.cardName;
                mover = GetComponent<Mover>();
                move = new Move();
                Controller = GetComponent<IMoveController>();
            }

            public abstract string GetCardName();
            public abstract int GetHealth();
            public abstract void SetHealth(int health);
            public abstract CardType GetCardType();
            public abstract void SetCardType(CardType type);
            public abstract Tile GetTile();
            public abstract void SetTile(Tile coordinate);
            public abstract Move GetMove();
            public abstract void SetMove(Move move);
            public abstract void ExecuteMove();
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
                return GetComponent<IAttacker>().CanMove(enemy);
            }
            public void ExecuteAttack()
            {
                GetComponent<IAttacker>().Attack();
            }
            public Shift GetShift()
            {
                return shifting;
            }
        }
    }
}

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
            protected Tile tile = null;
            protected Health health = null;
            protected Move move;
            protected string cardName = " ";
            protected IMover mover = null;
            protected IAttacker attacker = null;
            protected IMoveController controller;

            [Header("General Properties")]
            public CardProperties cardProperties = null;
            public Character characterType;

            public IMoveController Controller { get => controller; set => controller = value; }

            public void Start()
            {
                health = GetComponent<Health>();
                cardName = cardProperties.cardName;
                mover = GetComponent<IMover>();
                attacker = GetComponent<IAttacker>();
                Controller = GetComponent<IMoveController>();
                move = new Move();

                
            }

            public int GetHealth()
            {
                return health.Get();
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

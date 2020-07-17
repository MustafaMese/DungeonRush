using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush {
    namespace Cards
    {
        public class ItemCard : Card
        {
            public override void ExecuteMove()
            {
                mover.Move();
            }

            public override int GetHealth()
            {
                return health.Get();
            }


            public override string GetCardName()
            {
                return cardProperties.cardName;
            }

            public override Tile GetTile()
            {
                return tile;
            }

            public override CardType GetCardType()
            {
                return cardProperties.cardType;
            }

            public override void SetHealth(int health)
            {
                this.health.Set(health);
            }

            public override void IncreaseHealth(int health)
            {
                this.health.ChangeHealth(false, health);
            }

            public override void DecreaseHealth(int damage)
            {
                this.health.ChangeHealth(true, damage);
            }

            public override void SetTile(Tile coordinate)
            {
                this.tile = coordinate;
            }

            public override void SetCardType(CardType type)
            {
                cardProperties.cardType = type;
            }

            public override Move GetMove()
            {
                return this.move;
            }

            public override void SetMove(Move move)
            {
                this.move = move;
            }
        }
    }
}

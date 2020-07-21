using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            public override int GetHealth()
            {
                return health.Get();
            }

            public override string GetCardName()
            {
                return cardName;
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

            public override void ExecuteMove()
            {
                mover.Move();
            }
        }
    }
}

using DungeonRush.Cards;
using DungeonRush.Field;

namespace DungeonRush
{
    namespace Data
    {
        [System.Serializable]
        public class Move
        {
            public Move()
            {
                Reset();
            }

            public Move(Tile targetTile, Card card, MoveType type, bool canMoveToPlace)
            {
                this.targetTile = targetTile;
                this.card = card;
                this.cardTile = card.GetTile();
                this.type = type;
                this.canMoveToPlace = canMoveToPlace;
            }

            public Tile cardTile;
            public Tile targetTile;
            public Card card;
            public MoveType type;
            public bool canMoveToPlace;

            public Tile GetCardTile()
            {
                return this.cardTile;
            }

            public Tile GetTargetTile()
            {
                return this.targetTile;
            }

            public Card GetCard()
            {
                return this.card;
            }

            public MoveType GetMoveType()
            {
                return this.type;
            }

            public bool GetCanMove()
            {
                return this.canMoveToPlace;
            }

            public void Reset()
            {
                this.cardTile = null;
                this.targetTile = null;
                this.card = null;
                this.type = MoveType.NONE;
                this.canMoveToPlace = false;
            }

        }

        public enum MoveType
        {
            NONE,
            ATTACK,
            ITEM,
            COIN,
            EMPTY
        }
    }
}

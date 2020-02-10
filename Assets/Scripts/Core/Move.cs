using DungeonRush;
using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Moves
    {
        [System.Serializable]
        public struct Move
        {
            public Move(Tile cardTile, Tile targetTile, Card card, MoveType type, bool lastMove)
            {
                this.cardTile = cardTile;
                this.targetTile = targetTile;
                this.card = card;
                this.type = type;
                this.lastMove = lastMove;
            }

            public Tile cardTile;
            public Tile targetTile;
            public Card card;
            public MoveType type;
            public bool lastMove;

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

            public bool GetLastMove()
            {
                return this.lastMove;
            }

            public Move TranferMoveInfoToAnotherMove(Move move)
            {
                move.cardTile = this.cardTile;
                move.targetTile = this.targetTile;
                move.card = this.card;
                move.type = this.type;
                move.lastMove = this.lastMove;
                return move;
            }

            public void Reset()
            {
                this.cardTile = null;
                this.targetTile = null;
                this.card = null;
                this.type = MoveType.None;
                this.lastMove = false;
            }

        }

        public enum MoveType
        {
            None,
            Attack,
            Item,
            Coin,
            Empty
        }
    }
}

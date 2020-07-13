using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Shifting
{
    [CreateAssetMenu(menuName = "Shift/ZeroBoxShift")]
    public class ZeroBoxShift : Shift
    {
        public override bool Define(Card card, Swipe swipe)
        {
            Tile tile = card.GetTile();
            if (tile.GetCard() != null)
            {
                Card targetCard = tile.GetCard();
                if (targetCard.GetCardType() == CardType.ENEMY || targetCard.GetCardType() == CardType.PLAYER)
                {
                    Move move = new Move(tile, card, MoveType.ATTACK, false);
                    card.SetMove(move);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
        {
            throw new System.NotImplementedException();
        }

        public override Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles, Card attacker)
        {
            throw new System.NotImplementedException();
        }
    }
}

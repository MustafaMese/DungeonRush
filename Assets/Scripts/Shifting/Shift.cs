using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Shifting
{
    public abstract class Shift : ScriptableObject
    {
        protected int upperBorder;
        protected int lowerBorder;
        protected int rightBorder;
        protected int leftBorder;

        public abstract bool Define(Card card, Swipe swipe);
        public abstract Dictionary<Tile, Swipe> GetAvaibleTiles(Card card);
        public abstract Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles);
    }
}

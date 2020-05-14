using DungeonRush.Cards;
using DungeonRush.Managers;
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
    }
}

using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush {
    namespace Cards
    {
        [CreateAssetMenu (menuName = "Card/Template")]
        public class CardTemplate : ScriptableObject
        {
            public string cardName;
            public int health;
            public Sprite sprite;
            public CardType cardType;
            public int level;
            public ItemType itemType;
            public Character character;
        }
    }
}

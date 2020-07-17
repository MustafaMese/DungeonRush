using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush 
{
    namespace Property
    {
        [CreateAssetMenu(menuName = "Property/Template")]
        public class CardProperties : ScriptableObject
        {
            public string cardName;
            public int health;
            public CardType cardType;
            public Character character;
        }
    }
}

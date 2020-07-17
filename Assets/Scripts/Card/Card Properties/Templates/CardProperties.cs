using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush 
{
    namespace Property
    {
        [CreateAssetMenu(menuName = "Card/Template")]
        public class CardProperties : ScriptableObject
        {
            public string cardName;
            public CardType cardType;
            public Character character;
        }
    }
}

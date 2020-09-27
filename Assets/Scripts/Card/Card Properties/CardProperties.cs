using DungeonRush.Data;
using DungeonRush.Traits;
using UnityEngine;

namespace DungeonRush 
{
    namespace Property
    {
        [CreateAssetMenu(menuName = "ScriptableObjects/Card/Template")]
        public class CardProperties : ScriptableObject
        {
            public string cardName;
            public CardType cardType;
            public Character character;
            public Sprite characterIcon;
            public Stats cardStats;

            [Header("Enemy only")]
            public int level;
        }
    }
}

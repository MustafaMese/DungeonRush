using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Data
    {
        public enum CharacterType
        {
            PLAYER,
            MACHINE,
            SLIME,
            TOXIC,
            INFECTED,
            CORRUPTED,
            HUMAN,
            HUMANOID,
            DEMON,
            THIEF,
            ITEM_EVENT,
            WALL,
            TRAP
        }

        [CreateAssetMenu(menuName = "ScriptableObjects/Card/Character")]
        public class Character : ScriptableObject
        {
            public CharacterType cT;
            public List<CharacterType> enemies;

            public bool IsEnemy(Character character)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy == character.cT)
                        return true;
                }
                return false;
            }
        }

    }
}

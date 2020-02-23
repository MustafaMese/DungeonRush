using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace DataPackages
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
            ITEM,
            ALL
        }

        [CreateAssetMenu(menuName = "Card/Character")]
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

        public class EnemyRecognition : MonoBehaviour
        {

        }
    }
}

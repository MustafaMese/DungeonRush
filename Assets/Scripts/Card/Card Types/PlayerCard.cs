using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            [SerializeField] PlayerMover eventMover;
            public override void ExecuteMove()
            {
                print("Buradaysan devam et oç");
                mover.Move();
            }

            // TODO Move a göre is move finished methodlarını yazıcaksın.
        }
    }
}

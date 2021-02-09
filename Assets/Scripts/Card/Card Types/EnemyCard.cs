using DungeonRush.Data;
using DungeonRush.Property;
using DungeonRush.Controller;
using DungeonRush.Traits;
using UnityEngine;
using TMPro;

namespace DungeonRush
{
    namespace Cards
    {
        public class EnemyCard : Card
        {
            [SerializeField] TextMeshPro nameText = null;

            protected override void Initialize()
            {
                base.Initialize();
                health = GetComponent<Health>();
                mover = GetComponent<Mover>(); //
                attacker = GetComponent<Attacker>(); //
                Controller = GetComponent<IMoveController>(); //
                statusController = GetComponent<StatusController>(); //
                SetStats(); //

                move = new Move(); //

                if (GetCardType() != CardType.EVENT && GetCardType() != CardType.TRAP)
                    nameText.text = cardName;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Property;
using DungeonRush.Shifting;
using UnityEngine;

namespace DungeonRush.Traits
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Status/ShiftingStatus")]
    public class ShiftingStatus : Status
    {
        [SerializeField] Shift shifting;

        public override void Execute(Card card, bool lastTurn = false)
        {
            if (card != null)
                card.GetComponent<Mover>().ChangeShiftOneTurn(false, shifting);
        }
    }
}
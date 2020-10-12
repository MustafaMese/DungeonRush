using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Traits
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Status/FeatureSetter")]
    public class FeatureSetter : Status
    {
        public override void Execute(Card card, bool lastTurn = false)
        {
            return;
        }
    }
}
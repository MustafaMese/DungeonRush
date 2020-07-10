using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Effects;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "Skill/DoubleSlash", order = 1)]
    public class DoubleSlash : Skill
    {
        public int slashPower = 2;
        public float effectTime = 0.3f;
        public EffectObject slashAnimationPrefab;

        public override void Execute(Move move)
        {
            Card card = move.GetTargetTile().GetCard();
            card.DecreaseHealth(slashPower);
            slashAnimationPrefab.InitializeObject(effectTime, card.transform);
            Debug.Log("Double Slash!!");
        }

        public override void Initialize(Move move)
        {
            throw new System.NotImplementedException();
        }
    }
}

using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Effects;
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
            Debug.Log("Double Slash");
            Card targetCard = move.GetTargetTile().GetCard();
            Card card = move.GetCard();
            targetCard.DecreaseHealth(slashPower);

            if (slashAnimationPrefab.prefab == null)
                slashAnimationPrefab.InitializeObject(effectTime, targetCard.transform.position, card.transform);
            else
                slashAnimationPrefab.EnableObject(effectTime, targetCard.transform.position);
        }

        public override void Initialize(Move move)
        {
            throw new System.NotImplementedException();
        }
    }
}

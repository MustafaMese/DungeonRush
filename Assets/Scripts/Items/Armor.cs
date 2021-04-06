using DungeonRush.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Armor")]
    public class Armor : Item
    {
        [Multiline(2), SerializeField] string explanation;

        [Header("Armor Properties")]
        [Space(25f)]
        [SerializeField] int power;
        [SerializeField] BoneType bone;
        [SerializeField] List<ArmorImpact> impacts = new List<ArmorImpact>();
        [Space]
        [Header("If this is a dual item, then fill the following varible.")]
        [SerializeField] Sprite secondarySprite;

        public override void Execute(Card card)
        {
            TakeArmor(card);
        }

        public void TakeArmor(Card card)
        {
            for (var i = 0; i < impacts.Count; i++)
                ChangeStats(card, impacts[i].impactType, impacts[i].power, true);
        }

        public void TakeOffArmor(Card card)
        {
            for (var i = 0; i < impacts.Count; i++)
                ChangeStats(card, impacts[i].impactType, impacts[i].power, false);
        }

        private void ChangeStats(Card card, ArmorImpactType armorImpactType, int power, bool increase)
        {

            if(!increase)
                power *= -1;

            switch (armorImpactType)
            {
                case ArmorImpactType.MAX_HEALTH_INCREASE:
                    card.GetStats().MaximumHealth += power;
                    card.GetDamagable().SetMaxHealth(card.GetStats().MaximumHealth);
                    break;
                case ArmorImpactType.DAMAGE:
                    card.GetStats().Damage += power;
                    break;
                case ArmorImpactType.CRITIC_INCREASER:
                    card.GetStats().CriticChance += power;
                    break;
                case ArmorImpactType.DODGE_INCREASER:
                    card.GetStats().DodgeChance += power;
                    break;
                case ArmorImpactType.LOOT_CHANCE:
                    card.GetStats().LootChance += power;
                    break;
                case ArmorImpactType.LIFE_COUNT:
                    card.GetStats().LifeCount += power;
                    break;
                case ArmorImpactType.MOVE_COUNT:
                    card.GetStats().TotalMoveCount += power;
                    break;
            }
            
        }

        public override int GetPower()
        {
            return power;
        }

        public override BoneType GetBoneType()
        {
            return bone;
        }

        public override Sprite GetSecondarySprite()
        {
            return secondarySprite;
        }

        public override string GetExplanation()
        {
            return explanation;
        }

        public List<ArmorImpact> GetImpacts()
        {
            return impacts;
        }
    }
}

public enum ArmorImpactType
{
    MAX_HEALTH_INCREASE,
    DAMAGE,
    CRITIC_INCREASER,
    DODGE_INCREASER,
    LIFE_COUNT,
    MOVE_COUNT,
    LOOT_CHANCE
}




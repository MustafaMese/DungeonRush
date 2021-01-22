using System;
using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Potion")]
    public class Potion : Item
    {
        [SerializeField] int power;

        public override void Execute(Card card)
        {
            Impact(card);
        }

        private void Impact(Card card)
        {
            switch(GetItemType())
            {
                case ItemType.POTION:
                    card.IncreaseHealth(power);
                    break;       
                case ItemType.POISON:
                    card.DecreaseHealth(power);
                    break;
                case ItemType.MAX_HEALTH_INCREASER:
                    card.IncreaseMaxHealth(power);
                    break;
            }
        }

        public override int GetPower()
        {
            return power;
        }

        
    }
}
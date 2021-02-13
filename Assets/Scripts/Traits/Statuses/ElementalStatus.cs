using DungeonRush.Cards;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Traits
{
    public class ElementalStatus : Status
    {
        [SerializeField] ElementType elementType;

        public override void Execute(Card card, Tile tile)
        {
            ImpactOnSurface(tile);
            if(Power > 0)
                card.GetDamagable().IncreaseHealth(Power);
            else
                card.GetDamagable().DecreaseHealth(Power);
        }

        private void ImpactOnSurface(Tile tile)
        {
            EnvironmentCard trap = tile.GetEnvironmentCard();
            if(trap != null)
                trap.Evolve(elementType);
        }
    }
}
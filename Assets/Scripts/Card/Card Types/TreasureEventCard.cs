using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Managers;
using DungeonRush.Items;

namespace DungeonRush.Cards
{
    public class TreasureEventCard : EventCard
    {
        [SerializeField] ItemCard item = null;
        [SerializeField] float disapperTime = 0;
        [SerializeField] SpriteRenderer sprite;

        public override void GetEvent(Card card)
        {
            Item i = item.GetComponent<Item>();
            ItemMove(card, i);
            StartCoroutine(Disapper());
        }

        private void ItemMove(Card card, Item i)
        {
            if (i == null) return;

            card.GetComponent<ItemUser>().TakeItem(i);

        }

        private IEnumerator Disapper()
        {
            Color c;
            while (sprite.color.a > 0)
            {
                c = sprite.color;
                c.a -= Time.deltaTime / disapperTime;
                sprite.color = c;
                yield return null;
            }
            CardManager.RemoveCardForAttacker(GetTile().GetListNumber());
        }
    }
}
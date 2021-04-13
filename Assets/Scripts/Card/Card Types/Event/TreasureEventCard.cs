using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Managers;
using DungeonRush.Items;
using DungeonRush.UI;
using DungeonRush.Customization;

namespace DungeonRush.Cards
{
    public class TreasureEventCard : EventCard
    {
        [SerializeField] ItemType iType;
        [SerializeField] Item item = null;
        [SerializeField] float disapperTime = 0;

        protected override void Initialize()
        {
            base.Initialize();
            item = ItemDB.Instance.GetRandomItem(iType);
        }

        public override void GetEvent(Card card)
        {
            print("123");
            ItemMove(card, item);
            StartCoroutine(Disapper());
        }

        private void ItemMove(Card card, Item i)
        {
            if (i == null) return;
            
            card.GetComponent<ItemUser>().TakeItem(i);

        }

        private IEnumerator Disapper()
        {
            //Color c;
            //while (sprite.color.a > 0)
            //{
            //    c = sprite.color;
            //    c.a -= Time.deltaTime / disapperTime;
            //    sprite.color = c;
            //    yield return null;
            //}
            CardManager.RemoveCard(GetTile().GetCoordinate());
            yield return null;
        }
    }
}
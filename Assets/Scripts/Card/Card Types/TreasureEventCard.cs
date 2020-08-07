﻿using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using UnityEngine;
using DungeonRush.Managers;
using DungeonRush.Items;

namespace DungeonRush.Cards
{
    public class TreasureEventCard : EventCard
    {
        [SerializeField] ItemType iType;
        [SerializeField] Item item = null;
        [SerializeField] float disapperTime = 0;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] ItemStorage storage = null;
        protected override void Initialize()
        {
            base.Initialize();
            item = storage.GetRandomItemByType(iType);
        }

        public override void GetEvent(Card card)
        {
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
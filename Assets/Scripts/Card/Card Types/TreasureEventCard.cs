using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using DungeonRush.Cards;
using UnityEngine;

public class TreasureEventCard : EventCard
{
    [SerializeField] ItemCard item;

    public override void GetEvent(Card card)
    {
        Item i = item.GetComponent<Item>();
        ItemMove(card, i);
    }

    private void ItemMove(Card card, Item i)
    {
        if (i == null) return;

        if (i.GetItemType() == ItemType.POTION)
            card.GetComponent<ItemUser>().TakePotion(i);
        else if (i.GetItemType() == ItemType.WEAPON)
            card.GetComponent<ItemUser>().TakeWeapon(i);
        else if (i.GetItemType() == ItemType.ARMOR)
            card.GetComponent<ItemUser>().TakeArmor(i);

    }
}

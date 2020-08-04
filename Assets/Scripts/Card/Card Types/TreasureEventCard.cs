using System.Collections;
using System.Collections.Generic;
using DungeonRush.Property;
using DungeonRush.Cards;
using UnityEngine;
using DungeonRush.Managers;

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

        if (i.GetItemType() == ItemType.POTION)
            card.GetComponent<ItemUser>().TakePotion(i);
        else if (i.GetItemType() == ItemType.WEAPON)
            card.GetComponent<ItemUser>().TakeWeapon(i);
        else if (i.GetItemType() == ItemType.ARMOR)
            card.GetComponent<ItemUser>().TakeArmor(i);

    }

    private IEnumerator Disapper()
    {
        Color c;
        while(sprite.color.a > 0)
        {
            c = sprite.color;
            c.a -= Time.deltaTime / disapperTime;
            sprite.color = c;
            yield return null;
        }
        CardManager.RemoveCardForAttacker(GetTile().GetListNumber());
    }
}

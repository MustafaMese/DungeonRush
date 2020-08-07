using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    public class ItemStorage : MonoBehaviour
    {
        public List<Item> items = new List<Item>();

        public Item GetItem(int id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetId() == id)
                    return items[i];
            }

            return null;
        }

        public Item GetRandomItemByType(ItemType type)
        {
            List<Item> it = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetItemType() == type && !it.Contains(items[i]))
                    it.Add(items[i]);
            }

            var number = it.Count;

            if (number > 0)
            {
                var s = Random.Range(0, number);
                return it[s];
            }
            else
                return null;
        }
    } 
}

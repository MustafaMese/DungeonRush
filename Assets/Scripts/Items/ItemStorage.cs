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
    } 
}

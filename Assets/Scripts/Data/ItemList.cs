using System;
using System.Collections.Generic;
using DungeonRush.Items;

namespace DungeonRush.Data
{
    [Serializable]
    public class ItemList
    {
        public List<Item> items;
        public Difficulty difficulty;
    }
}


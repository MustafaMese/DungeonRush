using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Items
{
    public class ItemDB : MonoBehaviour
    {
        private static ItemDB instance = null;
        // Game Instance Singleton
        public static ItemDB Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public List<Item> allItems = new List<Item>();

        public Item GetItem(string id)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].GetId() == id)
                    return allItems[i];
            }

            return null;
        }

        public Item GetRandomItemByType(ItemType type)
        {
            List<Item> it = new List<Item>();
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].GetItemType() == type && !it.Contains(allItems[i]))
                    it.Add(allItems[i]);
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

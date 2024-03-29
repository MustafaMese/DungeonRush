﻿using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.Saving;
using DungeonRush.Skills;
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

            ItemsData data = SavingSystem.LoadItems();
            for (var i = 0; i < data.purchasedIDs.Count; i++)
                for (var y = 0; y < allItems.Count; y++)
                    if (data.GetID(allItems[y].GetID()))
                        allItems[y].Purchased = true;
        }

        public List<Item> allItems = new List<Item>();
        public List<Item> weapons = new List<Item>();
        public List<Item> armors = new List<Item>();
        public List<ItemList> levelItems = new List<ItemList>();
        public List<SkillObject> allSkills = new List<SkillObject>();

        public Item GetItem(string id)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].GetID() == id)
                    return allItems[i];
            }

            return null;
        }

        public SkillObject GetSkill(string id)
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                if (allSkills[i].GetID() == id)
                    return allSkills[i];
            }

            return null;
        }

        public SkillObject GetRandomSkill()
        {
            var number = allSkills.Count;
            if (number > 0)
            {
                var s = Random.Range(0, number);
                return allSkills[s];
            }
            else
                return null;
        }

        public Item GetRandomItem(ItemType type)
        {
            List<Item> items = SetCurrentItems(type);

            var number = items.Count;
            if (number > 0)
            {
                var ran = Random.Range(0, number);
                return items[ran];
            }
            else
                return null;
        }

        private List<Item> SetCurrentItems(ItemType type)
        {
            List<Item> items = new List<Item>();
            for (int y = 0; y < levelItems.Count; y++)
            {
                if ((int)levelItems[y].difficulty <= (int)LoadManager.Instance.levelDifficulty)
                {
                    List<Item> lItems = new List<Item>(levelItems[y].items);
                    for (var i = 0; i < levelItems[y].items.Count; i++)
                    {
                        if (lItems[i].GetItemType() == type && !items.Contains(lItems[i]))
                            items.Add(lItems[i]);
                    }
                }
            }

            return items;
        }
    } 
}

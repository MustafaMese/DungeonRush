using DungeonRush.Managers;
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
        }

        public List<Item> allItems = new List<Item>();
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
            List<Item> items = new List<Item>();

            for(int y = 0; y < levelItems.Count; y++)
            {
                if ((int)levelItems[y].difficulty <= (int)LoadManager.Instance.levelDifficulty)
                {
                    List<Item> lItems = new List<Item>(levelItems[y].items);
                    for (var i = 0; i < levelItems[y].items.Count; i++)
                    {
                        if(lItems[i].GetItemType() == type && !items.Contains(lItems[i]))
                            items.Add(lItems[i]);
                    }
                    break;
                }
            }

            // for (int i = 0; i < allItems.Count; i++)
            // {
            //     if (allItems[i].GetItemType() == type && !items.Contains(allItems[i]))
            //         items.Add(allItems[i]);
            // }

            var number = items.Count;
            print(number);
            if (number > 0)
            {
                var ran = Random.Range(0, number);
                return items[ran];
            }
            else
                return null;
        }
    } 
}

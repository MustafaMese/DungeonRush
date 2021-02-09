using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Items
    {
        public class ItemUser : MonoBehaviour
        {
            public bool isWeaponUser = false;
            public bool isArmorUser = false;
            public Item weapon = null;
            public Item armor = null;
            public SpriteRenderer weaponBone = null;
            public SpriteRenderer armorBone = null;

            private Card card = null;
            private PlayerCustomization customization;

            private bool isBeginning;

            private Dictionary<BoneType, Item> items = new Dictionary<BoneType, Item>(); 
            public int armorP;

            public List<Item> i = new List<Item>();

            private void Start()
            {
                armorP = 0;

                card = GetComponent<Card>();
                customization = GetComponent<PlayerCustomization>();
                isBeginning = true;

                items.Add(BoneType.HEAD, null);
                items.Add(BoneType.HELMET, null);
                items.Add(BoneType.BODY, null);
                items.Add(BoneType.BODY_ARMOR, null);
                items.Add(BoneType.ARM, null);
                items.Add(BoneType.LEG, null);
                items.Add(BoneType.WEAPON_LEFT, null);
                items.Add(BoneType.WEAPON_RIGHT, null);
                items.Add(BoneType.WEAPON_DUAL, null);
            }

            public List<string> GetItemsIDs()
            {
                List<string> ids = new List<string>();
                foreach (var item in items.Values)
                {
                    if(item != null)
                        ids.Add(item.GetID());
                }
                return ids;
            }

            public List<string> GetItemsNames()
            {
                List<string> names = new List<string>();
                foreach (var item in items.Values)
                {
                    if(item != null)
                        names.Add(item.GetName());
                }
                return names;
            }

            public void TakeItem(Item loot, bool openPanel = true)
            {
                BoneType lootBone = loot.GetBoneType();

                if(loot.GetItemType() != ItemType.WEAPON && loot.GetItemType() != ItemType.ARMOR)
                    loot.Execute(card);
                else
                {
                    if (items[lootBone] != null)
                        UIManager.Instance.EnableChoiceCanvas(items[loot.GetBoneType()], loot, this);
                    else
                    {
                        if(openPanel)
                            UIManager.Instance.EnableItemCanvas(loot);
                        i.Add(loot);
                        ExecuteItem(loot, lootBone);
                    }
                }
            }

            public void ExecuteItem(Item loot, BoneType lootBone)
            {
                items[lootBone] = loot;

                if (loot.GetItemType() == ItemType.ARMOR)
                    armorP += loot.GetPower();

                if (lootBone == BoneType.ARM || lootBone == BoneType.LEG || lootBone == BoneType.WEAPON_DUAL)
                    customization.ChangeBoneSprite(lootBone, loot.GetPrimarySprite(), loot.GetSecondarySprite());
                else
                    customization.ChangeBoneSprite(lootBone, loot.GetPrimarySprite());
            }

            public int GetArmor()
            {
                return armorP;
            }
        }
    }
}

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
            public SpriteRenderer weaponBone = null;

            private Card card = null;
            private PlayerCustomization customization;

            private Dictionary<BoneType, Item> items = new Dictionary<BoneType, Item>(); 
            public int armorP;

            private void Start()
            {
                armorP = 0;

                card = GetComponent<Card>();
                customization = GetComponent<PlayerCustomization>();

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
                if(loot.GetItemType() == ItemType.MAX_HEALTH_INCREASER || loot.GetItemType() == ItemType.POTION)
                    ExecuteItem(loot);
                else if(loot.GetItemType() == ItemType.WEAPON)
                {
                    if (items[BoneType.WEAPON_LEFT] != null)
                        UIManager.Instance.EnableChoiceCanvas(items[BoneType.WEAPON_LEFT], loot, this);
                    else if(items[BoneType.WEAPON_RIGHT] != null)
                        UIManager.Instance.EnableChoiceCanvas(items[BoneType.WEAPON_RIGHT], loot, this);
                    else if(items[BoneType.WEAPON_DUAL] != null)
                        UIManager.Instance.EnableChoiceCanvas(items[BoneType.WEAPON_DUAL], loot, this);
                    else
                    {
                        if (openPanel)
                            UIManager.Instance.EnableItemCanvas(loot);
                        ExecuteItem(loot, loot.GetBoneType());
                    }
                }
                else if(loot.GetItemType() == ItemType.ARMOR)
                {
                    BoneType lootBone = loot.GetBoneType();
                    if (items[lootBone] != null )
                        UIManager.Instance.EnableChoiceCanvas(items[lootBone], loot, this);
                    else
                    {
                        if (openPanel)
                            UIManager.Instance.EnableItemCanvas(loot);
                        ExecuteItem(loot, lootBone);
                    }
                }
            }

            public void ExecuteItem(Item loot)
            {
                loot.Execute(card);
            }

            public void ExecuteItem(Item loot, BoneType lootBone)
            {
                if(loot.GetItemType() == ItemType.ARMOR && items[lootBone] != null)
                {
                    Armor item = items[lootBone] as Armor;
                    item.TakeOffArmor(card);
                }

                loot.Execute(card);

                items[lootBone] = loot;
                UIManager.Instance.AddToItemSet(loot.GetPrimarySprite());

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

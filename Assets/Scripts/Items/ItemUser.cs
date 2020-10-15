using DungeonRush.Cards;
using DungeonRush.Property;
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

            [SerializeField] SpriteRenderer weaponBone = null;
            [SerializeField] SpriteRenderer armorBone = null;

            private Item weapon = null;
            private Item armor = null;
            private Card card = null;
            private Attacker attacker;

            private bool isBeginning;

            private void Start()
            {
                card = GetComponent<Card>();
                attacker = GetComponent<Attacker>();

                isBeginning = false;
            }

            public List<string> GetItemsIDs()
            {
                List<string> ids = new List<string>();
                if (weapon != null)
                    ids.Add(weapon.GetId());
                if (armor != null)
                    ids.Add(armor.GetId());

                return ids;
            }

            public List<string> GetItemsNames()
            {
                List<string> names = new List<string>();
                if (weapon != null)
                    names.Add(weapon.GetItemName());
                if (armor != null)
                    names.Add(armor.GetItemName());

                return names;
            }

            public void TakeItem(Item i)
            {
                switch (i.GetItemType())
                {
                    case ItemType.WEAPON:
                        TakeWeapon(i);
                        if (!isBeginning)
                            SetPickupCanvas(i);
                        else
                            isBeginning = false;
                        break;
                    case ItemType.POTION:
                        TakePotion(i);
                        break;
                    case ItemType.POISON:
                        TakePoison(i);
                        break;
                    case ItemType.ARMOR:
                        TakeArmor(i);
                        if (!isBeginning)
                            SetPickupCanvas(i);
                        else
                            isBeginning = false;
                        break;
                    case ItemType.MAX_HEALTH_INCREASER:
                        TakeMaxHealthIncreaser(i);
                        break;
                    case ItemType.COIN:
                        break;
                    case ItemType.NONE:
                        break;
                    default:
                        break;
                }
            }

            private void SetPickupCanvas(Item i)
            {
                UIManager.Instance.EnableItemCanvas(i);
            }

            #region WEAPON
            private void TakeWeapon(Item item)
            {
                SetWeapon(item);
                attacker.SetAttackStyle(item.GetAttackStyle());
            }
            private void SetWeapon(Item item)
            {
                this.weapon = item;
                weaponBone.sprite = item.GetSmallSprite();
            }
            public Item GetWeapon()
            {
                return weapon;
            }
            #endregion

            #region ARMOR
            private void TakeArmor(Item item)
            {
                SetArmor(item);
            }
            private void SetArmor(Item item)
            {
                this.armor = item;
                armorBone.sprite = item.GetSmallSprite();
            }
            public Item GetArmor()
            {
                return armor;
            }
            #endregion

            #region HEALTH
            private void TakePotion(Item item)
            {
                card.IncreaseHealth(item.GetPower());
            }

            private void TakePoison(Item item)
            {
                card.DecreaseHealth(item.GetPower());
            }

            private void TakeMaxHealthIncreaser(Item item)
            {
                card.IncreaseMaxHealth(item.GetPower());
            }
            #endregion
        }
    }
}

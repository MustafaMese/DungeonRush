using DungeonRush.Cards;
using System.Collections.Generic;
using TMPro;
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

            public Item weapon = null;
            public Item armor = null;
            private Card card = null;

            private void Start()
            {
                card = GetComponent<Card>();
            }

            public List<int> GetItemsIDs()
            {
                List<int> ids = new List<int>();
                if (weapon != null)
                    ids.Add(weapon.GetId());
                if (armor != null)
                    ids.Add(armor.GetId());

                return ids;
            }

            public void TakeItem(Item i)
            {
                if (i.GetItemType() == ItemType.POTION)
                    TakePotion(i);
                else if (i.GetItemType() == ItemType.WEAPON)
                    TakeWeapon(i);
                else if (i.GetItemType() == ItemType.ARMOR)
                    TakeArmor(i);
                else if (i.GetItemType() == ItemType.MAX_HEALTH_INCREASER)
                    TakeMaxHealthIncreaser(i);

            }

            #region WEAPON
            private void TakeWeapon(Item item)
            {
                SetWeapon(item);
            }
            private void SetWeapon(Item item)
            {
                if (this.weapon != null)
                {
                    Destroy(this.weapon.gameObject);
                }
                this.weapon = null;
                this.weapon = item;
                this.weapon.GetRenderer().sprite = null;
                weaponBone.sprite = item.itemSprite;
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
                if (this.armor != null)
                {
                    Destroy(this.armor.gameObject);
                }
                this.armor = null;
                this.armor = item;
                this.armor.GetRenderer().sprite = null;
                armorBone.sprite = item.itemSprite;
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

            private void TakeMaxHealthIncreaser(Item item)
            {
                card.IncreaseMaxHealth(item.GetPower());
            }
            #endregion
        }
    }
}

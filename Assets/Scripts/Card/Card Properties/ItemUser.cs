﻿using DungeonRush.Cards;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Property
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

            #region WEAPON
            public void TakeWeapon(Item item)
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
            public void TakeArmor(Item item)
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

            #region POTION
            public void TakePotion(Item item)
            {
                card.IncreaseHealth(item.GetPower());
                Destroy(item.gameObject);
            }
            #endregion
        }
    }
}

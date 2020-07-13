using DungeonRush.Cards;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Property
    {
        public class ItemUser : MonoBehaviour
        {
            private Item item;
            public SpriteRenderer weaponSprite;
            public TextMeshPro textMeshWeapon;

            Card card;

            private void Start()
            {
                card = GetComponent<Card>();
            }

            public Item GetItem()
            {
                return item;
            }

            public void TakeWeapon(Card item)
            {
                SetItem(item);
                Destroy(item.gameObject);
            }

            public void SetItem(Card item)
            {
                this.item.Set(item);
                //UpdateWeaponImage(item);
                UpdateWeaponText(this.item.health);
            }

            public void TakePotion(Card item)
            {
                card.IncreaseHealth(item.GetHealth());
                Destroy(item.gameObject);
            }

            public void UpdateWeaponImage(Card item)
            {
                weaponSprite.sprite = item.GetImage().sprite;

            }

            public void UpdateWeaponText(int health)
            {
                textMeshWeapon.text = health.ToString();
            }

            public void DecreaseItemHealth(int damage)
            {
                this.item.DecreaseHealth(damage);
                UpdateWeaponText(this.item.health);
            }

            public void ResetItem()
            {
                item.Reset();
                weaponSprite.sprite = null;
                textMeshWeapon.text = "0";
            }
        }
    }
}

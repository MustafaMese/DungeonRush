using DungeonRush.Cards;
using TMPro;
using UnityEngine;

namespace DungeonRush
{
    namespace Property
    {
        public class ItemUser : MonoBehaviour
        {
            [SerializeField] SpriteRenderer itemBone;

            public Item item;
            private Card card;

            private void Start()
            {
                card = GetComponent<Card>();
            }

            public Item GetItem()
            {
                return item;
            }

            public void TakeWeapon(Item item)
            {
                print("wow");
                SetItem(item);
            }

            public void SetItem(Item item)
            {
                print("1");
                if (this.item != null)
                {
                    Destroy(this.item.gameObject);
                    print("2");
                }
                print("3");
                print("i: " + item);
                this.item = null;
                this.item = item;
                this.item.GetRenderer().sprite = null;
                itemBone.sprite = item.itemSprite;
            }

            public void TakePotion(Item item)
            {
                card.IncreaseHealth(item.GetDamage());
                Destroy(item.gameObject);
            }
        }
    }
}

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
                SetItem(item);
                Destroy(item.gameObject);
            }

            public void SetItem(Item item)
            {
                this.item = item;
            }

            public void TakePotion(Item item)
            {
                card.IncreaseHealth(item.GetDamage());
                Destroy(item.gameObject);
            }


            public void ResetItem()
            {
                item = null;
            }
        }
    }
}

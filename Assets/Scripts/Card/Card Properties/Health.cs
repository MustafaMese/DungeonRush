using DungeonRush.Cards;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Health : MonoBehaviour
    {
        // TODO disapperı ekle.
        [SerializeField] int health;
        [SerializeField] Animator animator;

        private Card card;
        private ItemUser itemUser;
        private void Start()
        {
            card = GetComponent<Card>();
            if(GetComponent<ItemUser>())
                itemUser = GetComponent<ItemUser>();
        }

        private void Update()
        {
            if (health <= 0)
            {
                Death();
            }
        }

        private void Death()
        {

            if (card.GetCardType() == CardType.PLAYER)
                CardManager.RemoveCardForAttacker(card.GetTile().GetListNumber(), true);
            else
                CardManager.RemoveCardForAttacker(card.GetTile().GetListNumber(), false);
            
        }

        public void Set(int amount)
        {
            health = Mathf.Max(0, amount);
        }

        public int Get()
        {
            return health;
        }

        public void ChangeHealth(bool isDamage, int amount)
        {
            if (isDamage)
            {
                int armor = 0;
                if (itemUser && itemUser.GetArmor() != null)
                    armor = itemUser.GetArmor().power;
                amount -= armor;

                UpdateAnimation();
                health -= amount;
            }
            else
                health += amount;

            health = Mathf.Max(0, health);
        }

        private void UpdateAnimation()
        {
            if(card.GetCardType() != CardType.TRAP)
                animator.SetTrigger("hurt");
        }
    }
}
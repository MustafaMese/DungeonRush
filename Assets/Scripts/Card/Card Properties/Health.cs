using DungeonRush.Cards;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 0;
        [SerializeField] Animator animator = null;
        [SerializeField] CircleHealthBar bar;

        private int health = 0;
        private Card card = null;
        private ItemUser itemUser = null;
        private void Start()
        {
            card = GetComponent<Card>();
            if(GetComponent<ItemUser>())
                itemUser = GetComponent<ItemUser>();

            health = maxHealth;
            bar.SetMaxHealth(maxHealth);
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
                health = Mathf.Max(0, health);
            }
            else
            {
                health += amount;
                health = Mathf.Min(maxHealth, health);
            }

            StartCoroutine(bar.ActiveChanges(health, maxHealth));
        }

        private void UpdateAnimation()
        {
            if(card.GetCardType() != CardType.TRAP)
                animator.SetTrigger("hurt");
        }

        public void IncreaseMaxHealth(int h)
        {
            maxHealth += h;
            health += h;
            StartCoroutine(bar.ActiveChanges(health, maxHealth));
        }

        public void DecreaseMaxHealth(int h)
        {
            maxHealth -= h;
            health = Mathf.Min(health, maxHealth);
            StartCoroutine(bar.ActiveChanges(health, maxHealth));
        }
    }
}
using DungeonRush.Cards;
using DungeonRush.Managers;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float deathTime = 0.2f;
        [SerializeField] int maxHealth = 0;
        [SerializeField] Animator animator = null;
        [SerializeField] CircleHealthBar bar = null;

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
                StartCoroutine(Death());
            }
        }

        private IEnumerator Death()
        {
            if (card.GetCardType() == CardType.PLAYER)
                GameManager.gameState = GameState.STOP;

            yield return new WaitForSeconds(deathTime);
            CardManager.RemoveCardForAttacker(card.GetTile().GetListNumber());
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

                health -= amount;
                health = Mathf.Max(0, health);

                if (health > 0)
                    UpdateAnimation(false);
                else
                    UpdateAnimation(true);
            }
            else
            {
                health += amount;
                health = Mathf.Min(maxHealth, health);
            }

            StartCoroutine(bar.ActiveChanges(health, maxHealth));
        }

        private void UpdateAnimation(bool isDeath)
        {
            if (card.GetCardType() != CardType.TRAP)
            {
                if (!isDeath)
                    animator.SetTrigger("hurt");
                else
                    animator.SetTrigger("death");
            }
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
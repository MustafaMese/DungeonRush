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
        private void Start()
        {
            card = GetComponent<Card>();
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
                UpdateAnimation();
                health -= amount;
            }
            else
                health += amount;

            health = Mathf.Max(0, health);
        }

        private void UpdateAnimation()
        {
            animator.SetTrigger("hurt");
        }
    }
}
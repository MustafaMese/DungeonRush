using DungeonRush.Cards;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush
{
    namespace Property
    {
        public class Attacker : MonoBehaviour
        {
            public bool isItemUser = false;

            private Card card;
            private ItemUser itemUser;

            private void Start()
            {
                card = GetComponent<Card>();
                if(isItemUser)
                    itemUser = GetComponent<ItemUser>();
            }

            public bool CanAttack(Card enemy)
            {
                if (enemy.GetCardType() != CardType.ENEMY)
                    return true;

                int enemyHealth = enemy.GetHealth();
                int health = card.GetHealth();
                if (itemUser && itemUser.GetItem().exist)
                {
                    if (enemyHealth > itemUser.GetItem().GetHealth())
                        return false;
                }
                else if (health <= enemyHealth)
                    return false;
                return true;
            }

            public void AttackWithItem(Card enemy)
            {
                int itemHealth = itemUser.GetItem().GetHealth();
                int enemyHealth = enemy.GetHealth();
                if (enemyHealth == itemHealth)
                {
                    itemUser.ResetItem();
                    DestroyCard(enemy);
                    // Buralarda bazı sıkıntılar var ama itemlere gelince de halledilebilinir.
                }
                else if (enemyHealth < itemHealth)
                {
                    itemUser.DecreaseItemHealth(enemyHealth);
                }
                else
                {
                    enemy.DecreaseHealth(itemHealth);
                    itemUser.ResetItem();
                }
            }

            public void AttackWithoutItem(Card enemy)
            {
                int health = card.GetHealth();
                int enemyHealth;
                if (enemy != null)
                    enemyHealth = enemy.GetHealth();
                else
                    enemyHealth = 0;

                if (health > enemyHealth || enemy == null)
                {
                    card.DecreaseHealth(enemyHealth);
                }
                else
                {
                    card.DecreaseHealth(enemyHealth);
                    enemy.DecreaseHealth(health);
                    DestroyCard(card);

                    if(enemyHealth <= 0) 
                        DestroyCard(enemy);
                }
            }

            public void Attack(Card enemy)
            {
                if (isItemUser && itemUser && itemUser.GetItem().exist)
                    AttackWithItem(enemy);
                else
                    AttackWithoutItem(enemy);
            }

            public void LoadLoseScene()
            {
                LoadManager.LoadLoseScene();
            }

            public void DestroyCard(Card enemy) 
            {
                if (enemy == null) return;

                if (enemy.GetCardType() == CardType.PLAYER)
                    CardManager.RemoveCardForAttacker(enemy.GetTile().GetListNumber(), true);
                else
                    CardManager.RemoveCardForAttacker(enemy.GetTile().GetListNumber(), false);
            }
        }
    }
}

using DungeonRush.Cards;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
                int enemyHealth = enemy.GetHealth();
                if (health > enemyHealth)
                {
                    card.DecreaseHealth(enemyHealth);
                }
                else
                {
                    card.DecreaseHealth(enemyHealth);
                    enemy.DecreaseHealth(health);
                    if(card.GetCardType() == CardType.PLAYER)
                        Invoke("LoadLoseScene", 0.5f);
                    else
                    {
                        GameManager.addCard = true;
                        GameManager.cardListNumber = card.GetTile().GetListNumber();
                        card.Disappear();
                        Destroy(card.gameObject);
                    }
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
        }
    }
}

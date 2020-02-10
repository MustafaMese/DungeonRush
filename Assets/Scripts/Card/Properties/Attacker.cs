using DungeonRush.Cards;
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

            public bool CanAttack(EnemyCard enemy)
            {
                int enemyHealth = enemy.GetHealth();
                if (itemUser && itemUser.GetItem().exist)
                {
                    int itemHealth = itemUser.GetItem().GetHealth();
                    if (enemyHealth > itemHealth)
                    {
                        enemy.DecreaseHealth(itemHealth);
                        itemUser.ResetItem();
                        return false;
                    }
                    return true;
                }
                return true;
            }

            public void AttackWithItem(EnemyCard enemy)
            {
                int itemHealth = itemUser.GetItem().GetHealth();
                int enemyHealth = enemy.GetHealth();
                if (enemyHealth == itemHealth)
                    itemUser.ResetItem();
                else if (enemyHealth < itemHealth)
                    itemUser.DecreaseItemHealth(enemyHealth);
            }

            public void AttackWithoutItem(EnemyCard enemy)
            {
                int health = card.GetHealth();
                int enemyHealth = enemy.GetHealth();
                if (health > enemyHealth)
                    card.DecreaseHealth(enemyHealth);
                else
                    LoadManager.LoadLoseScene();
            }

            public void Attack(EnemyCard enemy)
            {
                if (isItemUser && itemUser && itemUser.GetItem().exist)
                    AttackWithItem(enemy);
                else
                    AttackWithoutItem(enemy);
            }
        }
    }
}

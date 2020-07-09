using DungeonRush.Cards;
using DungeonRush.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Health : MonoBehaviour
    {
        public int health;
        private Card card;

        private void Start()
        {
            card = GetComponent<Card>();
        }

        private void Update()
        {
            if (health <= 0)
                Death();
        }

        private void Death()
        {

            if (card.GetCardType() == CardType.PLAYER)
                CardManager.RemoveCardForAttacker(card.GetTile().GetListNumber(), true);
            else
                CardManager.RemoveCardForAttacker(card.GetTile().GetListNumber(), false);
            // ÖLME ANİMASYONU. FALAN
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
                health -= amount;
            else
                health += amount;

            health = Mathf.Max(0, health);
        }
    }
}
using DungeonRush.Cards;
using DungeonRush.Controller;
using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class Health : MonoBehaviour
    {
        public struct StatusActControl
        {
            public bool isAcid;

            public void Reset()
            {
                isAcid = false;
            }

            public void ActControl(List<StatusData> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].status.StatusType == StatusType.ACID)
                        isAcid = true;
                }
            }
        }
        private StatusActControl statusAct;

        [SerializeField] bool isPlayer = false;
        [SerializeField] float deathTime = 0.2f;
        [SerializeField] int maxHealth = 0;
        [SerializeField] HealthBar bar = null;

        [SerializeField] int health = 0;
        private Card card = null;
        private ItemUser itemUser = null;
        private StatusController statusController = null;
        private Animator animator = null;

        private void Start()
        {
            card = GetComponent<Card>();
            animator = card.Animator;
            statusController = card.GetComponent<StatusController>();
            if(GetComponent<ItemUser>())
                itemUser = GetComponent<ItemUser>();

            health = maxHealth;
            bar.SetMaxHealth(maxHealth);
            bar.ActiveChanges(health, maxHealth);
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
            UpdateAnimation(true);
            if (isPlayer)
            {
                this.enabled = false;
                GameManager.Instance.SetGameState(GameState.DEFEAT);
            }
            else
            {
                AIController controller = GetComponent<AIController>();
                controller.Stop();
                CardManager.Unsubscribe(card);

                yield return new WaitForSeconds(deathTime);
                CardManager.RemoveCardForAttacker(card.GetTile().GetCoordinate());
            }
        }

        public void SetCurrentHealth(int amount)
        {
            health = Mathf.Max(0, amount);
        }

        public void SetMaxHealth(int amount)
        {
            maxHealth = Mathf.Max(0, amount);
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetCurrentHealth()
        {
            return health;
        }

        public void ChangeHealth(bool isDamage, int amount)
        {
            statusAct.Reset();
            statusAct.ActControl(statusController.activeStatuses);

            if (isDamage)
            {
                if (!statusAct.isAcid)
                    amount = CalculateBlockedDamageByArmor(amount);

                health -= amount;
                health = Mathf.Max(0, health);

                if (health > 0)
                    UpdateAnimation(false);
                else
                {
                    if (card.LifeCount > 0)
                    {
                        print("Tekrar dirildi.");
                        health = (int)(maxHealth * 30 / 100);
                        card.LifeCount--;
                    }
                }
            }
            else
            {
                health += amount;
                health = Mathf.Min(maxHealth, health);
            }

            bar.ActiveChanges(health, maxHealth);
        }

        private int CalculateBlockedDamageByArmor(int amount)
        {
            int armor = 0;
            if (itemUser && itemUser.GetArmor() != null)
                armor = itemUser.GetArmor().GetPower();
            amount -= armor;
            amount = Mathf.Max(0, amount);
            return amount;
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
            bar.ActiveChanges(health, maxHealth);
        }

        public void DecreaseMaxHealth(int h)
        {
            maxHealth -= h;
            health = Mathf.Min(health, maxHealth);
            bar.ActiveChanges(health, maxHealth);
        }

    }
}
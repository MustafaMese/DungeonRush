using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Cards;
using DungeonRush.Data;

namespace DungeonRush.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        private static CollectableManager instance = null;
        // Game Instance Singleton
        public static CollectableManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            Instance = this;
            PrepareCoins();
        }

        [Header("UI References")]
        [SerializeField] GameObject animatedCoinPrefab;

        [Space, Header("Animation settings")]
        [SerializeField, Range(0.5f, 0.9f)] float minAnimDuration;
        [SerializeField, Range(0.9f, 2f)] float maxAnimDuration;
        [SerializeField] float spread;

        private ObjectPool objectPool = new ObjectPool();
        private PlayerCard player;

        private void Start()
        {
            player = FindObjectOfType<PlayerCard>();
        }

        private void PrepareCoins()
        {
            GameObject coin;
            coin = Instantiate(animatedCoinPrefab);
            FillPool(coin, 15);
        }

        private void FillPool(GameObject coin, int coinCount)
        {
            objectPool.SetObject(coin);
            objectPool.Fill(coinCount, transform);
        }

        private void Animate(Vector3 collectedCoinPosition)
        {
            int amount = Random.Range(1, 5);
            for (int i = 0; i < amount; i++)
            {
                GameObject coin = objectPool.Pull(transform);
                coin.SetActive(true);
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f);

                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                StartCoroutine(DeactivateCoin(coin, duration));
            }
        }

        private IEnumerator DeactivateCoin(GameObject coin, float duration)
        {
            yield return new WaitForSeconds(duration);
            coin.SetActive(false);
            objectPool.Push(coin);
        }

        public void GainXpAndGold(Vector3 collectedCoinPosition, int maxHealth)
        {
            CalculateXpAndGold(maxHealth);
            Animate(collectedCoinPosition);
        }

        private void CalculateXpAndGold(int maxHealth)
        {
            int gold = (maxHealth / 5) * 2;
            int xp = (maxHealth / 7) * 2;

            player.Gold += gold;
            player.Experience += xp;
        }
    }
}
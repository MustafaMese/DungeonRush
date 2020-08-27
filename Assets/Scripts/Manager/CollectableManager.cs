using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;

public class CollectableManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject animatedCoinPrefab;

    [Space]
    [Header("Avaible coins : (coin to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Space]
    [Header("Animation settings")]
    [SerializeField, Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField, Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] float spread;

    private PlayerCard player;
    private void Awake()
    {
        PrepareCoins();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerCard>();
    }

    private void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    private void Animate(Vector3 collectedCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = coinsQueue.Dequeue();
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
        coinsQueue.Enqueue(coin);
        player.Coins++;
        int sum = Random.Range(1, 3);
        player.Experience += sum;
    }

    public void AddCoins(Vector3 collectedCoinPosition, int level)
    {
        Animate(collectedCoinPosition, level);
    }
}

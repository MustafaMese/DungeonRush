using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Traits;
using UnityEngine;

public class Impact : MonoBehaviour
{
    [Header("General Settings")]
    [Space]
    [SerializeField] bool impactOnCards;
    [SerializeField] bool impactOnTiles;
    [SerializeField, Tooltip("Do you need status for this impact?")] StatusObject status;
    [SerializeField] ElementType elementType;
    [SerializeField, Tooltip("If you need, include itself too")] Vector2[] containedPoints;

    [Header("Effect Settings")]
    [Space]
    [SerializeField] GameObject effect;
    [SerializeField] float effectTime;
    [SerializeField, Tooltip("For object pooling")] int objectCount;

    [Header("Damage Settings")]
    [Space]
    [SerializeField] int power;
    [SerializeField] bool usingTextPopup;

    private ObjectPool pool;

    public void Execute(Tile tile)
    {
        if (impactOnCards)
        {
            Card card = tile.GetCard();

            card.GetDamagable().DecreaseHealth(power);
            if(status != null)
                card.GetStatusController().AddStatus(status);
        }

        if(impactOnTiles)
        {
            EnvironmentCard environmentCard = EnvironmentManager.Instance.GetEnvironmentCard(elementType);
            EnvironmentCard.Change(tile.GetEnvironmentCard(), environmentCard, tile);
        }

    }

    private void DoImpactOnCards(Tile tile)
    {
        if (!impactOnCards) return;

        Vector2 currentPosition = tile.GetCoordinate();
        for (var i = 0; i < containedPoints.Length; i++)
        {
            Vector2 newPos = currentPosition + containedPoints[i];
            Card card = Board.tilesByCoordinates[newPos].GetCard();

            card.GetDamagable().DecreaseHealth(power);
            if (status != null)
                card.GetStatusController().AddStatus(status);
        }
    }

    private void DoImpactOnTiles(Tile tile)
    {
        if(!impactOnTiles) return;

        Vector2 currentPosition = tile.GetCoordinate();
        EnvironmentCard environmentCard = EnvironmentManager.Instance.GetEnvironmentCard(elementType);
        for (var i = 0; i < containedPoints.Length; i++)
        {
            Vector2 newPos = currentPosition + containedPoints[i];
            if(Board.tilesByCoordinates.ContainsKey(newPos))
            {
                Tile newTile = Board.tilesByCoordinates[newPos];
                EnvironmentCard.Change(newTile.GetEnvironmentCard(), environmentCard, newTile);
            }
        }
    }

    private void Initialize()
    {
        Fill(pool, effect, objectCount);
    }

    private void Fill(ObjectPool pool, GameObject effect, int objectCount)
    {
        pool = new ObjectPool();

        pool.SetObject(effect);
        pool.Fill(objectCount, transform);
    }

    private void Animate()
    {
        EffectOperator.Instance.Operate(pool, transform, transform.position, effectTime);
        if(usingTextPopup)
            TextPopupManager.Instance.TextPopup(transform.position, power.ToString());
    }

    public IEnumerator Kill()
    {
        yield return new WaitForSeconds(effectTime);
        Destroy(gameObject);
    }

}

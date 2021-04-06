using System.Collections;
using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Traits;
using UnityEngine;

namespace DungeonRush.Cards
{
    public class Impact : MonoBehaviour
    {
        [Header("General Settings")]
        [Space]
        public string impactName;
        [SerializeField] bool impactOnCards;
        [SerializeField] bool impactOnTiles;
        [SerializeField, Tooltip("Do you need status for this impact?")] StatusObject status;
        public ElementType elementType;
        [SerializeField, Tooltip("If you need, include (0, 0) too")] Vector2[] containedPoints;

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
            Animate();

            if (impactOnCards)
                DoImpactOnCards(tile);

            if (impactOnTiles)
                DoImpactOnTiles(tile);

            StartCoroutine(Kill());
        }

        private void DoImpactOnCards(Tile tile)
        {
            if (!impactOnCards) return;

            Vector2 currentPosition = tile.GetCoordinate();
            for (var i = 0; i < containedPoints.Length; i++)
            {
                Vector2 newPos = currentPosition + containedPoints[i];
                if (Board.tilesByCoordinates.ContainsKey(newPos))
                {
                    Card card = Board.tilesByCoordinates[newPos].GetCard();
                    if (card != null && card.GetDamagable() != null)
                    {
                        card.GetDamagable().DecreaseHealth(power);

                        if (usingTextPopup)
                            TextPopupManager.Instance.TextPopup(transform.position, power.ToString());

                        if (status != null)
                            card.GetStatusController().AddStatus(status);
                    }
                }
            }
        }

        private void DoImpactOnTiles(Tile tile)
        {
            if (!impactOnTiles) return;

            Vector2 currentPosition = tile.GetCoordinate();
            for (var i = 0; i < containedPoints.Length; i++)
            {
                Vector2 newPos = currentPosition + containedPoints[i];
                if (Board.tilesByCoordinates.ContainsKey(newPos))
                {
                    Tile target = Board.tilesByCoordinates[newPos];
                    Change(target, target.GetEnvironmentCard());
                }
            }
        }

        private void Change(Tile targetT, EnvironmentCard targetC)
        {
            if (targetC == null) return;

            bool delete;
            var prefab = EnvironmentManager.Instance.GetElementPrefab(this, targetC.GetElementType(), out delete);
            if (delete)
                targetC.Remove(targetC);
            else if (prefab != null)
            {
                targetC.Remove(targetC);
                CardManager.Instance.AddCard(prefab, targetT);
            }
        }

        public void Initialize(Vector2 position)
        {
            transform.position = position;
            transform.SetParent(null);

            pool = new ObjectPool();
            Fill(pool, effect, objectCount);
        }

        private void Fill(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.Fill(objectCount, transform);
        }

        private void Animate()
        {
            EffectOperator.Instance.Operate(pool, transform, transform.position, effectTime);
        }

        private IEnumerator Kill()
        {
            yield return new WaitForSeconds(effectTime);
            Destroy(gameObject);
        }

    }

}

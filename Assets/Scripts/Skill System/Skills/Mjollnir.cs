using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class Mjollnir : PassiveSkill
    {
        private Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0),
                                        new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1),
                                        new Vector2(2, 0), new Vector2(-2, 0), new Vector2(0, 2), new Vector2(0, -2)};

        private List<Card> targets = new List<Card>();
        private List<Vector3> targetPositions = new List<Vector3>();

        private ObjectPool<MjollnirPositioning> effectPool;

        public override void Initialize(Card card)
        {
            tempCooldown = 0;
            this.card = card;

            InitializeLighting();
        }

        private void InitializeLighting()
        {
            effectPool = new ObjectPool<MjollnirPositioning>();
            effectPool.SetObject(effect.GetComponent<MjollnirPositioning>());
            effectPool.Fill(2, transform);

            for (var i = 0; i < effectPool.GetLength(); i++)
            {
                MjollnirPositioning obj = effectPool.Pull(transform);
                obj.Initialize(gameObject);
                effectPool.Push(obj);
            }
        }

        public override void Execute(Move move)
        {
            FindTargets(move);
            for (int i = 0; i < targets.Count; i++)
            {
                Card card = targets[i];
                if (card != null)
                    card.GetDamagable().DecreaseHealth(Power);
            }
        }

        public override IEnumerator Animate(Move move)
        {
            MjollnirPositioning mPos = effectPool.Pull(transform);
            mPos.transform.SetParent(null);
            mPos.transform.position = Vector3.zero;

            mPos.Execute(targetPositions, EffectTime);
            if (isUsingTextPopup)
                TextPopupManager.Instance.TextPopup(mPos.transform.position, Power.ToString());

            yield return new WaitForSeconds(EffectTime);

            mPos.Deactivate();
            mPos.transform.SetParent(transform);
            mPos.gameObject.SetActive(false);
            effectPool.Push(mPos);
        }

        private void FindTargets(Move move)
        {
            targets.Clear();
            targetPositions.Clear();
            Vector2 currentCoordinate = move.GetCardTile().GetCoordinate();

            bool isFinished = false;
            while (!isFinished)
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    Vector2 direction = directions[i];

                    Vector2 target = currentCoordinate + direction;
                    if (Board.tilesByCoordinates.ContainsKey(target))
                    {
                        Tile targetTile = Board.tilesByCoordinates[target];
                        Card targetCard = targetTile.GetCard();
                        if (targetCard != null && (targetCard.GetCardType() == CardType.ENEMY || targetCard.GetCardType() == CardType.PLAYER) &&
                                targetCard != move.GetCard() && !targets.Contains(targetCard))
                        {
                            targets.Add(targetCard);
                            targetPositions.Add(targetTile.GetCoordinate());
                            currentCoordinate = targetTile.GetCoordinate();
                            break;
                        }
                    }

                    if (i == directions.Length - 1)
                        isFinished = true;
                }

            }
        }
    }
}
﻿using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
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

        MjollnirPositioning mPos;

        public override void Initialize(Card card)
        {
            base.Initialize(card);
            GameObject obj = effectPool.Pull(transform);
            mPos = obj.GetComponent<MjollnirPositioning>();
            effectPool.Push(obj);
            mPos.Initialize();
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

        public override void PositionEffect(GameObject effect, Move move)
        {
            effect.transform.SetParent(null);
            effect.transform.position = Vector3.zero;
            mPos.Execute(targetPositions, EffectTime);
        }

        public override IEnumerator Animate(Move move)
        {
            // Burayı yap.
            yield return null;
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
                            targetPositions.Add(target);
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
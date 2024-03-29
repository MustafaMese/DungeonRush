﻿using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class Bomb : ActiveSkill
    {
        private Vector2[] directions = {new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0),
                                        new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1)};

        private List<Card> targets = new List<Card>();

        public override void Execute(Move move)
        {
            FindTargets(move);

            for (int i = 0; i < targets.Count; i++)
            {
                Card tCard = targets[i];
                if (tCard != null)
                    tCard.GetDamagable().DecreaseHealth(Power);
            }

            SkillButtonControl();
        }

        private void FindTargets(Move move)
        {
            targets.Clear();

            Vector2 coordinate = move.GetCardTile().GetCoordinate();

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];
                Vector2 targetCoordinate = coordinate + direction;

                if(Board.tilesByCoordinates.ContainsKey(targetCoordinate))
                {
                    Tile targetTile = Board.tilesByCoordinates[targetCoordinate];
                    Card tCard = targetTile.GetCard();

                    if (tCard != null && (tCard.GetCardType() == CardType.ENEMY || tCard.GetCardType() == CardType.PLAYER))
                        targets.Add(tCard);
                }
            }
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetCard().transform;
            effect.transform.SetParent(t);
            effect.transform.position = t.position;
        }
    }
}
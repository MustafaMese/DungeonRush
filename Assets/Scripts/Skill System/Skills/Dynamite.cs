﻿using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Skills
{
    public class Dynamite : ActiveSkill
    {
        private Vector2[] directions = {new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 0), new Vector2(2, 0),
                                            new Vector2(-1, 0), new Vector2(-2, 0), new Vector2(0, -1), new Vector2(0, -2)};

        private int[] angles = { 270, 90, 180, 0 };
        const float circleAngle = 360;

        private int index = 0;
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

        public override void PositionEffect(GameObject effect, Move move)
        {
            Transform t = move.GetCardTile().transform;
            effect.transform.position = t.position;
            float rot = effect.transform.rotation.z;

            rot = RotateAngle(rot, angles[index]);

            if (index < 3)
                index++;
            else
                index = 0;

            effect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        }
        
        private float RotateAngle(float z, int d)
        {
            return Mathf.MoveTowardsAngle(z, d, circleAngle);
        }

        public override int GetGameobjectCount(bool isTextPopup = false)
        {
            return 4;
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
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/Bomb")]
    public class Bomb : Skill
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
                    tCard.DecreaseHealth(Power);
            }
        }

        private void FindTargets(Move move)
        {
            targets.Clear();

            Vector2 coordinate = move.GetCardTile().GetCoordinate();
            int rL = Board.RowLength;

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];
                Vector2 targetCoordinate = coordinate + direction;

                if(targetCoordinate.x < rL && targetCoordinate.x >= 0 && targetCoordinate.y < rL && targetCoordinate.y >= 0)
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

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            return Vector3.zero;
        }
    }
}
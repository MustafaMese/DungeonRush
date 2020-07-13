using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/ThreeToOneBoxAttacking")]
    public class ThreeToOneBoxAttacking : AttackStyle
    {
        private List<Card> tempList = new List<Card>();

        public override void Attack(Move move, int damage)
        {
            Tile target = move.GetTargetTile();

            tempList = FindTargetTiles(move);
            tempList.Add(target.GetCard());

            for (int i = 0; i < tempList.Count; i++)
            {
                if(tempList[i] != null)
                    tempList[i].DecreaseHealth(damage);
            }
        }

        private List<Card> FindTargetTiles(Move move)
        {
            tempList.Clear();
            int rL = Board.RowLength;
            Tile t = move.GetTargetTile();
            Vector2 coordinate = t.transform.position;

            var dir = GetDirection(move);

            if(dir.y != 0)
            {
                if (coordinate.x < rL - 1)
                {
                    Tile upperT = Board.tilesByCoordinates[new Vector2(coordinate.x + 1, coordinate.y)];
                    if (upperT.GetCard() != null)
                        tempList.Add(upperT.GetCard());
                }
                if (coordinate.x > 0)
                {
                    Tile lowerT = Board.tilesByCoordinates[new Vector2(coordinate.x - 1, coordinate.y)];
                    if(lowerT.GetCard() != null)
                        tempList.Add(lowerT.GetCard());
                }
            }
            else if(dir.x != 0)
            {
                if (coordinate.y < rL - 1)
                {
                    Tile upperT = Board.tilesByCoordinates[new Vector2(coordinate.x, coordinate.y + 1)];
                    if (upperT.GetCard() != null)
                        tempList.Add(upperT.GetCard());
                }
                if (coordinate.y > 0)
                {
                    Tile lowerT = Board.tilesByCoordinates[new Vector2(coordinate.x, coordinate.y - 1)];
                    if (lowerT.GetCard() != null)
                        tempList.Add(lowerT.GetCard());
                }
            }

            
            return tempList;
        }

        private Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().transform.position - move.GetCardTile().transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }
    }
}
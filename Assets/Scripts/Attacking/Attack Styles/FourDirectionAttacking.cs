using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack/FourDirectionAttack")]
    public class FourDirectionAttacking : AttackStyle
    {
        List<Card> tempList = new List<Card>();

        private Vector2[] directions = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1),
                                            new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };

        public override void Attack(Move move, int damage)
        {
            tempList = FindTargetTiles(move);

            for (int i = 0; i < tempList.Count; i++)
            {
                if(tempList[i] != null)
                {
                    Card card = tempList[i];
                    card.DecreaseHealth(damage);
                }
            }
        }

        public override List<Card> GetAttackedCards(Move move)
        {
            tempList = FindTargetTiles(move);
            return tempList;
        }

        public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null)
        {
            effect.transform.position = tPos;
        }

        private List<Card> FindTargetTiles(Move move)
        {
            tempList.Clear();
            Tile t = move.GetCardTile();
            Vector2 coordinate = t.GetCoordinate();

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];
                Vector2 targetCoordinate = coordinate + direction;
                if (Board.tilesByCoordinates.ContainsKey(targetCoordinate))
                {
                    Card card = Board.tilesByCoordinates[targetCoordinate].GetCard();
                    if(card != null && (card.GetCardType() == CardType.ENEMY || card.GetCardType() == CardType.PLAYER))
                        tempList.Add(card);
                }
            }

            return tempList;
        }
    }
}

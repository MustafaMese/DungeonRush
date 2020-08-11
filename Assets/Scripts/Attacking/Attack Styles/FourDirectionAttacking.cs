using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Attacking
{
    [CreateAssetMenu(menuName = "Attack/FourDirectionAttack")]
    public class FourDirectionAttacking : AttackStyle
    {
        private List<Card> tempList = new List<Card>();

        public override void Attack(Move move, int damage)
        {
            throw new System.NotImplementedException();
        }

        public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null)
        {
            effect.transform.position = tPos;
        }

        private List<Card> FindTargetTiles(Move move)
        {
            tempList.Clear();
            int rL = Board.RowLength;
            Tile t = move.GetCardTile();
            Vector2 coordinate = t.GetCoordinate();
        }
    }
}

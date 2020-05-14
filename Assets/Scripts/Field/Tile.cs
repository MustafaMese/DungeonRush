using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Data;
using DungeonRush.Settings;
using UnityEngine;

namespace DungeonRush
{
    namespace Field 
    {
        public class Tile : MonoBehaviour
        {
            private Vector2Int coordinate;
            public Card card;
            private int listNumber;

            public Card GetCard()
            {
                return card;
            }

            public Vector2Int GetCoordinate()
            {
                return Geometry.GridPoint(coordinate.x, coordinate.y);
            }

            public void SetCoordinate(Vector3 pos)
            {
                this.coordinate = Geometry.GridFromPoint(pos);
            }

            public void SetCard(Card card)
            {
                this.card = card;
            }

            public bool IsTileOccupied()
            {
                if (this.card != null)
                    return true;
                return false;
            }

            public int GetListNumber()
            {
                return listNumber;
            }

            public void SetListNumber(int listNumber)
            {
                this.listNumber = listNumber;
            }

            public static void ChangeTile(Move move, bool isEmpty, bool isPlayer)
            {
                if (!isEmpty)
                    CardManager.RemoveCard(move.GetTargetTile(), false);
                if (isPlayer)
                    CardManager.Instance.SetInstantPlayerTile(move.GetTargetTile());
                    //GameManager.GetCardManager().SetInstantPlayerTile(move.GetTargetTile());
                move.GetTargetTile().SetCard(move.GetCard());
                //move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}

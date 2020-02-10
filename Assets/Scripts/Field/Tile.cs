using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Moves;
using DungeonRush.Settings;
using UnityEngine;

namespace DungeonRush
{
    namespace Element 
    {
        public class Tile : MonoBehaviour
        {
            private Vector2Int coordinate;
            private Card card;
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
                    GameManager.RemoveCard(move, false);
                if (isPlayer)
                    GameManager.GetCardManager().SetInstantPlayerTile(move.GetTargetTile());
                move.GetTargetTile().SetCard(move.GetCard());
                move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}

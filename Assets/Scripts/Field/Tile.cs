using DungeonRush.Cards;
using DungeonRush.Managers;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush
{
    namespace Field 
    {
        public class Tile : MonoBehaviour
        {
            [SerializeField] SpriteRenderer darkness = null;
            public Vector2 coordinate = Vector2.zero;
            public Card card = null;
            public int listNumber = 0;
            public Card trapCard = null;
            
            public void SetDarkness(Sprite sprite)
            {
                darkness.sprite = sprite;
            }

            public Card GetTrapCard()
            {
                return trapCard;
            }

            public Card GetCard()
            {
                return card;
            }

            public Vector2 GetCoordinate()
            {
                return coordinate;
            }

            public void SetCoordinate(Vector3 pos)
            {
                this.coordinate = pos;
            }

            public void SetTrapCard(Card card)
            {
                this.trapCard = card;
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
                move.GetTargetTile().SetCard(move.GetCard());
                move.GetCardTile().SetCard(null);
                move.GetCard().SetTile(move.GetTargetTile());
            }
        }
    }
}

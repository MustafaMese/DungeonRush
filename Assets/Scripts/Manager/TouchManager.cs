using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Element;
using static DungeonRush.Moves.Move;
using DungeonRush.Moves;

namespace DungeonRush
{
    namespace Managers {
        public class TouchManager : MonoBehaviour
        {
            public Board board;

            private void Start()
            {
            }

            void Update()
            {
                if (!Board.touched)
                    GetTouchedCell();
            }

            private void GetTouchedCell()
            {
                if (Input.touches.Length > 0)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit2D hit;
                    if (Physics2D.Raycast(ray.origin, ray.direction, 1000.0f))
                    {
                        hit = Physics2D.Raycast(ray.origin, ray.direction, 1000.0f);
                        foreach (var tile in board.GetCardPlaces())
                        {
                            if (hit.collider.gameObject == tile.gameObject)
                            {

                                if (tile.IsTileOccupied() && tile.GetCard().GetCardType() == CardType.PLAYER)
                                    continue;

                                // Oyuncunun kartı aranıyo burada.
                                foreach (var card in GameManager.GetCardManager().cards)
                                {
                                    if (card.GetCardType() != CardType.PLAYER)
                                        continue;

                                    Board.touched = true;
                                    card.isMoving = true;
                                    if (tile.IsTileOccupied())
                                    {
                                        if (tile.GetCard().GetCardType() == CardType.ENEMY)
                                        {
                                            Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Attack, false);
                                            GameManager.GetMoveMaker().SetInstantMove(move);
                                            return;
                                        }
                                        else if (tile.GetCard().GetCardType() == CardType.ITEM)
                                        {
                                            Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Item, false);
                                            GameManager.GetMoveMaker().SetInstantMove(move);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Empty, false);
                                        GameManager.GetMoveMaker().SetInstantMove(move);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit;
                    if (Physics2D.Raycast(ray.origin, ray.direction, 1000.0f))
                    {
                        hit = Physics2D.Raycast(ray.origin, ray.direction, 1000.0f);
                        foreach (var tile in board.GetCardPlaces())
                        {
                            if (hit.collider.gameObject == tile.gameObject)
                            {

                                if (tile.IsTileOccupied() && tile.GetCard().GetCardType() == CardType.PLAYER)
                                    continue;

                                // Oyuncunun kartı aranıyo burada.
                                foreach (var card in GameManager.GetCardManager().cards)
                                {
                                    if (card.GetCardType() != CardType.PLAYER)
                                        continue;
                                    Debug.Log("lol");
                                    Board.touched = true;
                                    card.isMoving = true;
                                    if (tile.IsTileOccupied())
                                    {
                                        if (tile.GetCard().GetCardType() == CardType.ENEMY)
                                        {
                                            Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Attack, false);
                                            GameManager.GetMoveMaker().SetInstantMove(move);
                                            return;
                                        }
                                        else if (tile.GetCard().GetCardType() == CardType.ITEM)
                                        {
                                            Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Item, false);
                                            GameManager.GetMoveMaker().SetInstantMove(move);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Move move = new Move(GameManager.GetCardManager().instantPlayerTile, tile, card, MoveType.Empty, false);
                                        GameManager.GetMoveMaker().SetInstantMove(move);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
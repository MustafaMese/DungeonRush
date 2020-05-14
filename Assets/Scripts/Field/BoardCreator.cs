using DungeonRush.Field;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Field
{
    [ExecuteAlways]
    public class BoardCreator : MonoBehaviour
    {
        public int rowLength;

        [SerializeField] Vector2 startPos;
        [SerializeField] float emptySpace;
        [SerializeField] float xSpace;
        [SerializeField] float ySpace;
        [SerializeField] Tile tilePrefab;

        public bool create;
        public bool delete;
        public bool display;

        Board board;

        private void Awake()
        {
            board = FindObjectOfType<Board>();
        }

        private void Update()
        {
            if (create)
            {
                create = false;
                Create();
            }

            if (delete)
            {
                delete = false;
                Delete();
            }

            if (display)
            {
                display = false;
                DisplayTiles();
            }
        }

        private void Delete()
        {
            if (board.cardPlaces != null)
            {
                for (int i = 0; i < board.cardPlaces.Count; i++)
                {
                    DestroyImmediate(board.cardPlaces[i].gameObject);
                }
                Board.tiles.Clear();
                board.cardPlaces = null;
            }
        }

        public void Create()
        {
            if (board.cardPlaces == null)
            {
                var list = new List<Tile>();
                var currentPos = startPos;

                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < rowLength; j++)
                    {
                        var tile = Instantiate(tilePrefab, currentPos, Quaternion.identity, board.transform);
                        list.Add(tile);
                        currentPos.x += xSpace + emptySpace;
                    }
                    currentPos.x = startPos.x;
                    currentPos.y -= ySpace + emptySpace;
                }

                InitializeTiles(list);
            }
        }

        public void InitializeTiles(List<Tile> cardPlaces)
        {
            int i = 0;
            foreach (var pos in cardPlaces)
            {
                pos.SetCoordinate(pos.transform.position);
                pos.SetCard(null);
                pos.SetListNumber(i);
                Board.tiles.Add(i, pos);
                i++;
            }
            board.SetCardPlaces(cardPlaces);
        }

        public void DisplayTiles()
        {
            foreach (var tile in Board.tiles.Values)
            {
                print("haha");
                print(tile.GetListNumber() + " " + tile);
            }
        }
    }
}
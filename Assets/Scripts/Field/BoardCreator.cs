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

        [SerializeField] Vector2 startPos = Vector2.zero;
        [SerializeField] float emptySpace = 0;
        [SerializeField] float xSpace = 0;
        [SerializeField] float ySpace = 0;
        [SerializeField] Tile tilePrefab = null;

        public static float _XSpace = 0;
        public static float _YSpace = 0;
        public static float _EmptySpace = 0;

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
                SetValues();
            }

            if (delete)
            {
                delete = false;
                Delete();
                SetValues();
            }

            if (display)
            {
                display = false;
                DisplayTiles();
                SetValues();
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
                Board.tilesByListnumbers.Clear();
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
                    currentPos.y += ySpace + emptySpace;
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
                Board.tilesByListnumbers.Add(i, pos);
                Board.tilesByCoordinates.Add(pos.transform.position, pos);
                i++;
            }
            board.SetCardPlaces(cardPlaces);
        }

        public void DisplayTiles()
        {
            foreach (var tile in Board.tilesByListnumbers.Values)
            {
                print(tile.GetListNumber() + " " + tile.GetCoordinate());
            }
        }

        public void SetValues() 
        {
            _XSpace = xSpace;
            _YSpace = ySpace;
            _EmptySpace = emptySpace;
        }
    }
}
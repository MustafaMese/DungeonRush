using DungeonRush.Field;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Field
{
    [ExecuteAlways]
    public class BoardCreator : MonoBehaviour
    {
        [Header("Board Creating Menu")]
        public int rowLength;
        [SerializeField] Vector2 startPos = Vector2.zero;
        [SerializeField] float emptySpace = 0;
        [SerializeField] float xSpace = 0;
        [SerializeField] float ySpace = 0;
        [SerializeField] Transform wallPrefabsParent = null;

        [Header("Tile Prefabs")]
        [SerializeField] Tile tilePrefab1 = null;
        [SerializeField] Tile tilePrefab2 = null;

        [Header("Wall Prefabs")]
        [SerializeField] Tile left = null;
        [SerializeField] Tile right = null;
        [SerializeField] Tile top = null;
        [SerializeField] Tile down = null;
        [SerializeField] Tile leftTop = null;
        [SerializeField] Tile leftDown = null;
        [SerializeField] Tile rightTop = null;
        [SerializeField] Tile rightDown = null;

        public static float _XSpace = 0;
        public static float _YSpace = 0;
        public static float _EmptySpace = 0;

        [Header("Commands")]
        public bool create;
        public bool delete;
        public bool display;

        private Board board;
        public List<Tile> outerWall = new List<Tile>();

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
        }

        private void Delete()
        {
            if (board.cardPlaces != null)
            {
                for (int i = 0; i < board.cardPlaces.Count; i++)
                {
                    DestroyImmediate(board.cardPlaces[i].gameObject);
                }

                for (int i = 0; i < outerWall.Count; i++)
                {
                    DestroyImmediate(outerWall[i].gameObject);
                }

                Board.tilesByListnumbers.Clear();
                Board.tilesByCoordinates.Clear();
                outerWall.Clear();
                board.cardPlaces = null;
            }
        }

        public void Create()
        {
            if (board.cardPlaces == null)
            {
                var list = new List<Tile>();
                var currentPos = startPos;
                CreateTiles(list, currentPos);
                CreateWall();
                InitializeTiles(list);
                wallPrefabsParent.transform.SetAsLastSibling();
                Board.RowLength = rowLength;
            }
        }

        #region WALL METHODS
        private void CreateWall()
        {
            LeftWalls();
            RightWalls();
            TopWalls();
            DownWalls();
            LeftTopWall();
            LeftDownWall();
            RightTopWall();
            RightDownWall();
        }
        private void RightDownWall()
        {
            Vector2 pos = new Vector2(rowLength, -1);
            Tile t = Instantiate(rightDown, pos, Quaternion.identity, wallPrefabsParent);
            outerWall.Add(t);
        }
        private void RightTopWall()
        {
            Vector2 pos = new Vector2(rowLength, rowLength);
            Tile t =Instantiate(rightTop, pos, Quaternion.identity, wallPrefabsParent);
            outerWall.Add(t);
        }
        private void LeftDownWall()
        {
            Vector2 pos = new Vector2(-1, -1);
            Tile t = Instantiate(leftDown, pos, Quaternion.identity, wallPrefabsParent);
            outerWall.Add(t);
        }
        private void LeftTopWall()
        {
            Vector2 pos;
            pos = new Vector2(-1, rowLength);
            Tile t = Instantiate(leftTop, pos, Quaternion.identity, wallPrefabsParent);
            outerWall.Add(t);
        }
        private void DownWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(i, -1);
                Tile t = Instantiate(down, pos, Quaternion.identity, wallPrefabsParent);
                outerWall.Add(t);
            }
        }
        private void TopWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(i, rowLength);
                Tile t = Instantiate(top, pos, Quaternion.identity, wallPrefabsParent);
                outerWall.Add(t);
            }
        }
        private void RightWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(rowLength, i);
                Tile t = Instantiate(right, pos, Quaternion.identity, wallPrefabsParent);
                outerWall.Add(t);
            }
        }
        private void LeftWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(-1, i);
                Tile t = Instantiate(left, pos, Quaternion.identity, wallPrefabsParent);
                outerWall.Add(t);
            }
        }
        #endregion

        #region TILE METHODS
        private void CreateTiles(List<Tile> list, Vector2 currentPos)
        {
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < rowLength; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        var tile = Instantiate(tilePrefab1, currentPos, Quaternion.identity, board.transform);
                        list.Add(tile);
                    }
                    else
                    {
                        var tile = Instantiate(tilePrefab2, currentPos, Quaternion.identity, board.transform);
                        list.Add(tile);
                    }
                    currentPos.x += xSpace + emptySpace;
                }
                currentPos.x = startPos.x;
                currentPos.y += ySpace + emptySpace;
            }
        }
        public void InitializeTiles(List<Tile> cardPlaces)
        {
            for (int i = 0; i < cardPlaces.Count; i++)
            {
                Tile pos = cardPlaces[i];
                pos.SetCoordinate(pos.transform.position);
                pos.SetCard(null);
                pos.SetListNumber(i);
                Board.tilesByListnumbers.Add(i, pos);
                Board.tilesByCoordinates.Add(pos.transform.position, pos);
            }
            board.SetCardPlaces(cardPlaces);
        }
        #endregion

        public void SetValues() 
        {
            _XSpace = xSpace;
            _YSpace = ySpace;
            _EmptySpace = emptySpace;
        }
    }
}
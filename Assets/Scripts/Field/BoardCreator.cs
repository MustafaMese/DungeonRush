using DungeonRush.Field;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
            List<Transform> siblings = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                siblings.Add(transform.GetChild(i));
            }

            if (board.cardPlaces != null)
            {
                for (int i = 0; i < siblings.Count; i++)
                {
                    DestroyImmediate(siblings[i].gameObject);
                }

                Board.tilesByCoordinates.Clear();
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
            var tile = PrefabUtility.InstantiatePrefab(rightDown) as Tile;
            tile.transform.position = pos;
            tile.transform.SetParent(transform);
            tile.SetSortingLayer(pos);
        }
        private void RightTopWall()
        {
            Vector2 pos = new Vector2(rowLength, rowLength);
            var tile = PrefabUtility.InstantiatePrefab(rightTop) as Tile;
            tile.transform.position = pos;
            tile.transform.SetParent(transform);
            tile.SetSortingLayer(pos);
        }
        private void LeftDownWall()
        {
            Vector2 pos = new Vector2(-1, -1);
            var tile = PrefabUtility.InstantiatePrefab(leftDown) as Tile;
            tile.transform.position = pos;
            tile.transform.SetParent(transform);
            tile.SetSortingLayer(pos);
        }
        private void LeftTopWall()
        {
            Vector2 pos = new Vector2(-1, rowLength);
            var tile = PrefabUtility.InstantiatePrefab(leftTop) as Tile;
            tile.transform.position = pos;
            tile.transform.SetParent(transform);
            tile.SetSortingLayer(pos);
        }
        private void DownWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(i, -1);
                var tile = PrefabUtility.InstantiatePrefab(down) as Tile;
                tile.transform.position = pos;
                tile.transform.SetParent(transform);
                tile.SetSortingLayer(pos);
            }
        }
        private void TopWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(i, rowLength);
                var tile = PrefabUtility.InstantiatePrefab(top) as Tile;
                tile.transform.position = pos;
                tile.transform.SetParent(transform);
                tile.SetSortingLayer(pos);
            }
        }
        private void RightWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(rowLength, i);
                var tile = PrefabUtility.InstantiatePrefab(right) as Tile;
                tile.transform.position = pos;
                tile.transform.SetParent(transform);
                tile.SetSortingLayer(pos);
            }
        }
        private void LeftWalls()
        {
            Vector2 pos;
            for (int i = 0; i < rowLength; i++)
            {
                pos = new Vector2(-1, i);
                var tile = PrefabUtility.InstantiatePrefab(left) as Tile;
                tile.transform.position = pos;
                tile.transform.SetParent(transform);

                pos.x = pos.x + 2;
                tile.SetSortingLayer(pos);
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
                        var tile = PrefabUtility.InstantiatePrefab(tilePrefab1) as Tile;
                        tile.transform.position = currentPos;
                        tile.transform.SetParent(board.transform);
                        tile.SetSortingLayer(currentPos);
                        list.Add(tile);
                    }
                    else
                    {
                        var tile = PrefabUtility.InstantiatePrefab(tilePrefab2) as Tile;
                        tile.transform.position = currentPos;
                        tile.transform.SetParent(board.transform);
                        tile.SetSortingLayer(currentPos);
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
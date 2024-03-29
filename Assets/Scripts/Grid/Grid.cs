﻿using System.Collections.Generic;
using UnityEngine;
using DungeonRush.Field;
using DungeonRush.Cards;

namespace DungeonRush.Grid
{
    public class Grid : MonoBehaviour
    {
        public List<GridStructure<Tile>> tileList = new List<GridStructure<Tile>>();
        public List<GridStructure<Card>> objectList = new List<GridStructure<Card>>();
        public float width = 1f;
        public float height = 1f;
        public Vector3 offset = new Vector3(1, 1, 0);
        public GameObject tilePrefab;
        public GameObject topWallPrefab;
        public GameObject downWallPrefab;
        public GameObject rightWallPrefab;
        public GameObject leftWallPrefab;
        public GameObject topLeftWallPrefab;
        public GameObject topRightWallPrefab;
        public GameObject downLeftWallPrefab;
        public GameObject downRightWallPrefab;
        public GameObject downRightOppositeWallPrefab;
        public GameObject downLeftOppositeWallPrefab;
        public GameObject topRightOppositeWallPrefab;
        public GameObject topLeftOppositeWallPrefab;
        public Color color = Color.white;
        public bool showStartPoint;

        public string[] fileNames;

        private void OnDrawGizmos()
        {
            Vector3 pos = UnityEngine.Camera.current.transform.position;

            if (showStartPoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(0, 0, -1f), 0.5f);
            }

            Gizmos.color = color;
            for (var y = pos.y - 800f; y < pos.y + 800.0f; y += height)
            {
                Gizmos.DrawLine(new Vector3(-100000.0f, Mathf.Floor(y / height) * height + offset.y, 0.0f),
                                new Vector3(100000.0f, Mathf.Floor(y / height) * height, 0.0f));
            }

            for (var x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += width)
            {
                Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width + offset.x, -1000000.0f, 0.0f),
                            new Vector3(Mathf.Floor(x / width) * width, 1000000.0f, 0.0f));
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace Settings
    {
        public class Geometry
        {
            public static Vector3 PointFromGrid(Vector2Int gridPoint)
            {
                float x = 1.5f + 2.6f * gridPoint.x;
                float y = -2.8f - 4.1f * gridPoint.y;
                return new Vector3(x, y, 0);
            }

            public static Vector2Int GridPoint(int col, int row)
            {
                return new Vector2Int(col, row);
            }

            public static Vector2Int GridFromPoint(Vector3 point)
            {
                int col = Mathf.FloorToInt((point.x / 2.6f) + 1.58f);
                int row = -1 * (Mathf.FloorToInt((point.y / 4.1f) - 0.69f));
                return new Vector2Int(col, row);
            }

            public static Vector2Int GridFromPointFixedLeft(Vector3 point)
            {
                int col = Mathf.FloorToInt((point.x / 2.6f) + 2.5f);
                int row = -1 * (Mathf.FloorToInt((point.y / 4.1f) - 0.69f));
                return new Vector2Int(col, row);
            }

            public static Vector2Int GridFromPointFixedTop(Vector3 point)
            {
                int col = Mathf.FloorToInt((point.x / 2.6f) + 1.58f);
                int row = -1 * Mathf.FloorToInt((point.y / 4.1f) - 1.62f);
                return new Vector2Int(col, row);
            }

        }
    }
}
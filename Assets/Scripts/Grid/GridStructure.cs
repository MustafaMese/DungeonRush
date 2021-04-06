using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Grid
{
    public class GridStructure<T> where T : MonoBehaviour
    {
        public T obj;
        public Vector2 position;

        public GridStructure(T t, Vector2 pos)
        {
            obj = t;
            position = pos;
        }

        public static bool Contains(List<GridStructure<T>> list, Vector2 pos)
        {
            foreach (var item in list)
            {
                if (item.position == pos)
                    return true;
            }
            return false;
        }

        public static T GetObject(List<GridStructure<T>> tileList, Vector2 pos)
        {
            foreach (var item in tileList)
            {
                if (item.position == pos)
                    return item.obj;
            }
            return null;
        }

        public static GridStructure<T> GetStructure(List<GridStructure<T>> list, Vector2 pos)
        {
            foreach (var item in list)
            {
                if (item.position == pos)
                    return item;
            }
            return null;
        }


    }
}

